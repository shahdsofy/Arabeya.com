using Arabeya.Core.Domain.Entities.Cars;
using Arabeya.Core.Domain.Entities.Common;
using Arabeya.Core.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arabeya.Core.Domain.Entities.Books
{
    public class Book:BaseAuditableEntity<int>
    {

        public string? UserId { get; set; }

        public int CarId { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public virtual required ApplicationUser User { get; set; }

       
        public virtual required Car Car { get; set; }
    }
}
