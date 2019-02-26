using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using YamlDotNet.RepresentationModel;

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




        static async Task<List<Category>> GetCategories(string fullPath)
        {
            var categories = await Parsers.ParseHead<Category>(fullPath);
            for (int i = 0; i < categories.Count(); i++)
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(fullPath, categories[i].Name));
                var files = di.GetFiles();

                categories[i].Pages =  await Parsers.ParseHead<Page>(Path.Combine(fullPath, categories[i].Name));
            }
            return categories;
        }


        static async Task ConvertFiles(string path, string folder, string search)
        {
            var fullPath = $"{path}\\{folder}\\article";

            Console.WriteLine();
            Console.WriteLine(folder);
            
            List<Category> categories = await GetCategories(fullPath);


            const string initialContent = "---\nversion: 1\n...";

            var sr = new StringReader(initialContent);
            var stream = new YamlStream();
            stream.Load(sr);

            var rootMappingNode = (YamlMappingNode) stream.Documents[0].RootNode;

            Parsers.GenerateYamlFromCategories(rootMappingNode, categories, folder);

            Directory.CreateDirectory($"C:\\git\\export\\_data\\");
            using (TextWriter writer = File.CreateText($"C:\\git\\export\\_data\\{folder}.yaml"))
                stream.Save(writer, false);


            Console.WriteLine();
            foreach (var categorory in categories)
            {
                Console.WriteLine(categorory.Name);
                DirectoryInfo di = new DirectoryInfo(Path.Combine(fullPath, categorory.Name));
                var files = di.GetFiles();

                foreach (var fInfo in files)
                {
                    if (fInfo.Name.Equals("index.php")) continue;
                    
                    // find all files with name from head and create correct header

                    var matchingHeadItem = from headItem in categorory.Pages
                                               where headItem.NameWithoutExtension == Path.GetFileNameWithoutExtension(fInfo.Name)
                                               select headItem;

                    if (matchingHeadItem.FirstOrDefault() == null) continue;
                    Console.WriteLine(matchingHeadItem.FirstOrDefault().Name);
                    var matchingHeadItemIIndex = categorory.Pages.IndexOf(matchingHeadItem.FirstOrDefault());

                    

                    var markdown = Parsers.HTML2Markdown(await File.ReadAllTextAsync(fInfo.FullName));

                    string header = $@"---
layout: post
title: ""{ matchingHeadItem.FirstOrDefault().Title} ""
categories:
    - ""{folder}""
    - ""{categorory.Name}""
sort_index: {matchingHeadItemIIndex}
author: ""David Malý""
--- 

";
                    Directory.CreateDirectory($"C:\\git\\export\\school-materials-export\\{folder}\\{categorory.Name}");
                    File.WriteAllText($"C:\\git\\export\\school-materials-export\\{folder}\\{categorory.Name}\\{DateHelpers.DateFileName}-{Path.GetFileNameWithoutExtension(fInfo.Name)}" + ".md", header + markdown);
                }

            }
        }




       
    }
}