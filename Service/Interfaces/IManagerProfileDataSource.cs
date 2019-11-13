using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    // Интерфейс доступа к профилю менеджера
    public interface IManagerProfileDataSource
    {
        Task<ManagerProfile> AddManagerProfileAsync(ManagerProfile managerProfile);
    }
}
