#pragma checksum "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "de9829eeb05c1756ab9b62be15045aef251f35fd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Pets_Details), @"mvc.1.0.view", @"/Views/Pets/Details.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Models.Pets;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Models.SearchPosts;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Models.Home;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Models.Owners;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Services.Pets.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Services.Owners;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Infrastructure;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\_ViewImports.cshtml"
using PetFinder.Services.SearchPosts.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de9829eeb05c1756ab9b62be15045aef251f35fd", @"/Views/Pets/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"36d62f3c693ebc48f89baa10772700a3e49f3044", @"/Views/_ViewImports.cshtml")]
    public class Views_Pets_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PetDetailsServiceModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString(" btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Pets", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
  
    ViewData["Title"] = Model.Name;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"container-fluid mb-4\">\r\n    <div class=\"row\">\r\n        <div class=\"col-md-12\">\r\n            <img");
            BeginWriteAttribute("src", " src=\"", 189, "\"", 210, 1);
#nullable restore
#line 11 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
WriteAttributeValue("", 195, Model.ImageUrl, 195, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"img-fluid w-100\"");
            BeginWriteAttribute("alt", " alt=\"", 235, "\"", 255, 1);
#nullable restore
#line 11 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
WriteAttributeValue("", 241, Model.Species, 241, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div class=\"container\">\r\n    <h2 class=\"text-center\">");
#nullable restore
#line 17 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
                       Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    <h3 class=\"text-center\">");
#nullable restore
#line 18 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
                       Write(Model.Species);

#line default
#line hidden
#nullable disable
            WriteLiteral(" - ");
#nullable restore
#line 18 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
                                        Write(Model.Size);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n    <div class=\"text-center mt-5\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "de9829eeb05c1756ab9b62be15045aef251f35fd7849", async() => {
                WriteLiteral("Edit");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 20 "D:\SoftUni-Work\C# ASP NET Core\Final Project\PetFinder\PetFinder\Views\Pets\Details.cshtml"
                                                                              WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PetDetailsServiceModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
