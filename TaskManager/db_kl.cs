using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;


namespace OTG
{
    /// <summary>
    /// класс для работы с БД
    /// </summary>
    public class db_kl
    {
        //строка подключения
        private FbConnectionStringBuilder cs;
        public string error { get; set; }

        public db_kl()
        {
            cs = new FbConnectionStringBuilder();
            //установка значений строки подключения
            cs.DataSource = "localhost";
            cs.Database = "C:\\OTG\\DB\\KL.FDB";
            cs.UserID = "SYSDBA";
            cs.Password = "masterkey";
            cs.Dialect = 3;
            error = string.Empty;
        }
        /// <summary>
        /// получить из БД хэш пароля для заданного пользователя
        /// </summary>
        /// <param name="otg">код ОТГ</param>
        /// <param name="county">код старостата</param>
        /// <param name="captain">код старосты</param>
        /// <returns>хэш пароля</returns>
        public string GetHashPassword(string otg, string captain)
        {
            if (otg.Length == 0)
                otg = "1";
            if (captain.Length == 0)
                captain = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT HASH FROM LOGIN WHERE OTG = {0} AND CAPTAIN = {1}", otg, captain);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "LOGIN");
            connection.Close();
            string hash = string.Empty;
            DataRowCollection dra = myDataSet.Tables["LOGIN"].Rows;
            foreach (DataRow dr in dra)
            {
                hash = dr[0].ToString();
                break;
            }
            return hash;
        }
        public List<string> GetListRayon(string otg)
        {
            List<string> rayon = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT DISTINCT RAYON FROM OTGSETTLEMENTS WHERE OTG = {0} ORDER BY RAYON", otg);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "RAYON");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["RAYON"].Rows;
            foreach (DataRow dr in dra)
            {
                rayon.Add(dr[0].ToString());
            }
            return rayon;
        }
        #region DOCTYPES
        /// <summary>
        /// список документов для заявления
        /// </summary>
        /// <returns>список документов для заявления</returns>
        public List<string> GetDocTypes()
        {
            List<string> docs = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT DOCNAME FROM DOCTYPES ORDER BY DOCTYPE");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "DOCTYPES");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["DOCTYPES"].Rows;
            foreach (DataRow dr in dra)
            {
                docs.Add(dr[0].ToString());
            }
            return docs;
        }
        /// <summary>
        /// получить название документа по индексу
        /// </summary>
        /// <param name="code">индекс</param>
        /// <returns>название документа</returns>
        public string GetNameDoc(string code)
        {
            if (code.Length == 0)
                code = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT DOCNAME FROM DOCTYPES WHERE DOCTYPE = {0}", code);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "DOCNAME");
            connection.Close();
            string docName = string.Empty;
            DataRowCollection dra = myDataSet.Tables["DOCNAME"].Rows;
            foreach (DataRow dr in dra)
            {
                docName = dr[0].ToString();
                break;
            }
            return docName;
        }
        /// <summary>
        /// вернуть индекс документа
        /// </summary>
        /// <param name="name">название</param>
        /// <returns>индекс</returns>
        public string GetIdxDoc(string name)
        {
            if (name.Length == 0)
                name = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT DOCTYPE FROM DOCTYPES WHERE DOCNAME = {0}", name);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "DOCIDX");
            connection.Close();
            string docIdx = string.Empty;
            DataRowCollection dra = myDataSet.Tables["DOCIDX"].Rows;
            foreach (DataRow dr in dra)
            {
                docIdx = dr[0].ToString();
                break;
            }
            return docIdx;
        }
        #endregion
        #region LOCALITYTYPE
        /// <summary>
        /// список типов населенных пунктов
        /// </summary>
        /// <returns>список типов населенных пунктов</returns>
        public List<string> GetLocalityTypes()
        {
            List<string> localities = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT TYPE FROM SETTLEMENTS ORDER BY CODE");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "LOCTYPES");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["LOCTYPES"].Rows;
            foreach (DataRow dr in dra)
            {
                localities.Add(dr[0].ToString());
            }
            return localities;
        }
        /// <summary>
        /// получить тип населенного пункта по индексу
        /// </summary>
        /// <param name="code">индекс</param>
        /// <returns>тип населенного пункта</returns>
        public string GetNameLocality(string code)
        {
            if (code.Length == 0)
                code = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT TYPE FROM SETTLEMENTS WHERE CODE = {0}", code);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "LOCNAME");
            connection.Close();
            string locName = string.Empty;
            DataRowCollection dra = myDataSet.Tables["LOCNAME"].Rows;
            foreach (DataRow dr in dra)
            {
                locName = dr[0].ToString();
                break;
            }
            return locName;
        }
        /// <summary>
        /// вернуть индекс типа населенного пункта
        /// </summary>
        /// <param name="name">тип населенного пункта</param>
        /// <returns>индекс</returns>
        public string GetIdxLocality(string name)
        {
            if (name.Length == 0)
                name = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT CODE FROM SETTLEMENTS WHERE TYPE = {0}", name);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "LOCIDX");
            connection.Close();
            string locIdx = string.Empty;
            DataRowCollection dra = myDataSet.Tables["LOCIDX"].Rows;
            foreach (DataRow dr in dra)
            {
                locIdx = dr[0].ToString();
                break;
            }
            return locIdx;
        }
        #endregion
        #region STREETTYPE
        /// <summary>
        /// список типов улиц
        /// </summary>
        /// <returns>список типов улиц</returns>
        public List<string> GetStreetTypes()
        {
            List<string> streets = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT TYPE FROM TYPESTREET ORDER BY CODE");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "STREET");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["STREET"].Rows;
            foreach (DataRow dr in dra)
            {
                streets.Add(dr[0].ToString());
            }
            return streets;
        }
        /// <summary>
        /// получить тип улицы по индексу
        /// </summary>
        /// <param name="code">индекс</param>
        /// <returns>тип улицы</returns>
        public string GetNameStreetType(string code)
        {
            if (code.Length == 0)
                code = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT TYPE FROM TYPESTREET WHERE CODE = {0}", code);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "STREET");
            connection.Close();
            string locName = string.Empty;
            DataRowCollection dra = myDataSet.Tables["STREET"].Rows;
            foreach (DataRow dr in dra)
            {
                locName = dr[0].ToString();
                break;
            }
            return locName;
        }
        /// <summary>
        /// вернуть индекс типа улицы
        /// </summary>
        /// <param name="name">тип улицы</param>
        /// <returns>индекс</returns>
        public string GetIdxStreetType(string name)
        {
            if (name.Length == 0)
                name = "1";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT CODE FROM TYPESTREET WHERE TYPE = {0}", name);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "STREET");
            connection.Close();
            string locIdx = string.Empty;
            DataRowCollection dra = myDataSet.Tables["STREET"].Rows;
            foreach (DataRow dr in dra)
            {
                locIdx = dr[0].ToString();
                break;
            }
            return locIdx;
        }
        #endregion
        #region RESTORE
        /// <summary>
        /// восстановить базу из резервной копии
        /// </summary>
        /// <param name="nameBackupFile">имя файла резервной копии</param>
        /// <returns>успешность</returns>
        public bool RestoreKl(string nameBackupFile)
        {
            bool result = true;
            if (System.IO.File.Exists(cs.Database))
                cs.Database = "C:\\OTG\\DB\\KL1.FDB";
            else
                cs.Database = "C:\\OTG\\DB\\KL.FDB";
            FbRestore restoreSvc = new FbRestore();
            restoreSvc.ConnectionString = cs.ToString();
            restoreSvc.BackupFiles.Add(new FbBackupFile(nameBackupFile, 2048));
            restoreSvc.Verbose = false;
            restoreSvc.PageSize = 4096;
            restoreSvc.Options = FbRestoreFlags.Create | FbRestoreFlags.Replace;
            restoreSvc.Execute();
            cs.Database = "C:\\OTG\\DB\\KL.FDB";
            return result;
        }
        #endregion
        #region HELPCODE 
        public DataRowCollection GetCodeHelp(string ptk)
        {
            if (ptk.Length == 0)
                ptk = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CODEHELP, NAMEHELP FROM KINDHELP WHERE CODEPTK = {0}", ptk);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "KINDHELP");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["KINDHELP"].Rows;
            return dra;
        }
        public DataRowCollection GetCodePtkById(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CODEPTK FROM CODEPTK WHERE ID = {0}", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PTK");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["PTK"].Rows;
            return dra;
        }
        public DataRowCollection GetCodeHelpSelf(string ptk)
        {
            if (ptk.Length == 0)
                ptk = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CODEPTK AS CODEHELP, NAMEPTK AS NAMEHELP FROM CODEPTK WHERE ID_PARENT = {0}", ptk);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "KINDHELP");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["KINDHELP"].Rows;
            return dra;
        }
        public DataRowCollection GetCodePtk()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM CODEPTK ORDER BY ID");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "CODEPTK");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["CODEPTK"].Rows;
            return dra;
        }
        public DataRowCollection GetCodePtkSelf()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT ID, CODEPTK, NAMEPTK FROM CODEPTK WHERE ID_PARENT = 1 ORDER BY ID");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "CODEPTK");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["CODEPTK"].Rows;
            return dra;
        }
        #endregion
        #region PILGA
        public DataRowCollection GetKindPilga()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CODE, CATNAME FROM CATEGPILG ORDER BY CODE");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "CATEGPILG");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["CATEGPILG"].Rows;
            return dra;
        }
        public DataRowCollection GetNamePilga(string code)
        {
            if (code.Length == 0)
                code = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CATNAME FROM CATEGPILG WHERE CODE = {0}", code);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "CATNAME");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["CATNAME"].Rows;
            return dra;
        }
        #endregion
        public bool CheckConnection()
        {
            bool result = false;
            string path = cs.Database;
            if (System.IO.File.Exists(path))
            {
                try
                {
                    string connectionString = cs.ToString();
                    FbConnection connection = new FbConnection(connectionString);
                    connection.Open();
                    connection.Close();
                    error = string.Empty;
                    result = true;
                }
                catch
                {
                    result = false;
                    error = string.Format("Помилка при з'єднанні з базою даних {0}", path);
                }
            }
            else
            {
                error = string.Format("База даних {0} не знайдена", path);
            }
            return result;
        }
    }
}
