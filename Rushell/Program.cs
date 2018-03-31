using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Rushell
{
    class Program
    {
        public static Version product = new Version(2,6,5,18);

        public static void head()
        {
            Console.WriteLine("Rushell Version: " + product + "\nFor: " + Environment.OSVersion + "\n");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void Main(string[] args)
        {
            Shareable s = new Shareable();
            Memory.PythonEnv.GetSearchPaths().Add(@"C:\Python27\Lib");
            Memory.PythonEsc.SetVariable("memory", s);
            Memory.LuaEnv["memory"] = s;

            Console.Title = "Rushell";
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length <= 0)
            {
                head();
                ReadConsole();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                if (File.Exists(args[0]))
                {
                    if(args.Length>0)
                        arguments(args);
                    StreamReader str = new StreamReader(args[0]);
                    string linea = "";
                    while ((linea = str.ReadLine()) != null)
                        ConsoleAnalizer(linea);
                    if(new FileInfo(args[0]).Extension==".rli")
                        ReadConsole();
                }
                else
                {
                    Procesar(args);
                    ReadConsole();
                }
            }
        }

        public static void ReadConsole()
        {
            while (true)
            {
                ConsoleAnalizer(Console.ReadLine());
            }
        }

        public static void ConsoleAnalizer(string comando)
        {
            if(Memory.python_ing || Memory.lua_ing)
            {
                if (comando.Length > 0)
                    if (comando == "end python" || comando == "end lua")
                        Procesar(comando.Split(' '));
                    else
                        Procesar(new string[] { comando });
                return;
            }
            ArrayList elementos = new ArrayList();
            string actual = null;
            bool comillas = false;
            for (int x = 0; x < comando.Length; x++)
            {
                if ((comando[x] == ' ' || comando[x] == '\t') && !comillas)
                {
                    if (actual != null)
                    {
                        elementos.Add(actual);
                        actual = null;
                    }
                }
                else if (comando[x] == '"')
                {
                    if (x - 1 > 0 && comando.Length > x - 1)
                        if (comando[x - 1] != '\\')
                            if (comillas)
                                comillas = false;
                            else
                                comillas = true;
                        else
                            actual = actual.Remove(actual.Length - 1) + comando[x];
                    else
                        if (comillas)
                        comillas = false;
                    else
                        comillas = true;
                }
                else
                {
                    if (actual == null)
                    {
                        actual = "";
                        actual += comando[x];
                    }
                    else
                    {
                        actual += comando[x];
                    }
                }
            }
            if (actual != null)
            {
                elementos.Add(actual);
                actual = null;
            }
            if (elementos.Count > 0)
                Procesar((string[])elementos.ToArray(typeof(string)));
        }

        public static void Procesar(string[] args)
        {
            Memory.Pila.Add(args);
            Clasificar(args);
            Memory.PilaActual++;
        }

        public static void Clasificar(string[] args)
        {
            if (args.Length > 0)
            {
                if (Memory.condition.Count > 0)
                {
                    if (args[0] == "end" && args[1] == "if" && args.Length == 2)
                    {
                        Memory.condition.RemoveAt(Memory.condition.Count - 1);
                    }
                    else if (args[0] == "else" && args.Length == 1)
                    {
                        ((bool[])Memory.condition[Memory.condition.Count - 1])[1] = ((bool[])Memory.condition[Memory.condition.Count - 1])[0];
                    }
                    else if (args[0] == "else" && args[1] == "if" && args.Length == 3)
                    {
                        if (!((bool[])Memory.condition[Memory.condition.Count - 1])[0] && new BooleanLogic(args[2]).operar())
                        {
                            ((bool[])Memory.condition[Memory.condition.Count - 1])[1] = false;
                            ((bool[])Memory.condition[Memory.condition.Count - 1])[0] = true;
                        }
                        else
                        {
                            ((bool[])Memory.condition[Memory.condition.Count - 1])[1] = true;
                        }
                    }
                    else if (!((bool[])Memory.condition[Memory.condition.Count - 1])[1])
                        swc(args);
                }
                else
                {
                    if (Memory.defining)
                    {
                        if (args[0] == "end" && args[1] == "def" && args.Length == 2)
                        {
                            Memory.defining = false;
                        }
                        else
                        {
                            ((ArrayList)Memory.defv[Memory.defv.Count - 1]).Add(args);
                        }
                    }
                    else if (Memory.python_ing)
                        if (args.Length == 2 && args[0] == "end" && args[1] == "python")
                            Commands.end(args);
                        else
                            Memory.PythonArgs += String.Join(" ", args) + "\n";
                    else if (Memory.lua_ing)
                        if (args.Length == 2 && args[0] == "end" && args[1] == "lua")
                            Commands.end(args);
                        else
                            Memory.LuaArgs += String.Join(" ", args) + "\n";
                    else
                        {
                            if (!Memory.repeaterstop && !Memory.whilerstop)
                                swc(args);
                            else if (Memory.whilerstop)
                            {
                                if (args[0] == "end" && args[1] == "while" && args.Length == 2)
                                    swc(args);
                            }
                            else if (Memory.repeaterstop)
                            {
                                if (args[0] == "end" && args[1] == "repeat" && args.Length == 2)
                                    swc(args);
                            }
                            else
                            {
                                Commands.error("Error de analisis");
                            }
                        }
                }
            }
        }

        public static void swc(string[] args)
        {
            switch (args[0])
            {
                case "news":
                    Memory.NewSyntax = true;
                    break;
                case "write":
                    Commands.write(args);
                    break;
                case "writef":
                    Commands.writef(args);
                    break;
                case "writeli":
                    Commands.writeli(args);
                    break;
                case "writeliln":
                    Commands.writeliln(args);
                    break;
                case "writeln":
                    Commands.writeln(args);
                    break;
                case "writefln":
                    Commands.writefln(args);
                    break;
                case "exit":
                    Environment.Exit(-1);
                    break;
                case "beep":
                    Commands.beep(args);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "repeat":
                    Commands.repeat(args);
                    break;
                case "math":
                    Commands.math(args);
                    break;
                case "logic":
                    Commands.logic(args);
                    break;
                case "if":
                    Commands.if_else(args);
                    break;
                case "var":
                    Memory.Add_V(true,args);
                    break;
                case "number":
                    Memory.Add_V("number",args);
                    break;
                case "bool":
                    Memory.Add_V("bool", args);
                    break;
                case "str":
                    Memory.Add_V("str", args);
                    break;
                case "lit":
                    Memory.Add_V("lit", args);
                    break;
                case "fun":
                    Memory.Add_F(args);
                    break;
                case "overfile":
                    Commands.writefile(args);
                    break;
                case "while":
                    Commands.while_(args);
                    break;
                case "wait":
                    Thread.Sleep(int.Parse(args[1]));
                    break;
                case "title":
                    Console.Title = args[1];
                    break;
                case "snt":
                    Commands.snt(args);
                    break;
                case "sa":
                    Commands.saltos(args);
                    break;
                case "\\n":
                    Console.WriteLine("");
                    break;
                case "\\e":
                    Console.Write(" ");
                    break;
                case "inv":
                    Commands.inv(args);
                    break;
                case "def":
                    Memory.Add_D(args);
                    break;
                case "import":
                    Memory.Import(args);
                    break;
                case "from":
                    Memory.from_i(args);
                    break;
                case "async":
                    Commands.async(args);
                    break;
                case "system":
                    Commands.system(args);
                    break;
                case "#":
                    break;
                case "run":
                    Commands.loadscript(args[1]);
                    break;
                case "end":
                    Commands.end(args);
                    break;
                case "lua":
                    Commands.lua(args);
                    break;
                case "python":
                    Commands.python(args);
                    break;
                case "instance":
                    Commands.instance(args);
                    break;
                case "output":
                    Commands.output(args);
                    break;
                case "return":
                    Commands.return_(args);
                    break;
                case "init":
                    if (Memory.init)
                    {
                        ArrayList r = new ArrayList(args);
                        r.RemoveAt(0);
                        swc((string[])r.ToArray(typeof(string)));
                    }
                    break;
                default:
                    if (Memory.varn.Contains(args[0]))
                        Memory.Set_V(args);
                    else if (Memory.defn.Contains(args[0]))
                        Memory.Call_D(args);
                    else if (Memory.dlln.Contains(args[0]))
                        Memory.dll_m(args);
                    else if (Memory.insn.Contains(args[0]))
                        Memory.ins_m(args);
                    else if (Memory.iton.Contains(args[0]))
                        Memory.ito_m(args);
                    else
                        Commands.error("Wrong command: " + args[0]);
                    break;
            }
        }

        public static void arguments(string[] fullargs)
        {
            for (int ar = 1; ar < fullargs.Length; ar++)
                Memory.Add_V(true, new string[] { "var", "arg" + (ar-1), fullargs[ar] });
        }
    }
}
