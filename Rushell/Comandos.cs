using Microsoft.Scripting.Hosting;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Rushell
{
    class Comandos
    {
        public static void writeln(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(Sintaxis.Analizar(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void write(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.Write(Sintaxis.Analizar(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void writeliln(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(args[ar]);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void writeli(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.Write(args[ar]);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void writef(string[] args)
        {
            List<string> l = new List<string>(args);
            l.RemoveAt(0);
            l.RemoveAt(0);
            for (int x = 0; x < l.Count; x++)
                l[x] = Sintaxis.Analizar(l[x]);
            Console.Write(String.Format(Sintaxis.Analizar(args[1]),l.ToArray()));
        }

        public static void writefln(string[] args)
        {
            args[1] += "\n";
            writef(args);
        }

        public static void error(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(new Exception(ln).ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void snt(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Sintaxis.Analizar(args[ar]);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void snt(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Sintaxis.Analizar(ln);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string exp(string wrt)
        {
            string n = new Expression(wrt).calculate().ToString();
            if (n == "NeuN")
                error("Ecuación matemática erronea: " + wrt);
            return n;
        }

        public static void end(string[] args)
        {
            if (args.Length == 2)
            {
                if (args[1] == "repeat")
                    repeater();
                else if (args[1] == "while")
                    whiler();
                else if (args[1] == "lua")
                    endlua();
                else if (args[1] == "python")
                    endpython();
                else
                    error("La declaración que intentas terminar no existe: " + args[1]);
            }
            else if (args.Length == 3)
            {
                if (args[1] == "prompt")
                    write(new string[] { args[2] });
                else
                    error("La declaración que intentas terminar no existe: " + args[1]);
                Console.ReadKey();
                Environment.Exit(-1);
            }
            else if (args.Length == 1)
            {
                Console.ReadKey();
                Environment.Exit(-1);
            }
            else
            {
                error("Demasiados argumentos para la funcion 'end'");
            }
        }

        public static void endpython()
        {
            if (Memoria.python_ing)
            {
                apython(Memoria.PythonArgs,false);
            }
            else
            {
                error("Se intento finalizar un proceso de python que no existe");
            }
        }

        public static void endlua()
        {
            if (Memoria.lua_ing)
            {
                alua(Memoria.LuaArgs,false);
            }
            else
            {
                error("Se intento finalizar un proceso de lua que no existe");
            }
        }

        public static void apython(string c, bool frompath)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                ScriptSource source = null;
                if(frompath)
                    source = Memoria.PythonEnv.CreateScriptSourceFromFile(c);
                else
                    source = Memoria.PythonEnv.CreateScriptSourceFromString(c);
                var compiled = source.Compile();
                var result = compiled.Execute(Memoria.PythonEsc);
                Memoria.python_ing = false;
                Memoria.PythonArgs = "";
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error terminal");
                end(new string[] {"end"});
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void alua(string c, bool frompath)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                if(frompath)
                    Memoria.LuaEnv.DoFile(c);
                else
                    Memoria.LuaEnv.DoString(c, "rushell");
                Memoria.lua_ing = false;
                Memoria.LuaArgs = "";
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Memoria.lua_ing = false;
                Memoria.LuaArgs = "";
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void beep(string[] args)
        {
            if (args.Length == 3)
                Console.Beep(int.Parse(args[1]), int.Parse(args[2]));
            else
                Console.Beep();
        }

        public static void repeat(string[] args)
        {
            int n = int.Parse(Sintaxis.Analizar(args[1]));
            if (args[2] == "then")
            {
                if (args.Length > 3)
                {
                    error("Demasiados terminos para el comando repeat");
                }
                else
                {
                    if (n == 0)
                        Memoria.repeaterstop = true;
                    Memoria.repeatvalue = 0;
                    Memoria.repeater.Add(new int[] { n-1, Memoria.PilaActual });
                }
            }
            else
            {
                ArrayList l = new ArrayList(args);
                l.RemoveAt(0);
                l.RemoveAt(0);
                for (int re = 0; re < n; re++)
                {
                    Memoria.repeatvalue = re;
                    Program.Procesar((string[])l.ToArray(typeof(string)));
                }
            }
        }

        public static void repeater()
        {
            Memoria.repeaterstop = false;
            if (Memoria.repeater.Count > 0)
            {
                int times = ((int[])Memoria.repeater[Memoria.repeater.Count - 1])[0];
                int value = ((int[])Memoria.repeater[Memoria.repeater.Count - 1])[1];
                ArrayList l = new ArrayList();
                int pl = Memoria.PilaActual - 1;
                int st = value + 1;
                if (Memoria.indef)
                {
                    pl += 1;
                    st += 1;
                }
                for (int x = st; x <= pl; x++)
                {
                    l.Add(Memoria.Pila[x]);
                }
                for (int x = 0; x < times; x++)
                {
                    Memoria.repeatvalue = x + 1;
                    for (int y = 0; y < l.Count; y++)
                    {
                        Program.Procesar((string[])l[y]);
                    }
                }
                Memoria.repeater.RemoveAt(Memoria.repeater.Count - 1);
            }
            else
            {
                error("No hay ningún repeat abierto");
            }
        }

        public static void while_(string[] args)
        {
            if (args[2] == "then")
            {
                if (args.Length > 3)
                {
                    error("Demasiados terminos para el comando while");
                }
                else
                {
                    if (!(new logicabooleana(args[1]).operar()))
                        Memoria.whilerstop = true;
                    Memoria.whiler.Add(new string[] { args[1], Memoria.PilaActual.ToString() });
                }
            }
            else
            {
                ArrayList l = new ArrayList(args);
                l.RemoveAt(0);
                l.RemoveAt(0);
                while (new logicabooleana(args[1]).operar())
                    Program.Procesar((string[])l.ToArray(typeof(string)));
            }
        }

        public static void whiler()
        {
            Memoria.whilerstop = false;
            if (Memoria.whiler.Count > 0)
            {
                string condicion = ((string[])Memoria.whiler[Memoria.whiler.Count - 1])[0];
                int value = int.Parse(((string[])Memoria.whiler[Memoria.whiler.Count - 1])[1]);
                ArrayList l = new ArrayList();
                int pl = Memoria.PilaActual - 1;
                if (Memoria.indef)
                    pl += 1;
                for (int x = value + 1; x <= pl; x++)
                {
                    l.Add(Memoria.Pila[x]);
                }
                while(new logicabooleana(condicion).operar())
                {
                    for (int y = 0; y < l.Count; y++)
                    {
                        Program.Clasificar((string[])l[y]);
                    }
                }
                Memoria.whiler.RemoveAt(Memoria.whiler.Count - 1);
            }
            else
            {
                error("No hay ningún while abierto");
            }
        }

        public static void math(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(exp(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void logic(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(new logicabooleana(args[ar]).operar().ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void instance(string[] args)
        {
            if (args.Length > 1)
                if (Memoria.varn.IndexOf(args[1]) > -1)
                {
                    object o = Memoria.varv[Memoria.varn.IndexOf(args[1])];
                    List<string> s = new List<string>(args);
                    s.RemoveAt(0);
                    s.RemoveAt(0);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    if (o is IronPython.Runtime.PythonFunction)
                        Memoria.PythonEnv.Operations.Invoke(o, s.ToArray());
                    else if (o is NLua.LuaFunction)
                        ((NLua.LuaFunction)o).Call(s.ToArray());
                    else
                        error(args[1] + " no es un tipo capaz de instanciarse");
                    Console.ForegroundColor = ConsoleColor.White;
                }
        }

        public static void output(string[] args)
        {
            if (args.Length == 3)
            {
                if (args[1] == "set")
                    Console.SetOut(File.AppendText(args[2]));
            }
            else if(args.Length == 2)
            {
                if (args[1] == "console")
                {
                    Console.Out.Flush();
                    Console.Out.Close();
                    Console.SetOut(Memoria.outwriter);
                }
            }
            else
                error("Demasiados argumentos para log");
        }

        public static void if_else(string[] args)
        {
            if (args[2] == "then")
            {
                bool noelse = true;
                bool block = false;
                if (new logicabooleana(args[1]).operar())
                {
                    if (args.Length > 3)
                    {
                        string[] sub = new string[args.Length - 3];
                        for (int pl = 0; pl < sub.Length; pl++)
                        {
                            sub[pl] = args[pl + 3];
                        }
                        Program.Procesar(sub);
                    }
                }
                else
                {
                    block = true;
                    noelse = false;
                }
                Memoria.condition.Add(new bool[] { noelse, block });
            }
            else
            {
                int els = Array.IndexOf(args, "else");
                if (els != -1)
                {
                    if (new logicabooleana(args[1]).operar())
                    {
                        string[] sub = new string[els - 2];
                        for (int pl = 0; pl < sub.Length; pl++)
                        {
                            sub[pl] = args[pl + 2];
                        }
                        Program.Procesar(sub);
                    }
                    else
                    {
                        string[] sub = new string[args.Length - (els + 1)];
                        for (int pl = 0; pl < sub.Length; pl++)
                        {
                            sub[pl] = args[pl + els + 1];
                        }
                        Program.Procesar(sub);
                    }
                }
                else if (new logicabooleana(args[1]).operar())
                {
                    string[] sub = new string[args.Length-2];
                    for (int pl = 0; pl < sub.Length; pl++)
                    {
                        sub[pl] = args[pl + 2];
                    }
                    Program.Procesar(sub);
                }
            }
        }

        public static void python(string[] args)
        {
            if (args.Length == 3)
            {
                if (args[1] == "run")
                {
                    apython(args[2], true);
                }
            }
            else if (args.Length == 1)
            {
                Memoria.python_ing = true;
            }
            else
            {
                error("Demasiados argumentos para el comando 'lua'");
            }
        }

        public static void lua(string[] args)
        {
            if (args.Length == 3)
            {
                if (args[1] == "run")
                {
                    alua(args[2], true);
                }
            }
            else if (args.Length == 1)
            {
                Memoria.lua_ing = true;
            }
            else
            {
                error("Demasiados argumentos para el comando 'lua'");
            }
        }

        public static int rand(string[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return new Random().Next(int.MinValue, int.MaxValue);
                case 2:
                    return new Random().Next(0, int.Parse(args[1]));
                case 3:
                    return new Random().Next(int.Parse(args[1]), int.Parse(args[2]));
                default:
                    error("demasiados argumentos para 'rand'");
                    return 0;
            }
        }

        public static void system(string[] args)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + string.Join(" ",args).Substring(args[0].Length+1));
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardError = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = false;
            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            string errors = proc.StandardError.ReadToEnd();
            writeli(new string[] { "writeli", result });
            if (errors != "")
                error(errors);
        }

        public static void inv(string[] args)
        {
            for (int ar = 1; ar < args.Length; ar++)
                Program.ConsoleAnalizer(args[ar].Replace("@","\""));
        }

        public static void saltos(string[] args)
        {
            for (int ar = 1; ar < args.Length; ar++)
                if (args[ar] == "\\n")
                    Console.WriteLine("");
                else if (args[ar] == "\\e")
                    Console.Write(" ");
                else
                    error("Salto inidentificado: " + args[ar]);
        }

        public static void loadscript(string path)
        {
            StreamReader str = new StreamReader(path);
            string linea = "";
            while ((linea = str.ReadLine()) != null)
                Program.ConsoleAnalizer(linea);
        }

        public static void writefile(string[] args)
        {
            StreamWriter toWr = new StreamWriter(args[1]);
            for (int fl = 2; fl < args.Length; fl++)
                toWr.WriteLine(args[fl]);
            toWr.Close();
        }

        public static string readfile(string path)
        {
            StreamReader toRe = new StreamReader(path);
            string reader = toRe.ReadToEnd();
            toRe.Close();
            return reader;
        }

        public static void async(string[] args)
        {
            string[] then = new string[args.Length - 1];
            for (int x = 1; x < args.Length; x++)
                then[x - 1] = args[x];
            Thread ambit = new Thread(() => Program.Procesar(then));
            ambit.Start();
        }
    }
}
