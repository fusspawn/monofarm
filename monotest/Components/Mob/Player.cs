using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static Entity CreatePlayerEntity(Scene Scene)
        {
            Entity Ent = new Entity("Player");
            Scene.addEntity(Ent);

            Ent.transform.scale=new Vector2(.5f, .5f);
            Sprite S = new Sprite(Scene.contentManager.Load<Texture2D>("manBlue_stand"));

            S.renderLayer = 1;
            Ent.addComponent(S);
            Ent.addComponent<SimpleMover>();
            Ent.addComponent<MouseWheelZoomer>();
            Ent.addComponent<PathFollower>();
            Ent.addComponent(new FollowCamera(Ent));

            Ent.colliders.add(new BoxCollider());
            

            
            Console.WriteLine("player created");
            return Ent;
        }
    }
}
