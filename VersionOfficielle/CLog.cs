using System;
using System.Collections.Generic;
using System.Drawing;

namespace VersionOfficielle
{
    public class CLog
    {
        public string PMessage { private set; get; }
         
        public CLog(string _message)
        {
            if (_message == null)
                throw new Exception("The message cannot be null!");

            PMessage = _message;                     
        }
    }
}
