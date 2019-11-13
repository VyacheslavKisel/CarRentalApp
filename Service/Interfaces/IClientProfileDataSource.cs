using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    // Интерфейс уровня доступа к профилю клиента
    public interface IClientProfileDataSource
    {
        Task<ClientProfile> GetClientProfileAsync(string id);
        Task<ClientProfile> AddClientProfileAsync(ClientProfile clientProfile);
    }
}
