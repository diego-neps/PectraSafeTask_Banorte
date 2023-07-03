using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PectraForms.WebApplication.BackEnd.API.Helpers
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException()
        {
        }

        public InvalidSessionException(string message)
        : base(message)
        {
        }

        public InvalidSessionException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

}