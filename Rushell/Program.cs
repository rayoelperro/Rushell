using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace Rushell
{
    class Program
    {
        public static Version producto = new Version(1,5,5,78);
        public static void Main(string[] args)
        {
            Console.Title = "Rushell";
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length <= 0)
            {
                Console.WriteLine("Rushell Version: " + producto + "\nFor: " + Environment.OSVersion);
                Console.WriteLine("");
                Console.BackgroundColor = ConsoleColor.Black;
                ReadConsole();
            }
            else
            {
                if(File.Exists(args[0]))
                {
                    Console.WriteLine("Rushell Version: " + producto + "\nFor: " + Environment.OSVersion);
                    Console.WriteLine("");
                    Console.BackgroundColor = ConsoleColor.Black;
                    StreamReader str = new StreamReader(args[0]);
                    string linea = "";
                    while ((linea = str.ReadLine()) != null)
                        ConsoleAnalizer(linea);
                    ReadConsole();
                }
                else
                {
                    Console.WriteLine("Rushell Version: " + producto + "\nFor: " + Environment.OSVersion);
                    Console.WriteLine("");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Clasificar(args);
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
            ArrayList elementos = new ArrayList();
            string actual = null;
            bool comillas = false;
            for (int x = 0; x < comando.Length; x++)
            {
                if (comando[x] == ' ' && !comillas)
                {
                    if (actual != null)
                    {
                        elementos.Add(actual);
                        actual = null;
                    }
                }
                else if (comando[x] == '"')
                {
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
                Clasificar((string[])elementos.ToArray(typeof(string)));
        }

        public static void Clasificar(string[] args)
        {
            if(args.Length > 0) switch (args[0])
                {
                    case "write":
                        Comandos.write(args);
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
                        Memoria.Add_V(args);
                        break;
                    case "fun":
                        Memoria.Add_F(args);
                        break;
                    case "cscom":
                        Comandos.CScompiler(args);
                        break;
                    case "call":
                        Comandos.write(Comandos.call(args));
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
                    case "#":
                        break;
                    case "run":
                        Comandos.loadscript(args[1]);
                        break;
                    case "end":
                        Console.ReadKey();
                        Environment.Exit(-1);
                        break;
                    default:
                        if (Memoria.varn.Contains(args[0]))
                            Memoria.Set_V(args);
                        else if (Memoria.defn.Contains(args[0]))
                            foreach (string line in Memoria.Get_D(args[0]))
                                ConsoleAnalizer(line.Replace("@", "\""));
                        else if (Memoria.dlln.Contains(args[0]))
                            Memoria.dll_m(args);
                        else if (Memoria.insn.Contains(args[0]))
                            Memoria.ins_m(args);
                        else
                            Comandos.error("Comando erroneo: " + args[0]);
                        break;
                }
        }
    }
}
