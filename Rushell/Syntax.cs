using System;
using System.Text.RegularExpressions;

namespace Rushell
{
    class Syntax
    {
        public static string Analizar(string a_)
        {
            if (Memory.NewSyntax)
                return new NewSyntax(a_).Result;
            a_ = a_.Replace("!always()", int.MaxValue.ToString());
            a_ = a_.Replace("!never()", int.MinValue.ToString());
            a_ = a_.Replace("!repeatvalue()", Memory.repeatvalue.ToString());
            a_ = a_.Replace("!here()", System.IO.Directory.GetCurrentDirectory());
            a_ = a_.Replace("!there()", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            a_ = a_.Replace("\\n", "\n");
            a_ = a_.Replace("\\t", "\t");

            if (Memory.varn.Count > 0 && Memory.varv.Count > 0)
            {
                for (int al = 0; al < Memory.varn.Count; al++)
                {
                    try
                    {
                        if (Memory.varv[al].ToString() == "System.String[]")
                        {
                            string[] sustituidor = (string[])Memory.varv[al];
                            int len = sustituidor.Length;
                            for (int st = 0; st < len; st++)
                            {
                                a_ = a_.Replace("!" + Memory.varn[al].ToString() + "-" + st, sustituidor[st]);
                            }
                            a_ = a_.Replace("!" + Memory.varn[al].ToString(), sustituidor[0]);
                        }
                        else
                            a_ = a_.Replace("!" + Memory.varn[al].ToString(), Memory.varv[al].ToString());
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memory.defn.Count > 0 && Memory.defv.Count > 0)
            {
                for (int al = 0; al < Memory.defn.Count; al++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memory.defn[al].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memory.defn[al].ToString());
                            bool ok = false;
                            for (int la = o + Memory.defn[al].ToString().Length + 1; la < a_.Length; la++)
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
                                if (la == o + Memory.defn[al].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            string[] args = null;
                            if (ael != "")
                            {
                                string[] aels = ael.Split(',');
                                args = new string[aels.Length + 1];
                                args[0] = Memory.defn[al].ToString();
                                for (int j = 1; j < args.Length; j++)
                                    args[j] = aels[j - 1];
                            }
                            else
                            {
                                args = new string[] { Memory.defn[al].ToString() };
                            }
                            Memory.Call_D(args);
                            a_ = a_.Replace("!" + Memory.defn[al].ToString() + "(" + ael + ")", "");
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memory.funn.Count > 0 && Memory.funv.Count > 0)
            {
                for (int ax = 0; ax < Memory.funn.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memory.funn[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memory.funn[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memory.funn[ax].ToString().Length + 1; la < a_.Length; la++)
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
                                if (la == o + Memory.funn[ax].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            a_ = a_.Replace("!" + Memory.funn[ax].ToString() + "(" + ael + ")", Memory.Cal_F(ax, ael).ToString());
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memory.dlln.Count > 0 && Memory.dllv.Count > 0)
            {
                for (int ax = 0; ax < Memory.dlln.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memory.dlln[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memory.dlln[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memory.dlln[ax].ToString().Length + 1; la < a_.Length; la++)
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
                                if (la == o + Memory.dlln[ax].ToString().Length + 1 && a_[la] == '.')
                                {
                                    ok = true;
                                }
                            }
                            string[] vi = ael.Split('(');
                            if (vi[1] == "")
                            {
                                string[] args = new string[2];
                                args[0] = Memory.dlln[ax].ToString();
                                args[1] = vi[0];
                                a_ = a_.Replace("!" + Memory.dlln[ax].ToString() + "." + ael + ")", Memory.dll_m(args).ToString());
                            }
                            else
                            {
                                string[] param = vi[1].Split(',');
                                string[] args = new string[2 + param.Length];
                                args[0] = Memory.dlln[ax].ToString();
                                args[1] = vi[0];
                                for (int x = 2; x < args.Length; x++)
                                    args[x] = param[x - 2];
                                a_ = a_.Replace("!" + Memory.dlln[ax].ToString() + "." + ael + ")", Memory.dll_m(args).ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memory.insn.Count > 0 && Memory.insv.Count > 0 && Memory.inso.Count > 0)
            {
                for (int ax = 0; ax < Memory.insn.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memory.insn[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memory.insn[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memory.insn[ax].ToString().Length + 1; la < a_.Length; la++)
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
                                if (la == o + Memory.insn[ax].ToString().Length + 1 && a_[la] == '.')
                                {
                                    ok = true;
                                }
                            }
                            string[] vi = ael.Split('(');
                            if (vi[1] == "")
                            {
                                string[] args = new string[2];
                                args[0] = Memory.insn[ax].ToString();
                                args[1] = vi[0];
                                a_ = a_.Replace("!" + Memory.insn[ax].ToString() + "." + ael + ")", Memory.ins_m(args).ToString());
                            }
                            else
                            {
                                string[] param = vi[1].Split(',');
                                string[] args = new string[2 + param.Length];
                                args[0] = Memory.insn[ax].ToString();
                                args[1] = vi[0];
                                for (int x = 2; x < args.Length; x++)
                                    args[x] = param[x - 2];
                                a_ = a_.Replace("!" + Memory.insn[ax].ToString() + "." + ael + ")", Memory.ins_m(args).ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            if (Memory.iton.Count > 0 && Memory.itov.Count > 0)
            {
                for (int ax = 0; ax < Memory.iton.Count; ax++)
                {
                    try
                    {
                        while (a_.Contains("!" + Memory.iton[ax].ToString()))
                        {
                            string ael = "";
                            int o = a_.IndexOf("!" + Memory.iton[ax].ToString());
                            bool ok = false;
                            for (int la = o + Memory.iton[ax].ToString().Length + 1; la < a_.Length; la++)
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
                                if (la == o + Memory.iton[ax].ToString().Length + 1 && a_[la] == '(')
                                {
                                    ok = true;
                                }
                            }
                            string[] args = null;
                            if (ael != "")
                            {
                                string[] aels = ael.Split(',');
                                args = new string[aels.Length + 1];
                                args[0] = Memory.iton[ax].ToString();
                                for (int j = 1; j < args.Length; j++)
                                    args[j] = aels[j - 1];
                            }
                            else
                            {
                                args = new string[] { Memory.iton[ax].ToString() };
                            }
                            a_ = a_.Replace("!" + Memory.iton[ax].ToString() + "(" + ael + ")", Memory.ito_m(args).ToString());
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
                string res = Commands.exp(wrt);
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
                a_ = new Regex(Regex.Escape("!logic(" + wrt + ")")).Replace(a_, new BooleanLogic("(" + wrt + ")").operar().ToString(), 1);
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
                a_ = new Regex(Regex.Escape("!overfile(" + wrt + ")")).Replace(a_, Commands.readfile(wrt), 1);
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
                a_ = new Regex(Regex.Escape("!rand(" + ael + ")")).Replace(a_, Commands.rand(args).ToString(), 1);
            }

            return a_;
        }
    }
}
