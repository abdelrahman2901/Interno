namespace E_Commerce_Inern_Project.Core.DTO.WishListDTO
{
    public class WishListResponse
    {
        public Guid WishlistID { get; set; }
        public Guid UserID { get; set; }
        public Guid ProductID { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
