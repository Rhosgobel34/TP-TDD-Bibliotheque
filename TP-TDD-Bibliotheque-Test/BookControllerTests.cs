using System.Net;
using System.Net.Http.Json;
using TP_TDD_Bibliotheque.Models;

namespace TP_TDD_Bibliotheque_Test
{
    public class BookControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public BookControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateBook_ShouldReturn201Created()
        {
            Book book = new Book
            {
                ISBN = "0201896834",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };

            var response = await _client.PostAsJsonAsync("/book", book);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenBookIsNull()
        {
            Book book = null;
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenEditorDoesNotExist()
        {
            Book book = new Book
            {
                ISBN = "0201896834",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 999
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}