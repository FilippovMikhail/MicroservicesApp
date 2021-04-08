using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Settings
{
    /// <summary>
    /// Настройки базы данных
    /// </summary>
    public class CatalogDatabaseSettings : ICatalogDatabaseSettings
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// База данных
        /// </summary>
        public string DatabaseName { get; set; }
        /// <summary>
        /// Наименование коллекции
        /// </summary>
        public string CollectionName { get; set; }
    }
}
