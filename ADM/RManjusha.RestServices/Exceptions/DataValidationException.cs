using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

namespace RManjusha.RestServices.Exceptions
{
    public class DataValidationException : Exception
    {
        public DataValidationException()
        { }

        public DataValidationException(string message)
            : base(message)
        { }

        public DataValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public DataValidationException(DbUpdateException innerException) :
        base(null, innerException)
        {
        }

        public override string Message
        {
            get
            {
                //var innerException = InnerException as DbUpdateException;
                //if (innerException != null)
                //{
                //    StringBuilder sb = new StringBuilder();

                //    sb.AppendLine();
                //    sb.AppendLine();
                //    foreach (var eve in innerException.Entries)
                //    {
                //        sb.AppendLine(string.Format("- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //            eve.Entry.Entity.GetType().FullName, eve.Entry.State));
                //        foreach (var ve in eve.ValidationErrors)
                //        {
                //            sb.AppendLine(string.Format("-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                //                ve.PropertyName,
                //                eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                //                ve.ErrorMessage));
                //        }
                //    }
                //    sb.AppendLine();

                //    return sb.ToString();
                //}

                return base.Message;
            }
        }
    }
}
