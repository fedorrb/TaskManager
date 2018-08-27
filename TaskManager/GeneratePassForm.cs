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
    public partial class GeneratePassForm : Form
    {
        ModeGenerator modeGen = ModeGenerator.None;
        private DB db2;
        List<FIOPassword> fioPassList = new List<FIOPassword>();
        public GeneratePassForm()
        {
            InitializeComponent();
            db2 = new DB();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                modeGen = ModeGenerator.NewOnly;
                fioPassList = db2.CreateLoginBase();
                richTextBox1.Clear();
                foreach (FIOPassword fp in fioPassList)
                {
                    String s = String.Format("{0,-60} {1,-5}\n\n", fp.FullName, fp.Password);
                    richTextBox1.AppendText(s);
                }
            }
            if (radioButton2.Checked == true)
            {
                modeGen = ModeGenerator.ForAll;
            }
            if (modeGen != ModeGenerator.None)
            {

            }
        }
    }
}
