using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
      using (var memoryStream = new MemoryStream())
      {
        file.CopyToAsync(memoryStream);

        using (var document = SpreadsheetDocument.Open(memoryStream, false))
        {
          WorkbookPart workbookPart = document.WorkbookPart;
          WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
          SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

          DataTable dataTable = new DataTable();

          // Assume the first row is the header row
          var headerRow = sheetData.Elements<Row>().First();
          foreach (Cell headerCell in headerRow)
          {
            dataTable.Columns.Add(GetCellValue(document, headerCell));
          }

          // Iterate through each row in the worksheet
          foreach (Row row in sheetData.Elements<Row>().Skip(1)) // Skip header row
          {
            DataRow dataRow = dataTable.NewRow();

            // Iterate through each cell in the row
            for (int i = 0; i < row.Elements<Cell>().Count(); i++)
            {
              Cell cell = row.Elements<Cell>().ElementAt(i);
              int actualCellIndex = GetColumnIndex(cell.CellReference);
              dataRow[actualCellIndex] = GetCellValue(document, cell);
            }

            dataTable.Rows.Add(dataRow);
          }

          return dataTable;
        }
      }
    }
    private static string GetCellValue(SpreadsheetDocument document, Cell cell)
    {
      SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
      string value = cell.CellValue?.InnerText;

      if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
      {
        return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
      }
      else
      {
        return value;
      }
    }

    // Convert cell reference to column index, e.g., "C2" to 2
    private static int GetColumnIndex(string cellReference)
    {
      int columnIndex = 0;
      foreach (char ch in cellReference)
      {
        if (Char.IsLetter(ch))
        {
          int value = Char.ToUpper(ch) - 'A';
          columnIndex = columnIndex * 26 + value;
        }
        else
        {
          return columnIndex;
        }
      }
      return columnIndex;
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
