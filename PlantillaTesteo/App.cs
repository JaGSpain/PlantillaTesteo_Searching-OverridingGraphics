using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace PlantillaTesteo
{
    public class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication a)
        {
            throw new NotImplementedException();
        }

        public Result OnStartup(UIControlledApplication a)
        {
            #region "Ribbon Panel Creation & button Creation"
            //Adding Tab and panel
            RibbonPanel panel = RibbonPanel(a);// We'll implement method RibbonPanel using a try catch and we'll create util.cs to manage exception

            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            //Creating button for The WPF Option
            if (panel.AddItem(
                new PushButtonData("PlantillaTesteo", "PlantillaTesteo", thisAssemblyPath, "PlantillaTesteo.Command")) is PushButton button)
            {
                button.ToolTip = "Testing Example";
                Uri uriImage = new Uri("pack://application:,,,/PlantillaTesteo;component/Resources/icon_img_32.png");
                BitmapImage largeImage = new BitmapImage(uriImage);
                button.LargeImage = largeImage;
            }
            #endregion //When push button called EntryCommands

            return Result.Succeeded;
        }
        #region RibbonPanel
        public RibbonPanel RibbonPanel(UIControlledApplication a)
        {
            string tab = "JAG Dev WPF Basic";
            RibbonPanel ribbonPanel = null;

            //try to create the ribbon tab
            try
            {
                a.CreateRibbonTab(tab);

            }
            catch (Exception ex)
            {
                //We'll create a file called Util.cs where we handle the expections.
                Util.HandleError(ex);

            }

            //Try to create a Ribbon Panel
            try
            {
                RibbonPanel panel = a.CreateRibbonPanel(tab, "Show WPF window example");

            }
            catch (Exception ex)
            {

                Util.HandleError(ex);
            }

            //Search existing tab for panel
            List<RibbonPanel> panels = a.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels.Where(p => p.Name == "Show WPF window example"))
            {
                ribbonPanel = p;
            }


            return ribbonPanel;
        }
        #endregion
    }

}
