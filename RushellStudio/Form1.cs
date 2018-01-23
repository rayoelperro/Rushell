using System;
using System.IO;
using System.Windows.Forms;

namespace RushellStudio
{
    public partial class Form1 : Form
    {
        private Control rtb;

        public Form1()
        {
            InitializeComponent();
            rtb = new Control(richTextBox1);
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog() { Filter= "Rushell source file (.rux)|*.rux|Text file (.txt)|*.rux|All Files (*.*)|*.*" };
            if (o.ShowDialog() == DialogResult.OK)
                richTextBox1.Text = File.ReadAllText(o.FileName);
        }

        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new SaveFileDialog() { Filter = "Rushell source file (.rux)|*.rux|Text file (.txt)|*.rux|All Files (*.*)|*.*" };
            if (o.ShowDialog() == DialogResult.OK)
                File.WriteAllText(o.FileName, richTextBox1.Text);
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
