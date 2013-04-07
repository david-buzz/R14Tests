using System.Windows.Media;

namespace Redbolts.TestCancelMethod.Composition
{
    public interface IRibbonItemMetaData
    {
        int PanelIndex { get; }
        string TabName { get; }
        string PanelName { get; }

        string LongDescription { get; }
        string Name { get; }
        string Tooltip { get; }
        string TooltipImage { get; }
       
        bool Visible { get; }
        bool Enabled { get; }
    }
}
