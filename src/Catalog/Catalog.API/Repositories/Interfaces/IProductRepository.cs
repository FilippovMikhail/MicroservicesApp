using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Получить список продуктов
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsAsync();
        /// <summary>
        /// Получить продукт по id
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        Task<Product> GetProductByIdAsync(string id);
        /// <summary>
        /// Получить список продуктов по наименованию продукта
        /// </summary>
        /// <param name="name">Наименование продукта</param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        /// <summary>
        /// Получить список продуктов по наименованию категории
        /// </summary>
        /// <param name="categoryName">Наименование категории</param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName);
        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        Task CreateAsync(Product product);
        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Product product);
        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(string id);
    }
}
