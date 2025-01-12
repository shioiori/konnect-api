using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace UTCClassSupport.API.Common
{
  public class ExcelHelper
  {
    public static DataTable ConvertExcelToDataTable(IFormFile file, FileTemplate template, int sheetIndex = 0)
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

        ISheet sheet = workbook.GetSheetAt(sheetIndex);
        switch (template)
        {
          case FileTemplate.User:
            dt = ConvertUserTemplateToDataTable(sheet);
            break;
          case FileTemplate.Timetable:
            dt = ConvertTimetableTemplateToDataTable(sheet);
            break;
        }
        return dt;
      }
    }

    public static System.Data.DataTable ConvertUserTemplateToDataTable(ISheet sheet)
    {
      var dataTable = new DataTable(sheet.SheetName);

      // write the header row
      var headerRow = sheet.GetRow(0);
      foreach (var headerCell in headerRow)
      {
        dataTable.Columns.Add(headerCell.ToString());
      }

      // write the record
      for (int i = 1; i < sheet.PhysicalNumberOfRows; i++)
      {
        var sheetRow = sheet.GetRow(i);
        if (sheetRow == null) continue;
        var dtRow = dataTable.NewRow();
        dtRow.ItemArray = dataTable.Columns
            .Cast<DataColumn>()
            .Select(c => sheetRow.GetCell(c.Ordinal, 
            MissingCellPolicy.CREATE_NULL_AS_BLANK)?.ToString() ?? String.Empty)
            ?.ToArray();
        dataTable.Rows.Add(dtRow);
      }
      return dataTable;
    }

    public static System.Data.DataTable ConvertTimetableTemplateToDataTable(ISheet sheet)
    {
      var dataTable = new DataTable(sheet.SheetName);

      // write the header row
      var headerRow = sheet.GetRow(9);
      foreach (var headerCell in headerRow)
      {
        if (headerCell.ToString() == String.Empty && headerCell.IsMergedCell) continue;
        dataTable.Columns.Add(headerCell.ToString());
      }

      // write the rest
      DataRow storeRow = null;
      for (int i = 10; i < sheet.PhysicalNumberOfRows; i++)
      {
        var sheetRow = sheet.GetRow(i);
        if (sheetRow == null) continue;
        var dtRow = dataTable.Columns
            .Cast<DataColumn>()
            .Select(c => sheetRow.GetCell(c.Ordinal, MissingCellPolicy.CREATE_NULL_AS_BLANK)?.ToString() ?? String.Empty)
            ?.ToArray();
        if (!int.TryParse(dtRow[0].ToString(), out var x))
        {
          break;
        }
        var newRow = dataTable.NewRow();
        for (int j = 0, k = 0; j < dtRow.Length; j++)
        {
          if (j == 2) continue;
          if (dtRow[j].ToString() == String.Empty && storeRow != null)
          {
            newRow[k] = storeRow[k];
          }
          else
          {
            newRow[k] = dtRow[j];
          }
          k++;
        }
        storeRow = newRow;
        dataTable.Rows.Add(storeRow);
      }
      return dataTable;
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
