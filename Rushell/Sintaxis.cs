using System;
using System.Text.RegularExpressions;

namespace Rushell
{
    class Sintaxis
    {
        public static string Analizar(string a_)
        {
            a_ = a_.Replace("!always()", int.MaxValue.ToString());
            a_ = a_.Replace("!never()", int.MinValue.ToString());
            a_ = a_.Replace("!repeatvalue()", Memoria.repeatvalue.ToString());
            a_ = a_.Replace("!here()", System.IO.Directory.GetCurrentDirectory());
            a_ = a_.Replace("!there()", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            a_ = a_.Replace("\\n", "\n");
            a_ = a_.Replace("\\t", "\t");

            if (Memoria.varn.Count > 0 && Memoria.varv.Count > 0)
            {
                for (int al = 0; al < Memoria.varn.Count; al++)
                {
                    try
                    {
                        if (Memoria.varv[al].ToString() == "System.String[]")
                        {
                            string[] sustituidor = (string[])Memoria.varv[al];
                            int len = sustituidor.Length;
                            for (int st = 0; st < len; st++)
                            {
                                a_ = a_.Replace("!" + Memoria.varn[al].ToString() + "-" + st, sustituidor[st]);
                            }
                            a_ = a_.Replace("!" + Memoria.varn[al].ToString(), sustituidor[0]);
                        }
                        else
                            a_ = a_.Replace("!" + Memoria.varn[al].ToString(), Memoria.varv[al].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memoria.defn.Count > 0 && Memoria.defv.Count > 0)
            {
                for (int al = 0; al < Memoria.defn.Count; al++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memoria.defn[al].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memoria.defn[al].ToString());
                            bool ok = false;
                            for (int la = o + Memoria.defn[al].ToString().Length + 1; la < a_.Length; la++)
                            {
                                if (ok)
                                {
                                    if (a_[la] == ')')
                                    {
                                        ok = false;
                                        break;
                                    }
                                    else
                                    {
                                        ael += a_[la];
                                    }
                                }
                                if (la == o + Memoria.defn[al].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            string[] args = null;
                            if (ael != "")
                            {
                                string[] aels = ael.Split(',');
                                args = new string[aels.Length + 1];
                                args[0] = Memoria.defn[al].ToString();
                                for (int j = 1; j < args.Length; j++)
                                    args[j] = aels[j - 1];
                            }
                            else
                            {
                                args = new string[] { Memoria.defn[al].ToString() };
                            }
                            Memoria.Call_D(args);
                            a_ = a_.Replace("!" + Memoria.defn[al].ToString() + "(" + ael + ")", "");
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memoria.funn.Count > 0 && Memoria.funv.Count > 0)
            {
                for (int ax = 0; ax < Memoria.funn.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memoria.funn[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memoria.funn[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memoria.funn[ax].ToString().Length + 1; la < a_.Length; la++)
                            {
                                if (ok)
                                {
                                    if (a_[la] == ')')
                                    {
                                        ok = false;
                                        break;
                                    }
                                    else
                                    {
                                        ael += a_[la];
                                    }
                                }
                                if (la == o + Memoria.funn[ax].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            a_ = a_.Replace("!" + Memoria.funn[ax].ToString() + "(" + ael + ")", Memoria.Cal_F(ax, ael).ToString());
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memoria.dlln.Count > 0 && Memoria.dllv.Count > 0)
            {
                for (int ax = 0; ax < Memoria.dlln.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memoria.dlln[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memoria.dlln[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memoria.dlln[ax].ToString().Length + 1; la < a_.Length; la++)
                            {
                                if (ok)
                                {
                                    if (a_[la] == ')')
                                    {
                                        ok = false;
                                        break;
                                    }
                                    else
                                    {
                                        ael += a_[la];
                                    }
                                }
                                if (la == o + Memoria.dlln[ax].ToString().Length + 1 && a_[la] == '.')
                                {
                                    ok = true;
                                }
                            }
                            string[] vi = ael.Split('(');
                            if (vi[1] == "")
                            {
                                string[] args = new string[2];
                                args[0] = Memoria.dlln[ax].ToString();
                                args[1] = vi[0];
                                a_ = a_.Replace("!" + Memoria.dlln[ax].ToString() + "." + ael + ")", Memoria.dll_m(args).ToString());
                            }
                            else
                            {
                                string[] param = vi[1].Split(',');
                                string[] args = new string[2 + param.Length];
                                args[0] = Memoria.dlln[ax].ToString();
                                args[1] = vi[0];
                                for (int x = 2; x < args.Length; x++)
                                    args[x] = param[x - 2];
                                a_ = a_.Replace("!" + Memoria.dlln[ax].ToString() + "." + ael + ")", Memoria.dll_m(args).ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memoria.insn.Count > 0 && Memoria.insv.Count > 0 && Memoria.inso.Count > 0)
            {
                for (int ax = 0; ax < Memoria.insn.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memoria.insn[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memoria.insn[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memoria.insn[ax].ToString().Length + 1; la < a_.Length; la++)
                            {
                                if (ok)
                                {
                                    if (a_[la] == ')')
                                    {
                                        ok = false;
                                        break;
                                    }
                                    else
                                    {
                                        ael += a_[la];
                                    }
                                }
                                if (la == o + Memoria.insn[ax].ToString().Length + 1 && a_[la] == '.')
                                {
                                    ok = true;
                                }
                            }
                            string[] vi = ael.Split('(');
                            if (vi[1] == "")
                            {
                                string[] args = new string[2];
                                args[0] = Memoria.insn[ax].ToString();
                                args[1] = vi[0];
                                a_ = a_.Replace("!" + Memoria.insn[ax].ToString() + "." + ael + ")", Memoria.ins_m(args).ToString());
                            }
                            else
                            {
                                string[] param = vi[1].Split(',');
                                string[] args = new string[2 + param.Length];
                                args[0] = Memoria.insn[ax].ToString();
                                args[1] = vi[0];
                                for (int x = 2; x < args.Length; x++)
                                    args[x] = param[x - 2];
                                a_ = a_.Replace("!" + Memoria.insn[ax].ToString() + "." + ael + ")", Memoria.ins_m(args).ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memoria.iton.Count > 0 && Memoria.itov.Count > 0)
            {
                for (int ax = 0; ax < Memoria.iton.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memoria.iton[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memoria.iton[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memoria.iton[ax].ToString().Length + 1; la < a_.Length; la++)
                            {
                                if (ok)
                                {
                                    if (a_[la] == ')')
                                    {
                                        ok = false;
                                        break;
                                    }
                                    else
                                    {
                                        ael += a_[la];
                                    }
                                }
                                if (la == o + Memoria.iton[ax].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            string[] args = null;
                            if (ael != "")
                            {
                                string[] aels = ael.Split(',');
                                args = new string[aels.Length + 1];
                                args[0] = Memoria.iton[ax].ToString();
                                for (int j = 1; j < args.Length; j++)
                                    args[j] = aels[j - 1];
                            }
                            else
                            {
                                args = new string[] { Memoria.iton[ax].ToString() };
                            }
                            a_ = a_.Replace("!" + Memoria.iton[ax].ToString() + "(" + ael + ")", Memoria.ito_m(args).ToString());
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            while (a_.Contains("!read"))
            {
                int fir = a_.IndexOf("!read");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!read".Length; lon < a_.Length; lon++)
                {
                    if(a_[lon] == ')')
                    {
                        ok = false;
                    }
                    else
                    {
                        if (ok)
                        {
                            wrt += a_[lon];
                        }
                    }
                    if(a_[lon] == '(')
                    {
                        ok = true;
                    }
                }
                Console.Write(wrt + " ");
                if(wrt != "")
                    a_ = new Regex(Regex.Escape("!read(" + wrt + ")")).Replace(a_, Console.ReadLine(), 1);
                else
                    a_ = new Regex(Regex.Escape("!read")).Replace(a_, Console.ReadLine(), 1);
            }

            while (a_.Contains("!math"))
            {
                int fir = a_.IndexOf("!math");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!math".Length; lon < a_.Length; lon++)
                {
                    if (a_[lon] == ')')
                    {
                        ok = false;
                    }
                    else
                    {
                        if (ok)
                        {
                            wrt += a_[lon];
                        }
                    }
                    if (a_[lon] == '(')
                    {
                        ok = true;
                    }
                }
                string res = Comandos.exp(wrt);
                a_ = new Regex(Regex.Escape("!math(" + wrt + ")")).Replace(a_, res, 1);
            }

            while (a_.Contains("!logic"))
            {
                int fir = a_.IndexOf("!logic");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!logic".Length; lon < a_.Length; lon++)
                {
                    if (a_[lon] == ')')
                    {
                        ok = false;
                    }
                    else
                    {
                        if (ok)
                        {
                            wrt += a_[lon];
                        }
                    }
                    if (a_[lon] == '(')
                    {
                        ok = true;
                    }
                }
                a_ = new Regex(Regex.Escape("!logic(" + wrt + ")")).Replace(a_, new logicabooleana("(" + wrt + ")").operar().ToString(), 1);
            }

            while (a_.Contains("!invoke"))
            {
                int fir = a_.IndexOf("!invoke");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!invoke".Length; lon < a_.Length; lon++)
                {
                    if (a_[lon] == ')')
                    {
                        ok = false;
                    }
                    else
                    {
                        if (ok)
                        {
                            wrt += a_[lon];
                        }
                    }
                    if (a_[lon] == '(')
                    {
                        ok = true;
                    }
                }
                Program.ConsoleAnalizer(wrt.Replace("@", "\""));
                a_ = new Regex(Regex.Escape("!invoke(" + wrt + ")")).Replace(a_, "");
            }

            while (a_.Contains("!overfile"))
            {
                int fir = a_.IndexOf("!overfile");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!overfile".Length; lon < a_.Length; lon++)
                {
                    if (a_[lon] == ')')
                    {
                        ok = false;
                    }
                    else
                    {
                        if (ok)
                        {
                            wrt += a_[lon];
                        }
                    }
                    if (a_[lon] == '(')
                    {
                        ok = true;
                    }
                }
                a_ = new Regex(Regex.Escape("!overfile(" + wrt + ")")).Replace(a_, Comandos.readfile(wrt), 1);
            }

            while (a_.Contains("!rand"))
            {
                string ael = "";
                int o = a_.IndexOf("!rand");
                bool ok = false;
                for (int la = o + "!rand".Length; la < a_.Length; la++)
                {
                    if (ok)
                    {
                        if (a_[la] == ')')
                        {
                            ok = false;
                        }
                        else
                        {
                            ael += a_[la];
                        }
                    }
                    if (la == o + "!rand".Length && a_[la] == '(')
                    {
                        ok = true;
                    }
                }
                string[] args = null;
                if (ael != "")
                {
                    string[] aels = ael.Split(',');
                    args = new string[aels.Length + 1];
                    args[0] = "rand";
                    for (int j = 1; j < args.Length; j++)
                        args[j] = aels[j - 1];
                }
                else
                    args = new string[] { "rand" };
                a_ = new Regex(Regex.Escape("!rand(" + ael + ")")).Replace(a_, Comandos.rand(args).ToString(), 1);
            }

            return a_;
        }
    }
}
