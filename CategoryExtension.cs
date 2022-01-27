using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R1TestWindowArea
{
    public static class CategoryExtension
    {
        public static BuiltInCategory GetBuiltInCategory(this Category category)
        {
            if (System.Enum.IsDefined(typeof(BuiltInCategory), category.Id.IntegerValue))
            {
                var result = (BuiltInCategory)category.Id.IntegerValue;
                return result;
            }
            return BuiltInCategory.INVALID;
        }
    }
}
