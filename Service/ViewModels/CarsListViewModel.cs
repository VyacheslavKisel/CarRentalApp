using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Service.ViewModels
{
    // Модель представления автомобиля, который является 
    // элементом списка автомобилей доступных для аренды
    public class CarsListViewModel
    {
        public IEnumerable<Car> Cars { set; get; }
        public List<string> Brands { set; get; }
        public List<string> QualityClasses { set; get; }
        public string[] BrandsSelected { set; get; }
        public string[] QualityClassesSelected { set; get; }
        public string SortPrice { set; get; }
        public string SortTitle { set; get; }
    }
}
