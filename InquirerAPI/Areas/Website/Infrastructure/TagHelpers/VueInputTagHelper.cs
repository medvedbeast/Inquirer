using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InquirerAPI.Website.Infrastructure.TagHelpers
{

    [HtmlTargetElement("input", Attributes = _attribute)]
    [HtmlTargetElement("select", Attributes = _attribute)]
    public class VueInputTagHelper : TagHelper
    {
        private const string _attribute = "vue-for";

        [HtmlAttributeName(_attribute)]
        public ModelExpression For { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeNotBound]
        public IHtmlGenerator Generator { get; set; }

        public VueInputTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var helper = new InputTagHelper(Generator);
            helper.ViewContext = ViewContext;
            helper.For = For;
            helper.Init(context);

            helper.Process(context, output);

            string name = For.Name.Substring(For.Name.LastIndexOf('.') + 1).ToLower();

            output.Attributes.SetAttribute(":id", $"`${{id}}.{ name }`");
            output.Attributes.SetAttribute(":name", $"`${{id}}.{ name }`");
        }
    }

}
