using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    // Модель профиля менеджера
    public class ManagerProfile
    {
        [Key]
        [MaxLength(128)]
        [ForeignKey("ApplicationUser")]
        public string Id { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public virtual ApplicationUser ApplicationUser { set; get; }
        public virtual ICollection<Order> Orders { set; get; }
    }
}
