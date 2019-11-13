using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления пользователя, который является элементом
    // списка пользователей доступного для вывода на веб-страницу
    public class UserListArticle
    {
        public string Id { set; get; }
        public string Email { set; get; }
        public string UserName { set; get; }
    }
}
