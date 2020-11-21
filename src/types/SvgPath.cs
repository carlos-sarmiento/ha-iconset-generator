using DotLiquid;

namespace HassIconGenerator
{
    [LiquidType("Path", "ClassName")]
    public class SvgPath
    {
        public SvgPath()
        {
            Path = "";
            ClassName = "";
        }

        public string Path { get; set; }
        public string ClassName { get; set; }
    }
}
