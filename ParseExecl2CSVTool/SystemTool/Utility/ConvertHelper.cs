using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Utility
{
    public class ConvertHelper
    {
        public static String ConvertEnumToString<T>(T eff)
        {
            return System.Enum.GetName(eff.GetType(), eff);
        }

        public static T ConverStringToEnum<T>(string enumValue)
        {
            try
            {
                T result = (T)System.Enum.Parse(typeof(T), enumValue);
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                if (info.PropertyType == typeof(decimal?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(double)));
                else if (info.PropertyType == typeof(DateTime?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(DateTime)));
                else if (info.PropertyType == typeof(int?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(int)));
                else
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) == null ? DBNull.Value : info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static List<T> DataTableToList<T>(DataTable data) where T : new()
        {
            List<T> listT = new List<T>();
            T objT;
            foreach (DataRow dr in data.Rows)
            {
                objT = new T();
                PropertyDescriptorCollection infos = TypeDescriptor.GetProperties(objT.GetType());
                foreach (DataColumn dc in data.Columns)
                {
                    foreach (PropertyDescriptor info in infos)
                    {
                        if (info.Name.ToUpper() == dc.ColumnName.ToUpper())
                        {
                            object value = dr[dc.ColumnName];
                            TypeConverter typeConverter = info.Converter;
                            if (value != DBNull.Value)
                            {
                                object obj = value;
                                if (typeConverter != null)
                                {
                                    if (value.GetType().Name == "Byte[]")
                                    {
                                        obj = value;
                                    }
                                    else
                                        obj = typeConverter.ConvertFromString(value.ToString());
                                }
                                info.SetValue(objT, obj);
                            }
                        }
                    }
                }
                listT.Add(objT);
            }
            return listT;
        }

        public static string RomanNumerals(int n)
        {
            int i = 0;
            string[] arabic = "1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1".Split(',');
            string[] roman = "M,CM,D,CD,C,XC,L,XL,X,IX,V,IV,I".Split(',');
            string result = "";
            while (n > 0)
            {
                while (n >= Convert.ToInt32(arabic[i]))
                {
                    n = n - Convert.ToInt32(arabic[i]);
                    result = result + roman[i];
                }
                i = i + 1;
            }
            return result;
        }

        public static void TranferValueBetween2Object(Object source, Object destination)
        {
            PropertyDescriptorCollection infoSource = TypeDescriptor.GetProperties(source.GetType());
            PropertyDescriptorCollection infoDestination = TypeDescriptor.GetProperties(destination.GetType());
            if (infoDestination != null && infoDestination.Count > 0)
            {
                foreach (PropertyDescriptor info in infoDestination)
                {
                    info.SetValue(destination, infoSource.Find(info.Name, true).GetValue(source));
                }
            }
        }

        /// <summary>
        /// Copy an object to destination object, only matching fields will be copied
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceObject">An object with matching fields of the destination object</param>
        /// <param name="destObject">Destination object, must already be created</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            try
            {
                // If either the source, or destination is null, return
                if (sourceObject == null || destObject == null)
                {
                    return;
                }

                // Get the type of each object
                Type sourceType = sourceObject.GetType();
                Type targetType = destObject.GetType();

                // Loop through the source properties
                foreach (PropertyInfo p in sourceType.GetProperties())
                {
                    // Get the matching property in the destination object
                    PropertyInfo targetObj = targetType.GetProperty(p.Name);
                    // If there is none, skip
                    if (targetObj == null)
                    {
                        continue;
                    }

                    // Set the value in the destination
                    targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
                }
            }
            catch (Exception ex)
            {
                LogEx.LogEx.Instance.WriteExceptionLog(ex,"CopyObject");
            }
        }
    }
}