using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ComboBox;

namespace RushellStudio
{
    class Project
    {
        private string extension = "";
        public string Extension { get => extension; }
        private string path = "";
        public string Path
        {
            get => path;
            set { path = value; extension = Path.Split('.')[Path.Split('.').Length - 1]; TEdit.Text = "    Rushell Studio: " + path; }
        }
        private RichTextBox rtb;
        private Form TEdit;
        private ComboBox con;
        private bool issave = true;

        public Project(RichTextBox rtb, Form TEdit, ComboBox con)
        {
            this.rtb = rtb;
            this.TEdit = TEdit;
            this.con = con;
        }

        public void Load()
        {
            switch (Confirm())
            {
                case DialogResult.Yes:
                    Save();
                    break;
                case DialogResult.Cancel:
                    return;
            }
            OpenFileDialog o = new OpenFileDialog() { Filter = "Rushell source file (.rux)|*.rux|CSharp source file (.cs)|*.cs|Python source file (.py)|*.py|Python window source file (.pyw)|*.pyw|Text file (.txt)|*.txt|All Files (*.*)|*.*" };
            if (o.ShowDialog() == DialogResult.OK)
            {
                Path = o.FileName;
                foreach (Object i in con.Items)
                    if (i.ToString().Contains(extension))
                        con.SelectedItem = i;
                Path = o.FileName;
                rtb.Text = File.ReadAllText(o.FileName);
            }
        }

        public void Save()
        {
            if (".rux.cs.py.pyw".Contains(Path) || Path == "")
            {
                SaveAs();
            }
            else
            {
                File.WriteAllText(Path, rtb.Text);
                issave = true;
            }
        }

        public void SaveAs()
        {
            SaveFileDialog o = new SaveFileDialog() { Filter = "Rushell source file (.rux)|*.rux|CSharp source file (.cs)|*.cs|Python source file (.py)|*.py|Python window source file (.pyw)|*.pyw|Text file (.txt)|*.txt|All Files (*.*)|*.*" };
            if (o.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(o.FileName, rtb.Text);
                issave = true;
                Path = o.FileName;
                foreach (Object i in con.Items)
                    if (i.ToString().Contains(extension))
                        con.SelectedItem = i;
                Path = o.FileName;
            }
        }

        public void Edited()
        {
            issave = false;
        }

        public void Exit(FormClosingEventArgs e)
        {
            switch (Confirm())
            {
                case DialogResult.Yes:
                    Save();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private DialogResult Confirm()
        {
            if (!issave)
            {
                DialogResult r = MessageBox.Show("¿Desea guardar el Script?", "Aviso", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                return r;
            }
            return DialogResult.OK;
        }

        public void Exec(string lang, string arguments)
        {
            try
            {
                ProcessStartInfo inf = new ProcessStartInfo(lang, arguments);
                inf.CreateNoWindow = false;
                inf.WindowStyle = ProcessWindowStyle.Normal;
                Process p = new Process();
                p.StartInfo = inf;
                p.Start();
                p.WaitForExit();
            }catch(Exception e)
            {
                MessageBox.Show(e.ToString(), "Error de ejecución", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(lang);
            }
        }
    }
}
