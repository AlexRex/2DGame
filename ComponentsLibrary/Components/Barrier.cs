﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    public class Barrier
    {
        public Animation BarrierAnimation;

        public Vector2 Position;

        public bool Active;

        public bool Destructible;

        public bool Collisionable;

        public float Health { get; set; }

        public int Width
        {
            get { return BarrierAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return BarrierAnimation.FrameHeight; }
        }

        
        public void Initialize(Animation animation, Vector2 position, bool destructible, bool collisionable)
        {
            BarrierAnimation = animation;

            Position = position;
            Health = 100f;

            Active = true;

            Destructible = destructible;
            Collisionable = collisionable;


        }

        public void Update(GameTime gameTime)
        {
            if (Health <= 0 && Destructible)
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
