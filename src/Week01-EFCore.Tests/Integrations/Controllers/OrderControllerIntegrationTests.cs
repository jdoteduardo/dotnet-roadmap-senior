using ECommerce.OrderManagement.API.Tests.Integrations.Factories;
using ECommerce.OrderManagement.API.Tests.Integrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.OrderManagement.API.Tests.Integrations.Controllers
{
    public class OrderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public OrderControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            factory.ResetDatabase();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreated_WhenRequestIsValid()
        {
            // criar produto
            var productPayload = new { Name = "Item Teste", Price = 100.00m, CategoryId = 1 };
            var prodContent = new StringContent(JsonSerializer.Serialize(productPayload), Encoding.UTF8, "application/json");
            var prodRes = await _client.PostAsync("/api/v1/products", prodContent);
            Assert.Equal(HttpStatusCode.Created, prodRes.StatusCode);
            var prodBody = await prodRes.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(prodBody, _jsonOptions);
            Assert.NotNull(createdProduct);

            // criar cliente
            var customerPayload = new { Name = "Cliente Teste", Email = "cliente@test.com" };
            var custContent = new StringContent(JsonSerializer.Serialize(customerPayload), Encoding.UTF8, "application/json");
            var custRes = await _client.PostAsync("/api/v1/customers", custContent);
            Assert.Equal(HttpStatusCode.Created, custRes.StatusCode);
            var custBody = await custRes.Content.ReadAsStringAsync();
            var createdCustomer = JsonSerializer.Deserialize<CustomerResponse>(custBody, _jsonOptions);
            Assert.NotNull(createdCustomer);

            // criar endereço
            var addressPayload = new
            {
                Street = "Rua Teste, 1",
                City = "Cidade",
                State = "ST",
                ZipCode = "00000-000",
                Country = "BR"
            };
            var addrContent = new StringContent(JsonSerializer.Serialize(addressPayload), Encoding.UTF8, "application/json");
            var addrRes = await _client.PostAsync("/api/v1/addresses", addrContent);
            Assert.Equal(HttpStatusCode.Created, addrRes.StatusCode);
            var addrBody = await addrRes.Content.ReadAsStringAsync();
            var createdAddress = JsonSerializer.Deserialize<AddressResponse>(addrBody, _jsonOptions);
            Assert.NotNull(createdAddress);

            // criar pedido
            var orderPayload = new
            {
                Items = new[] { new { ProductId = createdProduct!.Id, Quantity = 2 } },
                CustomerId = createdCustomer!.Id,
                AddressId = createdAddress!.Id
            };

            var orderContent = new StringContent(JsonSerializer.Serialize(orderPayload), Encoding.UTF8, "application/json");
            var orderRes = await _client.PostAsync("/api/v1/orders", orderContent);
            Assert.Equal(HttpStatusCode.Created, orderRes.StatusCode);

            var orderBody = await orderRes.Content.ReadAsStringAsync();
            var createdOrder = JsonSerializer.Deserialize<OrderResponse>(orderBody, _jsonOptions);
            Assert.NotNull(createdOrder);
            Assert.True(createdOrder!.Id > 0);
            Assert.Equal(createdCustomer.Id, createdOrder.CustomerId);
            Assert.Equal(createdAddress.Id, createdOrder.AddressId);
            Assert.Equal(100.00m * 2, createdOrder.SubTotal);
        }

        [Fact]
        public async Task GetById_ReturnsOrder_WhenOrderExists()
        {
            // criar produto
            var productPayload = new { Name = "Item GET", Price = 50.00m, CategoryId = 1 };
            var prodContent = new StringContent(JsonSerializer.Serialize(productPayload), Encoding.UTF8, "application/json");
            var prodRes = await _client.PostAsync("/api/v1/products", prodContent);
            Assert.Equal(HttpStatusCode.Created, prodRes.StatusCode);
            var prodBody = await prodRes.Content.ReadAsStringAsync();
            var createdProduct = JsonSerializer.Deserialize<ProductResponse>(prodBody, _jsonOptions);
            Assert.NotNull(createdProduct);

            // criar cliente
            var customerPayload = new { Name = "Cliente GET", Email = "get@test.com" };
            var custContent = new StringContent(JsonSerializer.Serialize(customerPayload), Encoding.UTF8, "application/json");
            var custRes = await _client.PostAsync("/api/v1/customers", custContent);
            Assert.Equal(HttpStatusCode.Created, custRes.StatusCode);
            var custBody = await custRes.Content.ReadAsStringAsync();
            var createdCustomer = JsonSerializer.Deserialize<CustomerResponse>(custBody, _jsonOptions);
            Assert.NotNull(createdCustomer);

            // criar endereço
            var addressPayload = new
            {
                Street = "Rua Get, 2",
                City = "Cidade",
                State = "ST",
                ZipCode = "11111-111",
                Country = "BR"
            };
            var addrContent = new StringContent(JsonSerializer.Serialize(addressPayload), Encoding.UTF8, "application/json");
            var addrRes = await _client.PostAsync("/api/v1/addresses", addrContent);
            Assert.Equal(HttpStatusCode.Created, addrRes.StatusCode);
            var addrBody = await addrRes.Content.ReadAsStringAsync();
            var createdAddress = JsonSerializer.Deserialize<AddressResponse>(addrBody, _jsonOptions);
            Assert.NotNull(createdAddress);

            // criar pedido
            var orderPayload = new
            {
                Items = new[] { new { ProductId = createdProduct!.Id, Quantity = 1 } },
                CustomerId = createdCustomer!.Id,
                AddressId = createdAddress!.Id
            };

            var orderContent = new StringContent(JsonSerializer.Serialize(orderPayload), Encoding.UTF8, "application/json");
            var orderRes = await _client.PostAsync("/api/v1/orders", orderContent);
            Assert.Equal(HttpStatusCode.Created, orderRes.StatusCode);
            var orderBody = await orderRes.Content.ReadAsStringAsync();
            var createdOrder = JsonSerializer.Deserialize<OrderResponse>(orderBody, _jsonOptions);
            Assert.NotNull(createdOrder);

            // consulta por id
            var getRes = await _client.GetAsync($"/api/v1/orders/{createdOrder!.Id}");
            Assert.Equal(HttpStatusCode.OK, getRes.StatusCode);
            var getBody = await getRes.Content.ReadAsStringAsync();
            var fetchedOrder = JsonSerializer.Deserialize<OrderResponse>(getBody, _jsonOptions);
            Assert.NotNull(fetchedOrder);
            Assert.Equal(createdOrder.Id, fetchedOrder!.Id);
            Assert.Equal(createdOrder.SubTotal, fetchedOrder.SubTotal);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            var response = await _client.GetAsync($"/api/v1/orders/9999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}