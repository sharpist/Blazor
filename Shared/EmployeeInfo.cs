using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Shared
{
    public class EmployeeInfo : IEquatable<EmployeeInfo>, IComparable<EmployeeInfo>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "enter your name")]
        [DisplayName("Name")]
        [MaxLength(16)]
        public String Name { get; set; }
        [Required(ErrorMessage = "enter your gender")]
        [DisplayName("Gender")]
        [MaxLength(8)]
        public String Gender { get; set; }
        [Required(ErrorMessage = "enter your city")]
        [DisplayName("City")]
        [MaxLength(32)]
        public String City { get; set; }
        [Required(ErrorMessage = "enter your country")]
        [DisplayName("Country")]
        [MaxLength(32)]
        public String Country { get; set; }

        public Int32 CompareTo(EmployeeInfo e)
        {
            if (e is not null)
                return this.Id.CompareTo(e.Id);
            throw new ArgumentException("Comparison is impossible, invalid argument");
        }

        public Boolean Equals([AllowNull] EmployeeInfo e) =>
            this.Name.Equals(e.Name) && this.Gender.Equals(e.Gender) &&
            this.City.Equals(e.City) && this.Country.Equals(e.Country)
            ? true : false;

        public override Int32 GetHashCode() => this.Id.GetHashCode();
    }
}
