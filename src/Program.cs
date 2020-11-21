using System;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Xml.Linq;
using DotLiquid;

namespace HassIconGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing Configuration File. Exiting");
                return;
            }

            var configPath = args[0];

            if (!File.Exists(configPath))
            {
                Console.WriteLine($"Config file: '{configPath}' does not exists");
                return;
            }

            var multiPathTemplate = Template.Parse(File.ReadAllText("templates/template-multipath.liquid"));
            var template = Template.Parse(File.ReadAllText("templates/template.liquid"));

            var config = JsonSerializer.Deserialize<IconConfig>(File.ReadAllText(configPath), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            if (config == null)
            {
                Console.WriteLine("Invalid Config file");
                return;
            }

            Console.WriteLine("Read Config File");

            var iconsets = config.Iconsets.Select(ProcessIconset).FilterNulls().ToList();

            if (!Directory.Exists(config.OutputPath))
            {
                Directory.CreateDirectory(config.OutputPath);
            }

            foreach (var iconset in iconsets)
            {
                var currentTemplate = iconset.Multipath ? multiPathTemplate : template;
                var renderedFile = currentTemplate.Render(Hash.FromAnonymousObject(new { icons = iconset.Icons.OrderBy(c => c.Name), name = iconset.Name }));

                File.WriteAllText($"{config.OutputPath}/{iconset.Name}.js", renderedFile);
            }
        }

        private static Iconset? ProcessIconset(Iconset iconset)
        {
            Console.WriteLine($"Processing {iconset.Name}");

            var path = $"{iconset.Directory}";
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"{iconset.Name} - {path} not found");
                return iconset;
            }

            var files = Directory.GetFiles(path, "*.svg");
            var icons = iconset.Multipath ? files.Select(GetMultiPathSvgIcon) : files.Select(GetSinglePathSvgIcon);

            iconset.Icons = icons.ToList().FilterNulls();

            return iconset;
        }

        private static SvgIcon? GetMultiPathSvgIcon(string filepath)
        {
            try
            {
                var data = File.ReadAllText(filepath);
                var xml = XDocument.Load(filepath);

                var filename = Path.GetFileNameWithoutExtension(filepath);
                var viewBox = xml.Root?.Attribute("viewBox")?.Value;
                var elements = xml.Root?.Elements();

                var paths = elements?.Where(c => c.Name.LocalName == "path").Select(c =>
                {
                    var path = c.Attribute("d")?.Value;
                    if (path == null)
                    {
                        return null;
                    }

                    return new SvgPath()
                    {
                        Path = path,
                        ClassName = c.Attribute("class")?.Value ?? ""
                    };
                }).FilterNulls().ToArray();

                var styles = elements?
                    .FirstOrDefault(c => c.Name.LocalName == "defs")?.Elements()
                    .FirstOrDefault(c => c.Name.LocalName == "style")?.Value ?? "";

                if (paths == null || viewBox == null || styles == null)
                {
                    return null;
                }

                return new SvgIcon
                {
                    Name = filename,
                    Paths = paths,
                    Viewbox = viewBox,
                    Styles = styles
                };
            }
            catch
            {
                return null;
            }
        }

        private static SvgIcon? GetSinglePathSvgIcon(string filepath)
        {
            try
            {
                var data = File.ReadAllText(filepath);
                var xml = XDocument.Load(filepath);

                var filename = Path.GetFileNameWithoutExtension(filepath);

                var viewBox = xml.Root?.Attribute("viewBox")?.Value;
                var path = xml.Root?.Elements().FirstOrDefault(c => c.Name.LocalName == "path")?.Attribute("d")?.Value;

                if (viewBox == null || path == null)
                {
                    return null;
                }

                return new SvgIcon
                {
                    Name = filename,
                    Paths = new[] { new SvgPath() { Path = path } },
                    Viewbox = viewBox
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
