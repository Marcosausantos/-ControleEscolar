﻿using ControleEscolar.Entities.Seguranca;
using ControleEscolar.Infraestructure.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace ControleEscolar.Service
{
    public class ApplicationUserManager : UserManager<Usuario, int>
    {
        public ApplicationUserManager(IUserStore<Usuario, int> store)
            : base(store)
        {

        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {            
            var manager = new ApplicationUserManager(new CustomUserStore(context.Get<ControleEscolarDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Usuario, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Usuario, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }        
    }    
}
