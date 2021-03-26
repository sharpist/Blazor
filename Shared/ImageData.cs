using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Shared
{
    public class ImageData : IEquatable<ImageData>, IComparable<ImageData>
    {
        public ImageData() { }
        public ImageData(string name, byte[] data, string format) =>
            (Name, Data, Format) = (name, data, format);

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        [MaxLength(50)]
        public String Name { get; set; }
        [Required]
        [Column(TypeName = "varbinary(MAX)")]
        [MaxLength]
        public Byte[] Data { get; set; }
        [Required]
        [Column(TypeName = "char(10)")]
        [MaxLength(10)]
        public String Format { get; set; }

        [NotMapped]
        public String ToBase64 =>
            // represent it as a data URL we can display
            $"data:{this.Format};base64,{Convert.ToBase64String(this.Data)}";

        public Int32 CompareTo(ImageData i)
        {
            if (i is not null)
                return this.Id.CompareTo(i.Id);
            throw new ArgumentException("Comparison is impossible, invalid argument");
        }

        public Boolean Equals([AllowNull] ImageData i) =>
            this.Data.Equals(i.Data) ? true : false;

        public override Int32 GetHashCode() => this.Id.GetHashCode();
    }
}
