using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

namespace SystemPlus.GoogleMail
{
    public static class MailGet
    {
        public static Profile GetProfileFromCredentials(UserCredential credential, string appName)
        {
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = appName,
            });

            return service.Users.GetProfile("me").Execute();
        }

        public static string GetEmailFromCredentials(UserCredential credential, string appName)
            => GetProfileFromCredentials(credential, appName).EmailAddress;

        public static bool TryGetProfileFromCredentials(UserCredential credential, string appName, out Profile profile)
        {
            profile = null;
            try
            {
                GmailService service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = appName,
                });
                profile = service.Users.GetProfile("me").Execute();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryGetEmailFromCredentials(UserCredential credential, string appName, out string email)
        {
            email = null;
            if (TryGetProfileFromCredentials(credential, appName, out Profile p))
            {
                email = p.EmailAddress;
                return true;
            }
            else
                return false;
        }
    }
}
