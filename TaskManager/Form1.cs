using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TaskManager
{
    public enum ShowMadeTask { ShowAll, ShowMade, ShowNotMade };
    public enum ModeGenerator { None, NewOnly, ForAll };
    public partial class MainForm : Form
    {
        private stPath stPathFiles;//пути
        private UserLogin userLogin = new UserLogin();//экземпляр класса аутентификации пользователя
        private string sPass; //пароль через событие
        private DB db2;
        private GLB glb = new GLB();
        private FileOperation fo = new FileOperation();
        private List<Employees> allExecutors = new List<Employees>();
        private ShowMadeTask currentShowMadeTask = ShowMadeTask.ShowNotMade;

        public MainForm()
        {
            InitializeComponent();
            stPathFiles = new stPath();
            stPathFiles.LoadIniFile();
            if (stPathFiles.CheckFirebirdDll)
            {
                db2 = new DB();
                glb.isConnection = true;
                if (!db2.CheckConnection())
                {
                    MessageBox.Show(db2.error);
                    glb.isConnection = false;
                }
            }
            else
                glb.isConnection = false;
        }

        private void Login()
        {
            splitContainer1.Enabled = false;
            toolStrip1.Enabled = false;
            if (glb.isConnection)
            {
                ShowLoginForm();
                if (userLogin.IsLogin())
                {
                    splitContainer1.Enabled = true;
                    toolStrip1.Enabled = true;
                    allExecutors = db2.GetSlaves(glb.employeeID);
                    if (glb.employeeID == 1)
                        toolStripDropDownButton1.Visible = true;

                    // Restore state data
                    try
                    {
                        //Point location = Properties.Settings.Default.DataGridViewFormLocation;
                        //Size size = Properties.Settings.Default.DataGridViewFormSize;

                        //// Set StartPosition to manual
                        //// after being sure there are no null values
                        //this.StartPosition = FormStartPosition.Manual;
                        //this.Location = location;
                        //this.Size = size;

                        // Restore the columns' state
                        StringCollection cols = Properties.Settings.Default.dgwColWidth;
                        string[] colsArray = new string[cols.Count];
                        cols.CopyTo(colsArray, 0);
                        Array.Sort(colsArray);
                        for (int i = 0; i < colsArray.Length; ++i)
                        {
                            string[] a = colsArray[i].Split(',');
                            int index = int.Parse(a[3]);
                            //this.dataGridView1.Columns[index].DisplayIndex = Int16.Parse(a[0]);
                            this.dataGridView1.Columns[index].Width = Int16.Parse(a[1]);
                            this.dataGridView1.Columns[index].Visible = bool.Parse(a[2]);
                        }
                    }
                    catch (NullReferenceException)
                    {
                        // This happens when settings values are empty
                    }

                    ShowTasks();
                }
            }
        }

        private void ShowLoginForm()
        {
            splitContainer1.Enabled = false;
            sPass = String.Empty;//password
            LoginForm loginForm = new LoginForm();//создание новой формы аутентификации
            loginForm.GetUserLogin(userLogin);//вызов метода и передача ему экземпляра класса userLogin
            loginForm.evt.evt += delegate(object o, StringArg ar) //используем анонимный делегат для упрощения кода
            {
                //событие срабатывает после закрытия окна аутентификации
                if (ar.str != null)
                {
                    this.Activate();
                    sPass = ar.str;//получить пароль
                    ar.str = String.Empty;//очистка
                    userLogin.Validate();//проверить введенные данные
                    if (userLogin.IsLogin())//если имя и пароль соответствуют то разрешаем работу
                    {
                        splitContainer1.Enabled = true;
                        glb.employeeID = userLogin.employeeID;
                    }
                }
            };
            //показать форму аутентификации
            loginForm.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //dataGridView1.Columns["DEADLINE"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            Login();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (userLogin.IsLogin())
            {
                stPathFiles.LastUser = glb.employeeID;
                stPathFiles.SaveIniFile();
                // Save column state data
                // including order, column width and whether or not the column is visible
                StringCollection stringCollection = new StringCollection();
                int i = 0;
                foreach (DataGridViewColumn column in this.dataGridView1.Columns)
                {
                    stringCollection.Add(string.Format(
                        "{0},{1},{2},{3}",
                        column.DisplayIndex.ToString("D2"),
                        column.Width,
                        column.Visible,
                        i++));
                }
                Properties.Settings.Default.dgwColWidth = stringCollection;
                Properties.Settings.Default.Save();
            }
        }

        private void ShowTasks()
        {
            UnregisterEventDGV1();
            dataGridView1.Visible = false;
            dataGridView1.Rows.Clear();
            int index = 0;
            List<int> numSlaves = new List<int>();
            foreach (Employees currExecutor in allExecutors)
            {
                numSlaves.Add(currExecutor.ID);
            }
            if (numSlaves.Count > 0)
            {
                string strInSlaves = string.Join(",", numSlaves.Select(x => x.ToString()).ToArray());
                DataRowCollection drcTasks = db2.GetTasksForSlave(strInSlaves, currentShowMadeTask, glb.employeeID);
                if (drcTasks.Count > 0)
                {
                    foreach (DataRow dr in drcTasks)
                    {
                        //if (dr["ISDELETE"].ToString().Equals("0"))
                        {
                            string dtDateInit = GetShortDateOnly(dr["DATEINIT"].ToString());
                            string dtDeadLine = GetShortDateOnly(dr["DEADLINE"].ToString());
                            string dtMakeDate = GetShortDateOnly(dr["MAKEDATE"].ToString());
                            string dtCommitDate = GetShortDateOnly(dr["COMMITDATE"].ToString());
                            string dtDateDoc = GetShortDateOnly(dr["DATEDOC"].ToString());
                            string priority = "нормальний";
                            if (dr["PRIORITY"].ToString().Equals("1"))
                                priority = "високий";
                            if (dr["PRIORITY"].ToString().Equals("2"))
                                priority = "терміново";

                            index = dataGridView1.Rows.Add(dr["ID"].ToString(),
                                dr["SUBJECT"].ToString(),
                                dr["SHORTDESCRIPTION"].ToString(),
                                dr["SLAVEFIO"].ToString(),
                                dtDateInit,
                                dtDeadLine,
                                dtMakeDate,
                                dr["MAKETASK"],
                                dr["MASTERFIO"].ToString(),
                                dtCommitDate,
                                dr["COMMITMAKE"],
                                priority,
                                dtDateDoc,
                                dr["NUMBERDOC"].ToString(),
                                dr["EXECUTORID"].ToString(),
                                dr["MANAGERID"].ToString());
                            if (dr["COMMITMAKE"].ToString().Equals("0")) //завдання не виконано
                            {
                                int daysDiff = CalcDate.DaysDiffNow(dtDeadLine);
                                if (daysDiff < 0)
                                    dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Red;
                                else if(daysDiff < 7)
                                    dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Green;
                                else
                                    dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Blue;
                            }
                            else
                                dataGridView1.Rows[index].DefaultCellStyle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            dataGridView1.Visible = true;
            RegisterEventsDGV1();
        }

        private void RegisterEventsDGV1()
        {
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
        }

        private void UnregisterEventDGV1()
        {
            dataGridView1.CellEndEdit -= dataGridView1_CellEndEdit;
        }

        private string GetShortDateOnly(string strIn)
        {
            if (strIn.Length < 10)
                return strIn;
            else
                return strIn.Substring(0, 10);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NewTaskForm newTaskForm = new NewTaskForm(glb.employeeID);
            newTaskForm.ShowDialog();
            ReloadData();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CellEndEdit -= dataGridView1_CellEndEdit;
            //асинхронний виклик функції, щоб можна було зробити ReloadData
            this.BeginInvoke(new MethodInvoker(() =>
            {
                UpdateCurrentTask(e.RowIndex);
            }));
        }

        private void UpdateCurrentTask(int rowIndex)
        {
            List<string> taskFields = new List<string>();
            taskFields.Add(dataGridView1.Rows[rowIndex].Cells["ID"].Value.ToString());
            if (taskFields[0].Length == 0)
                return;
            
            taskFields.Add(dataGridView1.Rows[rowIndex].Cells["EXECUTORID"].Value.ToString());
            taskFields.Add(dataGridView1.Rows[rowIndex].Cells["MANAGERID"].Value.ToString());
            if (taskFields[1].Equals(glb.employeeID.ToString()) && !taskFields[2].Equals(glb.employeeID.ToString())) //user is executor only
            {
                if (dataGridView1.Rows[rowIndex].Cells["MAKETASK"].Value.ToString().ToLower().Equals("true") ||
                    dataGridView1.Rows[rowIndex].Cells["MAKETASK"].Value.ToString().Equals("1"))
                    taskFields.Add("1");
                else
                    taskFields.Add("0");
                db2.UpdateTaskExecutor(taskFields);
            }
            else if (taskFields[1].Equals(glb.employeeID.ToString()) && taskFields[2].Equals(glb.employeeID.ToString())) //user is executor and manager
            {
                if (dataGridView1.Rows[rowIndex].Cells["MAKETASK"].Value.ToString().ToLower().Equals("true") ||
                    dataGridView1.Rows[rowIndex].Cells["MAKETASK"].Value.ToString().Equals("1"))
                {
                    taskFields.Add("1");
                    taskFields.Add("1");
                }
                else
                {
                    taskFields.Add("0");
                    taskFields.Add("0");
                }                
                db2.UpdateTaskExecutorManager(taskFields);
            }
            else if (taskFields[2].Equals(glb.employeeID.ToString())) //user is manager only
            {
                if (dataGridView1.Rows[rowIndex].Cells["COMMITMAKE"].Value.ToString().ToLower().Equals("true") ||
                    dataGridView1.Rows[rowIndex].Cells["COMMITMAKE"].Value.ToString().Equals("1"))
                    taskFields.Add("1");
                else
                    taskFields.Add("0");
                db2.UpdateTaskManager(taskFields);
            }
            ReloadData();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void ReloadData()
        {
            if (userLogin.IsLogin())
                ShowTasks();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowAll;
            ReloadData();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowMade;
            ReloadData();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowNotMade;
            ReloadData();
        }
     
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void allTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowAll;
            ReloadData();
        }

        private void doneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowMade;
            ReloadData();
        }

        private void undoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentShowMadeTask = ShowMadeTask.ShowNotMade;
            ReloadData();
        }

        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTaskForm newTaskForm = new NewTaskForm(glb.employeeID);
            newTaskForm.ShowDialog();
            if (newTaskForm.ShowDialog() == DialogResult.OK)
                ReloadData();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //if the form is minimized  
            //hide it from the task bar  
            //and show the system tray icon (represented by the NotifyIcon control)  
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }  
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void generatePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeneratePassForm gpf = new GeneratePassForm();
            gpf.ShowDialog();
        }

        private void newTaskByCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OneTask currTask = new OneTask();
            int currRow = dataGridView1.CurrentRow.Index;
            currTask.subject = dataGridView1.Rows[currRow].Cells["SUBJECT"].Value.ToString();
            currTask.shortDescription = dataGridView1.Rows[currRow].Cells["SHORTDESCR"].Value.ToString();
            DateTime dateDEADLINE = new DateTime();
            if (!DateTime.TryParse(dataGridView1.Rows[currRow].Cells["DEADLINE"].Value.ToString(), out dateDEADLINE))
            {
                dateDEADLINE = DateTime.Now;
            }
            currTask.deadLine = dateDEADLINE;
            currTask.priority = dataGridView1.Rows[currRow].Cells["PRIORITY"].Value.ToString();
            DateTime dateDATEDOC = new DateTime();
            if (!DateTime.TryParse(dataGridView1.Rows[currRow].Cells["DATEDOC"].Value.ToString(), out dateDATEDOC))
            {
                dateDATEDOC = DateTime.Now;
            }
            currTask.dateDoc = dateDATEDOC;
            currTask.numberDoc = dataGridView1.Rows[currRow].Cells["NUMBERDOC"].Value.ToString();
            NewTaskForm newTaskForm = new NewTaskForm(glb.employeeID, currTask);
            if(newTaskForm.ShowDialog() == DialogResult.OK)
                ReloadData();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ReportForm newReportForm = new ReportForm();
            newReportForm.ShowDialog();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm newReportForm = new ReportForm();
            newReportForm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currRow = dataGridView1.CurrentRow.Index;
            if (currRow >= 0 && currRow < (dataGridView1.RowCount-1))
            {
                string id = dataGridView1.Rows[currRow].Cells["ID"].Value.ToString();
                string manager = dataGridView1.Rows[currRow].Cells["MANAGERID"].Value.ToString();

                if (manager.Equals(glb.employeeID.ToString())) //user is manager
                {
                    db2.DeleteTask(id);
                    ReloadData();
                }
            }
        }

    }
}
