using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DGame.Components
{
    class Player
    {
        private String id;
        private String password;
        private bool isLogged;


        public Vector2 Position;
        private Vector2 previousPosition; // Go to this position when collides to one barrier
        int shootDirection;



        public bool Active;

        //Projectile
        Texture2D projectileTexture;
        List<Projectile> projectiles;

        Character character;


        KeyboardState oldState;

        public Player()
        {
            isLogged = false;
        }

        public void Initialize(List<Texture2D> charactersTexture, GraphicsDevice graphicsDevice)
        {

            character = new Character();

            character.Initialize(charactersTexture, 1, graphicsDevice);
            Position = new Vector2(graphicsDevice.Viewport.TitleSafeArea.X + character.Width / 2,
                graphicsDevice.Viewport.TitleSafeArea.Y + character.Height / 2
                + graphicsDevice.Viewport.TitleSafeArea.Height / 2);

            previousPosition = Position;
            Active = true;
            shootDirection = 0;

            //Init projectile
            projectileTexture = charactersTexture.ElementAt(0);
            projectiles = new List<Projectile>();


            oldState = Keyboard.GetState();

        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, List<Barrier> barriers)
        {

            handleInput(gameTime, graphicsDevice);


            UpdateCollision(barriers);

            previousPosition = Position; //Update the previous position

            character.UpdatePosition(Position);
            character.Update(gameTime);
            
            for (int i=0; i<projectiles.Count; i++)
            {
                

                projectiles[i].Update(gameTime);
                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }
        }


        private void handleInput(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            var kbState = Keyboard.GetState();

            if (kbState.IsKeyDown(Keys.Left))
            {
                this.Position.X -= character.Speed;
                shootDirection = 1;
            }

            if (kbState.IsKeyDown(Keys.Right))
            {
                this.Position.X += character.Speed;
                shootDirection = 0;
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                this.Position.Y -= character.Speed;
                shootDirection = 2;

            }

            if (kbState.IsKeyDown(Keys.Down))
            {
                this.Position.Y += character.Speed;
                shootDirection = 3;

            }

            if (kbState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space))
            {
                Projectile proj = new Projectile();
                proj.Initialize(Position, shootDirection, projectileTexture);

                projectiles.Add(proj);
                
            }

           
            oldState = kbState;


            this.Position.X = MathHelper.Clamp(this.Position.X, character.Width / 2,
                graphicsDevice.Viewport.Width - (character.Width / 2));
            this.Position.Y = MathHelper.Clamp(this.Position.Y, character.Height / 2,
                graphicsDevice.Viewport.Height - (character.Height / 2));

        }


        private void UpdateCollision(List<Barrier> barriers)
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

        public void Draw(SpriteBatch spriteBatch)
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
