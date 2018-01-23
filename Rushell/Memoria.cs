using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Rushell
{
    class Memoria
    {
        public static ArrayList Pila = new ArrayList();
        public static int PilaActual = 0;

        public static ArrayList varn = new ArrayList();
        public static ArrayList varv = new ArrayList();

        public static ArrayList funn = new ArrayList();
        public static ArrayList funv = new ArrayList();

        public static ArrayList defn = new ArrayList();
        public static ArrayList defp = new ArrayList();
        public static ArrayList defv = new ArrayList();

        public static ArrayList dlln = new ArrayList();
        public static ArrayList dllv = new ArrayList();

        public static ArrayList insn = new ArrayList();
        public static ArrayList insv = new ArrayList();
        public static ArrayList inso = new ArrayList();

        public static ArrayList iton = new ArrayList();
        public static ArrayList itov = new ArrayList();

        public static ArrayList condition = new ArrayList();
        public static ArrayList whiler = new ArrayList();
        public static ArrayList repeater = new ArrayList();

        public static bool whilerstop = false;
        public static bool repeaterstop = false;

        public static bool indef = false;

        public static bool defining = false;

        public static bool init = false;

        public static int repeatvalue = 0;

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

        public static void End_V(string name)
        {
            varv.RemoveAt(varn.LastIndexOf(name));
            varn.RemoveAt(varn.LastIndexOf(name));
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
            if (((string)arr[0])[0] == '$')
            {
                defp.Add(((string)arr[0]).Substring(1).Split(new string[] { "," }, StringSplitOptions.None));
                arr.RemoveAt(0);
            }
            else
            {
                defp.Add(new string[] { });
            }
            if(((string)arr[0]) == "then")
            {
                defining = true;
                arr.RemoveAt(0);
            }
            ArrayList ins = new ArrayList();
            ins.Add((string[])arr.ToArray(typeof(string)));
            defv.Add(ins);
        }

        public static void Call_D(string[] args)
        {
            string[] param = ((string[])defp[defn.IndexOf(args[0])]);
            if (param.Length != args.Length - 1)
                Comandos.error("El número de parámetros no coincidía");
            else
            {
                indef = true;
                for (int id = 0; id < param.Length; id++)
                    Add_V(new string[] { "var", param[id], args[id + 1] });
                foreach (string[] line in (ArrayList)defv[defn.IndexOf(args[0])])
                    Program.Procesar(line);
                for (int id = 0; id < param.Length; id++)
                    End_V(param[id]);
            }
        }

        public static void Import(string[] paths)
        {
            for (int imp = 1; imp < paths.Length; imp++)
            {
                if (File.Exists(paths[imp]))
                {
                    StreamReader sr = new StreamReader(paths[imp]);
                    string st = "";
                    init = true;
                    while ((st = sr.ReadLine()) != null)
                        if (st.StartsWith("def ") || st.StartsWith("init ") || defining)
                            Program.ConsoleAnalizer(st);
                    init = false;
                }
                else
                {
                    Comandos.error("El archivo: " + paths[imp] + " no existe");
                }
            }
        }

        public static void from_i(string[] args)
        {
            if (args[3].Equals("to") && args.Length == 5)
            {
                if (insn.Contains(args[1]))
                {
                    iton.Add(args[4]);
                    itov.Add(new string[] { args[1], args[2] });
                }
            }
            else if (args[2].Equals("load") && args.Length > 2)
            {
                if (!File.Exists(args[1]))
                    if (!File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + args[1]))
                    {
                        Comandos.error("Directorio no encontrado: " + args[1]);
                        return;
                    }
                    else
                        args[1] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + args[1];
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
            else if (args[2].Equals("instantiate") && args.Length > 2)
            {
                if (args.Length == 4)
                {
                    Type upper = (Type)dllv[dlln.IndexOf(args[1])];
                    ConstructorInfo cinf = upper.GetConstructor(Type.EmptyTypes);
                    object instance = cinf.Invoke(new object[] { });
                    insn.Add(args[3]);
                    insv.Add(instance);
                    inso.Add(upper);
                }
                else if (args.Length > 4)
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
                    Comandos.error("Insuficientes argumentos");
                }
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
            if (args.Length == 2)
            {
                return toinv.Invoke(null, new object[] { });
            }
            else if (args.Length > 2)
            {
                object[] param = new object[args.Length - 2];
                for (int x = 2; x < args.Length; x++)
                    param[x - 2] = args[x];
                return toinv.Invoke(null, param);
            }
            else
            {
                Comandos.error("Insuficientes argumentos");
            }
            return null;
        }

        public static object ins_m(string[] args)
        {
            Type imp = (Type)inso[insn.IndexOf(args[0])];
            MethodInfo toinv = imp.GetMethod(args[1]);
            if (args.Length == 2)
            {
                return toinv.Invoke(insv[insn.IndexOf(args[0])], new object[]{ });
            }
            else if (args.Length > 2)
            {
                ArrayList param = new ArrayList();
                bool paramson = false;
                ArrayList paramsons = new ArrayList();
                for (int x = 2; x < args.Length; x++)
                {
                    if (paramson)
                    {
                        paramsons.Add(Sintaxis.Analizar(args[x]));
                    }
                    else
                    {
                        if (args[x] == "$")
                            paramson = true;
                        else
                            param.Add(Sintaxis.Analizar(args[x]));
                    }
                }
                if (paramson)
                    param.Add((string[])paramsons.ToArray(typeof(string)));
                return toinv.Invoke(insv[insn.IndexOf(args[0])], (object[])param.ToArray(typeof(object)));
            }
            else
            {
                Comandos.error("Insuficientes argumentos");
            }
            return null;
        }

        public static object ito_m(string[] args)
        {
            string[] par = new string[args.Length + 1];
            for (int x = 1; x < par.Length; x++)
            {
                par[x] = args[x - 1];
            }
            string[] p = (string[])itov[iton.IndexOf(args[0])];
            par[0] = p[0];
            par[1] = p[1];
            return ins_m(par);
        }
    }
}