using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using ProceduralDungeon.Data.body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data
{
    public class Scene
    {


        protected MainClass _mainclass;
        protected Vector2 centered_screen;
        protected int screenW, screenH;
        protected float ratio;
        public Scene(MainClass _main)
        {
            _mainclass = _main;

        }



        public virtual void Unload()
        {

        }

        public virtual void Load(ContentManager _content)
        {
            screenW = Convert.ToInt32(_mainclass.previous_width);
            screenH = Convert.ToInt32(_mainclass.previous_height);
            ratio = screenH / screenW;

            centered_screen = new Vector2(_mainclass.previous_width / 2, _mainclass.previous_height / 2);


        }
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch _spritebatch)
        {





        }
    }
}