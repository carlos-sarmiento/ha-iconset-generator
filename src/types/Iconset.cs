using System.Collections.Generic;

namespace HassIconGenerator
{
    public class Iconset
    {
        public Iconset()
        {
            Icons = new SvgIcon[0];
            Multipath = false;
            Name = "";
            Directory = "";
        }

        public string Name { get; set; }

        public bool Multipath { get; set; }

        public string Directory { get; set; }

        public IEnumerable<SvgIcon> Icons { get; set; }
    }
}
