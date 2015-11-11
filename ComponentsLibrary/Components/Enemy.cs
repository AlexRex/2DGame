using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    [Serializable]
    public class Enemy : SuperPj
    {


        Player player;

        KeyboardState oldState;

        public Enemy()
        {
        }

        public override void Initialize(List<Texture2D> charactersTexture, GraphicsDevice graphicsDevice, ConnectionTest con, Player player, int playerChar)
        {

            character = new Character();

            character.Initialize(charactersTexture, playerChar, graphicsDevice);
            Position = new Vector2(graphicsDevice.Viewport.TitleSafeArea.X + character.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + character.Height / 2
                + graphicsDevice.Viewport.TitleSafeArea.Height / 2);

            previousPosition = Position;
            Active = true;
            shootDirection = 0;
            Health = character.Health;

            //Init projectile
            projectileTexture = charactersTexture.ElementAt(0);
            projectiles = new List<Projectile>();

            this.player = player;

            oldState = Keyboard.GetState();

            base.Initialize(charactersTexture, graphicsDevice, con, player, playerChar);

        }

        public override void Update(GameTime gameTime, List<Barrier> barriers, List<Collectable> collectables)
        {
 

          // handleInput(gameTime);


            if (player.Active)
                UpdatePlayerCollision();

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime, barriers, player);
                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }

            base.Update(gameTime, barriers, collectables);
        }


        private void handleInput(GameTime gameTime)
        {
            var kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.A))
            {
                this.Position.X -= character.Speed;
                shootDirection = 1;
            }

            if (kbState.IsKeyDown(Keys.D))
            {
                this.Position.X += character.Speed;
                shootDirection = 0;
            }

            if (kbState.IsKeyDown(Keys.W))
            {
                this.Position.Y -= character.Speed;
                shootDirection = 2;

            }

            if (kbState.IsKeyDown(Keys.S))
            {
                this.Position.Y += character.Speed;
                shootDirection = 3;

            }

            if (kbState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                if (character.Ammunition > 0)
                {
                    Projectile proj = new Projectile();
                    proj.Initialize(Position, shootDirection, projectileTexture, character.Strength);
                    projectiles.Add(proj);
                    character.Ammunition--;
                }
            }


            oldState = kbState;


            /*  this.Position.X = MathHelper.Clamp(this.Position.X, character.Width / 2,
                  graphicsDevice.Viewport.Width - (character.Width / 2));
              this.Position.Y = MathHelper.Clamp(this.Position.Y, character.Height / 2,
                  graphicsDevice.Viewport.Height - (character.Height / 2));*/

        }


        private void UpdatePlayerCollision()
        {
            Rectangle enemyBounds;
            Rectangle plBounds;

            enemyBounds = new Rectangle((int)this.Position.X,
                 (int)this.Position.Y,
                 character.Width,
                 character.Height);

            plBounds = new Rectangle((int)player.Position.X,
                (int)player.Position.Y,
                player.character.Width,
                player.character.Height);


            float w = 0.5f * (enemyBounds.Width + plBounds.Width);
            float h = 0.5f * (enemyBounds.Height + plBounds.Height);

            float dx = enemyBounds.Center.X - plBounds.Center.X;
            float dy = enemyBounds.Center.Y - plBounds.Center.Y;


            if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
            {
                float wy = w * dy;
                float hx = h * dx;

                if (wy > hx)
                {
                    if (wy > -hx)
                    {
                        //Console.WriteLine("ENEMY: collision at top");
                        this.Position.Y = player.Position.Y + player.character.Height;
                    }
                    else
                    {
                        //Console.WriteLine("ENEMY: collision at right");
                        this.Position.X = player.Position.X - player.character.Width;

                    }
                }
                else
                {
                    if (wy > -hx)
                    {
                        //Console.WriteLine("ENEMY: collision on left");
                        this.Position.X = player.Position.X + player.character.Width;


                    }
                    else
                    {
                        //Console.WriteLine("ENEMY: collision on bottom");
                        this.Position.Y = player.Position.Y - player.character.Height;

                    }
                }

            }


        }

        private void UpdateBarrierCollision(List<Barrier> barriers)
        {
            Rectangle playerBounds;
            Rectangle barrierBounds;

            playerBounds = new Rectangle((int)this.Position.X,
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
                float w = 0.5f * (playerBounds.Width + barrierBounds.Width);
                float h = 0.5f * (playerBounds.Height + barrierBounds.Height);

                float dx = playerBounds.Center.X - barrierBounds.Center.X;
                float dy = playerBounds.Center.Y - barrierBounds.Center.Y;


                if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
                {
                    float wy = w * dy;
                    float hx = h * dx;

                    if (wy > hx)
                    {
                        if (wy > -hx)
                        {
                            Console.WriteLine("enecollision at top");
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

        public override void Draw(SpriteBatch spriteBatch)
        {

            base.Draw(spriteBatch);
        }

       


    }
}
