using System.Xml;
using System.Xml.Linq;

namespace ApiGatewayYarp.Middlewares
{
    public class FilterRequestMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var request = context.Request;
            Console.WriteLine(request.Path.ToString());

            if(request.Method == "GET")
            {
                var tempArray = request.Path.ToString().Split('/');
                int id;
                int.TryParse(tempArray[tempArray.Length - 1], out id);

               
                if(id == 0)
                {
                    // if the request was for getting all the students
                    await next(context);
                    return;
                }


                XDocument doc = XDocument.Load("./debardedStudents.xml");

                foreach (XElement idElement in doc.Descendants("Id"))
                {
                    Console.WriteLine(idElement.Value);
                    var debardedId = int.Parse(idElement.Value.ToString());

                    if (id == debardedId)
                    {
                        await context.Response.WriteAsync($"Can not give Details of student with id {id}");
                        return;
                    }
                }
            }

            await next(context);
        }
    }

    public static class CustomMiddlewareExtensionMethod
    {
        public static IApplicationBuilder UseFilterRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FilterRequestMiddleware>();
        }
    }
}
