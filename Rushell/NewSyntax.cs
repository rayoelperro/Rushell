using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rushell
{
    class NewSyntax
    {
        private string line;

        public string Result
        {
            get
            {
                string[] tokens = Tokens(line);
                ArrayList levels = Level(tokens);
                return Evaluate(levels);
            }
        }

        private char[] mt = { '(', ')' };

        private char stc = '\'';

        public NewSyntax(string a_)
        {
            line = a_;
        }

        private string[] Tokens(string line)
        {
            List<string> tokens = new List<string>();
            bool o = false;
            int t = 0;
            string n = "";
            foreach(char c in line)
            {
                t++;
                if (o)
                {
                    if(c == stc)
                    {
                        tokens.Add("'"+n+"'");
                        n = "";
                        o = false;
                    }
                    else
                    {
                        n += c;
                    }
                }
                else
                {
                    if(c == stc)
                    {
                        o = true;
                        if(n.Length > 0)
                        {
                            tokens.Add(n);
                            n = "";
                        }
                    }
                    else if (c == ' ')
                    {
                        if(n.Length > 0)
                        {
                            tokens.Add(n);
                            n = "";
                        }
                    }
                    else if (mt.Contains(c))
                    {
                        if(n.Length > 0)
                        {
                            tokens.Add(n);
                            n = "";
                        }
                        tokens.Add(c.ToString());
                    }
                    else
                    {
                        n += c;
                        if (t == line.Length)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
                        {
                            tokens.Add(n);
                            n = "";
                        }
                    }
                }
            }
            return tokens.ToArray();
        }

        private ArrayList Level(string[] line)
        {
            ArrayList level = new ArrayList();
            int i = 0;
            bool ins = false;
            List<string> ts = new List<string>();
            foreach(string t in line)
            {
                if(t == "(")
                {
                    i++;
                    if(i == 1)
                    {
                        ins = true;
                    }
                    else
                    {
                        ts.Add(t);
                    }
                }
                else if(t == ")")
                {
                    i--;
                    if(i == 0)
                    {
                        level.Add(Level(ts.ToArray()));
                        ins = false;
                        ts.Clear();
                    }
                    else
                    {
                        ts.Add(t);
                    }
                }
                else if(ins)
                {
                    ts.Add(t);
                }
                else
                {
                    level.Add(t);
                }
            }
            return level;
        }

        private string Evaluate(ArrayList root)
        {
            string res = "";
            for(int i = 0; i < root.Count; i++)
            {
                if(root[i] is ArrayList)
                {
                    root[i] = Evaluate((ArrayList)root[i]);
                }
            }
            string[] news = (string[])root.ToArray(typeof(string));
            for (int i = 0; i < news.Length; i++)
            {
                if (i == 0) continue;

                if (news[i].StartsWith("'") && news[i].EndsWith("'"))
                {
                    news[i] = news[i].Substring(1).TrimEnd('\'');
                    continue;
                }

                switch (news[i])
                {
                    case "!always":
                        news[i] = int.MinValue.ToString();
                        break;
                    case "!never":
                        news[i] = int.MaxValue.ToString();
                        break;
                    case "!repeatvalue":
                        news[i] = Memory.repeatvalue.ToString();
                        break;
                    case "!here":
                        news[i] = System.IO.Directory.GetCurrentDirectory();
                        break;
                    case "!there":
                        news[i] = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                        break;
                    case "\\n":
                        news[i] = "\n";
                        break;
                    case "\\t":
                        news[i] = "\t";
                        break;
                    default:
                        if (news[i].StartsWith("!") && Memory.varn.Contains(news[i].Substring(1)))
                            news[i] = (string)Memory.varv[Memory.varn.IndexOf(news[i])];
                        break;
                }
            }
            res = SelectMethod(news);
            return res;
        }

        private string SelectMethod(string[] a)
        {
            string name = a[0];
            string[] args = a.Where((val, idx) => idx != 0).ToArray();
            int n = args.Length;
            switch (name)
            {
                case "read":
                    len("read", n, 1);
                    return SyntaxCommands.Readln(args[0]);
                case "math":
                    len("math", n, 1);
                    return SyntaxCommands.Math(args[0]);
                case "logic":
                    len("logic", n, 1);
                    return SyntaxCommands.Logic(args[0]);
                case "invoke":
                    len("invoke", n, 1);
                    return SyntaxCommands.Invoke(args[0]);
                case "overfile":
                    len("overfile", n, 1);
                    return SyntaxCommands.Overfile(args[0]);
                case "rand":
                    len("rand", n, 0, 1, 2);
                    return SyntaxCommands.Rand(a);
                case "join":
                    return SyntaxCommands.Join(args);
                case "idx":
                    len("idx", n, 2);
                    return SyntaxCommands.Index(args[0],args[1]);
                case "lit":
                    len("lit", n, 1);
                    return args[0];
                default:
                    if (Memory.defn.Contains(name))
                    {

                    }
                    else if (Memory.funn.Contains(name))
                    {

                    }
                    else if (Memory.dlln.Contains(name))
                    {

                    }
                    else if (Memory.insn.Contains(name))
                    {

                    }
                    else if (Memory.iton.Contains(name))
                    {

                    }
                    break;
            }
            Commands.error("Any method found with the name: " + name);
            return "Wrong name: " + name;
        }

        private bool len(string function, int args, params int[] req)
        {
            if (req.Contains(args)) return true;
            Commands.error("Wrong number of arguments for function: " + function + " " + args.ToString() + "/(" + string.Join(",",req) + ")");
            return false;
        }
    }
}
