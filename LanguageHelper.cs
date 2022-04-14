using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISXMLParse
{
    public static class LanguageHelper
    {
        private const string missingLanguage = "MISSING_KEY";

        public const string LANG_ACCESS_ERROR = "The system has encountered an error while processing your request.";
        public const string LANG_ACCESS_ERROR_BODY = "Please contact the Ministry at 1-866-866-0800 for assistance.";

        public const string LANG_UNAVAILABLE_ERROR = "The My Self Serve Portal is currently unavailable.";
        public const string LANG_UNAVAILABLE_ERROR_BODY = "Please try accessing this site at a later time.";

        public const string LANG_ICM_ERR_REVOKED = "Access to My Self Serve has been revoked";
        public const string LANG_ICM_ERR_REVOKED_BODY = "Please be aware that if your access has been revoked, you will not be able to complete the registration process and link your profile using a different BCeID and/or password.";

        public const string LANG_SESSION_EXPIRY = "Session Expired";
        public const string LANG_SESSION_EXPIRY_BODY = "Your session has expired: for performance reasons, this application cannot remain inactive for more than {0} minutes. Your session has been inactive for over {0} minutes and no longer has a connection with our servers. We apologize for any inconvenience.";

        public const string LANG_ACCOUNT_NOT_READY = "Your account is being prepared. This should take less than 5 minutes.";
        public const string LANG_ACCOUNT_NOT_READY_BODY = "Please try signing in again shortly.<br>Thank you for your patience.";

        public const string LANG_ACCOUNT_MAN_PROSPECT = "Your registration requires further processing and has been assigned to a worker.";
        public const string LANG_ACCOUNT_MAN_PROSPECT_BODY = "Please log on in 1 business day.<br>If you have an urgent request, please contact the Ministry at <a href=\"tel:+18668660800\">1-866-866-0800</a>.";

        public const string LANG_ACCOUNT_DEACTIVATED = "This appears to be an old account that is no longer active.";
        public const string LANG_ACCOUNT_DEACTIVATED_BODY = "If you have created a newer account please sign out and login using the new account.<br>If you require help accessing your account, please contact the Ministry at <a href=\"tel:+18668660800\">1-866-866-0800</a>.";

        public const string LANG_DEFAULT_ERROR = "Site Error";
        public const string LANG_DEFAULT_ERROR_BODY = "An unexpected error has occured and the Site Administrator has been notified.";

        /// <summary>
        /// Trims subject line to standard length and adds periods if applicable 
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>Modified Subject String</returns>
        public static string GetDictionaryKey(Dictionary<string, string> langDictionary, string dictionaryKey)
        {
            string dictionaryValue = missingLanguage;

            if (langDictionary.ContainsKey(dictionaryKey) == true)
            {
                dictionaryValue = langDictionary[dictionaryKey.ToUpper()];
            }
            else
            {
                dictionaryValue += ":" + dictionaryKey;
            }

            return dictionaryValue;
        }
    }
}
