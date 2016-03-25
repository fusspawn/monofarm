using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;

namespace monotest.Components.Movement
{
    /// <summary>
    /// adds basic movement with the arrow keys and includes collision detection and resolution. A debug line is displayed when a collision
    /// occurs in the direction of the collision normal.
    /// </summary>
    public class SimpleMover : Component, IUpdatable
    {
        float _speed = 1f;
        Mover _mover;


        public override void onAddedToEntity()
        {
            _mover = new Mover();
            entity.addComponent(_mover);
        }


        void IUpdatable.update()
        {
            var moveDir = Vector2.Zero;

            if (Input.isKeyDown(Keys.Left))
            {
                moveDir.X = -1f;
                entity.transform.rotationDegrees = -180f;
            }
            else if (Input.isKeyDown(Keys.Right))
            {
                moveDir.X = 1f;
                entity.transform.rotationDegrees = 0f;
            }

            if (Input.isKeyDown(Keys.Up))
            {
                moveDir.Y = -1f;
                entity.transform.rotationDegrees = -90f;
            }
            else if (Input.isKeyDown(Keys.Down))
            {
                moveDir.Y = 1f;
                entity.transform.rotationDegrees = 90f;
            }


            if (moveDir != Vector2.Zero)
            {
                var movement = moveDir * _speed;

                CollisionResult res;
                if (_mover.move(movement, out res))
                    Debug.drawLine(entity.transform.position, entity.transform.position + res.normal * 100, Color.Black, 0.3f);

                entity.scene.camera.position = entity.transform.position;
            }
        }
    }
}
