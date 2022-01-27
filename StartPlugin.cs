using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Linq;
using System.Text;

namespace R1TestWindowArea
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class StartPlugin : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            DocumentProvider.Document = uiDoc.Document;
            AreaCalculator areaCalculator = new AreaCalculator();
            areaCalculator.Show();
            return Result.Succeeded;
        }
    }
}
