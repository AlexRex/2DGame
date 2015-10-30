using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DGame.Components
{
    class Character : Player
    {
        public int Strength { get; set; }

        public int Ammunition { get; set; }

        public float Speed { get; set; }

        public int Health { get; set; }



        public Character(int type)
        {
            switch (type) {
                case 1:
                    Strength = 100;
                    Ammunition = 100;
                    Speed = 1.0f;
                    break;

                default:
                    Strength = 100;
                    Ammunition = 100;
                    Speed = 1.0f;
                break;

            }

            Health = 100;
            
        }

        public void Initialize(Texture2D texture)
        {

        }

        public void Update(GameTime gameTime)
        {
            
        }

       

    }

}
