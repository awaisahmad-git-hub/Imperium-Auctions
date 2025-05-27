using PrestigeAuction.CustomValidations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrestigeAuction.Models
{
    public class ChatMessage
    {
        [Key]
        public Guid MessageID { get; set; }
        [Required]
        [DisplayName("Message")]
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters.")]
        public string Message { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false)]
        [DisplayName("User ID")]
        public string SenderId { get; set; }=string.Empty;
        [Required]
        [DisplayName("Product ID")]
        public int ProductID { get; set; }
        [Required]
        [DisplayName("Timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow.ToLocalTime();
        [Required(AllowEmptyStrings = false)]
        [DisplayName("Sender Name")]
        public string SenderName { get; set; }= string.Empty;
    }
}
