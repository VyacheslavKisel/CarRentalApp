using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления элемента списка поврежденных 
    // автомобилей, доступного для вывода на веб-страницу
    public class DamageCarArticleList
    {
        public int Id { set; get; }
        public string Description { set; get; }
        public double CostRepair { set; get; }
        public bool InvoiceMessage { set; get; }
        public bool UnderRepairNow { set; get; }
        public string ManagerEmail { set; get; }
    }
}
