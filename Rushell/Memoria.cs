﻿using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public static ArrayList dlln = new ArrayList();
        public static ArrayList dllv = new ArrayList();

        public static ArrayList insn = new ArrayList();
        public static ArrayList insv = new ArrayList();
        public static ArrayList inso = new ArrayList();

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
                        if (st.StartsWith("def "))
                            Program.ConsoleAnalizer(st);
                }
                else
                {
                    Comandos.error("El archivo: " + paths[imp] + " no existe");
                }
            }
        }

        public static void from_i(string[] args)
        {
            if (args[2].Equals("load") && args.Length > 2)
            {
                Assembly ldr = Assembly.LoadFile(args[1]);
                for (int x = 3; x < args.Length; x++)
                {
                    Type cla = ldr.GetType(args[x]);
                    if (cla == null)
                        Comandos.error("Ruta invalida");
                    else
                    {
                        dlln.Add(args[x]);
                        dllv.Add(cla);
                    }
                }
            }
            else if (args[2].Equals("instance") && args.Length > 2)
            {
                Type[] alls = new Type[args.Length - 4];
                object[] parame = new object[args.Length - 4];
                for (int x = 0; x < alls.Length; x++)
                {
                    alls[x] = typeof(string);
                }
                for (int x = 4; x < args.Length; x++)
                {
                    parame[x - 4] = args[x];
                }
                Type upper = (Type)dllv[dlln.IndexOf(args[1])];
                ConstructorInfo cinf = upper.GetConstructor(alls);
                object instance = cinf.Invoke(parame);
                insn.Add(args[3]);
                insv.Add(instance);
                inso.Add(upper);
            }
            else
            {
                Comandos.error("Uso incorrecto de 'from'");
            }
        }

        public static object dll_m(string[] args)
        {
            Type imp = (Type)dllv[dlln.IndexOf(args[0])];
            MethodInfo toinv = imp.GetMethod(args[1]);
            object[] param = new object[args.Length - 2];
            for (int x = 2; x < args.Length; x++)
                param[x - 2] = args[x];
            return toinv.Invoke(null, param);
        }

        public static object ins_m(string[] args)
        {
            Type imp = (Type)inso[insn.IndexOf(args[0])];
            MethodInfo toinv = imp.GetMethod(args[1]);
            object[] param = new object[args.Length - 2];
            for (int x = 2; x < args.Length; x++)
                param[x - 2] = args[x];
            return toinv.Invoke(insv[insn.IndexOf(args[0])], param);
        }
    }
}