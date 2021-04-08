namespace Catalog.API.Settings
{
    /// <summary>
    /// Настройки базы данных
    /// </summary>
    public interface ICatalogDatabaseSettings
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// Название базы данных
        /// </summary>
        string DatabaseName { get; set; }
        /// <summary>
        /// Наименование коллекции
        /// </summary>
        string CollectionName { get; set; }
    }
}
