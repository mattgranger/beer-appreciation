using System.Threading.Tasks;
using BeerAppreciation.Data.Repositories.Context;
using BeerAppreciation.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace BeerAppreciation.Web
{
    using System.Net.Mail;

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<Appreciator>
    {
        public ApplicationUserManager(IUserStore<Appreciator> store)
            : base(store)
        {
        }

        public static ApplicationUserManager CreateDefault()
        {
            var manager = new ApplicationUserManager(new UserStore<Appreciator>(new DatabaseContext()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Appreciator>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.EmailService = new EmailService();

            return manager;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<Appreciator>(context.Get<DatabaseContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Appreciator>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<Appreciator>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            manager.EmailService = new EmailService();

            return manager;
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }
        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context.Get<DatabaseContext>());
            return new ApplicationRoleManager(roleStore);
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // convert IdentityMessage to a MailMessage
            var email =
               new MailMessage(new MailAddress("noreply@codefusion.com.au", "Beer Admin"),
               new MailAddress(message.Destination))
               {
                   Subject = message.Subject,
                   Body = message.Body,
                   IsBodyHtml = true
               };

            var client = new SmtpClient();
            client.SendCompleted += (s, e) => client.Dispose();
            return client.SendMailAsync(email);
        }
    }
}
