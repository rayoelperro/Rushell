using System;

namespace Rushell
{
    class SyntaxCommands
    {
        public static string Readln(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static string Math(string equation)
        {
            return Commands.exp(equation);
        }

        public static string Logic(string expression)
        {
            return new BooleanLogic("(" + expression + ")").operar().ToString();
        }

        public static string Invoke(string wht)
        {
            Program.ConsoleAnalizer(wht.Replace("@", "\""));
            return "";
        }

        public static string Overfile(string file)
        {
            return Commands.readfile(file);
        }

        public static string Rand(string[] args)
        {
            return Commands.rand(args).ToString();
        }

        public static string Join(string[] args)
        {
            string tr = "";
            foreach (string a in args) tr += a;
            return tr;
        }

        public static string Index(string var, string idx)
        {
            if (Memory.varn.Contains(var))
            {
                string cnt = Memory.varv[Memory.varn.IndexOf(var)].ToString();
                if(cnt == "System.String[]")
                {
                    return ((string[])Memory.varv[Memory.varn.IndexOf(var)])[int.Parse(idx)];
                }
                else
                {
                    Commands.error("Variable with name: " + var + " wasn't an array");
                }
            }
            else
                Commands.error("Any variable found with name: " + var);
            return "Wrong name: " + var;
        }
    }
}
