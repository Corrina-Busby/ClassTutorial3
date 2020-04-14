using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Gallery3Selfhost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up server configuration
            Uri _baseAddress = new Uri("http://localhost:60064/");
            HttpSelfHostConfiguration config = new HttpSelfHostConfiguration(_baseAddress);
            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
            );
            //return JSON as default format
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
            //new MediaTypeHeaderValue("text/html"));
            // Create server
            HttpSelfHostServer server = new HttpSelfHostServer(config);
            // Start listening
            server.OpenAsync().Wait();
            Console.WriteLine("Gallery Web-API Self hosted on " + _baseAddress);
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
            server.CloseAsync().Wait();

        }
    }
}
