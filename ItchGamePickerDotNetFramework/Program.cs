using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
namespace ItchGamePicker
{
    class Program
    {
        static int Main(string[] args)
        {
            var random = new Random();

            var json = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ItchGamePicker.games.json")).ReadToEnd();
            var games = SimpleJSON.JSONNode.Parse(json);

            var number = games["Games"].Count;

            Console.WriteLine("This program will randomly select 1 of the over 1,700 items available in the itch.io Bundle for Racial Justice and Equality.");
            Console.WriteLine("You can use the Enter key to have the program choose again, or Esc to quit.");
            Console.WriteLine("Press any key to begin...\n");
            Console.ReadKey();

            while (true)
            {
                var index = random.Next(games["Games"].Count);
                Console.Write("You should play ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{HttpUtility.HtmlDecode(games["Games"][index]["Name"])}");
                Console.ResetColor();
                OpenURL(games["Games"][index]["URL"]);
                Console.WriteLine("Press Enter to try again, or Escape to quit.\n");

                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        return 0;
                    }
                }
            }
        }

        public static void OpenURL(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
