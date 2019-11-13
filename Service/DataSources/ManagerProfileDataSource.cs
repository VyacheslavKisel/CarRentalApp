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
    // Уровень доступа к профилю менеджера
    public class ManagerProfileDataSource : IManagerProfileDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ManagerProfileDataSource()
        { }

        public async Task<ManagerProfile> AddManagerProfileAsync(ManagerProfile managerProfile)
        {
            ManagerProfile result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = applicationContext.ManagerProfiles.Add(managerProfile);

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Новый профиль менеджера успешно добавлен в БД");
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
