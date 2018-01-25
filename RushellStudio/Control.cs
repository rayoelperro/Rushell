using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RushellStudio
{
    class Control
    {
        private RichTextBox rb;
        private int selectStart;
        private Project prj;

        public Control(RichTextBox rb, Project prj)
        {
            this.rb = rb;
            this.prj = prj;
            this.rb.TextChanged += Rb_TextChanged;
        }

        private void Rb_TextChanged(object sender, EventArgs e)
        {
            prj.Edited();
            selectStart = rb.SelectionStart;
            rb.SelectAll();
            rb.SelectionColor = Color.LightGreen;
            rb.Select(selectStart, 0);
            switch (prj.Extension)
            {
                case "rux":
                    Rushell_W();
                    break;
                case "cs":
                    CSharp_W();
                    break;
                case "py":
                    Python_W();
                    break;
                case "pyw":
                    Python_W();
                    break;
            }
        }

        private void Rushell_W()
        {
            Rushell ru = new Rushell();
            CheckKeyWordList(Color.Blue, false, ru.KEYWORDS_01);
            CheckKeyWordList(Color.Purple, false, ru.KEYWORDS_02);
            CheckKeyWordList(Color.Green, false, ru.KEYWORDS_03);
            CheckKeyWordList(Color.Green, true, ru.KEYWORDS_04);
            CheckKeyWordList(Color.Yellow, false, ru.KEYWORDS_05);
            CheckKeyWordList(Color.Black, true, ru.KEYWORDS_06);
            CheckKeyWordList(Color.LightBlue, true, ru.KEYWORDS_07);
        }

        private void CSharp_W()
        {
            CSharp cs = new CSharp();
            CheckKeyWordList(Color.Blue, true, cs.KEYWORDS_01);
            CheckKeyWordList(Color.Purple, true, cs.KEYWORDS_02);
            CheckKeyWordList(Color.Green, true, cs.KEYWORDS_03);
            CheckKeyWordList(Color.Green, true, cs.KEYWORDS_04);
            CheckKeyWordList(Color.Yellow, true, cs.KEYWORDS_05);
            CheckKeyWordList(Color.Black, true, cs.KEYWORDS_06);
            CheckKeyWordList(Color.DodgerBlue, true, cs.KEYWORDS_07);
        }

        private void Python_W()
        {
            Python py = new Python();
            CheckKeyWordList(Color.Blue, true, py.KEYWORDS_01);
            CheckKeyWordList(Color.Green, true, py.KEYWORDS_03);
            CheckKeyWordList(Color.Green, true, py.KEYWORDS_04);
            CheckKeyWordList(Color.Yellow, true, py.KEYWORDS_05);
            CheckKeyWordList(Color.Black, true, py.KEYWORDS_06);
            CheckKeyWordList(Color.DodgerBlue, true, py.KEYWORDS_07);
            CheckKeyWordList(Color.Purple, true, py.KEYWORDS_02);
        }

        private void CheckKeyWordList(Color c, bool n, params string[] list)
        {
            foreach (string w in list)
                CheckKeyword(w, c, 0, n);

        }

        private void CheckKeyword(string word, Color color, int startIndex, bool continued)
        {
            if (rb.Text.Contains(word))
            {
                int index = -1;

                while ((index = rb.Text.IndexOf(word, (index + 1))) != -1)
                {
                    int k0 = (index + startIndex - 1);
                    int k1 = (index + startIndex + word.Length);
                    if (!(OutWord(k0, k1) && !continued))
                    {
                        rb.Select((index + startIndex), word.Length);
                        rb.SelectionColor = color;
                        rb.Select(selectStart, 0);
                        rb.SelectionColor = Color.LightGreen;
                    }
                }
            }
        }

        private bool OutWord(int k0, int k1)
        {
            bool a = false;
            bool b = false;
            if (k0 > 0)
                if (rb.Text[k0] != ' ' && rb.Text[k0] != '\n' && rb.Text[k0] != '\t')
                    a = true;
            if (k1 < rb.Text.Length)
                if (rb.Text[k1] != ' ' && rb.Text[k1] != '\n' && rb.Text[k1] != '\t')
                    b = true;
            return a || b;
        }

        private class Rushell
        {
            public readonly string[] KEYWORDS_01 = new string[] { "if", "else", "while", "var", "def", "end", "then", "fun", "exit", "inv", "overfile", "init", "repeat", "clear", "wait", "async" };
            public readonly string[] KEYWORDS_02 = new string[] { "write", "writeln", "writeli", "logic", "math" };
            public readonly string[] KEYWORDS_03 = new string[] { "true", "false", "&&", "||" };
            public readonly string[] KEYWORDS_04 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            public readonly string[] KEYWORDS_05 = new string[] { "import", "from", "load", "instantiate", "to" };
            public readonly string[] KEYWORDS_06 = new string[] { "!", "(", ")", "$", "@", "\"", "+", "*", "/", "-", "=", "<", ">" };
            public readonly string[] KEYWORDS_07 = new string[] { "!math", "!read", "!logic", "!invoke", "!overfile", "!arg", "!repeatvalue", "!always", "!never", "!here", "!there" };
        }

        private class CSharp
        {
            public readonly string[] KEYWORDS_01 = new string[] { "if", "else", "while", "for", "foreach", "class", "namespace", "using", "this", "delegate", "event", "in", "ref", "out", "delegate", "new", "break", "continue", "return", "yield", "await" };
            public readonly string[] KEYWORDS_02 = new string[] { "void", "int", "string", "bool", "long", "char", "double", "byte" };
            public readonly string[] KEYWORDS_03 = new string[] { "true", "false" };
            public readonly string[] KEYWORDS_04 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "||", "&&", "!" };
            public readonly string[] KEYWORDS_05 = new string[] { "Main", "Equals", "ToString", "GetHashCode", "GetType" };
            public readonly string[] KEYWORDS_06 = new string[] { "(", ")", "$", "@", "\"", "+", "*", "/", "=", "<", ">", "{", "}", "[", "]", ".", ";", ":", "'" };
            public readonly string[] KEYWORDS_07 = new string[] { "public", "private", "protected", "static", "internal", "async" };
        }

        private class Python
        {
            public readonly string[] KEYWORDS_01 = new string[] { "if", "else", "elif", "while", "for", "class", "def", "break", "continue", "in", "raw_input", "input", "exec", "return", "yield" };
            public readonly string[] KEYWORDS_02 = new string[] { "print" };
            public readonly string[] KEYWORDS_03 = new string[] { "True", "False" };
            public readonly string[] KEYWORDS_04 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "or", "and", "not" };
            public readonly string[] KEYWORDS_05 = new string[] { "import", "from", "as" };
            public readonly string[] KEYWORDS_06 = new string[] { "(", ")", "$", "@", "\"", "+", "*", "/", "=", "<", ">", "[", "]", ".", ":", "_", "'" };
            public readonly string[] KEYWORDS_07 = new string[] { "int", "str", "len", "range" };
        }
    }
}