using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data.scenes
{
    public class Main_menu : Scene
    {
        private bool loadingIsOK = false;
        Texture2D background;
        public Main_menu(MainClass _main) : base(_main)
        {
        }
        public override void Unload()
        {
            base.Unload();
        }

        public override void Load(ContentManager _content)
        {
            background = _content.Load<Texture2D>("main_background");
            loadingIsOK = true;
            base.Load(_content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spritebatch)
        {
            if (loadingIsOK)
            {
                // _spritebatch.Draw(background, new Rectangle(0, 0, 400, 400), Color.White);
                _spritebatch.Draw(background, new Rectangle(0, 0, (int)_mainclass.previous_width, (int)_mainclass.previous_height), Color.White);
            }

            base.Draw(gameTime, _spritebatch);
        }

    }
}