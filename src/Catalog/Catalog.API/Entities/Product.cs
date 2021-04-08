using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Entities
{
    /// <summary>
    /// Сущность Продукт
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Идентификатор продукта
        /// </summary>
        [BsonId] // MongoDB
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Указываем, что это должен быть идентификатор
        public string Id { get; set; }
        /// <summary>
        /// Наименование продукта
        /// </summary>
        [BsonElement("Name")] // Задаем название колонки
        public string Name { get; set; }
        /// <summary>
        /// Категория
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Файл изображения продукта
        /// </summary>
        public string ImageFile { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }
    }
}
