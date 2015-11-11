using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Components
{
    public class Projectile
    {

        float speed;
        public Vector2 Position;
        Animation sprite;

        int direction;

        public bool Active;

        public float Damage;

        TimeSpan spawnDuration;
        Stopwatch lifeTime;

        ConnectionTest con;

        public Projectile(){}

        public void Initialize(Vector2 initialPosition, int shootDirection, Texture2D texture, float Damage, ConnectionTest con)
        {
            speed = 25f;
            this.con = con;
            direction = shootDirection;

           // Console.WriteLine("new projectile");
            Position = initialPosition;

            sprite = new Animation();

            sprite.Initialize(texture, Position, 64, 64, 3, 30, Color.White, 1, true);

            this.Damage = Damage;
            Active = true;

            spawnDuration = TimeSpan.FromSeconds(.5f);

            lifeTime = new Stopwatch();
            lifeTime.Start();

        }

        public void Update(GameTime gameTime, List<Barrier> barriers, SuperPj pj)
        {
            switch (direction)
            {
                case 0:
                    sprite.Position.X += speed;
                    break;
                case 1:
                    sprite.Position.X += -speed;
                    break;
                case 2:
                    sprite.Position.Y += -speed;
                    break;
                case 3:
                    sprite.Position.Y += +speed;
                    break;
                default:
                    sprite.Position.X = 0;
                    sprite.Position.Y = 0;
                    break;
            }

           
           // Console.WriteLine(lifeTime.Elapsed.TotalSeconds);

            if (lifeTime.Elapsed.TotalSeconds >= spawnDuration.TotalSeconds)
            {
                Active = false;
                lifeTime.Stop();
            }

            UpdateCollisionBarriers(barriers);
            if(pj.Active)
                UpdateCollisionPj(pj);

            sprite.Update(gameTime);
        }


        public void UpdateCollisionPj(SuperPj pj)
        {
            Rectangle projectileBounds;
            Rectangle pjBounds;


            projectileBounds = new Rectangle((int)sprite.Position.X,
                 (int)sprite.Position.Y,
                 sprite.FrameWidth,
                 sprite.FrameHeight);

            pjBounds = new Rectangle((int)pj.Position.X,
                 (int)pj.Position.Y,
                 pj.character.Width,
                 pj.character.Height);

            if (projectileBounds.Intersects(pjBounds))
            {
                pj.Health -= Damage;
                Active = false;
            }
        }

        public void UpdateCollisionBarriers(List<Barrier> barriers)
        {
            Rectangle projectileBounds;
            Rectangle barrierBounds;

            projectileBounds = new Rectangle((int)sprite.Position.X,
                 (int)sprite.Position.Y,
                 sprite.FrameWidth,
                 sprite.FrameHeight);


            for (int i = 0; i < barriers.Count; i++)
            {
                if (barriers[i].Active)
                {
                    barrierBounds = new Rectangle((int)barriers[i].Position.X,
                    (int)barriers[i].Position.Y,
                    barriers[i].Width,
                    barriers[i].Height);

                    if (projectileBounds.Intersects(barrierBounds))
                    {
                        this.Active = false;
                        //Console.WriteLine("Projectile intersects barrier");
                        barriers[i].Health -= Damage;
                        //Console.WriteLine(barriers[i].Health);
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

    }
}
