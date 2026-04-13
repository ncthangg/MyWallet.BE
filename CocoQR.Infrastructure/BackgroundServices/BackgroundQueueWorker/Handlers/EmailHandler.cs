using CocoQR.Application.Contracts.ISubServices;
using CocoQR.Application.Contracts.IUnitOfWork;
using CocoQR.Domain.Constants.Enum;
using CocoQR.Domain.Helper;
using CocoQR.Infrastructure.BackgroundServices.BackgroundQueueWorker.Jobs;

namespace CocoQR.Infrastructure.BackgroundServices.BackgroundQueueWorker.Handlers
{
    public class EmailHandler
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public EmailHandler(IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(SendEmailJob job, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var smtpType = job.SmtpType
                ?? (!string.IsNullOrWhiteSpace(job.TemplateKey)
                    ? EmailTemplateSmtpResolver.Resolve(job.TemplateKey)
                    : SmtpSettingType.System);

            var smtpSetting = await _unitOfWork.SmtpSettings.GetActiveAsync(smtpType);
            if (smtpSetting == null)
            {
                throw new NonRetryableJobException($"No active SMTP setting for background email job. Type={smtpType}");
            }

            await _emailService.SendWithoutLogAsync(
                job.To,
                job.Subject,
                job.Body,
                smtpSetting,
                job.Direction,
                job.TemplateKey);

            if (job.EmailLogId.HasValue)
            {
                var emailLog = await _unitOfWork.EmailLogs.GetByIdAsync(job.EmailLogId.Value);
                if (emailLog != null)
                {
                    emailLog.Status = EmailLogStatus.SUCCESS;
                    emailLog.ErrorMessage = null;
                    await _unitOfWork.EmailLogs.UpdateAsync(emailLog);
                }
            }
        }
    }
}
