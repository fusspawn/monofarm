using monotest.Components.World;
using Nez;
using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace monotest.Util
{
    public class LuaManager
    {
        public long ScriptID = 0;
        private static LuaManager _instance;
        private static Dictionary<long, LuaTable> PersistantState = new Dictionary<long, LuaTable>();


        public static LuaManager Instance { 
                get { if (_instance == null) _instance = new LuaManager(); return _instance; }
        }

        

        public LuaManager()
        {
            Pool<Lua>.warmCache(40);
        }


        public Lua GetState()
        {
            Lua State = Pool<Lua>.obtain();            
            State.LoadCLRPackage();
            State["game"] = MainGame.Instance;
            State["ChunkManager"] = ChunkManager.Instance;
            State["itemdefs"] = Items.ItemDefinitions.Instance;
            State.DoFile("./Content/scripts/init_globals.lua");
            State.DoFile("./Content/scripts/items.lua");

            return State;
        }

        public void FreeState(Lua S)
        {
            Pool<Lua>.free(S);
        }
        
    }
}
