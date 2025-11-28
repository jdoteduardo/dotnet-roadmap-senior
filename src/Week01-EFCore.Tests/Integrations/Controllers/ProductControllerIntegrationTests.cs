using ECommerce.OrderManagement.API.Tests.Integrations.Factories;
using ECommerce.OrderManagement.API.Tests.Integrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.OrderManagement.API.Tests.Integrations.Controllers
{
    public class ProductControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ProductControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            factory.ResetDatabase();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsPagedProducts_WhenProductsExist()
        {
            var productsToCreate = new[]
            {
                new { Name = "Product A", Price = 10m, CategoryId = 1 },
                new { Name = "Product B", Price = 20m, CategoryId = 1 },
                new { Name = "Product C", Price = 30m, CategoryId = 1 }
            };

            foreach (var p in productsToCreate)
            {
                var content = new StringContent(JsonSerializer.Serialize(p), Encoding.UTF8, "application/json");
                var res = await _client.PostAsync("/api/v1/products", content);
                Assert.Equal(HttpStatusCode.Created, res.StatusCode);
            }

            var response = await _client.GetAsync("/api/v1/products?pageNumber=1&pageSize=2");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(response.Headers.Contains("X-Total-Count"));
            Assert.True(response.Headers.Contains("X-Page-Number"));
            Assert.True(response.Headers.Contains("X-Page-Size"));

            var totalCount = response.Headers.GetValues("X-Total-Count").FirstOrDefault();
            var pageNumber = response.Headers.GetValues("X-Page-Number").FirstOrDefault();
            var pageSize = response.Headers.GetValues("X-Page-Size").FirstOrDefault();

            Assert.Equal("3", totalCount);
            Assert.Equal("1", pageNumber);
            Assert.Equal("2", pageSize);

            var body = await response.Content.ReadAsStringAsync();
            var items = JsonSerializer.Deserialize<List<ProductResponse>>(body, _jsonOptions);

            Assert.NotNull(items);
            Assert.Equal(2, items!.Count);
            var names = items.Select(i => i.Name).ToList();
            Assert.Contains("Product A", names);
            Assert.Contains("Product B", names);
        }

        [Fact]
        public async Task GetById_ReturnsProduct_WhenProductExists()
        {
            var payload = new
            {
                Name = "Laptop",
                Price = 1500.00m,
                CategoryId = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdBody = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(createdBody, _jsonOptions);

            Assert.NotNull(createdProduct);

            var response = await _client.GetAsync($"/api/v1/products/{createdProduct!.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductResponse>(body, _jsonOptions);

            Assert.NotNull(product);
            Assert.Equal("Laptop", product!.Name);
            Assert.Equal(1, product.CategoryId);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var idInexistente = 9999;
            var response = await _client.GetAsync($"/api/v1/products/{idInexistente}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsProduct_WhenProductExists()
        {
            var payload = new
            {
                Name = "Tablet",
                Price = 500.00m,
                CategoryId = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdBody = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(createdBody, _jsonOptions);

            Assert.NotNull(createdProduct);

            var updatePayload = new
            {
                Id = 1,
                Name = "Updated Tablet",
                Price = 550.00m,
                CategoryId = 1
            };

            var updateContent = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");
            var updateResponse = await _client.PutAsync($"/api/v1/products/{createdProduct!.Id}", updateContent);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updatedBody = await updateResponse.Content.ReadAsStringAsync();
            var updatedProduct = JsonSerializer.Deserialize<ProductResponse>(updatedBody, _jsonOptions);

            Assert.NotNull(updatedProduct);
            Assert.Equal("Updated Tablet", updatedProduct!.Name);
            Assert.Equal(550.00m, updatedProduct.Price);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenIdIsDifferent()
        {
            var payload = new
            {
                Name = "Tablet",
                Price = 500.00m,
                CategoryId = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdBody = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(createdBody, _jsonOptions);

            Assert.NotNull(createdProduct);

            var updatePayload = new
            {
                Id = 999,
                Name = "Updated Tablet",
                Price = 550.00m,
                CategoryId = 1
            };

            var updateContent = new StringContent(JsonSerializer.Serialize(updatePayload), Encoding.UTF8, "application/json");
            var updateResponse = await _client.PutAsync($"/api/v1/products/{createdProduct!.Id}", updateContent);

            Assert.Equal(HttpStatusCode.BadRequest, updateResponse.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreated_WhenCategoryExists()
        {
            var payload = new
            {
                Name = "Smart TV",
                Price = 1999.90m,
                CategoryId = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<ProductResponse>(body, _jsonOptions);
            Assert.NotNull(product);
            Assert.Equal("Smart TV", product!.Name);
            Assert.Equal(1, product.CategoryId);
            Assert.True(product.Id > 0);
        }

        [Fact]
        public async Task CreateProduct_ReturnsBadRequest_WhenCategoryNotFound()
        {
            var payload = new
            {
                Name = "Produto inválido",
                Price = 10.0m,
                CategoryId = 999
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            Assert.Contains("Categoria", body, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductExists()
        {
            var payload = new
            {
                Name = "Smartphone",
                Price = 800.00m,
                CategoryId = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var createResponse = await _client.PostAsync("/api/v1/products", content);

            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

            var createdBody = await createResponse.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(createdBody, _jsonOptions);

            Assert.NotNull(createdProduct);

            var deleteResponse = await _client.DeleteAsync($"/api/v1/products/{createdProduct!.Id}");
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/api/v1/products/{createdProduct.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }
    }
}
