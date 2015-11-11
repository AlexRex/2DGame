using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    [Serializable]
    public class Collectable
    {
        public Animation CollectableAnimation;

        public Vector2 Position;

        public bool Active;

        static Random r;

        public int type;

        public int ScoreGiven;
        public int HealthGiven;
        public int StrengthGiven;
        public int SpeedGiven;
        public int AmmunitionGiven;

        public float Health { get; set; }

        public int Width
        {
            get { return CollectableAnimation.FrameWidth; }
        }

        public int Height
        {
            get { return CollectableAnimation.FrameHeight; }
        }

        static Collectable()
        {
            r = new Random();
        }

        public void Initialize(Vector2 position, int type)
        {
            

            Position = position;
            Health = 100f;

            Active = true;


            ScoreGiven = 10;
            HealthGiven = 0;
            StrengthGiven = 0;
            AmmunitionGiven = 0;
            SpeedGiven = 0;

            switch (type)
            {
                case 0:
                    ScoreGiven = 10;
                    break;
                case 1:
                    HealthGiven = 50;
                    break;
                case 2:
                    StrengthGiven = 5;
                    break;
                case 3:
                    AmmunitionGiven = 10;
                    break;
                case 4:
                    SpeedGiven = 5;
                    break;
                case 5:
                    SpeedGiven = -5;
                    break;
            }

        }

        public void setAnimation(Animation animation)
        {
            CollectableAnimation = animation;
        }

        public void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                this.Active = false;
            }

            CollectableAnimation.Position = Position;
            CollectableAnimation.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CollectableAnimation.Draw(spriteBatch);
        }
    }
}
