using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления создания заказа
    public class OrderCreature
    {
        public int CarId { set; get; }

        public int OrderStatusId { set; get; }

        public string ClientProfileId { set; get; }

        [Required(ErrorMessage = "Введите паспортные данные")]
        [RegularExpression(@"[А-Я]{2}[0-9]{6}", 
            ErrorMessage = "Введите паспортные данные в таком формате XXYYYYYY, " +
            "где XX - серия паспорта, введенная кириллицей YYYYYY - номер паспорта, введенный цифрами")]
        public string PassportData { set; get; }

        [Required(ErrorMessage = "Введите дату и время, с которого вы планируете арендовать автомобиль")]
        [DataType(DataType.DateTime, ErrorMessage = "Введите дату и время в формате yyyy.mm.dd hh:nn," +
            " где yyyy - год, mm - месяц, dd - день, hh - часы, yy - минуты")]
        public DateTime StartDate { set; get; }

        [Required(ErrorMessage = "Введите дату и время, до которого вы планируете арендовать автомобиль")]
        [DataType(DataType.DateTime, ErrorMessage = "Введите дату и время в формате yyyy.mm.dd hh:nn," +
            " где yyyy - год, mm - месяц, dd - день, hh - часы, yy - минуты")]
        public DateTime FinalDate { set; get; }

        public bool AvailabilityDriver { set; get; }
    }
}
