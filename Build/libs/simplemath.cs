using System;

class math
{
    public static void Main(string[] args)
    {
        switch (args[1])
        {
            case "+":
                Console.WriteLine(int.Parse(args[0]) + int.Parse(args[2]));
                break;
            case "-":
                Console.WriteLine(int.Parse(args[0]) - int.Parse(args[2]));
                break;
            case "*":
                Console.WriteLine(int.Parse(args[0]) * int.Parse(args[2]));
                break;
            case "/":
                Console.WriteLine(int.Parse(args[0]) / int.Parse(args[2]));
                break;
            default:
                Console.WriteLine("Error de simbología con: " + args[1]);
                break;
        }
    }
}