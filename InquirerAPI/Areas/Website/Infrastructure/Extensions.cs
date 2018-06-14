using InquirerAPI.Website.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.Encodings.Web;

namespace InquirerAPI.Website.Infrastructure
{
    public static class Extensions
    {
        public static IHtmlContent Component(this IHtmlHelper<dynamic> htmlHelper, VueComponent component)
        {
            IHtmlContent template = htmlHelper.Partial(component.Name, Convert.ChangeType(component.Template, component.Type));
            var script = htmlHelper.Partial(component.Name, Convert.ChangeType(component.Body, component.Type));
            var html = htmlHelper.Raw($@"<script id='{ component.Name }-template' type='text/x-template'>{template.Write()}</script>{script.Write()}");
            return html;
        }

        public static string Write(this IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
