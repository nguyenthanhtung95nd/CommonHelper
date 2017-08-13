using System;
using System.Data;
using System.Windows.Forms;
using Utility;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public readonly string path = @"C:\Users\nguye\Desktop\YSKH\GCS-Project\ParseExecl2CSVTool\SystemTool\Out\FileOutput.csv";
        public readonly string path1 = @"C:\Users\nguye\Desktop\YSKH\GCS-Project\ParseExecl2CSVTool\SystemTool\In\FileInput.xlsx";

        private void Form1_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            //dt = LoadData2DataTable();
            //CSVHelper.CreateCSVFile(dt, path);
            dt = ExcelHelper.ImportExcelToGrid(path1, 5);
            CSVHelper.CreateCSVFile(dt,path);
            dgvEmployee.DataSource = dt;
        }

        public DataTable LoadData2DataTable()
        {
            DataTable dt = new DataTable();
            var t = EmployeeModel.GetAll();
            dt = ConvertHelper.ListToDataTable(t);
            return dt;
        }
    }
}