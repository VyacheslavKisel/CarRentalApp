using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    // Интерфейс доступа к автомобилям, которые вернули клиенты
    public interface IReturnCarDataSource
    {
        Task<ReturnCar> AddReturnCarAsync(ReturnCar returnCar);
    }
}
