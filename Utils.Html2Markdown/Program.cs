using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Utils.Html2Markdown
{
    // TODO LINQ
    // TODO YIELD
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Converting");
            await ConvertFiles(@"C:\git\school-materials", "csharp", "*.php");
            await ConvertFiles(@"C:\git\school-materials", "xamarin", "*.php");
            await ConvertFiles(@"C:\git\school-materials", "zadani", "*.php");
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static string HTML2Markdown(string html)
        {
            var config = new ReverseMarkdown.Config
            {
                UnknownTags = Config.UnknownTagsOption.PassThrough,
                GithubFlavored = true,
                SmartHrefHandling = true // remove markdown output for links where appropriate
            };

            var converter = new ReverseMarkdown.Converter(config);
            string result = converter.Convert(html);

            return result;
        }


     

        static async Task ConvertFiles(string path, string folder, string search)
        {
            var fullPath = $"{path}\\{folder}\\article";
            
           // DirectoryInfo[] dirs = di.GetDirectories();

            Console.WriteLine();
            Console.WriteLine(folder);

            List<HeadItem> topHeadItems = await parseHead(fullPath);


            Console.WriteLine();
            foreach (var topHeadItem in topHeadItems)
            {
                Console.WriteLine(topHeadItem.Name);
                DirectoryInfo di = new DirectoryInfo(Path.Combine(fullPath, topHeadItem.Name));
                var files = di.GetFiles();

                List<HeadItem> headItems = await parseHead(Path.Combine( fullPath, topHeadItem.Name) );


                foreach (var fInfo in files)
                {
                    if (fInfo.Name.Equals("index.php")) continue;
                    
                    // find all files with name from head and create correct header

                    var matchingHeadItem = from headItem in headItems
                                               where headItem.NameWithoutExtension == Path.GetFileNameWithoutExtension(fInfo.Name)
                                               select headItem;

                    if (matchingHeadItem.FirstOrDefault() == null) continue;
                    Console.WriteLine(matchingHeadItem.FirstOrDefault().Name);

                    string date = DateTime.Now.ToString("yyyy-MM-dd");

                    var markdown = HTML2Markdown(await File.ReadAllTextAsync(fInfo.FullName));

                    string header = $@"---
layout: post
title: ""{ matchingHeadItem.FirstOrDefault().Title} ""
categories:
    -""{folder}""
    -""{topHeadItem.Name}""
author: ""David Malý""
--- 

";
                    Directory.CreateDirectory($"C:\\git\\school-materials-export\\{folder}\\{topHeadItem.Name}");
                    File.WriteAllText($"C:\\git\\school-materials-export\\{folder}\\{topHeadItem.Name}\\{date}-{Path.GetFileNameWithoutExtension(fInfo.Name)}" + ".md", header + markdown);
                }

            }
        }

        static async Task<List<HeadItem>> parseHead(string path)
        {
            List<HeadItem> headItems = new List<HeadItem>();
            var lines = await File.ReadAllLinesAsync(Path.Combine(path, "head.txt"));
            foreach(var line in lines)
            {
                var splited = line.Split(";");
                headItems.Add(new HeadItem
                {
                    Title = splited[0],
                    Name = splited[1]
                });
            }
            return headItems;
        }
    }
}