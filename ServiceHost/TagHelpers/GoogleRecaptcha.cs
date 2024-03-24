using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace ServiceHost.TagHelpers
{
  [HtmlTargetElement("google-recaptcha")]
  public class GoogleRecaptcha : TagHelper
  {
    private readonly IConfiguration _configuration;

    public GoogleRecaptcha(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      var siteKey = _configuration.GetSection("GoogleRecaptcha")["SiteKey"];
      output.TagName = "div";
      output.AddClass("g-recaptcha", HtmlEncoder.Default);
      output.Attributes.Add("data-sitekey", siteKey);
      base.Process(context, output);
    }
  }
}
