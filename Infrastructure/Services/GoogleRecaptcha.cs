using Application.Repositories;
using Application.ViewModels.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
  public class GoogleRecaptcha : IGoogleRecaptcha
  {
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;

    public GoogleRecaptcha(IConfiguration configuration, IHttpContextAccessor contextAccessor)
    {
      _configuration = configuration;
      _contextAccessor = contextAccessor;
    }

    public async Task<bool> IsSatisfy()
    {
      var secretKey = _configuration.GetSection("GoogleRecaptcha")["SecretKey"];

      var response = _contextAccessor.HttpContext.Request.Form["g-recaptcha-response"];

      var http = new HttpClient();

      var result = await http.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}", null);

      if (result.IsSuccessStatusCode)
      {
        var googleResponse = JsonConvert.DeserializeObject<RecapchaResponseDTO>(await result.Content.ReadAsStringAsync());

        return googleResponse.Success;
      }

      return false;
    }
  }
}
