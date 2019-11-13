using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Service.Models;
using Service.EF;
using NLog;

namespace Service
{
    // Уровень доступа к автомобилям доступным для аренды
    public class CarDataSource : ICarDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CarDataSource()
        { }

        public async Task<Car> GetCarAsync(int id)
        {
            Car result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = await applicationContext.Cars.FirstOrDefaultAsync(f => f.Id == id);

                    if (result == null)
                    {
                        logger.Debug($"Автомобиль по Id == {id} не был извлечен из БД");
                    }
                    else
                    {
                        logger.Debug($"Автомобиль по Id == {id} был успешно извлечен из БД");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return result;
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            var result = new List<Car>();

            logger.Debug("Совершено обращение к БД");

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = await applicationContext.Cars.ToListAsync();

                    if (result == null)
                    {
                        logger.Debug("Список автомобилей не был извлечен из БД");
                    }
                    else
                    {
                        logger.Debug("Список автомобилей был успешно извлечен из БД");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return result;
        }

        public async Task<Car> AddCarAsync(Car car)
        {
            Car result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = applicationContext.Cars.Add(car);

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Новый автомобиль успешно добавлен в БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return result;
        }

        public async Task<Car> UpdateCarAsync(Car car)
        {
            Car result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    applicationContext.Entry(car).State = EntityState.Modified;

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Редактирование записи было проведено успешно.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return result;
        }

        public async Task DeleteCarAsync(int id)
        {
            logger.Debug("Совершено обращение к БД");

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    var car = await applicationContext.Cars.FirstOrDefaultAsync(f => f.Id == id);

                    applicationContext.Entry(car).State = EntityState.Deleted;

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Удаление записи было проведено успешно.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }
    }
}
