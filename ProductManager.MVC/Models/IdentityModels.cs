using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.Data.Entity;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace IdentitySample.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Adicionado para guardar o nome completo na base de dados de autenticação para ser mais fácil gerir os utilizadores
        [DisplayName("Employee Name")]
        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", this.FullName));

            return userIdentity;
        }
    }

    //Disponibilizar nome completo no objecto User
    public static class IdentityExtensions
    {
        public static string GetOrganizationId(this IIdentity identity)
        {
            return ((ClaimsIdentity)identity).FindFirst("FullName").Value;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ProductManagerMVCIdentity", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}