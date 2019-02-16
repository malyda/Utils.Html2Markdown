using ReverseMarkdown;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Utils.Html2Markdown
{
    // TODO clean and refactor yaml parser
    class Parsers
    {
        public static string HTML2Markdown(string html)
        {
            var config = new ReverseMarkdown.Config
            {
                UnknownTags = Config.UnknownTagsOption.Drop,
                GithubFlavored = true,
                SmartHrefHandling = true // remove markdown output for links where appropriate
            };

            var converter = new ReverseMarkdown.Converter(config);
            string result = converter.Convert(html);

            // trim all tags
            string noHTML = Regex.Replace(result, "<.*?>", String.Empty);

            return noHTML;
        }


        public static async Task<List<T>> ParseHead<T>(string path) where T : HeadItem, new()
        {
            List<T> headItems = new List<T>();
            var lines = await File.ReadAllLinesAsync(Path.Combine(path, "head.txt"));
            foreach (var line in lines)
            {
                var splited = line.Split(";");
                headItems.Add(new T
                {
                    Title = splited[0],
                    Name = splited[1]
                });
            }
            return headItems;
        }


        public static void GenerateYamlFromCategories(YamlMappingNode rootMappingNode, List<Category> categories, string material)
        {
            var seq = new YamlSequenceNode();
            foreach (var category in categories)
            {
                var categoriesSeq = new YamlSequenceNode();

                foreach (var page in category.Pages)
                {
                    var prop = new YamlMappingNode();
                    prop.Add("title", page.Title);
                    prop.Add("url", $"{Helpers.CurrentDateURL}/{Path.GetFileNameWithoutExtension(page.Name)}" );
                    categoriesSeq.Add(prop);
                }
               // var seq2 = new YamlSequenceNode();

                var prop2 = new YamlMappingNode("title", category.Title);
                var prop3 = new YamlMappingNode("pages", categoriesSeq);
                //seq.Add(prop2);

                seq.Add(prop2);
                seq.Add(prop3);
               // seq.Add(seq);
                // categoriesSeq.Add(seq);
            }
            rootMappingNode.Add("categories", seq);

        //    File.WriteAllText($"C:\\git\\school-materials-export\\{material}.yaml", yaml);

        }
    }
}
