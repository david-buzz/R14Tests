using System.Linq;
using Autodesk.Revit.UI;

namespace Redbolts.TestCancelMethod.Extensions
{
    public static class RibbonExtensions
    {
        public static void AddRibbonTab(this UIControlledApplication uiCa, string tabName)
        {
            if (!HasTab(uiCa, tabName))
            {
                uiCa.CreateRibbonTab(tabName);
            }
        }

        public static RibbonPanel RibbonPanel(this UIControlledApplication uiCa, Tab tab, string panelName)
        {
            if (WriteEmptyComment(uiCa, panelName, "Panel name is empty")) return null;
            foreach (var panel in uiCa.GetRibbonPanels(tab).Where(panel => panel.Name == panelName))
            {
                return panel;
            }
            return uiCa.CreateRibbonPanel(tab, panelName);
        }

        public static RibbonPanel RibbonPanel(this UIControlledApplication uiCa, string tabName, string panelName)
        {
            if (WriteEmptyComment(uiCa, tabName, "Tab name is empty") ||
                WriteEmptyComment(uiCa, panelName, "Panel name is empty")) return null;
            // if no tab create it
            if (tabName.ToLower() == "addins") return RibbonPanel(uiCa, Tab.AddIns, panelName);
            if (tabName.ToLower() == "analyze") return RibbonPanel(uiCa, Tab.Analyze, panelName);
            AddRibbonTab(uiCa, tabName);
            foreach (var panel in uiCa.GetRibbonPanels(tabName).Where(panel => panel.Name == panelName))
            {
                return panel;
            }
            return uiCa.CreateRibbonPanel(tabName, panelName);
        }

        public static bool HasTab(this UIControlledApplication uiCa, string tabName)
        {
            try
            {
                uiCa.GetRibbonPanels(tabName);
                return true;
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException)
            {
                return false;
            }
        }

        private static bool WriteEmptyComment(UIControlledApplication app, string controlName, string comment)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                app.ControlledApplication.WriteJournalComment(comment, false);
                return true;
            }
            return false;
        }
    }
}
