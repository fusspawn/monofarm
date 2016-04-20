using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez.Console;
using NLua;
using NLua.Event;
using System.IO;
using NLua.Method;

namespace monotest.Util
{
    public class ConsoleFuncs
    {
        static TextWriter Writer;
        static Lua State;

        [Command("eval", "run lua string")]
        public static void RunLua(string String)
        {
            try
            {
                DebugConsole.instance.log("lua runstring: " + String);

                if (State == null)
                {
                    State = new Lua();
                    State.LoadCLRPackage();
                    State.RegisterFunction("print", typeof(ConsoleFuncs).GetMethod("Print"));
                    State["game"] = MainGame.Instance;
                    State.DoFile("./Content/scripts/init_globals.lua");
                }

                State.DoString(String);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                DebugConsole.instance.log("lua error: " + E.Message);
            }
        }

        [Command("lua-state", "dumps lua state")]
        public static void DumpGlobalState()
        {
            RunLua(@"for k,v in pairs(_G) do
                        print('Global key ' .. k ..  ' value ' .. tostring(v))
                    end");
        }
        

        public static void Print(string s)
        {
            if (Writer == null)
                Writer = File.CreateText("scriptlog.txt");

            Writer.WriteLine(s);
            Writer.Flush();
            DebugConsole.instance.log("lua> " + s);
        }
    }
}
