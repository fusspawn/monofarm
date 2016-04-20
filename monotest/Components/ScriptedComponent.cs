using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using NLua;
using Nez.Console;
using monotest.Util;
using monotest.Components.World;
using System.Threading;

namespace monotest.Components
{
    class ScriptedComponent : Nez.Component, IUpdatable
    {
        
        public long ThisScriptID;
        public LuaFunction OnUpdateFunc = null;
        public Lua OurState;

        public ScriptedComponent(string Path)
        {
            ScriptPath = Path;
            ThisScriptID = LuaManager.Instance.ScriptID += 1;
            OurState = LuaManager.Instance.GetState();
        }

        public readonly string ScriptPath;



        public override void onAddedToEntity()
        {
            if (!File.Exists("./Content/scripts/" + ScriptPath))
            {
                return;
            }

            SetState();
            OurState.DoFile("./Content/scripts/" + ScriptPath);
            base.onAddedToEntity();
        }

        public override void onRemovedFromEntity()
        {
            LuaManager.Instance.FreeState(OurState);
            base.onRemovedFromEntity();
        }


        public void update()
        {
                if (OnUpdateFunc != null)
                {
                    OnUpdateFunc.Call();
                }            
        }

        public void RegisterScriptEventHandler(LuaTable Handler)
        {
            if(Handler["OnInit"] != null)
            {
                (Handler["OnInit"] as LuaFunction).Call();
            }

            if(Handler["OnUpdate"] != null)
            {
                OnUpdateFunc = Handler["OnUpdate"] as LuaFunction;                
            }
        }


        public void SetState()
        {           
            OurState["entity"] = entity;
            OurState["component"] = this;
            OurState["scriptID"] = ThisScriptID;
        }
    }
}
