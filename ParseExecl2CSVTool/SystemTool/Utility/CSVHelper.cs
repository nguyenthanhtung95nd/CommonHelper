using System;
using System.Data;
using System.IO;
using System.Text;

namespace Utility
{
    public class CSVHelper
    {
        #region Helper

        public static void CreateCSVFile(DataTable dt, string strFilePath)
        {
            // Create the CSV file to which grid data will be exported.
            Stream s = File.Open(strFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter sw = new StreamWriter(s, Encoding.UTF8);

            // First we will write the headers.
            //DataTable dt = m_dsProducts.Tables[0];
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.TableName + dt.Columns[i].ColumnName);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        #endregion Helper
    }
}