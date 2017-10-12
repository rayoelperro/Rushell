using org.mariuszgromada.math.mxparser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rushell
{
    class Comandos
    {
        public static void write(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(Sintaxis.Analizar(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void write(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Sintaxis.Analizar(ln));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void error(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(Sintaxis.Analizar(args[ar]));
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void error(string ln)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Sintaxis.Analizar(ln));
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

        public static void beep(string[] args)
        {
            if (args.Length == 3)
                Console.Beep(int.Parse(args[1]), int.Parse(args[2]));
            else
                Console.Beep();
        }

        public static void repeat(string[] args)
        {
            ArrayList l = new ArrayList(args);
            l.RemoveAt(0);
            l.RemoveAt(0);
            int n = int.Parse(Sintaxis.Analizar(args[1]));
            for (int re = 0; re < n; re++)
            {
                Environment.SetEnvironmentVariable("Repeat", re.ToString(), EnvironmentVariableTarget.Process);
                Program.Clasificar((string[])l.ToArray(typeof(string)));
            }
        }

        public static void while_(string[] args)
        {
            ArrayList l = new ArrayList(args);
            l.RemoveAt(0);
            l.RemoveAt(0);
            while(new logicabooleana(args[1]).operar())
                Program.Clasificar((string[])l.ToArray(typeof(string)));
        }

        public static void math(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int ar = 1; ar < args.Length; ar++)
                Console.WriteLine(new Expression(Sintaxis.Analizar(args[ar])).calculate());
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void if_else(string[] args)
        {
            if (new logicabooleana(args[1]).operar())
            {
                int els = Array.IndexOf(args, "else");
                string[] sub = new string[els - 2];
                for (int pl = 0; pl < sub.Length; pl++)
                {
                    sub[pl] = args[pl + 2];
                }
                Program.Clasificar(sub);
            }
            else
            {
                int els = Array.IndexOf(args, "else");
                string[] sub = new string[args.Length - (els + 1)];
                for (int pl = 0; pl < sub.Length; pl++)
                {
                    sub[pl] = args[pl + els + 1];
                }
                Program.Clasificar(sub);
            }
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
        }

        public static void CScompiler(string[] args)
        {
            if (File.Exists(args[1]))
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + Memoria.CScompilerpath + " " + args[1]);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                string result = proc.StandardOutput.ReadToEnd();
            }
            else
            {
                error("El archivo: " + args[1] + " no existe");
            }
        }

        public static string call(string[] args)
        {
            string command = string.Join(" ", args).Substring(5);
            if (string.Join(" ", args).Substring(5).Length > args[1].Length)
                command = args[1] + Sintaxis.Analizar(" \"" + string.Join(" ", args).Substring(6 + args[1].Length).Replace(" ", "\" \"") + "\"");
            if (args[1].EndsWith(".jar"))
                command = "java -jar " + command;
            else if (args[1].EndsWith(".py"))
                command = "python " + command;
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.RedirectStandardInput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process proc = new Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            for (int hrg = 1; hrg < args.Length; hrg++)
                proc.StandardInput.WriteLine(args[hrg]);

            string result = proc.StandardOutput.ReadToEnd();
            return result;
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
            Thread ambit = new Thread(() => Program.Clasificar(then));
            ambit.Start();
        }
    }
}
