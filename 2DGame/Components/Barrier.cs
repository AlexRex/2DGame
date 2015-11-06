using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2DGame.Components
{
    class Barrier
    {
        public Animation BarrierAnimation;

        public Vector2 Position;

        public bool Active;

        public float Health { get; set; }

        public int Width
        {
            get { return BarrierAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return BarrierAnimation.FrameHeight; }
        }

        
        public void Initialize(Animation animation, Vector2 position)
        {
            BarrierAnimation = animation;

            Position = position;
            Health = 100f;

            Active = true;


        }

        public void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                this.Active = false;
            }

            BarrierAnimation.Position = Position;
            BarrierAnimation.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BarrierAnimation.Draw(spriteBatch);
        }
    }
}
