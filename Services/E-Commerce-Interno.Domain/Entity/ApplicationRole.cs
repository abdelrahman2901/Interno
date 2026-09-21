using E_Commerce_Inern_Project.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Inern_Project.Core.Domain.Entity
{
    public class ApplicationRole
    {
        [Key]
        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public RolesEnum RoleEnumID { get; set; }

        public ICollection<ApplicationUserRoles> UserRoles { get; set; }
    }
}
