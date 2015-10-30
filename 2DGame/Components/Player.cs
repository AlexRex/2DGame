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
        public bool isLogged;

        public Animation PlayerAnimation;

        public Vector2 Position;
        private Vector2 previousPosition; // Go to this position when collides to one barrier

        public bool Active;

        float playerMoveSpeed;


        public int Width { get { return PlayerAnimation.FrameWidth; }  }
        public int Height { get { return PlayerAnimation.FrameHeight; } }


        Character character;


        public Player()
        {
            isLogged = false;
        }

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation; 

            Position = position;
            previousPosition = Position;
            Active = true;

            playerMoveSpeed = 8.0f;

            character = new Character(1);

        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, List<Barrier> barriers)
        {

            handleInput(gameTime, graphicsDevice);

            UpdateCollision(barriers);

            previousPosition = Position; //Update the previous position

            PlayerAnimation.Position = this.Position; 
            PlayerAnimation.Update(gameTime);

        }


        private void handleInput(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.Position.X -= playerMoveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.Position.X += playerMoveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                this.Position.Y -= playerMoveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                this.Position.Y += playerMoveSpeed;
            }

            this.Position.X = MathHelper.Clamp(this.Position.X, PlayerAnimation.FrameWidth/2,
                graphicsDevice.Viewport.Width - (this.Width/2));
            this.Position.Y = MathHelper.Clamp(this.Position.Y, PlayerAnimation.FrameHeight/2,
                graphicsDevice.Viewport.Height - (this.Height/2));

        }


        private void UpdateCollision(List<Barrier> barriers)
        {
            Rectangle playerBounds;
            Rectangle barrierBounds;

            playerBounds = new Rectangle((int)this.Position.X,
                (int)this.Position.Y,
                this.Width,
                this.Height);

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
            PlayerAnimation.Draw(spriteBatch);


        }

         
    }
}
