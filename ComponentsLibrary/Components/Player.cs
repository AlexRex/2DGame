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
    class Player : SuperPj
    {


        Enemy enemy;

        KeyboardState oldState;


        public Player()
        {
        }

        public override void Initialize(List<Texture2D> charactersTexture, GraphicsDevice graphicsDevice, ConnectionTest con, Enemy enemy, int playerChar)
        {

            character = new Character();

            character.Initialize(charactersTexture, playerChar, graphicsDevice);
            Position = new Vector2(graphicsDevice.Viewport.TitleSafeArea.X + character.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + character.Height / 2
                + graphicsDevice.Viewport.TitleSafeArea.Height / 2);

            previousPosition = Position;
            Active = true;
            shootDirection = 0;
            Health = 100f;

            //Init projectile
            projectileTexture = charactersTexture.ElementAt(0);
            projectiles = new List<Projectile>();

            this.enemy = enemy;

            oldState = Keyboard.GetState();

            base.Initialize(charactersTexture, graphicsDevice, con, enemy, playerChar);

        }

        public override void Update(GameTime gameTime, List<Barrier> barriers, List<Collectable> collectables)
        {


            handleInput(gameTime);

            if(enemy.Active)
                UpdateEnemyCollision();

            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Update(gameTime, barriers, enemy);
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

            if (kbState.IsKeyDown(Keys.Left))
            {
                this.Position.X -= character.Speed;
                shootDirection = 1;
                con.Update();
            }

            if (kbState.IsKeyDown(Keys.Right))
            {
                this.Position.X += character.Speed;
                shootDirection = 0;
                con.Update();

            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                this.Position.Y -= character.Speed;
                shootDirection = 2;
                con.Update();


            }

            if (kbState.IsKeyDown(Keys.Down))
            {
                this.Position.Y += character.Speed;
                shootDirection = 3;
                con.Update();


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

                con.Shoot(shootDirection);
                
            }

           
            oldState = kbState;


          /*  this.Position.X = MathHelper.Clamp(this.Position.X, character.Width / 2,
                graphicsDevice.Viewport.Width - (character.Width / 2));
            this.Position.Y = MathHelper.Clamp(this.Position.Y, character.Height / 2,
                graphicsDevice.Viewport.Height - (character.Height / 2));*/

        }


        private void UpdateEnemyCollision()
        {
            Rectangle playerBounds;
            Rectangle enemyBounds;

            playerBounds = new Rectangle((int)this.Position.X,
                 (int)this.Position.Y,
                 character.Width,
                 character.Height);

            enemyBounds = new Rectangle((int)enemy.Position.X,
                (int)enemy.Position.Y,
                enemy.character.Width,
                enemy.character.Height);


            float w = 0.5f * (playerBounds.Width + enemyBounds.Width);
            float h = 0.5f * (playerBounds.Height + enemyBounds.Height);

            float dx = playerBounds.Center.X - enemyBounds.Center.X;
            float dy = playerBounds.Center.Y - enemyBounds.Center.Y;


            if (Math.Abs(dx) <= w && Math.Abs(dy) <= h)
            {
                float wy = w * dy;
                float hx = h * dx;

                if (wy > hx)
                {
                    if (wy > -hx)
                    {
                        //Console.WriteLine("PLAYER: collision at top");
                        this.Position.Y = enemy.Position.Y + enemy.character.Height;
                    }
                    else
                    {
                        //Console.WriteLine("PLAYER: collision at right");
                        this.Position.X = enemy.Position.X - enemy.character.Width;

                    }
                }
                else
                {
                    if (wy > -hx)
                    {
                        //Console.WriteLine("PLAYER: collision on left");
                        this.Position.X = enemy.Position.X + enemy.character.Width;


                    }
                    else
                    {
                        //Console.WriteLine("PLAYER: collision on bottom");
                        this.Position.Y = enemy.Position.Y - enemy.character.Height;

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
                float w = 1f * (playerBounds.Width + barrierBounds.Width);
                float h = 1f * (playerBounds.Height + barrierBounds.Height);

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
                            Console.WriteLine("plcollision at top");
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
