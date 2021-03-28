using System;

namespace RManjusha.RestServices.Helpers
{
    public static class ExceptionExtension
    {
        public static Exception GetInnermostException(this Exception ex)
        {
            if (ex == null)
                return null;
           
               
            if (ex.InnerException != null)
               return ex.InnerException.GetInnermostException();

            return ex;
        }
    }
}
