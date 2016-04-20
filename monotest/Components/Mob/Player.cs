using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.AI;
using monotest.Components.Movement;
using monotest.Components.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Console;
using Nez.Sprites;

namespace monotest.Components.Mob
{
    public class PlayerMaker
    {
        public static ScriptableEntity CreatePlayerEntity(Scene Scene)
        {
            ScriptableEntity Ent = new ScriptableEntity("Player");
            Scene.addEntity(Ent);
            (Ent.addComponent<CharacterRender>() as CharacterRender).Extras.Add(47);
            Ent.addComponent<SimpleMover>();
            Ent.addComponent<MouseWheelZoomer>();
            Ent.addComponent<PathFollower>();
            Ent.addComponent<MobData>();
            Ent.addComponent(new Inventory(10));
            Ent.addComponent(new FollowCamera(Ent));

            Ent.colliders.add(new BoxCollider());
            

            AIManager.AIList.Add(Ent);
            Console.WriteLine("player created");
            return Ent;
        }

        public static ScriptableEntity CreateDefaultMobEntity(string Name, Scene scene)
        {
            ScriptableEntity Ent = new ScriptableEntity(Name);
            scene.addEntity(Ent);


            Ent.addComponent<CharacterRender>();
            Ent.addComponent<PathFollower>();
            Ent.addComponent<MobData>();
            Ent.addComponent(new Inventory(10));
            Ent.addComponent(new ScriptedComponent("ai/ai_base.lua"));


            AIManager.AIList.Add(Ent);
            return Ent;
        }
    }
}
