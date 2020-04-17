using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity.ModelConfiguration;

namespace PXin.Model
{
    public class ModelBuilderHelper
    {
       public static List<Type> ModelMaps
        {
            get
            {
               Type baseType = typeof(EntityTypeConfiguration<>);
               return Assembly.GetExecutingAssembly().GetTypes().Where(c => c.BaseType.IsGenericType && c.BaseType.GetGenericTypeDefinition() == baseType).ToList();
            }
        }
    }
}
