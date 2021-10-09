using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProceduralDungeon.Data.body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data.scenes
{
    public class Mainscreen : Scene
    {
        protected MainClass _mainclass;
        private Texture2D logo;
        private double timer;
        public bool isHidingLogo = false;
        float fading = 0;


        public Mainscreen(MainClass _main) : base(_main)
        {
            _mainclass = _main;
        }

        public override void Load(ContentManager _content)
        {
            logo = _content.Load<Texture2D>("logo2");


            base.Load(_content);
        }

        public override void Update(GameTime gameTime)
        {
            double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            if (!isHidingLogo)
            {
                fading += 0.005f;
                if (fading > 1)
                {
                    isHidingLogo = true;
                }
            }
            else
            {
                timer += deltaTime;

                if (timer > 4)
                {
                    fading -= 0.02f;
                    if (fading <= 0)
                    {
                        fading = 0;
                        _mainclass.LoadNewScene(GameState.sceneType.level1);
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spritebatch)
        {
            if (!isHidingLogo)
            {
                _spritebatch.Draw(logo,
                    new Rectangle((int)centered_screen.X - 400, (int)centered_screen.Y - 200, 800, 300),
                    Color.White * fading);

                _spritebatch.DrawString(_mainclass._mainfont, "réalisé par mecanoCodeur",
                    new Vector2((int)centered_screen.X - 500, (int)centered_screen.Y + 200),
                    Color.White * fading);
            }
            base.Draw(gameTime, _spritebatch);
        }
    }
}