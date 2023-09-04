using System.Security.Claims;
using idunno.Authentication.Basic;

namespace GarageDoorMonitor
{
    public class AuthUtil
    {
        private readonly string _userName;
        private readonly string _password;

        public AuthUtil(string userName, string password)
        {
            _userName = userName;
            _password = password;
        }

        public Task BasicAuthValidateCredentials(ValidateCredentialsContext context)
        {
            if (context.Username == _userName &&
                context.Password == _password)
            {
                context.Principal = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim(
                            ClaimTypes.Name,
                            context.Username,
                            context.Options.ClaimsIssuer)
                    }, context.Scheme.Name));
                context.Success();
            }
            return Task.CompletedTask;
        }
    }
}
