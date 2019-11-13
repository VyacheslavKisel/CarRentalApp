using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления счета к оплате для 
    // клиента за аренду либо повреждение автомобиля 
    public class InvoiceForClient
    {
        public double InvoiceRental { set; get; }
        public double CostRepair { set; get; }
    }
}
