using Microsoft.AspNetCore.Identity;
namespace E_Commerce_Inern_Project.Core.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string RoleDescription { get; set; }
    }
}
