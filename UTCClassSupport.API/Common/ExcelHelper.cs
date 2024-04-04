using System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace UTCClassSupport.API.Common
{
  public class ExcelHelper
  {
    public static System.Data.DataTable ConvertExcelToDataTable(string filePath)
    {
      Excel.Application excelApp = new Excel.Application();
      Excel.Workbook excelWorkbook = null;
      try
      {
        System.Data.DataTable dt = new System.Data.DataTable();
        excelWorkbook = excelApp.Workbooks.Open(filePath);
        Excel.Worksheet worksheet = (Excel.Worksheet)excelWorkbook.Sheets[1];

        int rowCount = worksheet.UsedRange.Rows.Count;
        int columnCount = worksheet.UsedRange.Columns.Count;

        // Tạo các cột cho DataTable dựa trên số lượng cột trong Worksheet
        for (int i = 1; i <= columnCount; i++)
        {
          var columnName = (worksheet.Cells[1, i] as Excel.Range).Value2.ToString();
          dt.Columns.Add(columnName);
        }

        // Đọc dữ liệu từ Worksheet và thêm vào DataTable
        for (int row = 2; row <= rowCount; row++)
        {
          DataRow dataRow = dt.NewRow();
          for (int col = 1; col <= columnCount; col++)
          {
            var cellValue = (worksheet.Cells[row, col] as Excel.Range).Value2;
            dataRow[col - 1] = cellValue;
          }
          dt.Rows.Add(dataRow);
        }

        // Đóng workbook và giải phóng tài nguyên
        excelWorkbook.Close();
        excelApp.Quit();
        File.Delete(filePath);
        return dt;
      }
      catch (Exception ex)
      {
        excelWorkbook.Close();
        excelApp.Quit();
        File.Delete(filePath);
        throw ex;
      }
    }
    public static bool IsExcelFile(string filePath)
    {
      string fileName = Path.GetFileName(filePath);
      if (String.IsNullOrEmpty(fileName))
      {
        return false;
      }
      string ext = Path.GetExtension(fileName);
      if (ext != ".xls" && ext != ".xlsx")
      {
        return false;
      }
      return true;
    }

  }
}
