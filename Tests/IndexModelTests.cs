using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiTesterWeb.Pages;
using Xunit;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiTesterWeb.Tests
{
    public class IndexModelTests
    {
        [Fact]
        public async Task OnPostAsync_ReturnsPageResult()
        {
            // Arrange
            var model = new IndexModel
            {
                Url = "https://httpbin.org/get",
                Method = "GET"
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.Contains("Status:", model.Response);
        }
    }
}
