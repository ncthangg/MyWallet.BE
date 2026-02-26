using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Domain.Exceptions
{
    public interface IBusinessException
    {
        string Code { get; }
        string Message { get; }
        object? Data { get; }
    }
    public class DomainException : Exception, IBusinessException
    {
        public string Code { get; }
        public object? Data { get; }

        public DomainException(
            string code,
            string message,
            object? data = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
            Data = data;
        }
    }
}
