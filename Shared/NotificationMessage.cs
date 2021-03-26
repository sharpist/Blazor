using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Shared
{
    public class NotificationMessage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "enter sender name")]
        [DisplayName("Sender")]
        [Column(TypeName = "varchar(16)")]
        [MaxLength(16)]
        public String SenderName { get; set; }
        [Required(ErrorMessage = "enter receiver name")]
        [DisplayName("Receiver")]
        [Column(TypeName = "varchar(16)")]
        [MaxLength(16)]
        public String ReceiverName { get; set; }
        [Required(ErrorMessage = "enter message title")]
        [DisplayName("Title")]
        [Column(TypeName = "varchar(64)")]
        [MaxLength(64)]
        public String MsgTitle { get; set; }
        [Required(ErrorMessage = "enter message body")]
        [DisplayName("Message")]
        [Column(TypeName = "varchar(MAX)")]
        [MaxLength]
        public String MsgBody { get; set; }

        public DateTime MsgDate { get; set; } = DateTime.Now;
        [NotMapped]
        public string MsgDateSt => this.MsgDate.ToString("dd-MMM-yyyy");
    }
}
