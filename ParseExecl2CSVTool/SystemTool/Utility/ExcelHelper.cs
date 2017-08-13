using System;
using System.Data;
using System.Globalization;

namespace Utility
{
    public class ExcelHelper
    {
        public static DataTable ImportExcelToGrid(string pstrFilename, int iColEnd)
        {
            return ImportExcelToGrid(pstrFilename, iColEnd, 1);
        }

        public static DataTable ImportExcelToGrid(string pstrFilename, int iColEnd, int iRowNumber)
        {
            string mstr_FileName = pstrFilename;
            string mstr_PathFileName = mstr_FileName;

            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(mstr_FileName);
            Aspose.Cells.Worksheet ws = wb.Worksheets[0];

            DataTable dt = new DataTable();
            int iRowEnd = GetEndRow(ws, iRowNumber + 1);
            try
            {
                for (int j = 0; j < iColEnd; j++)
                {
                    string strDataColumn = ws.Cells[iRowNumber - 1, j].Value.ToString().Trim();
                    dt.Columns.Add(new DataColumn(strDataColumn, typeof(string)));
                }
                int i = iRowNumber;
                for (; i < iRowEnd; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < iColEnd; j++)
                    {
                        string strValue = ws.Cells[i, j].Value == null ? "" : ws.Cells[i, j].Value.ToString();
                        dr[j] = strValue;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception exp)
            {
            }
            return dt;
        }

        public static int GetEndCol(Aspose.Cells.Worksheet ws)
        {
            int iResult = 0;
            while (true)
            {
                string sValue = ws.Cells[0, iResult].Value == null ? "" : ws.Cells[1, iResult].Value.ToString();
                if (sValue != "")
                {
                    iResult++;
                }
                else break;
            }
            return iResult;
        }

        public static int GetEndRow(Aspose.Cells.Worksheet ws, int StartRow)
        {
            while (true)
            {
                string sValue = ws.Cells[StartRow, 0].Value == null ? "" : ws.Cells[StartRow, 0].Value.ToString();
                if (sValue != "")
                {
                    StartRow++;
                }
                else break;
            }
            return StartRow;
        }
        #region New
        public static DataTable importRosterExcel(string Filename)
        {
            try
            {
                return importRosterExcel2(Filename, 1, 1);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public static DataTable importRosterExcel2(string Filename, int iColNumber, int iRowNumber)
        {
            string mes1 = "";
            string mes2 = "";
            string mstr_FileName = Filename;
            string mstr_PathFileName = mstr_FileName;

            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(mstr_FileName);
            Aspose.Cells.Worksheet ws = wb.Worksheets[0];


            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            string DTformat = ci.DateTimeFormat.ShortDatePattern;
            //MessageBox.Show(ci.DateTimeFormat.ShortDatePattern);

            DataTable dt = new DataTable();
            dt.TableName = ws.Name;
            int iColEnd = GetEndCol(ws);
            int iRowEnd = GetEndRow(ws, iRowNumber + 4);
            try
            {
                for (int j = 0; j < iColEnd; j++)
                {
                    if (ws.Cells[iRowNumber, j].Value == null)
                        dt.Columns.Add(new DataColumn());
                    else
                    {
                        object cName = ws.Cells[iRowNumber - 1, j].Value;
                        object value = ws.Cells[iRowNumber, j].Value;
                        if (value.GetType() == typeof(DateTime))
                            dt.Columns.Add(new DataColumn(cName.ToString().Replace("\r\n", "").Replace("\n", "").Trim(), typeof(DateTime)));
                        else if (value.GetType() == typeof(double))
                            dt.Columns.Add(new DataColumn(cName.ToString().Replace("\r\n", "").Replace("\n", "").Trim(), typeof(decimal)));
                        else
                            dt.Columns.Add(new DataColumn(cName.ToString().Replace("\r\n", "").Replace("\n", "").Trim(), typeof(string)));
                    }
                }
                int i = iRowNumber + 3;
                for (; i < iRowEnd; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < iColEnd; j++)
                    {
                        object value = ws.Cells[i, j].Value;

                        if (value == null)
                        {
                            dr[j] = DBNull.Value;
                        }
                        else
                        {
                            string[] s = value.ToString().Trim().Split(new string[] { " _ " }, StringSplitOptions.None);
                            string str = value.ToString().Contains(" _ ") ? s[s.Length - 1] : value.ToString().Trim();
                            mes1 = dt.Columns[j].ColumnName;
                            mes2 = (i + 1).ToString();

                            if (dt.Columns[j].DataType == typeof(DateTime))
                            {
                                try
                                {
                                    dr[j] = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString(DTformat, CultureInfo.InvariantCulture);
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        DateTime creationDate;
                                        string[] sDate = value.ToString().Split('/');
                                        string sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                                        creationDate = Convert.ToDateTime(sDateTime);
                                        dr[j] = creationDate;
                                    }
                                    catch (Exception ex)
                                    {
                                        dr[j] = Convert.ToDateTime(value.ToString());
                                    }

                                }
                                //DateTime creationDate;
                                //if (DateTime.TryParseExact(value.ToString(), DTformat,
                                //                           CultureInfo.InvariantCulture, DateTimeStyles.None,
                                //                           out creationDate))
                                //{
                                //    dr[j] = creationDate;
                                //}
                                //else
                                //{
                                //    string[] sDate = value.ToString().Split('/');
                                //    string sDateTime = sDate[1] + '/' + sDate[0] + '/' + sDate[2];
                                //    creationDate = Convert.ToDateTime(sDateTime);
                                //    dr[j] = creationDate;
                                //}
                                //dr[j] = string.Format("{0:" + DTformat + "}", value);
                                //dr[j] = DateTime.ParseExact(value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString(DTformat, CultureInfo.InvariantCulture);
                                //DateTime.ParseExact(value.ToString(), DTformat, null);
                            }
                            else if (dt.Columns[j].DataType == typeof(decimal))
                            {
                                dr[j] = Convert.ToInt32(str);
                            }
                            else
                            {
                                dr[j] = str;
                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "Sai ở cột [" + mes1 + "], dòng [" + mes2 + "]";
                //MessageBox.Show(ex.Message);
                //Util.Log.Instance.WriteExceptionLog(ex, "importExcel");
                throw (ex);
            }
            return null;
        }
        #endregion
    }
}