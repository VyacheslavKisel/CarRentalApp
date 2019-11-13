using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    //Модель учетных данных пользователя
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public virtual ClientProfile ClientProfile { set; get; }

        public virtual ManagerProfile ManagerProfile { set; get; }
    }
}
