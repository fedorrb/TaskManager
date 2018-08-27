using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TaskManager
{
    public class stPath
    {
        public string DBDataSource { get; set; }
        public string DBDatabase { get; set; }
        public int LastUser { get; set; }
        public string PathReport { get; set; }
        public bool CheckFirebirdDll { get; set; }
        private string nameFirebirdDll;

        public stPath()
        {
            DBDataSource = String.Empty;
            DBDatabase = String.Empty;
            LastUser = 0;
            PathReport = String.Empty;
            nameFirebirdDll = "FirebirdSql.Data.FirebirdClient.dll";
            CheckFirebirdDll = true;
        }
        /// <summary>
        /// Чтение файла конфигурации
        /// </summary>
        public void LoadIniFile()
        {
            FileOperation fo = new FileOperation();
            string path = String.Empty;
            path = System.IO.Directory.GetCurrentDirectory();
            string pathDll = path;
            path += @"\taskmanager.ini";
            int elements = 0;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string[] allLines = System.IO.File.ReadAllLines(path);
                    elements = allLines.Length;
                    switch (elements)
                    {
                        case 4:
                            PathReport = allLines[3];
                            goto case 3;
                        case 3:
                            LastUser = int.Parse(allLines[2]);
                            goto case 2;
                        case 2:
                            DBDatabase = allLines[1];
                            goto case 1;
                        case 1:
                            DBDataSource = allLines[0];
                            break;
                        case 0:
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    DBDataSource = @"10.0.8.196";
                    DBDatabase = @"d:\taskman\db\ioc.fdb";
                    LastUser = 0;
                    PathReport = @"d:\taskman\report\";
                }
            }
            else
            {
                DBDataSource = @"10.0.8.196";
                DBDatabase = @"d:\taskman\db\ioc.fdb";
                LastUser = 0;
                PathReport = @"d:\taskman\report\";
            }

            //if (Directory.Exists(PathReport) == false)
            //{
            //    fo.MakeDir(PathReport);
            //}
            pathDll += "\\" + nameFirebirdDll;
            if (File.Exists(pathDll) == false)
            {
                MessageBox.Show("Відсутній файл бібліотеки " + pathDll, "Помилка!");
                CheckFirebirdDll = false;
            }
        }

        /// <summary>
        /// Запись файла конфигурации
        /// </summary>
        public void SaveIniFile()
        {
            string[] s = { DBDataSource,
                             DBDatabase,
                             LastUser.ToString(),
                             PathReport
                         };
            string path = String.Empty;
            path = Application.StartupPath;
            path += @"\taskmanager.ini";
            System.IO.File.WriteAllLines(path, s);
        }
    }
}
