using Inquirer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Inquirer.Infrastructure
{
    public static class Extensions
    {

        public static IHtmlContent Component(this IHtmlHelper<dynamic> html, string component)
        {
            var template = html.Partial(component, new VueComponent { Section = "template" });
            var script = html.Partial(component, new VueComponent { Section = "script" });
            var result = html.Raw($@"
                <script id='{ component }-template' type='text/x-template'>
                    {template.Write()}
                </script>
                {script.Write()}
                ");
            return result;
        }

        public static string Write(this IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        public static string ToUnderscoreCase(this string s)
        {
            return string.Concat(s.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
