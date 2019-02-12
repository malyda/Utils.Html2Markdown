using Html2Markdown;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Utils.Html2Markdown
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            await ConvertFiles(@"C:\git\school-materials", "csharp", "*.php");
            await ConvertFiles(@"C:\git\school-materials", "xamarin", "*.php");
            await ConvertFiles(@"C:\git\school-materials", "zadani", "*.php");
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static string HTML2Markdown(string html)
        { 
            var converter = new Converter();
            return converter.Convert(html);
        }
        
        static async Task ConvertFiles(string path,string folder, string search)
        {
            var dirs = Directory.GetFiles($"{path}\\{folder}", search, SearchOption.AllDirectories);
            Console.WriteLine(folder);
            Console.WriteLine();
            foreach (var dirInfo in dirs)
            {
                Console.WriteLine(dirInfo);
                System.IO.FileInfo fi = new System.IO.FileInfo(dirInfo);
                if (fi.Name.Equals("index.php")) continue;

                string parent = Directory.GetParent(dirInfo).Name;

                var markdown = HTML2Markdown(await File.ReadAllTextAsync(fi.FullName));

                Directory.CreateDirectory($"C:\\git\\school-materials-export\\{folder}\\{parent}");
                File.WriteAllText($"C:\\git\\school-materials-export\\{folder}\\{parent}\\{Path.GetFileNameWithoutExtension(fi.Name)}" + ".md", markdown);

            }
        }
    }
}