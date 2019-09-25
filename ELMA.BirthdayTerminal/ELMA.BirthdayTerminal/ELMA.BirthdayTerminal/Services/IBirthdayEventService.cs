using ELMA.BirthdayTerminal.Models;
using System.Collections.Generic;

namespace ELMA.BirthdayTerminal.Services
{
    /// <summary>
    /// Сервис работы с событиями
    /// </summary>
    public interface IBirthdayEventService
    {
        /// <summary>
        /// Запросить события из ELMA
        /// </summary>
        bool SyncronizeEvents(GlobalSettings settings);

        /// <summary>
        /// Получить сегодняшние события
        /// </summary>
        List<BirthdayEvent> GetTodayEvents();

        /// <summary>
        /// Получить будующие события
        /// </summary>
        List<BirthdayEvent> GetNexEvents();
    }
}
