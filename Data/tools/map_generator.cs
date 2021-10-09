using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data.tools
{
    public struct tile_map
    {
        public Rectangle rect_pos;
        public Texture2D sprite;
    }

    public struct room
    {
        public int tileW, tileH, totalW, totalH, total_tiles;
        public tile_map[] my_map;
    }

    public class map_generator
    {

        public map_generator()
        {

        }


    }
}