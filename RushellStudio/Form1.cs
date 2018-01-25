using System;
using System.IO;
using System.Windows.Forms;

namespace RushellStudio
{
    public partial class Form1 : Form
    {
        private Control rtb;
        private Project prj;

        public Form1()
        {
            InitializeComponent();
            prj = new Project(richTextBox1,this,comboBox1);
            rtb = new Control(richTextBox1,prj);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prj.Load();
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prj.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            prj.Exit(e);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (((ComboBox)sender).Text)
            {
                case "Rushell (.rux)":
                    prj.Path = ".rux";
                    break;
                case "CSharp (.cs)":
                    prj.Path = ".cs";
                    break;
                case "Python (.py)":
                    prj.Path = ".py";
                    break;
                case "Python (.pyw)":
                    prj.Path = ".pyw";
                    break;
                default:
                    break;
            }
            EndCharge();
        }

        private void EndCharge()
        {
            dep.DropDownItems.Clear();
            switch (prj.Path)
            {
                case ".rux":
                    AddItemDep("01","Ejecutar Script",(o,n) =>
                    {
                        prj.Save();
                        prj.Exec("Rushell", "\"" + prj.Path + "\"");
                    });
                    break;
                case ".cs":
                    string script = prj.Path.Replace(".cs", ".exe");
                    AddItemDep("01", "Compilar Script", (o, n) =>
                    {
                        prj.Save();
                        prj.Exec(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe", "\"/out:" + prj.Path.Replace(".cs", ".exe") + "\" " + "\"" + prj.Path + "\"");
                    });
                    AddItemDep("02", "Ejecutar Script", (o, n) =>
                    {
                        prj.Save();
                        prj.Exec(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe", "\"/out:" + prj.Path.Replace(".cs", ".exe") + "\" " + "\"" + prj.Path + "\"");
                        prj.Exec("\"" + prj.Path.Replace(".cs",".exe") + "\"", "");
                    });
                    break;
                case ".py":
                    AddItemDep("01", "Ejecutar Script", (o, n) =>
                    {
                        prj.Save();
                        prj.Exec("python", "\"" + prj.Path + "\"");
                    });
                    break;
                case ".pyw":
                    AddItemDep("01", "Ejecutar Script", (o, n) =>
                    {
                        prj.Save();
                        prj.Exec("python", "\"" + prj.Path + "\"");
                    });
                    break;
                default:
                    break;
            }
        }

        private void AddItemDep(string name, string text, EventHandler click)
        {
            dep.DropDownItems.Add(new ToolStripMenuItem(text, null, click, name));
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            prj.SaveAs();
        }
    }
}
