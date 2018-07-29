using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionOfficielle
{
    class CPhoneNumber
    {
        public ulong PhoneNumber;
        public string SMSProviderAddress;

        /// <summary>
        /// Default and only constructor of this class.
        /// </summary>
        /// <param name="_number">Phone number.</param>
        /// <param name="_SMSProviderAddress">This is the SMS gateway of the provider. 
        /// Must be provided in this format: mynumber@provider.com</param>
        public CPhoneNumber(ulong _number, string _SMSProviderAddress)
        {
            PhoneNumber = _number;
            SMSProviderAddress = _SMSProviderAddress;
        }

        /// <summary>
        /// Get the phone email (phonenumber@emailProviderAddress.com)
        /// </summary>
        /// <returns>Returns the phone email address.</returns>
        public string getPhoneEmail()
        {
            return Convert.ToString(PhoneNumber) + "@" + SMSProviderAddress;
        }
    }
}
