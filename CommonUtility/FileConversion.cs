using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class FileConversion
    {
        public DataTable ReadExcelFileDOM(string filename, String sheetName, string sheetId)
        {
            DataTable table;

            using (SpreadsheetDocument myDoc = SpreadsheetDocument.Open(filename, false))
            {

                WorkbookPart workbookPart = myDoc.WorkbookPart;
                //Sheet worksheet = workbookPart.Workbook.Descendants<Sheet>().First();

                WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheetId));
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                List<List<string>> totalRows = new List<List<string>>();

                int maxCol = 0;
                foreach (Row r in sheetData.Elements<Row>())
                {
                    // Add the empty row.
                    string value = null;
                    while (totalRows.Count < r.RowIndex - 1)
                    {
                        List<string> emptyRowValues = new List<string>();
                        for (int i = 0; i < maxCol; i++)
                        {
                            emptyRowValues.Add("");
                        }
                        totalRows.Add(emptyRowValues);
                    }


                    List<string> tempRowValues = new List<string>();
                    foreach (Cell c in r.Elements<Cell>())
                    {
                        #region get the cell value of c.
                        if (c != null)
                        {
                            value = c.InnerText;

                            // If the cell represents a numeric value, you are done. 
                            // For dates, this code returns the serialized value that 
                            // represents the date. The code handles strings and Booleans
                            // individually. For shared strings, the code looks up the 
                            // corresponding value in the shared string table. For Booleans, 
                            // the code converts the value into the words TRUE or FALSE.
                            if (c.DataType != null)
                            {
                                switch (c.DataType.Value)
                                {
                                    case CellValues.SharedString:
                                        // For shared strings, look up the value in the shared 
                                        // strings table.
                                        var stringTable = workbookPart.
                                            GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                                        // If the shared string table is missing, something is 
                                        // wrong. Return the index that you found in the cell.
                                        // Otherwise, look up the correct text in the table.
                                        if (stringTable != null)
                                        {
                                            value = stringTable.SharedStringTable.
                                                ElementAt(int.Parse(value)).InnerText;
                                        }
                                        break;

                                    case CellValues.Boolean:
                                        switch (value)
                                        {
                                            case "0":
                                                value = "FALSE";
                                                break;
                                            default:
                                                value = "TRUE";
                                                break;
                                        }
                                        break;
                                }
                            }

                            Console.Write(value + "  ");
                        }
                        #endregion

                        // Add the cell to the row list.
                        int i = Convert.ToInt32(c.CellReference.ToString().ToCharArray().First() - 'A');

                        // Add the blank cell in the row.
                        while (tempRowValues.Count < i)
                        {
                            tempRowValues.Add("");
                        }
                        tempRowValues.Add(value);
                    }

                    // add the row to the totalRows.
                    maxCol = processList(tempRowValues, totalRows, maxCol);

                    Console.WriteLine();
                }

                table = ConvertListListStringToDataTable(totalRows, maxCol);
            }
            return table;
        }

        int processList(List<string> tempRows, List<List<string>> totalRows, int MaxCol)
        {
            if (tempRows.Count > MaxCol)
            {
                MaxCol = tempRows.Count;
            }

            totalRows.Add(tempRows);
            return MaxCol;
        }

        DataTable ConvertListListStringToDataTable(List<List<string>> totalRows, int maxCol)
        {
            DataTable table = new DataTable();
            for (int i = 0; i < maxCol; i++)
            {
                table.Columns.Add();
            }
            foreach (List<string> row in totalRows)
            {
                while (row.Count < maxCol)
                {
                    row.Add("");
                }
                table.Rows.Add(row.ToArray());
            }
            return table;
        }

        public List<SheetName> PassFileName(string filename)
        {
            List<SheetName> sheetNameList = new List<SheetName>();
            var results = GetAllWorksheets(filename);
            foreach (Sheet item in results)
            {
                SheetName sheetNameObj = new SheetName();
                sheetNameObj.Id = Convert.ToString(item.Id);
                sheetNameObj.Name = Convert.ToString(item.Name);
                sheetNameList.Add(sheetNameObj);
            }
            return sheetNameList;
        }

        public Sheets GetAllWorksheets(string fileName)
        {
            Sheets theSheets = null;

            using (SpreadsheetDocument document =
                SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                theSheets = wbPart.Workbook.Sheets;
            }
            //FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            //WorkbookPart hssfworkbook = new WorkbookPart();
            //for (int i = 0; i < hssfworkbook.; i++)
            //{
            //    Console.WriteLine(hssfworkbook.GetSheetName(i));
            //}
            //file.Close();

            //string filename = "C:\\romil.xlsx";

            //object missing = System.Reflection.Missing.Value;

            //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            //Microsoft.Office.Interop.Excel.Workbook wb = excel.Workbooks.Open(filename, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

            //ArrayList sheetname = new ArrayList();

            //foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in wb.Sheets)
            //{
            //    sheetname.Add(sheet.Name);
            //}

            return theSheets;
        }

        public List<SheetName> ListSheetInExcel(string filePath)
        {
            OleDbConnectionStringBuilder sbConnection = new OleDbConnectionStringBuilder();
            String strExtendedProperties = String.Empty;
            sbConnection.DataSource = filePath;
            if (Path.GetExtension(filePath).Equals(".xls"))//for 97-03 Excel file
            {
                sbConnection.Provider = "Microsoft.Jet.OLEDB.4.0";
                strExtendedProperties = "Excel 8.0;HDR=Yes;IMEX=1";//HDR=ColumnHeader,IMEX=InterMixed
            }
            else if (Path.GetExtension(filePath).Equals(".xlsx"))  //for 2007 Excel file
            {
                sbConnection.Provider = "Microsoft.ACE.OLEDB.12.0";
                strExtendedProperties = "Excel 12.0;HDR=Yes;IMEX=1";
            }
            sbConnection.Add("Extended Properties", strExtendedProperties);
            List<SheetName> listSheet = new List<SheetName>();
            using (OleDbConnection conn = new OleDbConnection(sbConnection.ToString()))
            {
                conn.Open();
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                    {
                        SheetName sheetObj = new SheetName();
                        sheetObj.Id = Convert.ToString("");
                        sheetObj.Name = Convert.ToString(drSheet["TABLE_NAME"].ToString());
                        listSheet.Add(sheetObj);
                    }
                }
            }
            return listSheet;
        }

        public class SheetName
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }


    }
}
