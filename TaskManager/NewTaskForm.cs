using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TaskManager
{
    public partial class NewTaskForm : Form
    {
        private DB db2 = new DB();
        private List<string> subjects = new List<string>();
        private List<Employees> executors = new List<Employees>();
        List<string> newTask = new List<string>();
        private int masterID;
        private OneTask taskFields = new OneTask();

        public NewTaskForm()
        {
            InitializeComponent();
            this.masterID = 0;
        }

        public NewTaskForm(int p)
        {
            InitializeComponent();
            this.masterID = p;
        }

        public NewTaskForm(int p, OneTask currTask)
        {
            InitializeComponent();
            this.masterID = p;
            taskFields = currTask;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SpellCheck())
            {
                Employees selectedEmpl = (Employees)comboBox2.SelectedItem;
                newTask.Clear();
                newTask.Add(comboBox1.Text);
                newTask.Add(textBox1.Text);
                newTask.Add(selectedEmpl.ID.ToString());
                newTask.Add(dateTimePicker1.Value.ToShortDateString());
                newTask.Add(masterID.ToString());
                newTask.Add(comboBox3.SelectedIndex.ToString());
                newTask.Add(dateTimePicker2.Value.ToShortDateString());
                newTask.Add(textBox2.Text);
                db2.InsertTask(newTask);
                MessageBox.Show("Завдання створено.", "OK");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void NewTaskForm_Load(object sender, EventArgs e)
        {
            HideErrorMark();
            subjects = db2.GetSubjects();
            comboBox1.DataSource = subjects;

            executors = db2.GetSlaves(masterID);
            comboBox2.DataSource = executors;
            comboBox2.DisplayMember = "FullName";
            comboBox2.ValueMember = "ID";

            comboBox3.SelectedIndex = 0;

            comboBox1.Text = taskFields.subject;
            textBox1.Text = taskFields.shortDescription;
            dateTimePicker1.Value = taskFields.deadLine;

            int idxPriority = comboBox3.FindStringExact(taskFields.priority.ToUpper());
            if (idxPriority < 0)
                idxPriority = 0;
            comboBox3.SelectedItem = comboBox3.Items[idxPriority];
            comboBox3.SelectedIndex = idxPriority;

            dateTimePicker2.Value = taskFields.dateDoc;
            textBox2.Text = taskFields.numberDoc;
        }

        private bool SpellCheck()
        {
            HideErrorMark();
            if (comboBox1.Text.Length < 3)
            {
                label11.Visible = true;
                return false;
            }
            if (textBox1.Text.Length < 3)
            {
                label8.Visible = true;
                return false;
            }
            if (CalcDate.DaysDiff(dateTimePicker1.Value, DateTime.Now) < (-31) || CalcDate.DaysDiff(dateTimePicker1.Value, DateTime.Now) > 365)
            {
                label9.Visible = true;
                return false;
            }
            if (CalcDate.DaysDiff(dateTimePicker2.Value, DateTime.Now) < (-365) || CalcDate.DaysDiff(dateTimePicker2.Value, DateTime.Now) > 1)
            {
                label10.Visible = true;
                return false;
            }
            HideErrorMark();
            return true;
        }

        private void HideErrorMark()
        {
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
        }
        
    }
}
