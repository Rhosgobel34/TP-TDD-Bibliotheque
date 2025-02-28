using System.Net;
using System.Net.Http.Json;
using System.Text;
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

        private static string GenerateValidISBN10()
        {
            Random random = new Random();
            int[] digits = new int[9];

            for (int i = 0; i < 9; i++)
            {
                digits[i] = random.Next(0, 10);
            }

            int total = 0;
            for (int i = 0; i < 9; i++)
            {
                total += digits[i] * (10 - i);
            }

            int remainder = total % 11;
            int checkDigit = (11 - remainder) % 11;

            StringBuilder isbn = new StringBuilder();
            foreach (var digit in digits)
            {
                isbn.Append(digit);
            }

            isbn.Append(checkDigit == 10 ? 'X' : checkDigit.ToString());

            return isbn.ToString();
        }

        private static string GenerateValidISBN13()
        {
            Random random = new Random();
            int[] isbn = new int[12];

            for (int i = 0; i < 12; i++)
            {
                isbn[i] = random.Next(0, 10);
            }

            int total = 0;
            for (int i = 0; i < 12; i++)
            {
                total += (i % 2 == 0) ? isbn[i] : isbn[i] * 3;
            }

            int remainder = total % 10;
            int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

            return string.Concat(isbn.Select(d => d.ToString())) + checkDigit;
        }

        [Fact]
        public async Task CreateBookWithValidIsbn13_ShouldReturn201Created()
        {
            Book book = new Book
            {
                ISBN = GenerateValidISBN13(),
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };

            var response = await _client.PostAsJsonAsync("/book", book);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateBookWithValidIsbn10_ShouldReturn201Created()
        {
            Book book = new Book
            {
                ISBN = GenerateValidISBN10(),
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

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenISBNIsNullOrEmpty()
        {
            Book book = new Book
            {
                ISBN = "",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenTitleIsNullOrEmpty()
        {
            Book book = new Book
            {
                ISBN = "0201896834",
                Title = "",
                Available = true,
                EditorId = 1
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenISBNAlreadyExists()
        {
            Book book = new Book
            {
                ISBN = "0201896834",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };
            await _client.PostAsJsonAsync("/book", book);
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenEditorIdIsZero()
        {
            Book book = new Book
            {
                ISBN = "0201896834",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 0
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenIsbn10IsInvalid()
        {
            Book book = new Book
            {
                ISBN = "01234567895",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ShouldReturn400BadRequest_WhenIsbn13IsInvalid()
        {
            Book book = new Book
            {
                ISBN = "9781234567895",
                Title = "The Art of Computer Programming",
                Available = true,
                EditorId = 1
            };
            var response = await _client.PostAsJsonAsync("/book", book);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}