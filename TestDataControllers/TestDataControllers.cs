using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

namespace AllClassifieds.TestDataControllers
{
    class TestDataController
    {


        public String GetBaseURL(int testIndex)
        {
            String baseURL = null;

            ////To obtain the current solution path/project path
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            //Append the html report file to current project path
            //string reportPath = projectPath + "Reports\\TestRunReport.html";


            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(projectPath+ "\\TestData\\TestData.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            baseURL = xlRange.Cells[testIndex,1].Value2.ToString();

            xlWorkbook.Close();
            xlApp.Quit();

            return baseURL;
        }

        public String GetSearchInput (int testIndex)
        {
            String searchInput = null;

            ////To obtain the current solution path/project path
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(projectPath + "\\TestData\\TestData.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            searchInput = xlRange.Cells[testIndex,2].Value2.ToString();

            xlWorkbook.Close();
            xlApp.Quit();

            return searchInput;
        }

        public String GetCountry (int testIndex)
        {
            String country = null;

            ////To obtain the current solution path/project path
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(projectPath + "\\TestData\\TestData.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            country = xlRange.Cells[testIndex,3].Value2.ToString();

            xlWorkbook.Close();
            xlApp.Quit();

            return country;
        }

        public int GetLastTestIndex()
        {
            string pth = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
            string actualPath = pth.Substring(0, pth.LastIndexOf("bin"));
            string projectPath = new Uri(actualPath).LocalPath;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(projectPath + "\\TestData\\TestData.xlsx");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int lastTestIndex = 2;
            


            while (xlRange.Cells[lastTestIndex,1].Value2 != null)
            {
                lastTestIndex++;
            }

            return lastTestIndex-1;
        }
    }
}
