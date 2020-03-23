using WebWindows.Blazor;

namespace BlazorConfTool.Desktop
{
    class Program
    {
        static void Main(string[] args)
        {
            ComponentsDesktop.Run<Startup>("ConfTool", "wwwroot/index.html");
        }
    }
}
