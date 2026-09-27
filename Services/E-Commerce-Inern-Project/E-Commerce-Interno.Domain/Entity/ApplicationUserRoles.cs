using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Interno.Domain.Entity
{
    public class ApplicationUserRoles
    {
        public Guid UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public ApplicationUser User { get; set; }

        public Guid RoleID { get; set; }
        [ForeignKey(nameof(RoleID))]
        public ApplicationRole Role { get; set; }

        public bool IsDeleted { get; set; } = false;    
    }
}
