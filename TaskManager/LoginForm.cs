using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace TaskManager
{
    public partial class LoginForm : Form
    {
        public UserLogin uLogin;
        private DB db2 = new DB();
        List<Employees> allEmployees = new List<Employees>();
        //List<Level> allLevels = new List<Level>();
        private stPath stPathFiles;//пути

        public AppEvents evt = new AppEvents(); //событие

        private const int CS_NOCLOSE = 0x200;
        /// <summary>
        /// Переопределение параметров создания формы.
        /// Убрать кнопку закрытия окна.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;
                return cp;
            }
        }

        public LoginForm()
        {
            InitializeComponent();
            uLogin = new UserLogin();
            stPathFiles = new stPath();
            stPathFiles.LoadIniFile();
            allEmployees.Clear();
            db2.GetAllEmloyees(ref allEmployees);
            comboBox1.DataSource = allEmployees;
            comboBox1.DisplayMember = "FullName";
            comboBox1.ValueMember = "ID";
            SetLastEmployee();
            //перелік користувачів
        }

        private void SetLastEmployee()
        {
            int realIndex = 0;
            if (stPathFiles.LastUser > 0)
            {
                foreach (Employees currEmp in allEmployees)
                {
                    if (currEmp.ID == stPathFiles.LastUser)
                        break;
                    realIndex++;
                }
            }
            comboBox1.SelectedIndex = realIndex;
        }

        public void GetUserLogin(UserLogin ul)
        {
            uLogin = ul;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uLogin.SetPass("");
            uLogin.pass = "";
            evt.OnStringEvt(""); //вызов события
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // получаем весь выделенный объект
            Employees selectedEmpl = (Employees)comboBox1.SelectedItem;
            uLogin.employeeID = selectedEmpl.ID;
            uLogin.fullName = selectedEmpl.FullName;
            uLogin.SetPass(textBox2.Text);
            uLogin.pass = textBox2.Text;
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            sb.AppendFormat("{0};{1}", uLogin.employeeID, textBox2.Text);
            //событие возвращает значение из LoginForm в Form1
            evt.OnStringEvt(sb.ToString());
            this.Close();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SelectNextControl(textBox2, true, true, false, true);
        }
    }
}
