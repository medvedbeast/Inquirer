using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace InquirerAPI.Website.Infrastructure.TagHelpers
{

    [HtmlTargetElement("span", Attributes = _attribute)]
    public class VueValidationTagHelper : TagHelper
    {
        private const string _attribute = "vue-validation-for";

        [HtmlAttributeName(_attribute)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeNotBound]
        public IHtmlGenerator Generator { get; set; }

        public VueValidationTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var helper = new ValidationMessageTagHelper(Generator);
            helper.ViewContext = ViewContext;
            helper.For = For;
            helper.Init(context);

            await helper.ProcessAsync(context, output);

            string name = For.Name.Substring(For.Name.LastIndexOf('.') + 1).ToLower();

            output.Attributes.SetAttribute(":data-valmsg-for", $"`${{id}}.{ name }`");
        }
    }

}
