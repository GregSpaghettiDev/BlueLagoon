using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Common.Excel
{
    public static class ExcelCreator
    {
        public static byte[] WriteExcel<T>(List<T> rows, string sheetName = "Sheet1") where T : class
        {
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rows), (typeof(DataTable)));

            using (var memoryStream = new MemoryStream())
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(sheetName);

                List<string> columns = new();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in table.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;

                    foreach (string col in columns)
                    {
                        row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }

                    rowIndex++;
                }

                for (int i = 0; i < table.Columns.Count; i++)
                    excelSheet.AutoSizeColumn(i);

                workbook.Write(memoryStream, false);

                return memoryStream.ToArray();
            }
        }
    }
}