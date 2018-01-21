using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

public class CSC
{
    private static string CScompilerpath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";

    public static string exe(string name){
        return compile(name,"/t:exe");
    }

    public static string winexe(string name){
        return compile(name,"/t:winexe");
    }
    
    public static string lib(string name){
        return compile(name,"/t:library");
    }

    private static string compile(string name, string mode){
        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + CScompilerpath + " " + mode + " " + name);
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();

        return proc.StandardOutput.ReadToEnd();
    }
}

public class General
{
    public static string call(params string[] args){
        string command = "";
        if (args[0].EndsWith(".jar"))
            command = "java -jar " + args[0];
        else if (args[0].EndsWith(".py"))
            command = "python " + args[0];
        else if (args[0].EndsWith(".go"))
            command = "go run " + args[0];
        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command + string.Join(" ",args).Substring(args[0].Length));
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardInput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        return proc.StandardOutput.ReadToEnd();
    }

    public static string system(params string[] args){
        ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + string.Join(" ",args));
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardInput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        procStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        Process proc = new Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        return proc.StandardOutput.ReadToEnd();
    }
}

public class ConsoleCaller
{
    protected Process c;
    protected string r = null;

    public void Start(string name){
        Config(name,new string[]{});
    }

    public void Start(string name, params string[] args){
        Config(name,args);
    }

    private void Config(string name, string[] args){
        ProcessStartInfo sti = new ProcessStartInfo(name,string.Join(" ",args));
        sti.UseShellExecute = false;
        sti.RedirectStandardInput = true;
        sti.RedirectStandardOutput = true;
        sti.RedirectStandardError = true;
        sti.WindowStyle = ProcessWindowStyle.Hidden;
        c = new Process();
        c.StartInfo = sti;
        c.Start();
    }

    public virtual void Write(string pline){
        c.StandardInput.WriteLine(pline);
    }

    public virtual string Read(){
        if(r != null)
            return r;
        Console.WriteLine("Hace falta finalizar el proceso");
        return "null";
    }

    public virtual string Finish(){
        if(r == null)
        {
            r = c.StandardOutput.ReadToEnd();
            c.WaitForExit();
        }
		return r;
    }
}

public class PythonCaller : ConsoleCaller
{
    public PythonCaller(){
        Start("Python.exe",@"libs\Run.py");
    }

    public override string Finish(){
        if(r == null)
        {
            Write("exit()");
            r = c.StandardOutput.ReadToEnd();
            c.WaitForExit();
        }
		return r;
    }
}