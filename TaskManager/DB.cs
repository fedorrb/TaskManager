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

namespace TaskManager
{
    class DB
    {
        //строка подключения
        FbConnectionStringBuilder cs;
        public string error;
        public DB()
        {
            cs = new FbConnectionStringBuilder();
            //установка значений строки подключения
            cs.DataSource = "10.0.8.196";
            //cs.DataSource = "localhost";
            cs.Database = "d:\\taskman\\db\\ioc.fdb";
            cs.UserID = "SYSDBA";
            cs.Password = "masterkey";
            cs.Dialect = 3;
            cs.Port = 3050;
        }

        public bool CheckConnection()
        {
            bool result = false;
            string path = cs.Database;
            //if (System.IO.File.Exists(path))
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
            //else
            //{
                //error = string.Format("База даних {0} не знайдена", path);
            //}
            return result;
        }

        public void GetAllEmloyees(ref List<Employees> empl)
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT ID, FULLNAME FROM EMPLOYEES ORDER BY FULLNAME");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "PERSON");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["PERSON"].Rows;
            empl.Clear();
            foreach (DataRow dr in dra)
            {
                Employees employee = new Employees();
                employee.ID = int.Parse(dr[0].ToString());
                employee.FullName = dr[1].ToString();
                empl.Add(employee);
            }
        }
        /// <summary>
        /// отримати список відповідності постановник - виконавець
        /// </summary>
        /// <param name="allLevel"></param>
        public void GetAllLevels(ref List<Level> allLevel)
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT * FROM LEVELRELATION");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "LEVEL");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["LEVEL"].Rows;
            allLevel.Clear();
            foreach (DataRow dr in dra)
            {
                Level lvl = new Level();
                lvl.masterID = int.Parse(dr[0].ToString());
                lvl.slaveID = int.Parse(dr[1].ToString());
                allLevel.Add(lvl);
            }
        }
        /// <summary>
        /// список всіх можливих виконавців для заданого коду постановника
        /// </summary>
        /// <param name="masterID">код постановника</param>
        /// <returns>список всіх можливих виконавців</returns>
        public List<Employees> GetSlaves(int masterID)
        {
            List<Level> allLevels = new List<Level>();
            GetAllLevels(ref allLevels);
            List<int> slaves = FindSlavesRecursively(ref allLevels, masterID);

            List<Employees> emplList = new List<Employees>();
            List<Employees> newEmplList = new List<Employees>();
            GetAllEmloyees(ref emplList);
            foreach (int s in slaves)
            {
                foreach(Employees currEmp in emplList)
                {
                    if (s == currEmp.ID)
                        newEmplList.Add(currEmp);
                }
            }

            return newEmplList;
        }
        /// <summary>
        /// пошук всіх кодів підлеглих
        /// </summary>
        /// <param name="allLevel">повний список відносин</param>
        /// <param name="masterID">код постановника</param>
        /// <returns></returns>
        private List<int> FindSlavesRecursively(ref List<Level> allLevel, int masterID)
        {
            List<int> allSlaves = new List<int>();
            allSlaves.Add(masterID);
            foreach (Level curLvl in allLevel)
            {
                if (curLvl.masterID == masterID)
                {
                    allSlaves.Add(curLvl.slaveID);
                    allSlaves.AddRange(FindSlavesRecursively(ref allLevel, curLvl.slaveID));
                }
            }
            allSlaves = allSlaves.Distinct().ToList();
            return allSlaves;
        }
        /// <summary>
        /// отримати перелік тем
        /// </summary>
        /// <returns>список тем</returns>
        public List<string> GetSubjects()
        {
            List<string> subjects = new List<string>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"SELECT SUBJECT, FREQUENCY FROM SUBJECTS ORDER BY FREQUENCY, SUBJECT");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "FREQ");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["FREQ"].Rows;
            subjects.Clear();
            foreach (DataRow dr in dra)
            {
                subjects.Add(dr[0].ToString());
            }
            return subjects;
        }
        /// <summary>
        /// зберегти тему
        /// </summary>
        /// <param name="subject">тема</param>
        public void SaveSubject(string subject)
        {
            int freq = GetSubjectFrequency(subject);
            if (freq > 0)
            {
                freq++;
                UpdateSubject(subject, freq);
            }
            else
            {
                InsertSubject(subject, 1);
            }
        }
        /// <summary>
        /// додати нову тему
        /// </summary>
        /// <param name="subject">тема</param>
        /// <param name="freq">частота</param>
        private void InsertSubject(string subject, int freq)
        {
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO SUBJECTS(SUBJECT, FREQUENCY, LASTDATEUSE) VALUES(@SUBJECT, @FREQUENCY, @LASTDATEUSE)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@SUBJECT", subject));
            ins_com.Parameters.Add(new FbParameter("@FREQUENCY", freq));
            ins_com.Parameters.Add(new FbParameter("@LASTDATEUSE", dt1.ToShortDateString()));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
        }
        /// <summary>
        /// оновити тему завдання
        /// </summary>
        /// <param name="subject">тема</param>
        /// <param name="freq">частота</param>
        private void UpdateSubject(string subject, int freq)
        {
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE SUBJECTS SET FREQUENCY=@FREQUENCY, LASTDATEUSE=@LASTDATEUSE WHERE SUBJECT = @SUBJECT";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@FREQUENCY", freq.ToString()));
            ins_com.Parameters.Add(new FbParameter("@LASTDATEUSE", dt1.ToShortDateString()));
            ins_com.Parameters.Add(new FbParameter("@SUBJECT", subject));
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
        }
        /// <summary>
        /// отримати частоту використання теми
        /// </summary>
        /// <param name="subject">тема</param>
        /// <returns>частота</returns>
        private int GetSubjectFrequency(string subject)
        {
            int freq = 0;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = @"SELECT FREQUENCY FROM SUBJECTS WHERE SUBJECT = @SUBJECT";
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            sel_com.Parameters.Add(new FbParameter("@SUBJECT", subject));
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "FREQ");
            connection.Close();
            string countStr = string.Empty;
            DataRowCollection dra = myDataSet.Tables["FREQ"].Rows;
            foreach (DataRow dr in dra)
            {
                countStr = dr[0].ToString();
                break;
            }
            if (countStr != "0")
            {
                if (!int.TryParse(countStr, out freq))
                    freq = 0;
            }
            return freq;
        }
        /// <summary>
        /// записати нове заавдання
        /// </summary>
        /// <param name="newTask">перелік значень полів</param>
        public void InsertTask(List<string> newTask)
        {
            if (newTask.Count != 8)
                return;
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string newId = GenerateNewID();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"INSERT INTO TASKS(ID, SUBJECT, SHORTDESCRIPTION, EXECUTORID, DEADLINE, MANAGERID, DATEINIT, PRIORITY, DATEDOC, NUMBERDOC)
VALUES (@ID, @SUBJECT, @SHORTDESCRIPTION, @EXECUTORID, @DEADLINE, @MANAGERID, @DATEINIT, @PRIORITY, @DATEDOC, @NUMBERDOC)";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", newId));
            ins_com.Parameters.Add(new FbParameter("@SUBJECT", newTask[0]));
            ins_com.Parameters.Add(new FbParameter("@SHORTDESCRIPTION", newTask[1]));
            ins_com.Parameters.Add(new FbParameter("@EXECUTORID", newTask[2]));
            ins_com.Parameters.Add(new FbParameter("@DEADLINE", newTask[3]));
            ins_com.Parameters.Add(new FbParameter("@MANAGERID", newTask[4]));
            ins_com.Parameters.Add(new FbParameter("@DATEINIT", dt1.ToShortDateString()));
            ins_com.Parameters.Add(new FbParameter("@PRIORITY", newTask[5]));
            ins_com.Parameters.Add(new FbParameter("@DATEDOC", newTask[6]));
            ins_com.Parameters.Add(new FbParameter("@NUMBERDOC", newTask[7]));
            ins_com.ExecuteNonQuery();
            ins_tr.Commit();
            connection.Close();
            SaveSubject(newTask[0]);
        }
        /// <summary>
        /// оновити статус завдання
        /// </summary>
        /// <param name="taskFields">перелік значень полів для оновлення</param>
        public void UpdateTaskExecutorManager(List<string> taskFields)
        {
            if (taskFields.Count != 5)
                return;
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE TASKS SET MAKETASK=@MAKETASK, COMMITMAKE=@COMMITMAKE,
MAKEDATE=@MAKEDATE, COMMITDATE=@COMMITDATE WHERE ID=@ID";
            if(taskFields[3].Equals("0"))
                ins_str = @"UPDATE TASKS SET MAKETASK=@MAKETASK, COMMITMAKE=@COMMITMAKE,
MAKEDATE=NULL, COMMITDATE=NULL WHERE ID=@ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", taskFields[0]));
            ins_com.Parameters.Add(new FbParameter("@MAKETASK", taskFields[3]));
            ins_com.Parameters.Add(new FbParameter("@MAKEDATE", dt1.ToShortDateString()));
            ins_com.Parameters.Add(new FbParameter("@COMMITMAKE", taskFields[4]));
            ins_com.Parameters.Add(new FbParameter("@COMMITDATE", dt1.ToShortDateString()));
            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch (Exception e)
            {
                ins_tr.Rollback();
            }
            connection.Close();
        }
        /// <summary>
        /// оновити статус завдання
        /// </summary>
        /// <param name="taskFields">перелік значень полів для оновлення</param>
        public void UpdateTaskExecutor(List<string> taskFields)
        {
            if (taskFields.Count != 4)
                return;
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE TASKS SET MAKETASK=@MAKETASK, MAKEDATE=@MAKEDATE WHERE ID=@ID";
            if(taskFields[3].Equals("0"))
                ins_str = @"UPDATE TASKS SET MAKETASK=@MAKETASK, MAKEDATE = NULL WHERE ID=@ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", taskFields[0]));
            ins_com.Parameters.Add(new FbParameter("@MAKETASK", taskFields[3]));
            ins_com.Parameters.Add(new FbParameter("@MAKEDATE", dt1.ToShortDateString()));
            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch (Exception e)
            {
                ins_tr.Rollback();
            }
            connection.Close();
        }
        /// <summary>
        /// оновити статус завдання
        /// </summary>
        /// <param name="taskFields">перелік значень полів для оновлення</param>
        public void UpdateTaskManager(List<string> taskFields)
        {
            if (taskFields.Count != 4)
                return;
            DateTime dt1 = new DateTime();
            dt1 = DateTime.Now;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE TASKS SET COMMITMAKE=@COMMITMAKE, COMMITDATE=@COMMITDATE WHERE ID=@ID";
            if (taskFields[3].Equals("0"))
                ins_str = @"UPDATE TASKS SET COMMITMAKE=@COMMITMAKE, COMMITDATE = NULL WHERE ID=@ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", taskFields[0]));
            ins_com.Parameters.Add(new FbParameter("@COMMITMAKE", taskFields[3]));
            ins_com.Parameters.Add(new FbParameter("@COMMITDATE", dt1.ToShortDateString()));
            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch (Exception e)
            {
                ins_tr.Rollback();
            }
            connection.Close();
        }
        /// <summary>
        /// выполнить хранимую процедуру
        /// </summary>
        /// <returns>значение генератора</returns>
        public string GenerateNewID()
        {
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            FbCommand command = new FbCommand("SP_GEN_NEW_TABLE_ID", connection);
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

        public DataRowCollection GetTasksForSlave(string strInSlaves, ShowMadeTask showMadeTask, int masterID)
        {
            string paramShowMade = "0,1";
            if (showMadeTask == ShowMadeTask.ShowMade)
                paramShowMade = "1";
            if (showMadeTask == ShowMadeTask.ShowNotMade)
                paramShowMade = "0";
            List<int> slaves = new List<int>();
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"select t.*, e.shortname as slavefio, m.shortname as masterfio
from tasks t
left join employees e
on t.executorid = e.id
left join employees m
on t.managerid = m.id
where t.executorid in ({0}) and t.isdelete = 0 and t.commitmake in ({1})
and (t.executorid != t.managerid or t.managerid = {2})
order by t.maketask, t.deadline", strInSlaves, paramShowMade, masterID);
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "FREQ");
            connection.Close();
            DataRowCollection dra = myDataSet.Tables["FREQ"].Rows;
            return dra;
        }

        private string GenerateHash(string employeeID, StringBuilder pass)
        {
            string hash;//хэш в виде строки
            string salt;//строка набор символов и пароль
            byte[] mas; //массив байтов для вычисления хэша
            salt = @"y8nq5kNhw" + employeeID + pass;
            byte[] saltedPwBytes1 = Encoding.UTF8.GetBytes(salt);//сформировать массив байтов из соли(набор символов + пароль) (UTF8,UNICODE)
            WhirlpoolManaged wm = new WhirlpoolManaged(); //создание новой спиральной хэш функции
            mas = wm.ComputeHash(saltedPwBytes1);//получить спиральную хэш функцию в виде массива байтов
            hash = Convert.ToBase64String(mas); //преобразовать в строку
            return hash;
        }

        private string GenerateRandomPassword(int seed)
        {
            Random rnd = new Random(DateTime.Now.Millisecond + seed);
            string result = rnd.Next(1000, 10000).ToString();
            return result;
        }

        public List<FIOPassword> CreateLoginBase()
        {
            List<FIOPassword> listFioPassw = new List<FIOPassword>();
            //1 отримати з бази працівників всіх у кого немає паролю
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format(@"select * from employees where char_length(hashcode) < 80");
            DataSet myDataSet = new DataSet();
            FbCommand sel_com = new FbCommand(selectSQL, connection);
            FbDataAdapter myDataAdapter = new FbDataAdapter(sel_com);
            myDataAdapter.Fill(myDataSet, "EMPL");
            connection.Close();
            string strNum = string.Empty;
            DataRowCollection dra = myDataSet.Tables["EMPL"].Rows;
            //2 для них сгенерировать пароль, для даного пароля сгенерировать хэш, записать
            connection.Open();
            FbTransaction ins_tr = connection.BeginTransaction();
            string ins_str;
            ins_str = "UPDATE EMPLOYEES SET HASHCODE=@HASHCODE WHERE ID=@ID";
            StringBuilder sbPass = new StringBuilder();
            StringBuilder sbHash = new StringBuilder();
            int seed = 0;
            try
            {
                foreach (DataRow dr in dra)
                {
                    FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
                    sbPass.Clear();
                    sbPass.Append(GenerateRandomPassword(seed));
                    sbHash.Clear();
                    sbHash.Append(GenerateHash(dr[0].ToString(), sbPass));
                    ins_com.Parameters.Add(new FbParameter("@HASHCODE", sbHash.ToString()));
                    ins_com.Parameters.Add(new FbParameter("@ID", dr[0].ToString()));
                    ins_com.ExecuteNonQuery();
                    FIOPassword fp = new FIOPassword();
                    fp.FullName = dr[2].ToString();
                    fp.Password = sbPass.ToString();
                    listFioPassw.Add(fp);
                    seed += 100;
                }
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
            return listFioPassw;
        }

        public string GetHashPassword(string id)
        {
            if (id.Length == 0)
                id = "0";
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string selectSQL;
            selectSQL = string.Format("SELECT HASHCODE FROM EMPLOYEES WHERE ID = {0}", id);
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
        public void DeleteTask(string id)
        {
            if (id.Length == 0)
                return;
            string connectionString = cs.ToString();
            FbConnection connection = new FbConnection(connectionString);
            connection.Open();
            string ins_str;
            ins_str = @"UPDATE TASKS SET ISDELETE=1 WHERE ID=@ID";
            FbTransaction ins_tr = connection.BeginTransaction();
            FbCommand ins_com = new FbCommand(ins_str, connection, ins_tr);
            ins_com.Parameters.Add(new FbParameter("@ID", id));
            try
            {
                ins_com.ExecuteNonQuery();
                ins_tr.Commit();
            }
            catch (Exception e)
            {
                ins_tr.Rollback();
            }
            connection.Close();
        }
    }
}