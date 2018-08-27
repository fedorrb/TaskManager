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
    public class db_otg
    {
        //строка подключения
        FbConnectionStringBuilder cs;
        private string OTG;
        private int count;
        public string GetOTG()
        {
            return this.OTG;
        }
        public void SetOTG(string otg)
        {
            if (OTG.Length == 0)
                OTG = otg;
        }
        public int GetCountApplicant()
        {
            return count;
        }
        public string error;

        public db_otg()
        {
            cs = new FbConnectionStringBuilder();
            //установка значений строки подключения
            cs.DataSource = "localhost";
            cs.Database = "C:\\OTG\\DB\\OTG.FDB";
            cs.UserID = "SYSDBA";
            cs.Password = "masterkey";
            cs.Dialect = 3;
            OTG = string.Empty;
            error = string.Empty;
            count = 0;
            GetOTGFromDB();
        }
        #region SELECT FROM DB
        /// <summary>
        /// есть ли записи в базе заявлений
        /// </summary>
        private void GetCountOTGFromDB()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT COUNT(*) FROM APPLICANT");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "APPLICANT");
            connection.Close();
            string countStr = string.Empty;
            DataRowCollection dra = myDataSet.Tables["APPLICANT"].Rows;
            foreach (DataRow dr in dra)
            {
                countStr = dr[0].ToString();
                break;
            }
            if (countStr != "0")
            {
                if (!int.TryParse(countStr, out count))
                    count = 0;
            }
        }
        /// <summary>
        ///  есть ли запись в базе адресов
        /// </summary>
        /// <param name="id">id заявления</param>
        /// <param name="fact">0-фактический, 1-регистрационный</param>
        /// <returns></returns>
        private bool AddressIsRecordExists(string id, string fact)
        {
            bool result = false;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT COUNT(*) FROM ADDRESS WHERE ID = {0} AND FACT = {1}", id, fact);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "ADDR");
            connection.Close();
            string countStr = string.Empty;
            DataRowCollection dra = myDataSet.Tables["ADDR"].Rows;
            foreach (DataRow dr in dra)
            {
                countStr = dr[0].ToString();
                break;
            }
            if (countStr != "0")
                result = true;
            return result;
        }
        /// <summary>
        /// найти номер ОТГ в базе заявлений
        /// </summary>
        private void GetOTGFromDB()
        {
            if (CheckConnection())
            {
                GetCountOTGFromDB();
                if (count > 0)
                {
                    string connectionString = cs.ToString();
                    FbConnection connection = new FbConnection(connectionString);
                    connection.Open();
                    string selectSQL;
                    selectSQL = string.Format("SELECT DISTINCT LOCAL_COMMUNITY FROM APPLICANT");
                    DataSet myDataSet = new DataSet();
                    FbCommand sel_com = new FbCommand(selectSQL, connection);
                    FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
                    myDataAdapter.Fill(myDataSet, "APPLICANT");
                    connection.Close();
                    DataRowCollection dra = myDataSet.Tables["APPLICANT"].Rows;
                    foreach (DataRow dr in dra)
                    {
                        OTG = dr[0].ToString();
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// получить список годов из базы заявлений
        /// </summary>
        /// <returns>список годов</returns>
        public List<string> GetMinYear()
        {
            List<string> years = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT DISTINCT ANNUM FROM APPLICANT ORDER BY ANNUM");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "APPLICANT");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["APPLICANT"].Rows;
            foreach (DataRow dr in dra)
            {
                years.Add(dr[0].ToString());
            }
            return years;
        }
        /// <summary>
        /// получить список заявлений в году
        /// </summary>
        /// <param name="yy">год</param>
        /// <returns></returns>
        public DataTable GetApplicant(string yy)
        {
            if (yy.Length == 0)
                yy = "2018";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            //selectSQL = string.Format(@"SELECT NUMBER_APPLICANT, SURNAME, FIRSTNAME, MIDDLENAME, ID_CODE,
//DATEREGISTR, SENT, WRONG FROM APPLICANT WHERE ANNUM = {0}  ORDER BY NUMBER_APPLICANT", yy);
            selectSQL = string.Format(@"SELECT * FROM APPLICANT WHERE ANNUM = {0}  ORDER BY NUMBER_APPLICANT", yy);
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            connection.Close();
            DataTable dt1 = new DataTable("APPLICANT");
            myDataAdapter.Fill(dt1);
            return dt1;
        }
        /// <summary>
        /// получить заявление по ID
        /// </summary>
        /// <param name="id">ID номер заявления</param>
        /// <returns>запись БД</returns>
        public DataTable GetCurrentApplicant(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM APPLICANT WHERE ID = {0}", id);
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            connection.Close();
            DataTable dt1 = new DataTable("APPLICANT");
            myDataAdapter.Fill(dt1);
            return dt1;
        }
        /// <summary>
        /// вернуть максимальный номер заявления из журнала по заданным параметрам
        /// </summary>
        /// <param name="otg">ОТГ</param>
        /// <param name="starosta">староста</param>
        /// <param name="year">год</param>
        /// <returns>номер заявления</returns>
        public int GetMaxNumberApplicant(string otg, string starosta, string year)
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT MAX(NUMBER_APPLICANT) FROM APPLICANT WHERE ANNUM = {0} AND LOCAL_COMMUNITY = {1} AND CAPTAIN = {2}", year, otg, starosta);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "MAXNUM");
            connection.Close();
            int maxNum = 0;
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["MAXNUM"].Rows;
            foreach (DataRow dr in dra)
            {
                strNum = dr[0].ToString();
                break;
            }
            Int32.TryParse(strNum, out maxNum);
            return maxNum;
        }
        /// <summary>
        /// найти ID по заданным критериям
        /// </summary>
        /// <param name="otg">старостат</param>
        /// <param name="starosta">староста</param>
        /// <param name="year">год</param>
        /// <param name="numApp">номер заявления</param>
        /// <returns>ID код записи</returns>
        public string GetIDByNumberApplicant(string otg, string starosta, string year, string numApp)
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT ID FROM APPLICANT WHERE ANNUM={0} AND LOCAL_COMMUNITY={1} AND CAPTAIN={2} AND NUMBER_APPLICANT={3}", year, otg, starosta, numApp);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "ID");
            connection.Close();
            string strID = string.Empty;
            DataRowCollection dra = myDataSet.Tables["ID"].Rows;
            foreach (DataRow dr in dra)
            {
                strID = dr[0].ToString();
                break;
            }
            return strID;
        }
        /// <summary>
        /// получить список заявлений в году
        /// </summary>
        /// <param name="yy">год</param>
        /// <returns></returns>
        public DataRowCollection GetAddress(string id, string fact)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM ADDRESS WHERE (ID = {0} AND FACT = {1})", id, fact);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "ADDRESS");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["ADDRESS"].Rows;
            return dra;
        }
        #endregion
        #region INSERT INTO DB
        /// <summary>
        /// вставить новую запись в журнал заявлений
        /// </summary>
        /// <param name="fields">список полей на запись</param>
        /// <returns>успешность</returns>
        public bool CreateNewApplicant(List<string> fields)
        {
            bool result = true;

            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO APPLICANT(ID, ANNUM, NUMBER_APPLICANT, LOCAL_COMMUNITY, CAPTAIN, SURNAME, FIRSTNAME, MIDDLENAME, ID_CODE,
REFUSAL, PASSPORTN, PASSPORTS, PASSPDATE, GIVEN, DOCTYPE, BIRTHDATE, PHONENUMBER, DATEREGISTR, SCAN, INDOTS, LOCALITYCODE, LOCALITYNAME, STREETCODE,
STREET, BUILDING, APARTMENT, UPSZN, FILIAL, BITRECORD, SENT, READY, WRONG, MOUNTAIN, INSURED, GENDER, PASSPDATEEND, STR1, DT1, INT1)
VALUES(@ID, @ANNUM, @NUMBER_APPLICANT, @LOCAL_COMMUNITY, @CAPTAIN, @SURNAME, @FIRSTNAME, @MIDDLENAME, @ID_CODE, @REFUSAL, @PASSPORTN, @PASSPORTS,
@PASSPDATE, @GIVEN, @DOCTYPE, @BIRTHDATE, @PHONENUMBER, @DATEREGISTR, @SCAN, @INDOTS, @LOCALITYCODE, @LOCALITYNAME, @STREETCODE, @STREET, @BUILDING,
@APARTMENT, @UPSZN, @FILIAL, @BITRECORD, @SENT, @READY, @WRONG, @MOUNTAIN, @INSURED, @GENDER, @PASSPDATEEND, @STR1, @DT1, @INT1)";
            if (fields.Count < 6)
                AddEmptyFields(ref fields);
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@ANNUM", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@NUMBER_APPLICANT", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@LOCAL_COMMUNITY", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@CAPTAIN", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@REFUSAL", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[13]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[14]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[15]));
            ins_com.Parameters.Add(new FbParameter("@PHONENUMBER", fields[16]));
            ins_com.Parameters.Add(new FbParameter("@DATEREGISTR", fields[17]));
            ins_com.Parameters.Add(new FbParameter("@SCAN", fields[18]));
            ins_com.Parameters.Add(new FbParameter("@INDOTS", fields[19]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYCODE", fields[20]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYNAME", fields[21]));
            ins_com.Parameters.Add(new FbParameter("@STREETCODE", fields[22]));
            ins_com.Parameters.Add(new FbParameter("@STREET", fields[23]));
            ins_com.Parameters.Add(new FbParameter("@BUILDING", fields[24]));
            ins_com.Parameters.Add(new FbParameter("@APARTMENT", fields[25]));
            ins_com.Parameters.Add(new FbParameter("@UPSZN", fields[26]));
            ins_com.Parameters.Add(new FbParameter("@FILIAL", fields[27]));
            ins_com.Parameters.Add(new FbParameter("@BITRECORD", fields[28]));
            ins_com.Parameters.Add(new FbParameter("@SENT", fields[29]));
            ins_com.Parameters.Add(new FbParameter("@READY", fields[30]));
            ins_com.Parameters.Add(new FbParameter("@WRONG", fields[31]));
            ins_com.Parameters.Add(new FbParameter("@MOUNTAIN", fields[32]));
            ins_com.Parameters.Add(new FbParameter("@INSURED", fields[33]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[34]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[35]));
            ins_com.Parameters.Add(new FbParameter("@STR1", fields[36]));
            ins_com.Parameters.Add(new FbParameter("@DT1", fields[37]));
            ins_com.Parameters.Add(new FbParameter("@INT1", fields[38]));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        public bool SaveAddress(List<string> fields)
        {
            bool result = true;
            if (AddressIsRecordExists(fields[0], fields[1]))
                UpdateAddress(fields);
            else
                CreateNewAddress(fields);
            return result;
        }
        /// <summary>
        /// сохранить новый адресс
        /// </summary>
        /// <param name="fields">список полей</param>
        /// <returns>успешность</returns>
        public bool CreateNewAddress(List<string> fields)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO ADDRESS(ID, FACT, SAMEADRESS, INDOTS, LOCALITYCODE, LOCALITYNAME, STREETCODE,
STREET, BUILDING, APARTMENT, LEASE, LEASEBEG, LEASEEND) VALUES(@ID, @FACT, @SAMEADRESS, @INDOTS, @LOCALITYCODE, @LOCALITYNAME, @STREETCODE,
@STREET, @BUILDING, @APARTMENT, @LEASE, @LEASEBEG, @LEASEEND)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@FACT", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@SAMEADRESS", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@INDOTS", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYCODE", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@STREETCODE", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@STREET", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@BUILDING", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@APARTMENT", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@LEASE", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@LEASEBEG", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@LEASEEND", fields[12]));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        #endregion
        #region UPDATE DB
        public bool UpdateApplicant(List<string> fields)
        {
            bool result = true;

            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;

            ins_str = @"UPDATE APPLICANT SET SURNAME=@SURNAME, FIRSTNAME=@FIRSTNAME, MIDDLENAME=@MIDDLENAME, ID_CODE=@ID_CODE,
REFUSAL=@REFUSAL, PASSPORTN=@PASSPORTN, PASSPORTS=@PASSPORTS, PASSPDATE=@PASSPDATE, GIVEN=@GIVEN, DOCTYPE=@DOCTYPE, 
BIRTHDATE=@BIRTHDATE, PHONENUMBER=@PHONENUMBER, DATEREGISTR=@DATEREGISTR, SCAN=@SCAN, INDOTS=@INDOTS, LOCALITYCODE=@LOCALITYCODE,
LOCALITYNAME=@LOCALITYNAME, STREETCODE=@STREETCODE, STREET=@STREET, BUILDING=@BUILDING, APARTMENT=@APARTMENT, UPSZN=@UPSZN, FILIAL=@FILIAL,
BITRECORD=@BITRECORD, SENT=@SENT, READY=@READY, WRONG=@WRONG, MOUNTAIN=@MOUNTAIN, INSURED=@INSURED, GENDER=@GENDER, PASSPDATEEND=@PASSPDATEEND,
STR1=@STR1, DT1=@DT1, INT1=@INT1 WHERE ID = @ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@REFUSAL", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[13]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[14]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[15]));
            ins_com.Parameters.Add(new FbParameter("@PHONENUMBER", fields[16]));
            ins_com.Parameters.Add(new FbParameter("@DATEREGISTR", fields[17]));
            ins_com.Parameters.Add(new FbParameter("@SCAN", fields[18]));
            ins_com.Parameters.Add(new FbParameter("@INDOTS", fields[19]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYCODE", fields[20]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYNAME", fields[21]));
            ins_com.Parameters.Add(new FbParameter("@STREETCODE", fields[22]));
            ins_com.Parameters.Add(new FbParameter("@STREET", fields[23]));
            ins_com.Parameters.Add(new FbParameter("@BUILDING", fields[24]));
            ins_com.Parameters.Add(new FbParameter("@APARTMENT", fields[25]));
            ins_com.Parameters.Add(new FbParameter("@UPSZN", fields[26]));
            ins_com.Parameters.Add(new FbParameter("@FILIAL", fields[27]));
            ins_com.Parameters.Add(new FbParameter("@BITRECORD", fields[28]));
            ins_com.Parameters.Add(new FbParameter("@SENT", fields[29]));
            ins_com.Parameters.Add(new FbParameter("@READY", fields[30]));
            ins_com.Parameters.Add(new FbParameter("@WRONG", fields[31]));
            ins_com.Parameters.Add(new FbParameter("@MOUNTAIN", fields[32]));
            ins_com.Parameters.Add(new FbParameter("@INSURED", fields[33]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[34]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[35]));
            ins_com.Parameters.Add(new FbParameter("@STR1", fields[36]));
            ins_com.Parameters.Add(new FbParameter("@DT1", fields[37]));
            ins_com.Parameters.Add(new FbParameter("@INT1", fields[38]));
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));

            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch
            {
                ins_tr.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
        public bool UpdateAddress(List<string> fields)
        {
            bool result = true;

            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE ADDRESS SET SAMEADRESS=@SAMEADRESS, INDOTS=@INDOTS, LOCALITYCODE=@LOCALITYCODE, LOCALITYNAME=@LOCALITYNAME,
STREETCODE=@STREETCODE, STREET=@STREET, BUILDING=@BUILDING, APARTMENT=@APARTMENT, LEASE=@LEASE, LEASEBEG=@LEASEBEG, LEASEEND=@LEASEEND
WHERE (ID = @ID AND FACT = @FACT)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@SAMEADRESS", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@INDOTS", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYCODE", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@LOCALITYNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@STREETCODE", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@STREET", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@BUILDING", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@APARTMENT", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@LEASE", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@LEASEBEG", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@LEASEEND", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@FACT", fields[1]));
            try
            {
                ins_com.ExecuteNonQuery();
            }
            catch
            {
                ins_tr.Rollback();
            }
            ins_tr.Commit();
            connection.Close();

            return result;
        }
        #endregion
        #region STORED PROCEDURES
        /// <summary>
        /// выполнить хранимую процедуру
        /// </summary>
        /// <returns>значение генератора</returns>
        public string GenerateNewID()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            FbCommand command = new FbCommand("SP_GEN_APPLICANT_ID", connection);
            command.CommandType = CommandType.StoredProcedure;
            FbParameter par = new FbParameter();
            par = command.Parameters.Add("@ID1", FbDbType.Integer);
            par.Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            string newID;
            newID = command.Parameters["@ID1"].Value.ToString();
            connection.Close();
            return newID;
        }
        #endregion
        #region DELETE
        /// <summary>
        /// удалить запись из таблицы заявлений и всех связанных с ней по внешнему ключу
        /// </summary>
        /// <param name="id">id записи</param>
        /// <returns>успех</returns>
        public bool DeleteApplicantAll(string id)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"DELETE FROM APPLICANT WHERE ID = @ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", id));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        public bool DeleteByMarkRecord(string id)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE APPLICANT SET WRONG = 1 WHERE ID = @ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", id));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        #endregion
        #region BACKUP RESTORE
        /// <summary>
        /// создать резервную копию базы заявлений
        /// </summary>
        /// <param name="nameBackupFile">имя файла резервной копии</param>
        /// <returns>успешность</returns>
        public bool BackupOTG(string nameBackupFile)
        {
            bool result = true;
            FbBackup backupSvc = new FbBackup();
            backupSvc.ConnectionString = cs.ToString();
            backupSvc.BackupFiles.Add(new FbBackupFile(nameBackupFile, 2048));
            backupSvc.Verbose = false;
            backupSvc.Options = FbBackupFlags.IgnoreLimbo;
            backupSvc.Execute();
            return result;
        }
        /// <summary>
        /// восстановить базу из резервной копии
        /// </summary>
        /// <param name="nameBackupFile">имя файла резервной копии</param>
        /// <returns>успешность</returns>
        public bool RestoreOTG(string nameBackupFile)
        {
            bool result = true;
            if (System.IO.File.Exists(cs.Database))
                cs.Database = "C:\\OTG\\DB\\OTG1.FDB";
            else
                cs.Database = "C:\\OTG\\DB\\OTG.FDB";
            FbRestore restoreSvc = new FbRestore();
            restoreSvc.ConnectionString = cs.ToString();
            restoreSvc.BackupFiles.Add(new FbBackupFile(nameBackupFile, 2048));
            restoreSvc.Verbose = false;
            restoreSvc.PageSize = 4096;
            restoreSvc.Options = FbRestoreFlags.Create | FbRestoreFlags.Replace;
            restoreSvc.Execute();
            cs.Database = "C:\\OTG\\DB\\OTG.FDB";
            return result;
        }
        #endregion
        #region SET DEFAULTS
        /// <summary>
        /// добавить значения по умолчанию
        /// </summary>
        /// <param name="fields">список полей на запись</param>
        public void AddEmptyFields(ref List<string> fields)
        {
            DateTime localDate = DateTime.Now;
            DateTime dt1900 = new DateTime(1900, 1, 1);
            fields.Add("");//5
            fields.Add("");//6
            fields.Add("");//7
            fields.Add("");//8
            fields.Add("0");//9
            fields.Add("");//10
            fields.Add("");//11
            fields.Add(localDate.ToString());//12
            fields.Add("");//13
            fields.Add("0");//14
            fields.Add(localDate.ToString());//15
            fields.Add("");//16
            fields.Add(localDate.ToString());//17
            fields.Add("0");//18
            fields.Add("");//19
            fields.Add("0");//20
            fields.Add("");//21
            fields.Add("0");//22
            fields.Add("");//23
            fields.Add("");//24
            fields.Add("");//25
            fields.Add("0");//26
            fields.Add("0");//27
            fields.Add("0");//28
            fields.Add("0");//29
            fields.Add("0");//30
            fields.Add("0");//31
            fields.Add("0");//32
            fields.Add("0");//33
            fields.Add("0");//34
            fields.Add(dt1900.ToString().Substring(0, 10));//35
            fields.Add("");//36
            fields.Add(localDate.ToString());//37
            fields.Add("0");//38
        }
        public void SetDefAddress(ref List<string> fields)
        {
            fields.Clear();
            DateTime localDate = DateTime.Now;
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("");
            fields.Add("0");
            fields.Add("");
            fields.Add("0");
            fields.Add("");
            fields.Add("");
            fields.Add("");
            fields.Add("0");
            fields.Add(localDate.ToString());
            fields.Add(localDate.ToString());
        }
        public void SetDefFamilyMembers(ref List<string> fields)
        {
            fields.Clear();
            DateTime localDate = DateTime.Now;
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("");
            fields.Add("0");
            fields.Add("");
            fields.Add("");
            fields.Add("");
            fields.Add(localDate.ToString());
            fields.Add("");
            fields.Add("");
            fields.Add(localDate.ToString());
            fields.Add("");
            fields.Add("0");
            fields.Add("0");
            fields.Add(localDate.ToString());
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("0");
            fields.Add("");
            fields.Add("0");
            fields.Add("");
            fields.Add("");
            fields.Add("0");
        }
        public void SetDefPrivilege(ref List<string> fields)
        {
            fields.Clear();
            DateTime localDate = DateTime.Now;
            fields.Add("0");
            fields.Add("0");
            fields.Add("");
            fields.Add("");
            fields.Add(localDate.ToString());
            fields.Add(localDate.ToString());
            fields.Add("");
            fields.Add("0");
            fields.Add(localDate.ToString());
            fields.Add(localDate.ToString());
            fields.Add(localDate.ToString());
            fields.Add("0");
            fields.Add("");
        }
        #endregion
        #region FAMILYMEMBERS
        /// <summary>
        /// получить список членов семьи
        /// </summary>
        /// <param name="id">id заявления</param>
        /// <returns>список членов семьи</returns>
        public DataRowCollection GetFamilyMembers(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM FAMILYMEMBERS WHERE ID = {0} ORDER BY MEMBER", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "FAMILYMEMBERS");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["FAMILYMEMBERS"].Rows;
            return dra;
        }
        /// <summary>
        /// удалить из базы члена семьи
        /// </summary>
        /// <param name="id">id заявления</param>
        /// <param name="member">номер члена семьи (условный)</param>
        /// <returns>успешность</returns>
        public bool DeleteFamilyMember(string id, string member)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"DELETE FROM FAMILYMEMBERS WHERE ID = @ID AND MEMBER = @MEMBER";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", id));
            ins_com.Parameters.Add(new FbParameter("@MEMBER", member));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        /// <summary>
        /// добавить нового члена семьи
        /// </summary>
        /// <param name="fields">список полей</param>
        /// <returns>успешность</returns>
        public bool InsertFamilyMember(List<string> fields)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO FAMILYMEMBERS(ID,MEMBER,FAMILY,ID_CODE,REFUSAL,SURNAME,FIRSTNAME,MIDDLENAME,BIRTHDATE,PASSPORTN,PASSPORTS,
PASSPDATE,GIVEN,DOCTYPE,GENDER,PASSPDATEEND,SINGLEMOM,MARRIAGE,WITHFATHER,PENSION,INVALID,UPSZN,LSNUMBER,CARE,PIB,LSNUMBER2,AGREE)
VALUES(@ID,@MEMBER,@FAMILY,@ID_CODE,@REFUSAL,@SURNAME,@FIRSTNAME,@MIDDLENAME,@BIRTHDATE,@PASSPORTN,@PASSPORTS,
@PASSPDATE,@GIVEN,@DOCTYPE,@GENDER,@PASSPDATEEND,@SINGLEMOM,@MARRIAGE,@WITHFATHER,@PENSION,@INVALID,@UPSZN,@LSNUMBER,@CARE,@PIB,@LSNUMBER2,@AGREE)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@MEMBER", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@FAMILY", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@REFUSAL", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[13]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[14]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[15]));
            ins_com.Parameters.Add(new FbParameter("@SINGLEMOM", fields[16]));
            ins_com.Parameters.Add(new FbParameter("@MARRIAGE", fields[17]));
            ins_com.Parameters.Add(new FbParameter("@WITHFATHER", fields[18]));
            ins_com.Parameters.Add(new FbParameter("@PENSION", fields[19]));
            ins_com.Parameters.Add(new FbParameter("@INVALID", fields[20]));
            ins_com.Parameters.Add(new FbParameter("@UPSZN", fields[21]));
            ins_com.Parameters.Add(new FbParameter("@LSNUMBER", fields[22]));
            ins_com.Parameters.Add(new FbParameter("@CARE", fields[23]));
            ins_com.Parameters.Add(new FbParameter("@PIB", fields[24]));
            ins_com.Parameters.Add(new FbParameter("@LSNUMBER2", fields[25]));
            ins_com.Parameters.Add(new FbParameter("@AGREE", fields[26]));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        /// <summary>
        /// изменить данные о члене семьи
        /// </summary>
        /// <param name="fields">список полей</param>
        /// <returns>успешность</returns>
        public bool UpdateFamilyMember(List<string> fields)
        {
            bool result = true;

            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE FAMILYMEMBERS SET FAMILY=@FAMILY, ID_CODE=@ID_CODE, REFUSAL=@REFUSAL, SURNAME=@SURNAME,
FIRSTNAME=@FIRSTNAME, MIDDLENAME=@MIDDLENAME, BIRTHDATE=@BIRTHDATE, PASSPORTN=@PASSPORTN, PASSPORTS=@PASSPORTS, PASSPDATE=@PASSPDATE, GIVEN=@GIVEN,
DOCTYPE=@DOCTYPE, GENDER=@GENDER, PASSPDATEEND=@PASSPDATEEND, SINGLEMOM=@SINGLEMOM, MARRIAGE=@MARRIAGE, WITHFATHER=@WITHFATHER, PENSION=@PENSION, INVALID=@INVALID, UPSZN=@UPSZN,
LSNUMBER=@LSNUMBER, CARE=@CARE, PIB=@PIB, LSNUMBER2=@LSNUMBER2, AGREE=@AGREE
WHERE (ID = @ID AND MEMBER = @MEMBER)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@MEMBER", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@FAMILY", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@REFUSAL", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[13]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[14]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[15]));
            ins_com.Parameters.Add(new FbParameter("@SINGLEMOM", fields[16]));
            ins_com.Parameters.Add(new FbParameter("@MARRIAGE", fields[17]));
            ins_com.Parameters.Add(new FbParameter("@WITHFATHER", fields[18]));
            ins_com.Parameters.Add(new FbParameter("@PENSION", fields[19]));
            ins_com.Parameters.Add(new FbParameter("@INVALID", fields[20]));
            ins_com.Parameters.Add(new FbParameter("@UPSZN", fields[21]));
            ins_com.Parameters.Add(new FbParameter("@LSNUMBER", fields[22]));
            ins_com.Parameters.Add(new FbParameter("@CARE", fields[23]));
            ins_com.Parameters.Add(new FbParameter("@PIB", fields[24]));
            ins_com.Parameters.Add(new FbParameter("@LSNUMBER2", fields[25]));
            ins_com.Parameters.Add(new FbParameter("@AGREE", fields[26]));
            try
            {
                ins_com.ExecuteNonQuery();
            }
            catch
            {
                ins_tr.Rollback();
            }
            ins_tr.Commit();
            connection.Close();

            return result;
        }
        /// <summary>
        /// проверить наличие в базе такого члена семьи
        /// </summary>
        /// <param name="id">ид код заявления</param>
        /// <param name="member">номер члена семьи</param>
        /// <returns>найден или нет</returns>
        private bool FamilyMemberIsRecordExists(string id, string member)
        {
            bool result = false;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT COUNT(*) FROM FAMILYMEMBERS WHERE ID = {0} AND MEMBER = {1}", id, member);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PERSON");
            connection.Close();
            string countStr = string.Empty;
            DataRowCollection dra = myDataSet.Tables["PERSON"].Rows;
            foreach (DataRow dr in dra)
            {
                countStr = dr[0].ToString();
                break;
            }
            if (countStr != "0")
                result = true;
            return result;
        }
        public bool SaveFamilyMember(List<string> fields)
        {
            bool result = true;
            if (FamilyMemberIsRecordExists(fields[0], fields[1]))
                UpdateFamilyMember(fields);
            else
                InsertFamilyMember(fields);
            return result;
        }
        #endregion
        #region PERSON ID
        /// <summary>
        /// добавить данные о человеке по его ID, вызывается из SavePerson
        /// </summary>
        /// <param name="fields">список полей на запись</param>
        /// <returns>успешность</returns>
        public bool InsertPerson(List<string> fields)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO PERSON(ID_CODE,DATEADD,SURNAME,FIRSTNAME,MIDDLENAME,PASSPORTN,PASSPORTS,PASSPDATE,GIVEN,DOCTYPE,BIRTHDATE,PHONENUMBER,GENDER,PASSPDATEEND)
VALUES(@ID_CODE,@DATEADD,@SURNAME,@FIRSTNAME,@MIDDLENAME,@PASSPORTN,@PASSPORTS,@PASSPDATE,@GIVEN,@DOCTYPE,@BIRTHDATE,@PHONENUMBER,@GENDER,@PASSPDATEEND)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@DATEADD", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PHONENUMBER", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[13]));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
        /// <summary>
        /// изменить данные о человеке по его ID, вызывается из SavePerson
        /// </summary>
        /// <param name="fields">список полей на запись</param>
        /// <returns>успешность</returns>
        public bool UpdatePerson(List<string> fields)
        {
            bool result = true;

            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE PERSON SET SURNAME=@SURNAME, FIRSTNAME=@FIRSTNAME, MIDDLENAME=@MIDDLENAME, PASSPORTN=@PASSPORTN,
PASSPORTS=@PASSPORTS, PASSPDATE=@PASSPDATE, GIVEN=@GIVEN, DOCTYPE=@DOCTYPE, BIRTHDATE=@BIRTHDATE, PHONENUMBER=@PHONENUMBER, GENDER=@GENDER, PASSPDATEEND=@PASSPDATEEND
WHERE (ID_CODE = @ID_CODE AND DATEADD = @DATEADD)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID_CODE", fields[0]));
            ins_com.Parameters.Add(new FbParameter("@DATEADD", fields[1]));
            ins_com.Parameters.Add(new FbParameter("@SURNAME", fields[2]));
            ins_com.Parameters.Add(new FbParameter("@FIRSTNAME", fields[3]));
            ins_com.Parameters.Add(new FbParameter("@MIDDLENAME", fields[4]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTN", fields[5]));
            ins_com.Parameters.Add(new FbParameter("@PASSPORTS", fields[6]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATE", fields[7]));
            ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[8]));
            ins_com.Parameters.Add(new FbParameter("@DOCTYPE", fields[9]));
            ins_com.Parameters.Add(new FbParameter("@BIRTHDATE", fields[10]));
            ins_com.Parameters.Add(new FbParameter("@PHONENUMBER", fields[11]));
            ins_com.Parameters.Add(new FbParameter("@GENDER", fields[12]));
            ins_com.Parameters.Add(new FbParameter("@PASSPDATEEND", fields[13]));
            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch
            {
                ins_tr.Rollback();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }
        /// <summary>
        /// набор данных о человеке по его ID коду
        /// </summary>
        /// <param name="id">ID код</param>
        /// <returns>успешность</returns>
        public DataRowCollection GetPerson(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM PERSON WHERE ID_CODE = {0} ORDER BY DATEADD", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PERSON");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["PERSON"].Rows;
            return dra;
        }
        /// <summary>
        /// существует ли запись про человека по заданному условию, для вставки или обновления
        /// </summary>
        /// <param name="id">идент. код</param>
        /// <param name="dt">дата</param>
        /// <returns></returns>
        private bool PersonIsRecordExists(string id, string dt)
        {
            bool result = false;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT COUNT(*) FROM PERSON WHERE ID_CODE = {0} AND \"DATEADD\" = '{1}'", id, dt);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PERSON");
            connection.Close();
            string countStr = string.Empty;
            DataRowCollection dra = myDataSet.Tables["PERSON"].Rows;
            foreach (DataRow dr in dra)
            {
                countStr = dr[0].ToString();
                break;
            }
            if (countStr != "0")
                result = true;
            return result;
        }
        /// <summary>
        /// добавить данные о человеке по его ID
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public bool SavePerson(List<string> fields)
        {
            bool result = true;
            if (PersonIsRecordExists(fields[0], fields[1]))
                UpdatePerson(fields);
            else
                InsertPerson(fields);
            return result;
        }
        #endregion
        #region SCAN LIST
        public bool DeleteScanList(string id)
        {
            bool result = false;
            if (!CheckSendAppl(id))
            {
                string connectionString = cs.ToString();
                FbConnection connection = new FbConnection(connectionString);
                connection.Open();
                string ins_str;
                ins_str = @"DELETE FROM SCANLIST WHERE ID = @ID";
                FbTransaction ins_tr = connection.BeginTransaction();
                FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                ins_com.Parameters.Add(new FbParameter("@ID", id));
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
                connection.Close();
                result = true;
            }
            return result;
        }
        public bool SaveScanList(string id, string scanFileName)
        {
            bool result = false;
            if (!CheckSendAppl(id))
            {
                string connectionString = cs.ToString();
                FbConnection connection = new FbConnection(connectionString);
                connection.Open();
                string ins_str;
                ins_str = @"INSERT INTO SCANLIST(ID,FILENAME) VALUES(@ID,@FILENAME)";
                FbTransaction ins_tr = connection.BeginTransaction();
                FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                ins_com.Parameters.Add(new FbParameter("@ID", id));
                ins_com.Parameters.Add(new FbParameter("@FILENAME", scanFileName));
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
                connection.Close();
                result = true;
            }
            return result;
        }
        public DataRowCollection GetScanList(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM SCANLIST WHERE ID = {0}", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "SCANLIST");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["SCANLIST"].Rows;
            return dra;
        }
        #endregion
        #region CODE PTK
        /// <summary>
        /// запись ответа в базу
        /// </summary>
        /// <param name="key">набор ключевых полей</param>
        /// <param name="fields">поля ответа</param>
        /// <returns>успешность записи</returns>
        public bool SaveAnswer1(List<string> key, List<string> fields)
        {
            bool result = false;
            string id = GetIDByNumberApplicant(key[1], key[2], key[5], key[3]);
            if (id.Length > 0)
            {
                string connectionString = cs.ToString();
                FbConnection connection = new FbConnection(connectionString);
                connection.Open();
                string ins_str;
                ins_str = @"UPDATE APPKINDHELP SET ANSWER=@ANSWER,DATEANSWER=@DATEANSWER,DATEBEG=@DATEBEG,DATEEND=@DATEEND,SUMMA=@SUMMA,LSNLS=@LSNLS
WHERE (ID = @ID AND KFN = @KFN)";
                FbTransaction ins_tr = connection.BeginTransaction();
                FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                ins_com.Parameters.Add(new FbParameter("@ID", id));
                ins_com.Parameters.Add(new FbParameter("@KFN", key[7]));
                ins_com.Parameters.Add(new FbParameter("@ANSWER", fields[2]));
                ins_com.Parameters.Add(new FbParameter("@DATEANSWER", fields[1]));
                ins_com.Parameters.Add(new FbParameter("@DATEBEG", fields[3]));
                ins_com.Parameters.Add(new FbParameter("@DATEEND", fields[4]));
                ins_com.Parameters.Add(new FbParameter("@SUMMA", fields[6]));
                ins_com.Parameters.Add(new FbParameter("@LSNLS", fields[7]));
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
                connection.Close();
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codePtk"></param>
        /// <returns></returns>
        public DataRowCollection GetAnswer1ByPtk(string id, string codePtk)
        {
            if (id.Length == 0)
                id = "0";
            if (codePtk.Length == 0)
                codePtk = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM APPKINDHELP WHERE ID = {0} AND CODEPTK = {1} AND ANSWER > 0 ORDER BY ID", id, codePtk);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "APPKINDHELP");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["APPKINDHELP"].Rows;
            return dra;
        }
        public bool DeleteAppKindHelp(string id, string codePtk)
        {
            bool result = false;
            if (id.Length > 0 && codePtk.Length > 0)
            {
                if (!CheckSendAppl(id))
                {
                    string connectionString = cs.ToString();
                    FbConnection connection = new FbConnection(connectionString);
                    connection.Open();
                    string ins_str;
                    ins_str = @"DELETE FROM APPKINDHELP WHERE ID = @ID AND CODEPTK=@CODEPTK";
                    FbTransaction ins_tr = connection.BeginTransaction();
                    FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                    ins_com.Parameters.Add(new FbParameter("@ID", id));
                    ins_com.Parameters.Add(new FbParameter("@CODEPTK", codePtk));
                    ins_com.ExecuteNonQuery();
                    ins_tr.Commit();
                    connection.Close();
                    result = true;
                }
            }
            return result;
        }
        public bool SaveAppKindHelp(string id, string codePtk, string kindHelp, string kfn)
        {
            bool result = false;
            if (id.Length > 0 && codePtk.Length > 0 && kfn.Length > 0)
            {
                if (!CheckSendAppl(id))
                {
                    string connectionString = cs.ToString();
                    FbConnection connection = new FbConnection(connectionString);
                    connection.Open();
                    string ins_str;
                    ins_str = @"INSERT INTO APPKINDHELP(ID,CODEPTK,KINDHELP,KFN) VALUES(@ID,@CODEPTK,@KINDHELP,@KFN)";
                    FbTransaction ins_tr = connection.BeginTransaction();
                    FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                    ins_com.Parameters.Add(new FbParameter("@ID", id));
                    ins_com.Parameters.Add(new FbParameter("@CODEPTK", codePtk));
                    ins_com.Parameters.Add(new FbParameter("@KINDHELP", kindHelp));
                    ins_com.Parameters.Add(new FbParameter("@KFN", kfn));
                    ins_com.ExecuteNonQuery();
                    ins_tr.Commit();
                    connection.Close();
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codePtk"></param>
        /// <returns></returns>
        public DataRowCollection GetAppKindHelp(string id, string codePtk)
        {
            if (id.Length == 0)
                id = "0";
            if (codePtk.Length == 0)
                codePtk = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT KINDHELP FROM APPKINDHELP WHERE ID = {0} AND CODEPTK = {1} ORDER BY ID", id, codePtk);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "APPKINDHELP");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["APPKINDHELP"].Rows;
            return dra;
        }
        /// <summary>
        /// список порядковых номеров кодов ПТК
        /// код в базе (+2)
        /// </summary>
        /// <param name="id">ид код заявления</param>
        /// <returns>список</returns>
        public DataRowCollection GetCheckedCodePtk(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT DISTINCT CODEPTK FROM APPKINDHELP WHERE ID = {0} ORDER BY ID", id);
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
        #region PRIVILEGE
        public DataTable GetTablePrivilegeById(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CATEGORY,DOCUMENTS,DOCUMENTN,DOCBEG,DOCEND,GIVEN,ANSWER FROM PRIVILEGE WHERE ID = {0} ORDER BY CATEGORY", id);
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            connection.Close();
            DataTable dt1 = new DataTable("PRIVILEGE");
            myDataAdapter.Fill(dt1);
            return dt1;
        }
        public DataRowCollection GetRowsPrivilegeById(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CATEGORY,DOCUMENTS,DOCUMENTN,DOCBEG,DOCEND,GIVEN,ANSWER FROM PRIVILEGE WHERE ID = {0} ORDER BY CATEGORY", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PRIVILEGE");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["PRIVILEGE"].Rows;
            return dra;
        }
        public bool SavePrivilege(List<string> fields)
        {
            bool result = false;
                if (!CheckSendAppl(fields[0]))
                {
                    string connectionString = cs.ToString();
                    FbConnection connection = new FbConnection(connectionString);
                    connection.Open();
                    string ins_str;
                    ins_str = @"INSERT INTO PRIVILEGE(ID,CATEGORY,DOCUMENTS,DOCUMENTN,DOCBEG,DOCEND,GIVEN,ANSWER,DATEANSWER,DATEBEG,DATEEND,SUMMA,NLS)
VALUES(@ID,@CATEGORY,@DOCUMENTS,@DOCUMENTN,@DOCBEG,@DOCEND,@GIVEN,@ANSWER,@DATEANSWER,@DATEBEG,@DATEEND,@SUMMA,@NLS)";
                    FbTransaction ins_tr = connection.BeginTransaction();
                    FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                    ins_com.Parameters.Add(new FbParameter("@ID", fields[0]));
                    ins_com.Parameters.Add(new FbParameter("@CATEGORY", fields[1]));
                    ins_com.Parameters.Add(new FbParameter("@DOCUMENTS", fields[2]));
                    ins_com.Parameters.Add(new FbParameter("@DOCUMENTN", fields[3]));
                    ins_com.Parameters.Add(new FbParameter("@DOCBEG", fields[4]));
                    ins_com.Parameters.Add(new FbParameter("@DOCEND", fields[5]));
                    ins_com.Parameters.Add(new FbParameter("@GIVEN", fields[6]));
                    ins_com.Parameters.Add(new FbParameter("@ANSWER", fields[7]));
                    ins_com.Parameters.Add(new FbParameter("@DATEANSWER", fields[8]));
                    ins_com.Parameters.Add(new FbParameter("@DATEBEG", fields[9]));
                    ins_com.Parameters.Add(new FbParameter("@DATEEND", fields[10]));
                    ins_com.Parameters.Add(new FbParameter("@SUMMA", fields[11]));
                    ins_com.Parameters.Add(new FbParameter("@NLS", fields[12]));
                    ins_com.ExecuteNonQuery();
                    ins_tr.Commit();
                    connection.Close();
                    result = true;
                }
            return result;
        }
        public bool DelPrivilege(string id, string category)
        {
            bool result = false;
            if (!CheckSendAppl(id))
            {
                string connectionString = cs.ToString();
                FbConnection connection = new FbConnection(connectionString);
                connection.Open();
                string ins_str;
                ins_str = @"DELETE FROM PRIVILEGE WHERE ID = @ID AND CATEGORY = @CATEGORY";
                FbTransaction ins_tr = connection.BeginTransaction();
                FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                ins_com.Parameters.Add(new FbParameter("@ID", id));
                ins_com.Parameters.Add(new FbParameter("@CATEGORY", category));
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
                connection.Close();
                result = true;
            }
            return result;
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
                    error = "";
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
        /// <summary>
        /// проверка отослано ли заявление
        /// </summary>
        /// <param name="id">ид код заявления</param>
        /// <returns>да/нет</returns>
        public bool CheckSendAppl(string id)
        {
            bool result = false;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT SENT FROM APPLICANT WHERE ID = {0}", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "SENTAPP");
            connection.Close();
            string sent = string.Empty;
            DataRowCollection dra = myDataSet.Tables["SENTAPP"].Rows;
            foreach (DataRow dr in dra)
            {
                sent = dr[0].ToString();
                break;
            }
            if (sent != "0")
                result = true;
            return result;
        }
        /// <summary>
        /// получить список дел готовых к отправке
        /// </summary>
        /// <returns>список дел готовых к отправке</returns>
        public DataRowCollection GetReadyToSend()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM APPLICANT WHERE READY = 1 AND SENT = 0 ORDER BY ID");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "IDAPPLICANT");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["IDAPPLICANT"].Rows;
            return dra;
        }
        /// <summary>
        /// получить список видов выплат в заявлении
        /// </summary>
        /// <param name="id">ид код заявления</param>
        /// <returns>список видов выплат</returns>
        public DataRowCollection GetListKfnID(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT CODEPTK, KFN FROM APPKINDHELP WHERE ID = {0}", id);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "KFN");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["KFN"].Rows;
            return dra;
        }
        /// <summary>
        /// пометить записи отосланные в УПСЗН
        /// </summary>
        /// <param name="id">ид код заявления</param>
        /// <returns>успешность</returns>
        public bool MarkSent(string id)
        {
            bool result = true;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE APPLICANT SET SENT = 1 WHERE ID = @ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", id));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            return result;
        }
    }
}
