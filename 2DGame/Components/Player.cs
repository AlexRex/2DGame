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

        public bool Active;

        float playerMoveSpeed;




        Character character;


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

            playerMoveSpeed = character.Speed;




        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, List<Barrier> barriers)
        {

            handleInput(gameTime, graphicsDevice);

            UpdateCollision(barriers);

            previousPosition = Position; //Update the previous position

            character.UpdatePosition(Position);


            character.Update(gameTime);

        }


        private void handleInput(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Position.X -= character.Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Position.X += character.Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.Position.Y -= character.Speed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.Position.Y += character.Speed;
            }

            this.Position.X = MathHelper.Clamp(this.Position.X, character.Width/2,
                graphicsDevice.Viewport.Width - (character.Width/2));
            this.Position.Y = MathHelper.Clamp(this.Position.Y, character.Height/2,
                graphicsDevice.Viewport.Height - (character.Height/2));
                
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

               if (playerBounds.Intersects(barrierBounds))
                {
                    Console.WriteLine("Touching");
                    this.Position = previousPosition; //If is touching go back to the previous position 
                }
                else
                    Console.WriteLine("Nope");
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // PlayerAnimation.Draw(spriteBatch);
            character.Draw(spriteBatch);

        }

         
    }
}
