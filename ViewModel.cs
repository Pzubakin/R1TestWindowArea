using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace R1TestWindowArea
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public int CalculatedWindowsCount
        {
            get { return calculatedWindowsCount; }
            set
            {
                calculatedWindowsCount = value;
                NotifyPropertyChanged();
            }
        }
        public string CalculateWindowsProgress
        {
            get { return calculateWindowsProgress; }
            set
            {
                calculateWindowsProgress = value;
                NotifyPropertyChanged();
            }
        }
        public string TotalArea
        {
            get { return totalArea; }
            set
            {
                totalArea = value;
                NotifyPropertyChanged();
            }
        }
        public RelayCommand CalculateAreas
        {
            get
            {
                if (calculateAreas == null)
                {
                    calculateAreas = new RelayCommand(obj => GetAreas());
                }
                return calculateAreas;
            }
        }
        private string calculateWindowsProgress;
        private string totalArea;
        private int calculatedWindowsCount;
        private RelayCommand calculateAreas;
        public void GetAreas()
        {
            CalculatedWindowsCount = 0;
            double areaResult = 0;
            var windows = new FilteredElementCollector(DocumentProvider.Document).
               OfCategory(BuiltInCategory.OST_Windows).
               WhereElementIsNotElementType().
               ToElements();
            if (windows != null)
            {
                foreach (var element in windows)
                {
                    if (element is FamilyInstance)
                    {
                        CalculatedWindowsCount++;
                        CalculateWindowsProgress = $"Windows counted: {CalculatedWindowsCount}";
                        var elementGeometry = element.get_Geometry(new Options());
                        if (elementGeometry != null)
                        {
                            foreach (GeometryObject elementGeometryObj in elementGeometry)
                            {
                                if (elementGeometryObj is GeometryInstance geometryInstance)
                                {
                                    var windowGeometry = geometryInstance.GetInstanceGeometry();
                                    areaResult += GetWindowGlassArea(windowGeometry);
                                }
                            }
                        }
                    }
                }
            }
            TotalArea = $"Windows glass area = {Math.Round(areaResult, 2)} m2";
        }
        private double GetWindowGlassArea(GeometryElement windowGeometry)
        {
            double result = 0;
            foreach (var geometryObject in windowGeometry)
            {
                if (geometryObject is Solid solid)
                {
                    GraphicsStyle graphicsStyle = DocumentProvider.Document.GetElement(geometryObject.GraphicsStyleId) as GraphicsStyle;
                    if (graphicsStyle != null)
                    {
                        if (graphicsStyle.GraphicsStyleCategory.GetBuiltInCategory() == BuiltInCategory.OST_WindowsGlassProjection)
                        {
                            result += UnitUtils.Convert(solid.SurfaceArea, DisplayUnitType.DUT_SQUARE_FEET, DisplayUnitType.DUT_SQUARE_METERS);
                        }
                    }
                }
            }
            return result;
        }
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
