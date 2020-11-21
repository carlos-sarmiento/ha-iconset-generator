using System.Collections.Generic;
using DotLiquid;

namespace HassIconGenerator
{
    [LiquidType("Paths", "Viewbox", "Name")]
    public class SvgIcon
    {
        public SvgIcon()
        {
            Paths = new SvgPath[0];
            Viewbox = "";
            Name = "";
            Styles = "";
        }

        public IEnumerable<SvgPath> Paths { get; set; }
        public string Viewbox { get; set; }
        public string Name { get; set; }
        public string Styles { get; set; }
    }
}
