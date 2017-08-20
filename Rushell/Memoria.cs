using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rushell
{
    class Memoria
    {
        public static ArrayList varn = new ArrayList();
        public static ArrayList varv = new ArrayList();

        public static ArrayList funn = new ArrayList();
        public static ArrayList funv = new ArrayList();

        public static ArrayList defn = new ArrayList();
        public static ArrayList defv = new ArrayList();

        public static string CScompilerpath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";

        public static void Add_V(string[] args)
        {
            varn.Add(args[1]);
            if(args.Length > 3)
            {
                string[] n_ = new string[args.Length - 2];
                for (int dx = 2; dx < args.Length; dx++)
                    n_[dx - 2] = Sintaxis.Analizar(args[dx]);
                varv.Add(n_);
            }
            else if(args.Length == 3)
            {
                varv.Add(Sintaxis.Analizar(args[2]));
            }
            else
            {
                varv.Add("null");
            }
        }

        public static void Set_V(string[] args)
        {
            int vno = varn.IndexOf(args[0]);
            if(varv[vno].ToString() == "System.String[]")
            {
                string[] varvv = (string[])varv[vno];
                for (int dm = 1; dm < args.Length; dm++)
                {
                    if ((dm % 2) != 0)
                        varvv[int.Parse(args[dm])] = args[dm + 1];
                }
                varv[vno] = varvv;
            }
            else
            {
                if (args.Length > 2)
                {
                    string[] arg = new string[args.Length - 2];
                    for (int ags = 0; ags < arg.Length; ags++)
                    {
                        arg[ags] = Sintaxis.Analizar(args[ags + 2]);
                    }
                    varv[vno] = arg;
                }
                else if (args.Length == 2)
                {
                    varv[vno] = Sintaxis.Analizar(args[1]);
                }
            }
        }

        public static void Add_F(string[] args)
        {
            string px = "";
            for (int nl = 1; nl < args.Length; nl++)
            {
                px += args[nl];
            }
            funn.Add(args[1]);
            funv.Add(new Function(px));
        }

        public static double Cal_F(int place, string prm)
        {
            double[] partes = Sintaxis.Analizar(prm).Split(',').Select(double.Parse).ToArray();
            Function calc = (Function)funv[place];
            return calc.calculate(partes);
        }

        public static void Add_D(string[] args)
        {
            defn.Add(args[1]);
            ArrayList arr = new ArrayList(args);
            arr.RemoveAt(0);
            arr.RemoveAt(0);
            defv.Add((string[])arr.ToArray(typeof(string)));
        }

        public static string[] Get_D(string name)
        {
            return (string[])defv[defn.IndexOf(name)];
        }

        public static void Import(string[] paths)
        {
            for (int imp = 1; imp < paths.Length; imp++)
            {
                if (File.Exists(paths[imp]))
                {
                    StreamReader sr = new StreamReader(paths[imp]);
                    string st = "";
                    while ((st = sr.ReadLine()) != null)
                    {
                        if (st.StartsWith("def "))
                            Program.ConsoleAnalizer(st);
                    }
                }
                else
                {
                    Comandos.error("El archivo: " + paths[imp] + " no existe");
                }
            }
        }
    }
}
