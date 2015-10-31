using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//The most of this class is from Dylan Wilson camera class
//Dylan Wilson: http://www.dylanwilson.net/implementing-a-2d-camera-in-monogame
//Implemented by me: lookAt method. 

namespace _2DGame.Components
{
    class Camera
    {
        private readonly Viewport _viewport;

        public Camera(Viewport viewport)
        {
            _viewport = viewport;

            Rotation = 0;
            Zoom = 1f;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
        }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }

        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }


        public void lookAt(Vector2 position)
        {
            Position = position - new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);
        }
    }
}
