using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DGame.Components
{
    [Serializable]
    class SuperPj
    {


        public Vector2 Position;
        protected Vector2 previousPosition; // Go to this position when collides to one barrier
        protected int shootDirection;


        public float Health { get; set; }

        public bool Active;

        //Projectile
        protected Texture2D projectileTexture;
        protected List<Projectile> projectiles;


        public Character character;

        protected ConnectionTest con;

        protected GraphicsDevice graphicsDevice;

        public SuperPj()
        {
        }


        // Initialize for Player
        public virtual void Initialize(List<Texture2D> charactersTexture, GraphicsDevice graphicsDevice, ConnectionTest con, Enemy enemy)
        {
            this.graphicsDevice = graphicsDevice;
            this.con = con;
        }

        // Initialize for Enemy
        public virtual void Initialize(List<Texture2D> charactersTexture, GraphicsDevice graphicsDevice, ConnectionTest con, Player player)
        {
            this.graphicsDevice = graphicsDevice;
            this.con = con;
        }


        public virtual void Update(GameTime gameTime, List<Barrier> barriers)
        {
            // Console.WriteLine(Health);
            if (Health <= 0)
            {
                Active = false;
            }


            UpdateBarrierCollision(barriers);

            previousPosition = Position; //Update the previous position

            character.UpdatePosition(Position);
            character.Update(gameTime);


            

           // con.Update();
        }


        private void UpdateBarrierCollision(List<Barrier> barriers)
        {
            Rectangle superPj;
            Rectangle barrierBounds;

            superPj = new Rectangle((int)this.Position.X,
                 (int)this.Position.Y,
                 character.Width,
                 character.Height);

            for (int i = 0; i < barriers.Count; i++)
            {

                barrierBounds = new Rectangle((int)barriers[i].Position.X,
                    (int)barriers[i].Position.Y,
                    barriers[i].Width,
                    barriers[i].Height);

                /* 
                  Intersection, doesn't look at which edge
                  if (playerBounds.Intersects(barrierBounds))
                  {
                      Console.WriteLine("Touching");
                      this.Position = previousPosition; //If is touching go back to the previous position 
                  }*/


                //looking for the edge touched and correct the position 
                float w = 0.5f * (superPj.Width + barrierBounds.Width);
                float h = 0.5f * (superPj.Height + barrierBounds.Height);

                float dx = superPj.Center.X - barrierBounds.Center.X;
                float dy = superPj.Center.Y - barrierBounds.Center.Y;


                if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
                {
                    float wy = w * dy;
                    float hx = h * dx;

                    if (wy > hx)
                    {
                        if (wy > -hx)
                        {
                            Console.WriteLine("collision at top");
                            this.Position.Y = barriers[i].Position.Y + barriers[i].Height;
                        }
                        else
                        {
                            Console.WriteLine("collision at right");
                            this.Position.X = barriers[i].Position.X - barriers[i].Width;

                        }
                    }
                    else
                    {
                        if (wy > -hx)
                        {
                            Console.WriteLine("collision on left");
                            this.Position.X = barriers[i].Position.X + barriers[i].Width;


                        }
                        else
                        {
                            Console.WriteLine("collision on bottom");
                            this.Position.Y = barriers[i].Position.Y - barriers[i].Height;

                        }
                    }

                }


            }


        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // PlayerAnimation.Draw(spriteBatch);
            character.Draw(spriteBatch);
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }
        }


    }
}
