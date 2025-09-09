using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ApiTesterWeb.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public string Url { get; set; } = "";
        [BindProperty] public string Method { get; set; } = "GET";
        [BindProperty] public string Headers { get; set; } = "";
        [BindProperty] public string Body { get; set; } = "";
        [BindProperty] public string Token { get; set; } = "";

        public string Response { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                using var client = new HttpClient();
                var request = new HttpRequestMessage(new HttpMethod(Method), Url);

                // 处理 Headers
                if (!string.IsNullOrEmpty(Headers))
                {
                    var headerLines = Headers.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in headerLines)
                    {
                        var parts = line.Split(':', 2);
                        if (parts.Length == 2)
                        {
                            request.Headers.TryAddWithoutValidation(parts[0].Trim(), parts[1].Trim());
                        }
                    }
                }

                if (!string.IsNullOrEmpty(Token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                }

                // Body
                if (Method == "POST" || Method == "PUT")
                {
                    request.Content = new StringContent(Body ?? "", Encoding.UTF8, "application/json");
                }

                // 发送请求
                var response = await client.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();

                // 格式化 JSON
                if (response.Content.Headers.ContentType?.MediaType == "application/json")
                {
                    try
                    {
                        var parsed = JToken.Parse(responseText);
                        responseText = parsed.ToString(Newtonsoft.Json.Formatting.Indented);
                    }
                    catch { }
                }

                Response = $"Status: {(int)response.StatusCode} {response.ReasonPhrase}\n\n{responseText}";
            }
            catch (Exception ex)
            {
                Response = $"Error: {ex.Message}";
            }

            return Page();
        }
    }
}
