using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentSupplyHandler.Tests.CRUDClients
{
    abstract class CRUDClient<T>
    {
        private readonly HttpClient _client;
        private readonly TestWebApplicationFactory<Startup> _factory;
        protected abstract string relativeUri { get; }// = "/api/equipment";
        Uri absoluteUri = null;
        public CRUDClient(TestWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            absoluteUri = new Uri(_client.BaseAddress, relativeUri);
        }

        public virtual async Task CreateAsync(T element)
        {
            var response = await _client.PostAsJsonAsync(absoluteUri, element);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateAsync(T element)
        {
            var response = await _client.PutAsJsonAsync(absoluteUri, element);
            response.EnsureSuccessStatusCode();
        }
        public async Task<T> GetFirstElement()
        {
            var response = await _client.GetAsync(absoluteUri);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadAsAsync<IEnumerable<T>>()).First();
        }

        public async Task<T> GetElementById(string id)
        {
            var response = await _client.GetAsync(absoluteUri.AbsoluteUri + "/" + id);
            response.EnsureSuccessStatusCode();
            return (await response.Content.ReadAsAsync<T>());
        }
        public async Task DeleteElement(string id)
        {
            var response = await _client.DeleteAsync(absoluteUri.AbsoluteUri + "/" + id);
            response.EnsureSuccessStatusCode();
        }
    }
}
