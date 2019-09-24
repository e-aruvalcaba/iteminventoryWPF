using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace ItemInventoryApp.Classes
{
    class ExcelGenerator
    {
        public Microsoft.Office.Interop.Excel.Application excel;
        public Microsoft.Office.Interop.Excel.Workbook workBook;
        public Microsoft.Office.Interop.Excel.Worksheet workSheet;
        public Microsoft.Office.Interop.Excel.Range cellRange;

        public void GenerateExcel(DataTable DtIN, string path, string name)
        {
            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.DisplayAlerts = false;
                excel.Visible = false;
                workBook = excel.Workbooks.Add(Type.Missing);
                workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.ActiveSheet;
                workSheet.Name = "Reporte Pedidos";
                System.Data.DataTable tempDt = DtIN;
                //dgExcel.ItemsSource = tempDt.DefaultView;
                workSheet.Cells.Font.Size = 11;
                int rowcount = 1;
                for (int i = 1; i <= tempDt.Columns.Count; i++) //taking care of Headers.  
                {
                    string colname = "";
                    switch (tempDt.Columns[i - 1].ColumnName)
                    {
                        case "Name": colname = "Nombre"; break;
                        case "Items": colname = "Lista de Productos"; break;
                        case "Status": colname = "Estado"; break;
                        case "Total": colname = "Total en Pesos"; break;
                        case "Date": colname = "Fecha"; break;
                        case "Time": colname = "Hora"; break;
                        case "dateTime": colname = "Generado al: "; break;
                        //Parte para item popular
                        case "Description": colname = "Descripcion del Producto"; break;
                        case "Price": colname = "Precio en Pesos"; break;
                        case "TotalInPedidos": colname = "Total Veces encontrado en Pedidos"; break;
                        default: colname = tempDt.Columns[i - 1].ColumnName; break;
                    }

                    workSheet.Cells[1, i] = colname;//tempDt.Columns[i - 1].ColumnName;
                }
                foreach (System.Data.DataRow row in tempDt.Rows) //taking care of each Row  
                {
                    rowcount += 1;
                    for (int i = 0; i < tempDt.Columns.Count; i++) //taking care of each column  
                    {
                        workSheet.Cells[rowcount, i + 1] = row[i].ToString();
                    }
                }
                cellRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[rowcount, tempDt.Columns.Count]];
                cellRange.EntireColumn.AutoFit();

                string[] paths = { string.Format(@"{0}", path), name };
                workBook.SaveAs(System.IO.Path.Combine(paths));
                //workBook.SaveAs(System.IO.Path.Combine(@"C:\InventoryApp\","Excel book Name "));  
                workBook.Close();
                excel.Quit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creando el report: " + ex.Message);
            }
        }

        public DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor pdt in properties)
                {
                    row[pdt.Name] = pdt.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

    }// end of the way
}
