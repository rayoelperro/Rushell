using Microsoft.Scripting.Hosting;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Rushell
{
    class Commands
    {
        public static void writeln(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(Syntax.Analizar(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void write(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.Write(Syntax.Analizar(args[ar]));
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
                l[x] = Syntax.Analizar(l[x]);
            Console.Write(String.Format(Syntax.Analizar(args[1]),l.ToArray()));
        }

        public static void writefln(string[] args)
        {
            args[1] += "\n";
            writef(args);
        }

        public static void error(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Runtime error: " + ln);
            Console.ForegroundColor = ConsoleColor.White;
            Environment.Exit(-1);
        }

        public static void snt(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Syntax.Analizar(args[ar]);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void snt(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Syntax.Analizar(ln);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string exp(string wrt)
        {
            string n = new Expression(wrt).calculate().ToString();
            if (n == "NeuN")
                error("Wrong mathematic equation: " + wrt);
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
                    error("The declaration you tried finish doesn't exists: " + args[1]);
            }
            else if (args.Length == 3)
            {
                if (args[1] == "prompt")
                    write(new string[] { args[2] });
                else
                    error("The declaration you tried finish doesn't exists: " + args[1]);
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
                error("Too much arguments for the 'end' function");
            }
        }

        public static void endpython()
        {
            if (Memory.python_ing)
            {
                apython(Memory.PythonArgs,false);
            }
            else
            {
                error("It tried to finish a python process that doesn't exists");
            }
        }

        public static void endlua()
        {
            if (Memory.lua_ing)
            {
                alua(Memory.LuaArgs,false);
            }
            else
            {
                error("It tried to finish a lua process that doesn't exists");
            }
        }

        public static void apython(string c, bool frompath)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                ScriptSource source = null;
                if(frompath)
                    source = Memory.PythonEnv.CreateScriptSourceFromFile(c);
                else
                    source = Memory.PythonEnv.CreateScriptSourceFromString(c);
                var compiled = source.Compile();
                var result = compiled.Execute(Memory.PythonEsc);
                Memory.python_ing = false;
                Memory.PythonArgs = "";
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Memory.python_ing = false;
                Memory.PythonArgs = "";
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(-1);
            }
        }

        public static void alua(string c, bool frompath)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                if(frompath)
                    Memory.LuaEnv.DoFile(c);
                else
                    Memory.LuaEnv.DoString(c, "rushell");
                Memory.lua_ing = false;
                Memory.LuaArgs = "";
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Memory.lua_ing = false;
                Memory.LuaArgs = "";
                Console.WriteLine(e.ToString());
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(-1);
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
            int n = int.Parse(Syntax.Analizar(args[1]));
            if (args[2] == "then")
            {
                if (args.Length > 3)
                {
                    error("Too much arguments for the 'repeat' command");
                }
                else
                {
                    if (n == 0)
                        Memory.repeaterstop = true;
                    Memory.repeatvalue = 0;
                    Memory.repeater.Add(new int[] { n-1, Memory.PilaActual });
                }
            }
            else
            {
                ArrayList l = new ArrayList(args);
                l.RemoveAt(0);
                l.RemoveAt(0);
                for (int re = 0; re < n; re++)
                {
                    Memory.repeatvalue = re;
                    Program.Procesar((string[])l.ToArray(typeof(string)));
                }
            }
        }

        public static void repeater()
        {
            Memory.repeaterstop = false;
            if (Memory.repeater.Count > 0)
            {
                int times = ((int[])Memory.repeater[Memory.repeater.Count - 1])[0];
                int value = ((int[])Memory.repeater[Memory.repeater.Count - 1])[1];
                ArrayList l = new ArrayList();
                int pl = Memory.PilaActual - 1;
                int st = value + 1;
                if (Memory.indef)
                {
                    pl += 1;
                    st += 1;
                }
                for (int x = st; x <= pl; x++)
                {
                    l.Add(Memory.Pila[x]);
                }
                for (int x = 0; x < times; x++)
                {
                    Memory.repeatvalue = x + 1;
                    for (int y = 0; y < l.Count; y++)
                    {
                        Program.Procesar((string[])l[y]);
                    }
                }
                Memory.repeater.RemoveAt(Memory.repeater.Count - 1);
            }
            else
            {
                error("No repeat opened");
            }
        }

        public static void while_(string[] args)
        {
            if (args[2] == "then")
            {
                if (args.Length > 3)
                {
                    error("Too much arguments for the 'while' command");
                }
                else
                {
                    if (!(new BooleanLogic(args[1]).operar()))
                        Memory.whilerstop = true;
                    Memory.whiler.Add(new string[] { args[1], Memory.PilaActual.ToString() });
                }
            }
            else
            {
                ArrayList l = new ArrayList(args);
                l.RemoveAt(0);
                l.RemoveAt(0);
                while (new BooleanLogic(args[1]).operar())
                    Program.Procesar((string[])l.ToArray(typeof(string)));
            }
        }

        public static void whiler()
        {
            Memory.whilerstop = false;
            if (Memory.whiler.Count > 0)
            {
                string condicion = ((string[])Memory.whiler[Memory.whiler.Count - 1])[0];
                int value = int.Parse(((string[])Memory.whiler[Memory.whiler.Count - 1])[1]);
                ArrayList l = new ArrayList();
                int pl = Memory.PilaActual - 1;
                if (Memory.indef)
                    pl += 1;
                for (int x = value + 1; x <= pl; x++)
                {
                    l.Add(Memory.Pila[x]);
                }
                while(new BooleanLogic(condicion).operar())
                {
                    for (int y = 0; y < l.Count; y++)
                    {
                        Program.Clasificar((string[])l[y]);
                    }
                }
                Memory.whiler.RemoveAt(Memory.whiler.Count - 1);
            }
            else
            {
                error("No while opened");
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
                Console.WriteLine(new BooleanLogic(args[ar]).operar().ToString());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void instance(string[] args)
        {
            if (args.Length > 1)
                if (Memory.varn.IndexOf(args[1]) > -1)
                {
                    object o = Memory.varv[Memory.varn.IndexOf(args[1])];
                    List<string> s = new List<string>(args);
                    s.RemoveAt(0);
                    s.RemoveAt(0);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    if (o is IronPython.Runtime.PythonFunction)
                        Memory.PythonEnv.Operations.Invoke(o, s.ToArray());
                    else if (o is NLua.LuaFunction)
                        ((NLua.LuaFunction)o).Call(s.ToArray());
                    else
                        error(args[1] + " isn't a type able to instance itself");
                    Console.ForegroundColor = ConsoleColor.White;
                }
        }

        public static void return_(string[] args)
        {
            if (args.Length != 2) error("Wrong number of returns");
            Memory.LastRet = args[1];
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
                    Console.SetOut(Memory.outwriter);
                }
            }
            else
                error("Too much arguments for the 'log' command");
        }

        public static void if_else(string[] args)
        {
            if (args[2] == "then")
            {
                bool noelse = true;
                bool block = false;
                if (new BooleanLogic(args[1]).operar())
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
                Memory.condition.Add(new bool[] { noelse, block });
            }
            else
            {
                int els = Array.IndexOf(args, "else");
                if (els != -1)
                {
                    if (new BooleanLogic(args[1]).operar())
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
                else if (new BooleanLogic(args[1]).operar())
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
                Memory.python_ing = true;
            }
            else
            {
                error("Too much arguments for the command 'python'");
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
                Memory.lua_ing = true;
            }
            else
            {
                error("Too much arguments for the command 'lua'");
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
                    error("Too much arguments for the command 'rand'");
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
