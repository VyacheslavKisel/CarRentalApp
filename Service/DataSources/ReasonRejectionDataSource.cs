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
    // Уровень доступа к причине отказа в аренде автомобилей
    public class ReasonRejectionDataSource : IReasonRejectionDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ReasonRejectionDataSource()
        { }

        public async Task<ReasonRejection> AddReasonRejectionAsync(ReasonRejection reasonRejection)
        {
            ReasonRejection result = null;

            try
            {
                using (var applicationContext = new ApplicationContext())
                {
                    result = applicationContext.ReasonRejections.Add(reasonRejection);

                    await applicationContext.SaveChangesAsync();

                    logger.Debug("Новая причина отказа успешно добавлена в БД");
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
