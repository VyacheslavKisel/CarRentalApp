using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Service.Models;
using Service.ViewModels;
using Service.EF;
using NLog;

namespace Service
{
    // Уровень доступа к заказам аренды автомобилей
    public class OrderDataSource : IOrderDataSource
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private ApplicationContext applicationContext;
        public OrderDataSource()
        {
            applicationContext = new ApplicationContext();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            Order result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                result = await applicationContext.Orders.FirstOrDefaultAsync(f => f.Id == id);
                
                if (result == null)
                {
                    logger.Debug($"Заказ по Id == {id} не был извлечен из БД");
                }
                else
                {
                    logger.Debug($"Заказ по Id == {id} был успешно извлечен из БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            List<Order> result = null;
            logger.Debug("Совершено обращение к БД");

            try
            {
                result = await applicationContext.Orders.ToListAsync();
                if (result == null)
                {
                    logger.Debug("Список заказов не был извлечен из БД");
                }
                else
                {
                    logger.Debug("Список заказов был успешно извлечен из БД");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            Order result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                result = applicationContext.Orders.Add(order);

                await applicationContext.SaveChangesAsync();

                logger.Debug("Новый заказ успешно добавлен в БД");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            Order result = null;

            logger.Debug("Совершено обращение к БД");

            try
            {
                applicationContext.Entry(order).State = EntityState.Modified;

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
