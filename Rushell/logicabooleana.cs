using System;

namespace Rushell
{
    class logicabooleana
    {
        public string Expresion;

        public logicabooleana(string expresion)
        {
            this.Expresion = expresion;
        }

        public bool operar()
        {
            string sfinal = Sintaxis.Analizar(Expresion);
            return evaluar(organizar(sfinal));
        }

        private string organizar(string sfinal)
        {
            string reconstruir = "";
            string actual = "";
            foreach (char pt in sfinal)
            {
                if (pt == '(')
                {
                    reconstruir += actual;
                    actual = "";
                    reconstruir += pt;
                }
                else if (pt == ')')
                {
                    reconstruir += booleano(actual);
                    reconstruir += pt;
                    actual = "";
                }
                else
                {
                    actual += pt;
                }
            }
            return reconstruir;
        }

        private string booleano(string expresion)
        {
            string res = expresion;
            if (expresion.Contains(" == "))
            {
                string[] vl = expresion.Split(new string[] { " == " }, StringSplitOptions.None);
                if (vl[0].Equals(vl[1]))
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" != "))
            {
                string[] vl = expresion.Split(new string[] { " != " }, StringSplitOptions.None);
                if (!vl[0].Equals(vl[1]))
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" ? "))
            {
                expresion = expresion.Replace(" ? ", " p ");
                string[] vl = expresion.Split(new string[] { " p " }, StringSplitOptions.None);
                if (vl[0].Contains(vl[1]))
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" !? "))
            {
                expresion = expresion.Replace(" !? ", " p ");
                string[] vl = expresion.Split(new string[] { " p " }, StringSplitOptions.None);
                if (!vl[0].Contains(vl[1]))
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" <= "))
            {
                string[] vl = expresion.Split(new string[] { " <= " }, StringSplitOptions.None);
                float v1 = float.Parse(vl[0]);
                float v2 = float.Parse(vl[1]);
                if (v1 <= v2)
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" >= "))
            {
                string[] vl = expresion.Split(new string[] { " >= " }, StringSplitOptions.None);
                float v1 = float.Parse(vl[0]);
                float v2 = float.Parse(vl[1]);
                if (v1 >= v2)
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" < "))
            {
                string[] vl = expresion.Split(new string[] { " < " }, StringSplitOptions.None);
                float v1 = float.Parse(vl[0]);
                float v2 = float.Parse(vl[1]);
                if (v1 < v2)
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion.Contains(" > "))
            {
                string[] vl = expresion.Split(new string[] { " > " }, StringSplitOptions.None);
                float v1 = float.Parse(vl[0]);
                float v2 = float.Parse(vl[1]);
                if (v1 > v2)
                {
                    res = "true";
                }
                else
                {
                    res = "false";
                }
            }
            else if (expresion[expresion.Length - 1] == '?')
            {
                if (Memoria.varn.Contains(expresion.Substring(0, expresion.Length - 1)))
                    res = "true";
                else
                    res = "false";
            }
            return res;
        }

        private bool evaluar(string expression)
        {
            expression = expression.ToLower();
            expression = expression.Replace("false", "0");
            expression = expression.Replace("true", "1");
            expression = expression.Replace(" ", "");
            string temp;
            do
            {
                temp = expression;
                expression = expression.Replace("(0)", "0");
                expression = expression.Replace("(1)", "1");
                expression = expression.Replace("0&&0", "0");
                expression = expression.Replace("0&&1", "0");
                expression = expression.Replace("1&&0", "0");
                expression = expression.Replace("1&&1", "1");
                expression = expression.Replace("0||0", "0");
                expression = expression.Replace("0||1", "1");
                expression = expression.Replace("1||0", "1");
                expression = expression.Replace("1||1", "1");
            }
            while (temp != expression);
            if (expression == "0")
                return false;
            if (expression == "1")
                return true;
            return false;
        }
    }
}
