using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;
using Service.EF;
using NLog;

namespace Service
{
    // Уровень доступа к автомобилям, которые были возращены клиентами
    public class ReturnCarDataSource : IReturnCarDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ReturnCarDataSource()
        { }

        public async Task<ReturnCar> AddReturnCarAsync(ReturnCar returnCar)
        {
            ReturnCar result = null;

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = applicationContext.ReturnCars.Add(returnCar);

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Новый возврат автомобиля успешно добавлен в БД");
                }
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
