using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления заказа, который является элементом
    // списка заказов достпого для вывода на веб-страницу
    public class OrderListArticle
    {
        public int Id { set; get; }
        public string PassportData { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime FinalDate { set; get; }
        public bool AvailabilityDriver { set; get; }
        public int CarId { set; get; }
        public string NameOrderStatus { set; get; }
        public string ManagerEmail { set; get; }
    }
}
