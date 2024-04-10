using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using UTCClassSupport.API.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace UTCClassSupport.API.Common
{
  public class ExcelHelper
  {
    public static System.Data.DataTable ConvertExcelToDataTable(IFormFile file)
    {
      if (file == null || file.Length == 0)
      {
        return null;
      }
      DataTable dt = new DataTable();

      using (var stream = file.OpenReadStream())
      {
        IWorkbook workbook;

        // Determine the appropriate workbook format based on the file extension
        if (Path.GetExtension(file.FileName).Equals(".xls"))
          workbook = new HSSFWorkbook(stream); // For Excel 97-2003 (*.xls) files
        else if (Path.GetExtension(file.FileName).Equals(".xlsx"))
          workbook = new XSSFWorkbook(stream); // For Excel 2007 or newer (*.xlsx) files
        else
          throw new Exception("Invalid file format");

        ISheet sheet = workbook.GetSheetAt(0); // Assuming the data is on the first sheet

        var dataTable = new DataTable(sheet.SheetName);

        // write the header row
        var headerRow = sheet.GetRow(0);
        foreach (var headerCell in headerRow)
        {
          dataTable.Columns.Add(headerCell.ToString());
        }

        // write the rest
        for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
        {
          var sheetRow = sheet.GetRow(i);
          if (sheetRow == null) continue;
          var dtRow = dataTable.NewRow();
          dtRow.ItemArray = dataTable.Columns
              .Cast<DataColumn>()
              .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK)?.ToString() ?? String.Empty)
              ?.ToArray();
          dataTable.Rows.Add(dtRow);
        }
        return dataTable;
      }
    }


    public static bool IsExcelFile(string filePath)
    {
      if (String.IsNullOrEmpty(filePath))
      {
        return false;
      }
      string ext = Path.GetExtension(filePath);
      if (ext != ".xls" && ext != ".xlsx")
      {
        return false;
      }
      return true;
    }

  }
}
