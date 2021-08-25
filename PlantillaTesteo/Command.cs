using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace PlantillaTesteo
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    
    public class Command:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Getting proj info
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            List<Level> levels = getLevels(doc);
            List<Floor> floors = getFloors(doc);
            List<Wall> walls = getWalls(doc);

       

            foreach (Level l in levels)
            {
                if(l.Name=="Level 2")
                {
                    TaskDialog.Show("Id.", l.Id.ToString());
                }
            }

            //Elements to find.. Necesitare una lista
            List<Floor> f =floors.FindAll(x => x.Name == "Story1");
           //List<Wall> w = walls.Find(x => x.Name == "w1");
           
           
           
            //Remarking and overriding color of selected elements 
           Element solidFill = new FilteredElementCollector(doc)
                 .OfClass(typeof(FillPatternElement))
                 .Where(q => q.Name.Equals("<Solid fill>")).First();
           
            
            
            //Sobreesponemos graficos en verde
           OverrideGraphicSettings ogs = new OverrideGraphicSettings();                  
                   
             ogs.SetSurfaceBackgroundPatternColor(new Color(0,255,0));
             ogs.SetSurfaceBackgroundPatternId(solidFill.Id);
            
            //Registrando cambios
            using (Transaction t = new Transaction(doc,"search"))
            {
                try {
                    t.Start();

                    //Lo primero que hacemos es limpiar todos los elementos.(lo mismo sale rentable hacer metodo)
                    //Empleamos default en todos los elementos (suelo,muros) que hayan podido ser modificados 
                    //Busca todos los floor y devuelveles el Foreground normal
                    //Default graphics a todos los suelos.

                    #region FloorGraphicOverride
                    foreach (Floor fl in floors)
                    {
                        //Pasamos default graphic settings
                        OverrideGraphicSettings defaulgrap = new OverrideGraphicSettings();
                        doc.ActiveView.SetElementOverrides(fl.Id, defaulgrap);
                    }
                                
                    //Para cada floor del tipo buscado
                    foreach (Floor flr in f)
                    {
                        doc.ActiveView.SetElementOverrides(flr.Id, ogs);

                    }
                    #endregion

                    //Hacemos lo mismo para los niveles.
                    
                    t.Commit();

                }
                catch
                {
                    //Si no encuentro 
                    if (f == null)
                    {
                        TaskDialog.Show("Warning", "The search has not been successful");

                    }
                }
            }


                return Result.Succeeded;
        }

        //Get levels
        public List<Level> getLevels(Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            ICollection<Element> levels = col.OfClass(typeof(Level)).ToElements();
            List<Level> list_lvl = new List<Level>();
            foreach(Level lvl in levels)
            {
                list_lvl.Add(lvl);
            }
            return list_lvl;
        }

        //Get Floors
        public List<Floor> getFloors(Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            ICollection<Element> floors = col.OfClass(typeof(Floor)).ToElements();
            List<Floor> List_Floors = new List<Floor>();
            foreach(Floor f in floors)
            {
                List_Floors.Add(f);
            }
            return List_Floors;

        }
        //Get walls
        public List<Wall> getWalls(Document doc)
        {
            FilteredElementCollector col = new FilteredElementCollector(doc);
            ICollection<Element> walls = col.OfClass(typeof(Wall)).ToElements();
            List<Wall> List_Walls = new List<Wall>();
            foreach(Wall w in walls)
            {
                List_Walls.Add(w);
            }

            return List_Walls;
        }
        
    
    }
}
