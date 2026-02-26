using MyWallet.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWallet.Application.Exceptions
{
    public class ApplicationException : Exception, IBusinessException
    {
        public string Code { get; }
        public object? Data { get; }

        public ApplicationException(
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
