using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DGame.Menus
{
    class CharacterSelection
    {
        int active;

        Game1 game;

        Vector2 screenDimension;

        SpriteFont spFont;

        KeyboardState oldState;

        public void Initialize(Vector2 screenDimension, SpriteFont font, Game1 game)
        {
            this.screenDimension = screenDimension;
            spFont = font;

            this.game = game;

            active = 0;

            oldState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            handleInput(gameTime);

        }

        private void handleInput(GameTime gameTime)
        {
            var kbState = Keyboard.GetState();


            if (kbState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                active--;
            }

            else if (kbState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                active++;
            }

            else if (kbState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                game.GameState = Game1.STATE.InGame;
            }

            if (active > 1)
            {
                active = 1;
            }
            else if (active < 0)
            {
                active = 0;
            }

            oldState = kbState;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active == 0)
            {
                spriteBatch.DrawString(spFont, "Char 1", screenDimension / 2, Color.Black);
                spriteBatch.DrawString(spFont, "Char 2", new Vector2(screenDimension.X / 2, screenDimension.Y / 2 + 60), Color.White);
            }
            else if (active == 1)
            {
                spriteBatch.DrawString(spFont, "Char 1", screenDimension / 2, Color.White);
                spriteBatch.DrawString(spFont, "Char 2", new Vector2(screenDimension.X / 2, screenDimension.Y / 2 + 60), Color.Black);
            }


        }
    }
}
