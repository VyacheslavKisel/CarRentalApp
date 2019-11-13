using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления для добавления,
    // редактирования, удаления автомобиля
    public class CarViewModel
    {
        public CarViewModel(int id, string title, string brand, string qulityClass, double price)
        {
            Id = id;
            Title = title;
            Brand = brand;
            QulityClass = qulityClass;
            Price = price;
        }

        public CarViewModel()
        { }

        public int Id { set; get; }

        [Required(ErrorMessage = "Введите название автомобиля")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "Длина названия автомобиля должна быть от 3 до 128 символов")]
        public string Title { set; get; }

        [Required(ErrorMessage = "Введите марку автомобиля")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "Длина названия марки автомобиля должна быть от 3 до 128 символов")]
        public string Brand { set; get; }

        [Required(ErrorMessage = "Введите класс качества автомобиля")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "Длина названия класса качества автомобиля должна быть от 3 до 128 символов")]
        public string QulityClass { set; get; }
        
        [Required(ErrorMessage = "Введите стоимость проката автомобиля за сутки")]
        [Range(0.01, 1000000, ErrorMessage = "Стоимость проката автомобиля за сутки должна быть не менее 0.01 и не более 1000000")]
        public double Price { set; get; }
    }
}
