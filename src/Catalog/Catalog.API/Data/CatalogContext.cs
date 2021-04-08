using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            // Создадим соединение с MongoDb
            // ConnectionString берется из файла appsettings.json
            MongoClient client = new(connectionString: settings.ConnectionString);
            // Получаем БД в MongoDb
            var database = client.GetDatabase(name: settings.DatabaseName);

            // Загружаем коллекцию продуктов из БД
            // В качестве параметра передаем имя коллекции из appsettings.json
            Products = database.GetCollection<Product>(name: settings.CollectionName);

            // Заполняем коллекцию Products данными
            CatalogContextSeed.SeedData(Products);
        }

        /// <summary>
        /// Коллекция продуктов
        /// </summary>
        public IMongoCollection<Product> Products { get;}
    }
}
