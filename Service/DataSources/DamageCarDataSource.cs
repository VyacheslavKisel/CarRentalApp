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
    // Уровень доступа к поврежденным автомобилям
    public class DamageCarDataSource : IDamageCarDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ApplicationContext applicationContext;
        public DamageCarDataSource()
        {
            applicationContext = new ApplicationContext();
        }

        public async Task<DamageCar> GetDamageCarAsync(int id)
        {
            DamageCar result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                result = await applicationContext.DamageCars.FirstOrDefaultAsync(f => f.Id == id);
                if (result == null)
                {
                    logger.Debug($"Поврежденный автомобиль по Id == {id} не был извлечен из БД");
                }
                else
                {
                    logger.Debug($"Поврежденный втомобиль по Id == {id} был успешно извлечен из БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<IEnumerable<DamageCar>> GetDamageCarsAsync()
        {
            var result = new List<DamageCar>();

            logger.Debug("Совершено обращение к БД");
            try
            {
                result = await applicationContext.DamageCars.ToListAsync();
                if (result == null)
                {
                    logger.Debug("Список поврежденный автомобилей не был извлечен из БД");
                }
                else
                {
                    logger.Debug("Список поврежденный автомобилей был успешно извлечен из БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<DamageCar> AddDamageCarAsync(DamageCar damageCar)
        {
            DamageCar result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                result = applicationContext.DamageCars.Add(damageCar);

                await applicationContext.SaveChangesAsync();

                logger.Debug("Новый поврежденный автомобиль успешно добавлен в БД");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<DamageCar> UpdateDamageCarAsync(DamageCar damageCar)
        {
            DamageCar result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                applicationContext.Entry(damageCar).State = EntityState.Modified;

                await applicationContext.SaveChangesAsync();

                logger.Debug("Редактирование записи было проведено успешно.");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }
    }
}
