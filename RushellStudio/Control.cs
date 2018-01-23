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

        readonly string[] KEYWORDS_01 = new string[] { "if", "else", "while", "var", "def", "end", "then", "fun", "exit", "inv", "overfile", "init", "repeat", "clear", "wait", "async" };
        readonly string[] KEYWORDS_02 = new string[] { "write", "writeln", "writeli", "logic", "math" };
        readonly string[] KEYWORDS_03 = new string[] { "true", "false", "&&", "||" };
        readonly string[] KEYWORDS_04 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        readonly string[] KEYWORDS_05 = new string[] { "import", "from", "load", "instantiate", "to" };
        readonly string[] KEYWORDS_06 = new string[] { "!", "(", ")", "$" };
        readonly string[] KEYWORDS_07 = new string[] { "!math", "!read", "!logic", "!invoke", "!overfile", "!arg", "!repeatvalue", "!always", "!never", "!here", "!there" };

        public Control(RichTextBox rb)
        {
            this.rb = rb;
            this.rb.TextChanged += Rb_TextChanged;
        }

        private void Rb_TextChanged(object sender, EventArgs e)
        {
            selectStart = rb.SelectionStart;
            rb.SelectAll();
            rb.SelectionColor = Color.WhiteSmoke;
            rb.Select(selectStart, 0);

            CheckKeyWordList(KEYWORDS_01, Color.Blue, false);
            CheckKeyWordList(KEYWORDS_02, Color.Purple, false);
            CheckKeyWordList(KEYWORDS_03, Color.Green, false);
            CheckKeyWordList(KEYWORDS_04, Color.Green, true);
            CheckKeyWordList(KEYWORDS_05, Color.Yellow, false);
            CheckKeyWordList(KEYWORDS_06, Color.Black, true);
            CheckKeyWordList(KEYWORDS_07, Color.LightBlue, true);
        }

        private void CheckKeyWordList(string[] list, Color c, bool n)
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
                    if (OutWord(k0, k1) && !continued)
                        color = Color.WhiteSmoke;
                    rb.Select((index + startIndex), word.Length);
                    rb.SelectionColor = color;
                    rb.Select(selectStart, 0);
                    rb.SelectionColor = Color.WhiteSmoke;
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
    }
}
