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
    class WaitingPlayersMenu
    {

        Game1 game;

        Vector2 screenDimension;

        SpriteFont spFont;

        public void Initialize(Vector2 screenDimension, SpriteFont font, Game1 game)
        {
            this.screenDimension = screenDimension;
            spFont = font;

            this.game = game;
        }

       

        public void Draw(SpriteBatch spriteBatch)
        {
            int posX = 150;
            int posY = 380;
            spriteBatch.DrawString(spFont, "Waiting another player", new Vector2(posX, posY), Color.White);

        }
    }
}
