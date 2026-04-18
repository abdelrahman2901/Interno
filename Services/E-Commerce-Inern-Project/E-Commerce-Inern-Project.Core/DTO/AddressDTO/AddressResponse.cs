 

namespace E_Commerce_Inern_Project.Core.DTO.AddressDTO
{
    public class AddressResponse
    {
        public Guid AddressID { get; set; }
        public Guid UserID { get; set; }
        public Guid CityID { get; set; }
        public Guid AreaID { get; set; }
        public string AddressLabel { get; set; }
        public string MainAddress { get; set; }
        public string? BackUpAddress { get; set; }
        public string BackUpPhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDefault { get; set; }
    }
}
