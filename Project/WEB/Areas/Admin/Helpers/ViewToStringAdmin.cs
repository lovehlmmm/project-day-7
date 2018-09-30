using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WEB.Areas.Admin.Helpers
{
    public class ViewToStringAdmin:ControllerBase
    {

        protected override void ExecuteCore() { }
        public static string RenderViewToString(string controllerName, string viewName, string areaName, object viewData, RequestContext rctx)
        {
            try
            {
                using (var writer = new StringWriter())
                {
                    var routeData = new RouteData();
                    routeData.Values.Add("controller", controllerName);
                    routeData.Values.Add("Area", areaName);
                    routeData.DataTokens["area"] = areaName;

                    var fakeControllerContext = new ControllerContext(rctx, new ViewToStringAdmin());
                    //new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                    fakeControllerContext.RouteData = routeData;

                    var razorViewEngine = new RazorViewEngine();

                    var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                    var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);

                    razorViewResult.View.Render(viewContext, writer);
                    return writer.GetStringBuilder().ToString();
                    //use example
                    //String renderedHTML = RenderViewToString("Email", "MyHTMLView", myModel );
                    //where file MyHTMLView.cstml is stored in Views/Email/MyHTMLView.cshtml. Email is a fake controller name.
                }
            }
            catch (Exception ex)
            {
                //do your exception handling here
            }
            return string.Empty;
        }
    }
}