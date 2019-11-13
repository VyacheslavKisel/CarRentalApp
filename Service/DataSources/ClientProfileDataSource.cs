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
    // Уровень доступа к профилю клиента
    public class ClientProfileDataSource : IClientProfileDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ApplicationContext applicationContext;
        public ClientProfileDataSource()
        {
            applicationContext = new ApplicationContext();
        }

        public async Task<ClientProfile> GetClientProfileAsync(string id)
        {
            ClientProfile result = null;

            logger.Debug("Совершено обращение к БД");
            try
            {
                result = await applicationContext.ClientProfiles.FirstOrDefaultAsync(f => f.Id == id);

                if (result == null)
                {
                    logger.Debug($"Профиль клиента по Id == {id} не был извлечен из БД");
                }
                else
                {
                    logger.Debug($"Профиль клиента по Id == {id} был успешно извлечен из БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            return result;
        }

        public async Task<ClientProfile> AddClientProfileAsync(ClientProfile clientProfile)
        {
            ClientProfile result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                result = applicationContext.ClientProfiles.Add(clientProfile);

                await applicationContext.SaveChangesAsync();

                logger.Debug("Новый профиль клиента успешно добавлен в БД");
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
