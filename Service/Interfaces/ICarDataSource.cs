using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    // Интерфейс уровня доступа к автомобилям доступных для аренды
    public interface ICarDataSource
    {
        Task<Car> GetCarAsync(int id);
        Task<IEnumerable<Car>> GetCarsAsync();
        Task<Car> AddCarAsync(Car car);
        Task<Car> UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}
