using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Capstone;

namespace Capstone.Classes
{
    /// <summary>
    /// This class should contain any and all details of access to files
    /// </summary>
    public class FileAccess
    {
        private const string sourceFile = @"C:\Catering\cateringsystem.csv";
        private const string destinationLogFile = @"C:\Catering\log.txt";
        private const string destinationTotalSalesFile = @"C:\Catering\totalsales.txt";
        private const string SourceTotalSalesFile = @"C:\Catering\totalsalessource.txt";
        CateringSystem auditList = new CateringSystem();
        public void ReadFiles(CateringSystem system) // reads data from a csv file to populate CateringItem subclasses with objects
        {
            using (StreamReader fileInput = new StreamReader(sourceFile))
            {
                while (!fileInput.EndOfStream)
                {
                    string line = fileInput.ReadLine();
                    string[] CateringItemArray = line.Split("|");
                    
                    if (CateringItemArray[0] == "B")
                    {
                        Beverages bev = new Beverages(CateringItemArray[2], double.Parse(CateringItemArray[3]), CateringItemArray[1], 10);
                        system.AddCateringItem( bev);
                    }

                    else if (CateringItemArray[0] == "A")
                    {
                        Appetizers app = new Appetizers(CateringItemArray[2], double.Parse(CateringItemArray[3]), CateringItemArray[1], 10);
                        system.AddCateringItem( app);
                    }

                    else if (CateringItemArray[0] == "E")
                    {
                        Entrees ent = new Entrees(CateringItemArray[2], double.Parse(CateringItemArray[3]), CateringItemArray[1], 10);
                        system.AddCateringItem( ent);
                    }

                    else if (CateringItemArray[0] == "D")
                    {
                        Dessert des = new Dessert(CateringItemArray[2], double.Parse(CateringItemArray[3]), CateringItemArray[1], 10);
                        system.AddCateringItem( des);
                    }
                }
            }
        }
     
        public void WriteAuditLog(CateringSystem system) 
        {
            using (StreamWriter sw = new StreamWriter(destinationLogFile, true))
            {
                List<string> auditLogStrings = system.GetAuditEntries();
                foreach (string log in auditLogStrings)
                {
                    sw.WriteLine(log);
                    
                }
            }
        }

        // These files should be read from / written to in the DataDirectory
        private const string CateringFileName = @"cateringsystem.csv";
        private const string SourceReportFileName = @"totalsales.txt";
    }
}
