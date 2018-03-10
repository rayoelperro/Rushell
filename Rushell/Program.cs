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
            Memoria.PythonEnv.GetSearchPaths().Add(@"C:\Python27\Lib");
            Memoria.PythonEsc.SetVariable("memory", s);
            Memoria.LuaEnv["memory"] = s;

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
            if(Memoria.python_ing || Memoria.lua_ing)
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
            Memoria.Pila.Add(args);
            Clasificar(args);
            Memoria.PilaActual++;
        }

        public static void Clasificar(string[] args)
        {
            if (args.Length > 0)
            {
                if (Memoria.condition.Count > 0)
                {
                    if (args[0] == "end" && args[1] == "if" && args.Length == 2)
                    {
                        Memoria.condition.RemoveAt(Memoria.condition.Count - 1);
                    }
                    else if (args[0] == "else" && args.Length == 1)
                    {
                        ((bool[])Memoria.condition[Memoria.condition.Count - 1])[1] = ((bool[])Memoria.condition[Memoria.condition.Count - 1])[0];
                    }
                    else if (args[0] == "else" && args[1] == "if" && args.Length == 3)
                    {
                        if (!((bool[])Memoria.condition[Memoria.condition.Count - 1])[0] && new logicabooleana(args[2]).operar())
                        {
                            ((bool[])Memoria.condition[Memoria.condition.Count - 1])[1] = false;
                            ((bool[])Memoria.condition[Memoria.condition.Count - 1])[0] = true;
                        }
                        else
                        {
                            ((bool[])Memoria.condition[Memoria.condition.Count - 1])[1] = true;
                        }
                    }
                    else if (!((bool[])Memoria.condition[Memoria.condition.Count - 1])[1])
                        swc(args);
                }
                else
                {
                    if (Memoria.defining)
                    {
                        if (args[0] == "end" && args[1] == "def" && args.Length == 2)
                        {
                            Memoria.defining = false;
                        }
                        else
                        {
                            ((ArrayList)Memoria.defv[Memoria.defv.Count - 1]).Add(args);
                        }
                    }
                    else if (Memoria.python_ing)
                        if (args.Length == 2 && args[0] == "end" && args[1] == "python")
                            Comandos.end(args);
                        else
                            Memoria.PythonArgs += String.Join(" ", args) + "\n";
                    else if (Memoria.lua_ing)
                        if (args.Length == 2 && args[0] == "end" && args[1] == "lua")
                            Comandos.end(args);
                        else
                            Memoria.LuaArgs += String.Join(" ", args) + "\n";
                    else
                        {
                            if (!Memoria.repeaterstop && !Memoria.whilerstop)
                                swc(args);
                            else if (Memoria.whilerstop)
                            {
                                if (args[0] == "end" && args[1] == "while" && args.Length == 2)
                                    swc(args);
                            }
                            else if (Memoria.repeaterstop)
                            {
                                if (args[0] == "end" && args[1] == "repeat" && args.Length == 2)
                                    swc(args);
                            }
                            else
                            {
                                Comandos.error("Error de analisis");
                            }
                        }
                }
            }
        }

        public static void swc(string[] args)
        {
            switch (args[0])
            {
                case "write":
                    Comandos.write(args);
                    break;
                case "writef":
                    Comandos.writef(args);
                    break;
                case "writeli":
                    Comandos.writeli(args);
                    break;
                case "writeliln":
                    Comandos.writeliln(args);
                    break;
                case "writeln":
                    Comandos.writeln(args);
                    break;
                case "writefln":
                    Comandos.writefln(args);
                    break;
                case "exit":
                    Environment.Exit(-1);
                    break;
                case "beep":
                    Comandos.beep(args);
                    break;
                case "clear":
                    Console.Clear();
                    break;
                case "repeat":
                    Comandos.repeat(args);
                    break;
                case "math":
                    Comandos.math(args);
                    break;
                case "logic":
                    Comandos.logic(args);
                    break;
                case "if":
                    Comandos.if_else(args);
                    break;
                case "var":
                    Memoria.Add_V(true,args);
                    break;
                case "number":
                    Memoria.Add_V("number",args);
                    break;
                case "bool":
                    Memoria.Add_V("bool", args);
                    break;
                case "str":
                    Memoria.Add_V("str", args);
                    break;
                case "fun":
                    Memoria.Add_F(args);
                    break;
                case "overfile":
                    Comandos.writefile(args);
                    break;
                case "while":
                    Comandos.while_(args);
                    break;
                case "wait":
                    Thread.Sleep(int.Parse(args[1]));
                    break;
                case "title":
                    Console.Title = args[1];
                    break;
                case "snt":
                    Comandos.snt(args);
                    break;
                case "sa":
                    Comandos.saltos(args);
                    break;
                case "\\n":
                    Console.WriteLine("");
                    break;
                case "\\e":
                    Console.Write(" ");
                    break;
                case "inv":
                    Comandos.inv(args);
                    break;
                case "def":
                    Memoria.Add_D(args);
                    break;
                case "import":
                    Memoria.Import(args);
                    break;
                case "from":
                    Memoria.from_i(args);
                    break;
                case "async":
                    Comandos.async(args);
                    break;
                case "system":
                    Comandos.system(args);
                    break;
                case "#":
                    break;
                case "run":
                    Comandos.loadscript(args[1]);
                    break;
                case "end":
                    Comandos.end(args);
                    break;
                case "lua":
                    Comandos.lua(args);
                    break;
                case "python":
                    Comandos.python(args);
                    break;
                case "instance":
                    Comandos.instance(args);
                    break;
                case "output":
                    Comandos.output(args);
                    break;
                case "init":
                    if (Memoria.init)
                    {
                        ArrayList r = new ArrayList(args);
                        r.RemoveAt(0);
                        swc((string[])r.ToArray(typeof(string)));
                    }
                    break;
                default:
                    if (Memoria.varn.Contains(args[0]))
                        Memoria.Set_V(args);
                    else if (Memoria.defn.Contains(args[0]))
                        Memoria.Call_D(args);
                    else if (Memoria.dlln.Contains(args[0]))
                        Memoria.dll_m(args);
                    else if (Memoria.insn.Contains(args[0]))
                        Memoria.ins_m(args);
                    else if (Memoria.iton.Contains(args[0]))
                        Memoria.ito_m(args);
                    else
                        Comandos.error("Comando erroneo: " + args[0]);
                    break;
            }
        }

        public static void arguments(string[] fullargs)
        {
            for (int ar = 1; ar < fullargs.Length; ar++)
                Memoria.Add_V(true, new string[] { "var", "arg" + (ar-1), fullargs[ar] });
        }
    }
}
