using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Получить список продуктов
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var result = await _context
                            .Products
                            .Find(_ => true) // Мы не устанавливаем фильтр, мы просто хотим получить всю коллекцию
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// Получить продукт по id
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        public async Task<Product> GetProductByIdAsync(string id)
        {
            var result = await _context
                            .Products
                            .Find(_ => _.Id == id)
                            .FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// Получить список продуктов по наименованию продукта
        /// </summary>
        /// <param name="name">Наименование продукта</param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            // Создаем определение фильтра, чтобы вернуть множественный ответ от MongoDb
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(_ => _.Name, name);

            var result = await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// Получить список продуктов по наименованию категории
        /// </summary>
        /// <param name="categoryName">Наименование категории</param>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName)
        {
            // Создаем фильтр
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(_ => _.Category, categoryName);

            var result = await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
            return result;
        }

        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task CreateAsync(Product product)
        {
            await _context
                .Products
                .InsertOneAsync(product);
        }

        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            // Обновляем объект Product
            // MongoDb хранит Products, мы по id находим объект и заменяем его на новый JSON
            var updateResult = await _context
                                    .Products
                                    .ReplaceOneAsync(filter: _ => _.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(string id)
        {
            // Создаем фильтр
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(_ => _.Id, id);

            var deleteResult = await _context
                            .Products
                            .DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged
                    && deleteResult.DeletedCount > 0;
        }
    }
}
