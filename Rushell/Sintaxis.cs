using org.mariuszgromada.math.mxparser;
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
            a_ = a_.Replace("!repetivevalue()", Environment.GetEnvironmentVariable("Repeat", EnvironmentVariableTarget.Process));
            a_ = a_.Replace("!here()", System.IO.Directory.GetCurrentDirectory());
            a_ = a_.Replace("!there()", System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

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
                        if(a_.Contains("!" + Memoria.defn[al].ToString()))
                        {
                            a_ = a_.Replace("!" + Memoria.defn[al].ToString(), "");
                            foreach (string line in Memoria.Get_D(Memoria.defn[al].ToString()))
                                Program.ConsoleAnalizer(line.Replace("@", "\""));
                            Console.ForegroundColor = ConsoleColor.Cyan;
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
                a_ = new Regex(Regex.Escape("!math(" + wrt + ")")).Replace(a_, new Expression(wrt).calculate().ToString(), 1);
            }

            while (a_.Contains("!call"))
            {
                int fir = a_.IndexOf("!call");
                string wrt = "";
                bool ok = false;
                for (int lon = fir + "!call".Length; lon < a_.Length; lon++)
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
                a_ = new Regex(Regex.Escape("!call(" + wrt + ")")).Replace(a_, string.Join(" ", Comandos.call(("call," + wrt).Split(','))), 1);
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

            return a_;
        }
    }
}
