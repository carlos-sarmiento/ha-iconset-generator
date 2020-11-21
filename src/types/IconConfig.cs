using System.Collections.Generic;

namespace HassIconGenerator
{
    public class IconConfig
    {
        public IconConfig()
        {
            Iconsets = new Iconset[0];
            OutputPath = "js";
        }

        public string OutputPath { get; set; }

        public IEnumerable<Iconset> Iconsets { get; set; }
    }
}
