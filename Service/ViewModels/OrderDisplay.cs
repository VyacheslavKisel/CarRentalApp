using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления вывода информации о заказе на веб-страницу
    public class OrderDisplay
    {
        public int Id { set; get; }
        public string PassportData { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime FinalDate { set; get; }
        public bool AvailabilityDriver { set; get; }
        public int CarId { set; get; }
        public string NameOrderStatus { set; get; }
        public string ClientEmail { set; get; }
        public string ManagerEmail { set; get; }
        public double PricePerDay { set; get; }
        public double Invoice { set; get; }
    }
}
