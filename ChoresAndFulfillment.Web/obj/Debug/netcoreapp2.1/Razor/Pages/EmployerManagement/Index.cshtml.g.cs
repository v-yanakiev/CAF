#pragma checksum "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\EmployerManagement\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8fc9cf7a01b5e24903d9c878a09eb3858e2bf1de"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Pages_EmployerManagement_Index), @"mvc.1.0.razor-page", @"/Pages/EmployerManagement/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure.RazorPageAttribute(@"/Pages/EmployerManagement/Index.cshtml", typeof(AspNetCore.Pages_EmployerManagement_Index), null)]
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
#line 1 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\_ViewImports.cshtml"
using ChoresAndFulfillment;

#line default
#line hidden
#line 3 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\_ViewImports.cshtml"
using ChoresAndFulfillment.Data;

#line default
#line hidden
#line 4 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\_ViewImports.cshtml"
using ChoresAndFulfillment.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8fc9cf7a01b5e24903d9c878a09eb3858e2bf1de", @"/Pages/EmployerManagement/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"412a7e7f43634818bd7caabe04b7e7da02a7f454", @"/Pages/_ViewImports.cshtml")]
    public class Pages_EmployerManagement_Index : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\EmployerManagement\Index.cshtml"
  
    ViewData["Title"] = "Employer view";
    Layout = "~/Pages/Shared/_Layout.cshtml";

#line default
#line hidden
            BeginContext(162, 11, true);
            WriteLiteral("<h3>Hello, ");
            EndContext();
            BeginContext(174, 30, false);
#line 7 "C:\Users\vasko\source\repos\CAF-master\ChoresAndFulfillment.Web\Pages\EmployerManagement\Index.cshtml"
      Write(HttpContext.User.Identity.Name);

#line default
#line hidden
            EndContext();
            BeginContext(204, 227, true);
            WriteLiteral(". Here is what you can do from here:</h3>\n<ul>\n    <li>\n        <a href=\"/CreateJob/Index\">Publish a new job</a>\n    </li>\n    <li>\n        <a href=\"/YourActiveJobs/Index\">Look through all your active jobs</a>\n    </li>\n</ul>\n\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ChoresAndFulfillment.Pages.EmployerManagement.IndexModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ChoresAndFulfillment.Pages.EmployerManagement.IndexModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<ChoresAndFulfillment.Pages.EmployerManagement.IndexModel>)PageContext?.ViewData;
        public ChoresAndFulfillment.Pages.EmployerManagement.IndexModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591
