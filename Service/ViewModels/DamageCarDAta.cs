using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    public class DamageCarData
    {
        [Required(ErrorMessage = "Введите описание повреждения автомобиля")]
        [StringLength(5000, ErrorMessage = "Длина описания повреждения должно быть не менее 5 и не более 5000 символов", MinimumLength = 5)]
        public string Description { set; get; }

        [Required(ErrorMessage = "Введите стоимость ремонта автомобиля")]
        [Range(0.01, 1000000, ErrorMessage = "Стоимость ремонта автомобиля должна быть не менее 0.01 и не более 1000000")]
        public double CostRepair { set; get; }
    }
}
