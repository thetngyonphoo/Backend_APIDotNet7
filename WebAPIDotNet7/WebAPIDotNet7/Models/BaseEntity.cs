namespace WebAPIDotNet7.Models
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int UpdadtedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
