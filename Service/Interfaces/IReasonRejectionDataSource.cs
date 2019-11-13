using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;

namespace Service
{
    // Интерфейс доступа к причинам отказа в аренде автомобилей
    public interface IReasonRejectionDataSource
    {
        Task<ReasonRejection> AddReasonRejectionAsync(ReasonRejection reasonRejection);
    }
}
