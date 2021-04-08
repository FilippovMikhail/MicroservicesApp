using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// GET api/v1/[controller]
        /// <summary>
        /// Получить список продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync()
        {
            var products = await _repository
                    .GetProductsAsync();
            return Ok(products);
        }

        /// Get api/v1/[controller]/5
        /// <summary>
        /// Получить продукт по id
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        [HttpGet]
        //[ActionName(nameof(GetProductById))]
        [Route("{id:length(24)}", Name = nameof(GetProductByIdAsync))]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductByIdAsync(string id)
        {
            var product = await _repository
                .GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }

            return Ok(product);
        }

        /// GET api/v1/[controller]/GetProductsByNameAsync/name
        /// <summary>
        /// Получить список продуктов по наименованию продукта
        /// </summary>
        /// <param name="name">Наименование продукта</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductsByNameAsync/{name}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByNameAsync(string name)
        {
            var products = await _repository
                    .GetProductsByNameAsync(name);
            return Ok(products);
        }

        /// GET api/v1/[controller]/GetProductsByCategoryAsync/categoryName
        /// <summary>
        /// Получить список продуктов по наименованию категории
        /// </summary>
        /// <param name="categoryName">Наименование категории</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductsByCategoryAsync/{categoryName}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategoryAsync(string categoryName)
        {
            var products = await _repository
                    .GetProductsByCategoryAsync(categoryName);
            return Ok(products);
        }

        /// POST api/v1/[controller]/items
        /// <summary>
        /// Создать продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateAsync([FromBody] Product product)
        {
            await _repository
                .CreateAsync(product);
            // После создания объекта Product, выполняем перенаправление, чтобы получить Product
            return CreatedAtRoute(nameof(GetProductByIdAsync), new { id = product.Id }, product);
        }

        /// PUT api/v1/[controller]/items
        /// <summary>
        /// Обновить продукт
        /// </summary>
        /// <param name="product">Сущность Product</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> UpdateAsync([FromBody] Product product)
        {
            var productItem = await _repository
                    .GetProductByIdAsync(product.Id);
            if (productItem == null)
            {
                return NotFound(new { Message = $"Item with id: {product.Id} not found." });
            }
            var isUpdate = await _repository
                    .UpdateAsync(product);
            return Ok(isUpdate);
        }

        /// DELETE api/v1/[controller]/id
        /// <summary>
        /// Удалить продукт
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteAsync(string id)
        {
            var productItem = await _repository
                    .GetProductByIdAsync(id);
            if (productItem==null)
            {
                return NotFound(new { Message = $"Item with id: {id} not found." });
            }
            var isDelete = await _repository
                    .DeleteAsync(id);
            return Ok(isDelete);
        }
    }
}
