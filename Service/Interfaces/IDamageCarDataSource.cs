using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    // Интерфейс доступа к поврежденным автомобилям
    public interface IDamageCarDataSource
    {
        Task<DamageCar> GetDamageCarAsync(int id);
        Task<IEnumerable<DamageCar>> GetDamageCarsAsync();
        Task<DamageCar> AddDamageCarAsync(DamageCar damageCar);
        Task<DamageCar> UpdateDamageCarAsync(DamageCar damageCar);
    }
}
