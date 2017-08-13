using Domain;
using System.Data;

namespace Utility
{
    public class DynamicObject
    {
        /// <summary>
        /// Creates an object from the specified type and calls the DataReader => Object mapping function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DynamicEntity LoadObjectFromDataReader(IDataReader reader)
        {
            // Create complex dynamic property and add child properties:
            DynamicEntity theInstanceType = new DynamicEntity();

            for (int f = 0; f < reader.FieldCount; f++)
            {
                string fName = reader.GetName(f);
                object value = reader.GetValue(f);

                theInstanceType[fName] = value;
            }

            return theInstanceType;
        }
    }
}