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



   
    class Level1 : level_template
    {

        protected Texture2D town;
        protected Rectangle rect_position_background;
        protected Rectangle rect_position_background2;
        protected Tile[] map;
        protected int[] current_map;
        protected int[] default_map;
        protected int mapW, mapH, mapSize;
        protected List<Rectangle> list_collideBox, list_temp;

        protected Vector2 camera;
        protected Rectangle rect_camera;

        protected List<Rectangle> list_gate, list_spawn;
        protected List<Rectangle> list_occlusion_ambiante;
        protected Texture2D sprite_occlusion_ambiante;

        protected Point target1, target2, target3, target4, horizontal_target1, horizontal_target2, horizontal_target3, horizontal_target4, vertical_target1, vertical_target2, vertical_target3, vertical_target4;

        protected int delta1,delta2;
        protected Rectangle colision_playerposition;
        protected float tileSize = 0;
        protected Rectangle colision_coin_dectection;
        public Level1(MainClass _main) : base(_main)
        {
        }



        public override void Load(ContentManager _content)
        {
            base.Load(_content);

            sprite_occlusion_ambiante = _content.Load<Texture2D>("occlusion_ambiante");
            list_occlusion_ambiante = new List<Rectangle>();



             colision_coin_dectection= new Rectangle(
      my_player.rect_position.X,
      my_player.rect_position.Y,
      my_player.rect_position.Width,
      my_player.rect_position.Height);




            //-----------------------------------
            // initialiser les données pour les colisions
            //-----------------------------------



            colision_playerposition = new Rectangle(
           my_player.rect_position.X ,
           my_player.rect_position.Y + my_player.rect_position.Width / 4, my_player.rect_position.Width / 4, my_player.rect_position.Height / 4);

             target1 = new Point(colision_playerposition.X, colision_playerposition.Y);
             target2 = new Point(colision_playerposition.X + colision_playerposition.Width, colision_playerposition.Y);
             target3 = new Point(colision_playerposition.X + colision_playerposition.Width, colision_playerposition.Y + colision_playerposition.Height);
             target4 = new Point(colision_playerposition.X, colision_playerposition.Y + colision_playerposition.Height);

             delta1 = 100;
             //delta2 = my_player.rect_position.Width / 6;
            delta2 = colision_playerposition.Width / 6;

            vertical_target1 = new Point(colision_playerposition.X + delta2, colision_playerposition.Y - delta1);
             vertical_target2 = new Point(colision_playerposition.X + colision_playerposition.Width - delta2, colision_playerposition.Y - delta1);
             vertical_target3 = new Point(colision_playerposition.X + colision_playerposition.Width - delta2, colision_playerposition.Y + colision_playerposition.Height + delta1);
             vertical_target4 = new Point(colision_playerposition.X + delta2, colision_playerposition.Y + colision_playerposition.Height + delta1);

             horizontal_target1 = new Point(colision_playerposition.X - delta1, colision_playerposition.Y + delta2);
             horizontal_target2 = new Point(colision_playerposition.X + colision_playerposition.Width + delta1, colision_playerposition.Y + delta2);
             horizontal_target3 = new Point(colision_playerposition.X + colision_playerposition.Width + delta1, colision_playerposition.Y + colision_playerposition.Height - delta2);
             horizontal_target4 = new Point(colision_playerposition.X - delta1, colision_playerposition.Y + colision_playerposition.Height - delta2);
           

          /*  target1 = new Point(my_player.rect_position.X, my_player.rect_position.Y);
            target2 = new Point(my_player.rect_position.X + my_player.rect_position.Width, my_player.rect_position.Y);
            target3 = new Point(my_player.rect_position.X + my_player.rect_position.Width, my_player.rect_position.Y + my_player.rect_position.Height);
            target4 = new Point(my_player.rect_position.X, my_player.rect_position.Y + my_player.rect_position.Height);

            delta1 = 100;
            //delta2 = my_player.rect_position.Width / 6;
            delta2 = colision_playerposition.Width / 6;

            vertical_target1 = new Point(my_player.rect_position.X + delta2, my_player.rect_position.Y - delta1);
            vertical_target2 = new Point(my_player.rect_position.X + my_player.rect_position.Width - delta2, my_player.rect_position.Y - delta1);
            vertical_target3 = new Point(my_player.rect_position.X + my_player.rect_position.Width - delta2, my_player.rect_position.Y + my_player.rect_position.Height + delta1);
            vertical_target4 = new Point(my_player.rect_position.X + delta2, my_player.rect_position.Y + my_player.rect_position.Height + delta1);

            horizontal_target1 = new Point(my_player.rect_position.X - delta1, my_player.rect_position.Y + delta2);
            horizontal_target2 = new Point(my_player.rect_position.X + my_player.rect_position.Width + delta1, my_player.rect_position.Y + delta2);
            horizontal_target3 = new Point(my_player.rect_position.X + my_player.rect_position.Width + delta1, my_player.rect_position.Y + my_player.rect_position.Height - delta2);
            horizontal_target4 = new Point(my_player.rect_position.X - delta1, my_player.rect_position.Y + my_player.rect_position.Height - delta2);

            */




            list_gate = new List<Rectangle>();
            list_spawn = new List<Rectangle>();



            default_map = new int[418] {
              1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
              1,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,1,
              1,0,1,1,0,1,0,1,0,1,0,1,0,1,0,1,1,0,1,
              1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,0,1,
              1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1,
              1,0,1,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,1,
              1,0,1,0,1,1,0,1,1,1,1,1,0,1,1,0,1,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,1,1,0,1,0,1,1,0,1,1,0,1,0,1,1,0,1,
              1,0,1,1,0,1,0,1,0,0,0,1,0,1,0,1,1,0,1,
              8,7,0,0,0,1,0,1,1,1,1,1,0,1,0,0,0,7,8,
              1,0,1,1,0,1,0,0,0,0,0,0,0,1,0,1,1,0,1,
              1,0,0,1,0,1,0,0,0,0,0,0,0,1,0,1,0,0,1,
              1,1,0,1,0,1,0,1,1,1,1,1,0,1,0,1,0,1,1,
              1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,
              1,0,1,1,0,1,1,1,0,1,0,1,1,1,0,1,1,0,1,
              1,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,1,
              1,0,1,0,1,1,0,1,0,1,0,1,0,1,1,0,1,0,1,
              1,0,1,0,0,0,0,1,0,1,0,1,0,0,0,0,1,0,1,
              1,0,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1
            
            
             };




            list_collideBox = new List<Rectangle>();
            list_temp = new List<Rectangle>();
            int limit_left = joy_L.rect_joy_back.X + joy_L.rect_joy_back.Width;

            mapW = 19;
            mapH = 22;
            mapSize = mapW * mapH;
            map = new Tile[mapSize];

            current_map = new int[418]
          {
              1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,
              1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1



          };

            current_map = default_map;

            int k, z;
            k = 0;
            z = -1;



            tileSize = my_player.rect_position.Width +40;

            for (int i=0; i<mapSize;i++)
            {

                if(i%mapW==0)
                {
                    k = 0;
                    z++;
                }
                map[i] = new Tile();

                map[i].rect_position = new Rectangle((int)(tileSize * k) + limit_left, (int)(tileSize * z), (int)(tileSize), (int)(tileSize));
                
               
                map[i].text_sprite = _content.Load<Texture2D>("ground");

                if (current_map[i] == 1)
                {
                    if (i - mapW >= 0)
                    {
                        if (current_map[i - mapW] == 1)
                        {
                            map[i - mapW].rect_frame = new Rectangle(48, 0, 48, 48);

                        }
                    }

                    map[i].rect_frame = new Rectangle(0, 0, 48, 48);

                    list_collideBox.Add(new Rectangle((int)(tileSize * k) + limit_left, (int)(tileSize * z), (int)(tileSize), (int)(tileSize)));

                }
                else if (current_map[i] == 0)
                {
                    map[i].rect_frame = new Rectangle(48 * 2, 0, 48, 48);

                    coin newC = new coin();
                    newC.text_sprite = coinsprite;
                    newC.rect_position = new Rectangle((int)(tileSize * k) + limit_left + (int)(tileSize) / 6, (int)(tileSize * z) + (int)(tileSize) / 6, (int)(tileSize), (int)(tileSize));
                    newC.rect_frame = new Rectangle(0, 0, 48, 48);
                    list_coins.Add(newC);

                }
                else if (current_map[i] == 8)
                {
                    map[i].rect_frame = new Rectangle(48 * 2, 0, 48, 48);

                 /*   coin newC = new coin();
                    newC.text_sprite = coinsprite;
                    newC.rect_position = new Rectangle((int)(tileSize * k) + limit_left + (int)(tileSize) / 6, (int)(tileSize * z) + (int)(tileSize) / 6, (int)(tileSize), (int)(tileSize));
                    newC.rect_frame = new Rectangle(0, 0, 48, 48);
                    list_coins.Add(newC);*/

                    list_gate.Add(map[i].rect_position);
                }
                else if (current_map[i] == 7)
                {
                    map[i].rect_frame = new Rectangle(48 * 2, 0, 48, 48);

                    coin newC = new coin();
                    newC.text_sprite = coinsprite;
                    newC.rect_position = new Rectangle((int)(tileSize * k) + limit_left + (int)(tileSize) / 6, (int)(tileSize * z) + (int)(tileSize) / 6, (int)(tileSize), (int)(tileSize));
                    newC.rect_frame = new Rectangle(0, 0, 48, 48);
                    list_coins.Add(newC);

                    list_spawn.Add(new Rectangle(
                        (int)(tileSize * k) + limit_left, 
                      (int)(tileSize * z) , 
                      (int)(tileSize), 
                      (int)(tileSize)));

                }

                k++;
            }

            for (int i = mapSize-1; i >0 ; i--)
            {

                
                if(current_map[i]==1)
                {
                    if (i + mapW < mapSize)
                    {
                        if (current_map[i + mapW] == 0)
                        {
                            Rectangle rect_occlusion = new Rectangle(
                                map[i].rect_position.X,
                                 map[i].rect_position.Y + (int)tileSize / 2,
                                (int)(tileSize),
                                (int)(tileSize));
                            list_occlusion_ambiante.Add(rect_occlusion);
                        }
                    }
                }
                     
                    
              
            }




          



            rect_camera = map[1].rect_position;
           /*     town = _content.Load<Texture2D>("town");
            float ratio = 300 / 1000;

            int width = (screenW / 2);
            rect_position_background = new Rectangle(screenW / 4, 0, width, (int)(width / 300) * 1000);
            rect_position_background2 = new Rectangle(rect_position_background.X, -rect_position_background.Height, rect_position_background.Width,
            rect_position_background.Height);*/
        }




        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Teleport_colision();
            CheckCollision();
            rect_camera = MovePlayer(rect_camera).rect_position;
            camera = MovePlayer(rect_camera).camera_move;
          
            for(int i=0; i<map.Length;i++)
            {
                map[i].Update_RectPosition(camera);
            }

            if (list_coins.Count > 0)
            {
                list_coins.ForEach(it => it.UpdateAnim());

                Rectangle colision_playerposition = new Rectangle(
          my_player.rect_position.X ,
          my_player.rect_position.Y  , 
          my_player.rect_position.Width , 
          my_player.rect_position.Height);


        /*        Rectangle colision_playerposition = new Rectangle(
         my_player.rect_position.X - my_player.rect_position.Width,
         my_player.rect_position.Y - my_player.rect_position.Height + my_player.rect_position.Width / 4, my_player.rect_position.Width / 2, my_player.rect_position.Height / 2);
        */

                foreach (coin it in list_coins)
                {

                    Rectangle box = new Rectangle(it.rect_position.X - (int)camera.X, it.rect_position.Y - (int)camera.Y,(int)tileSize,(int)tileSize );// it.rect_position.Width, it.rect_position.Height);

                    if (box.Contains(target1)
                        || box.Contains(target2)
                        || box.Contains(target3)
                        || box.Contains(target4))
                    {
                        it.animState = 1;
                        score += 10;
                        snd_get_coin.Play();
                    }
                }

                list_coins.RemoveAll(it => it.animState == 1);
            }
        }


        public void Teleport_colision()
        {
            int counter = 0;

            foreach (Rectangle it in list_gate)
            {
                Rectangle box = new Rectangle(it.X - (int)camera.X, it.Y - (int)camera.Y, it.Width, it.Height);

                //-----------------------------------
                // corriger le delta avec la position du joueur
                // pour caler comme il faut la téléportation
                //-----------------------------------
              /*  Rectangle brokentplayerposition = new Rectangle(
              my_player.rect_position.X - my_player.rect_position.Width,
              my_player.rect_position.Y - my_player.rect_position.Height + my_player.rect_position.Width / 4, my_player.rect_position.Width / 2, my_player.rect_position.Height / 2);
              */


                if (box.Contains(target1))
                {

                    int result = counter + 1;

                    if ((counter + 1) > list_spawn.Count-1) 
                        result = 0;

                   Rectangle rect_teleport = 
                        new Rectangle(list_spawn[result].X - (int)camera.X - colision_playerposition.X,
                        list_spawn[result].Y - (int)camera.Y - colision_playerposition.Y,
                        it.Width,
                        it.Height);

                  

                    rect_camera = new Rectangle(
                        rect_camera.X + rect_teleport.X + box.Width / 2,// -(int)tileSize,
                        rect_camera.Y + rect_teleport.Y + box.Height/2,// - (int)tileSize,
                        rect_camera.Width, 
                        rect_camera.Height);

                    old_moveX = rect_camera.X;
                    old_moveY = rect_camera.Y;

                    camera = MovePlayer(rect_camera).camera_move;
                    return;
                }

                counter++;
            }
        }

        public void CheckCollision()
        {

            int horizontal_state = 0;
            int vertical_state = 0;

            int itW = list_collideBox[0].Width;
            int itH = list_collideBox[0].Height;

            foreach (Rectangle it in list_collideBox)
            {
                Rectangle box = new Rectangle(it.X - (int)camera.X, it.Y - (int)camera.Y, itW, itH);

           
                if (box.Contains(target2) || box.Contains(target1))
                {
                    if (box.Contains(vertical_target2) || box.Contains(vertical_target1))
                    { 
                      //-----------------------------------
                      // touche en haut
                      //-----------------------------------
                        vertical_colision = 0;
                        vertical_state = 20;
                        break;
                    }
                }

               else if (box.Contains(target4) || box.Contains(target3))
                {
                    if (box.Contains(vertical_target4) || box.Contains(vertical_target3))
                    { //-----------------------------------
                      // touche en bas
                      //-----------------------------------
                        vertical_colision = 1;
                        vertical_state = 20;
                        break;
                    }
                }

            }


            if (vertical_state == 0)
            {
                vertical_colision = 2;

            }


            foreach (Rectangle it in list_collideBox)
            {
                Rectangle box = new Rectangle(it.X - (int)camera.X, it.Y - (int)camera.Y, itW, itH);

                if (box.Contains(target1) || box.Contains(target4))
                {
                  if (box.Contains(horizontal_target1) || box.Contains(horizontal_target4))
                    {
                        //-----------------------------------
                        // touche à gauche
                        //-----------------------------------
                        horizontal_colision = 0;
                        horizontal_state = 20;
                        break;
                    }

                }

                if (box.Contains(target2) || box.Contains(target3))
                {
                    if (box.Contains(horizontal_target2) || box.Contains(horizontal_target3))
                    {
                        //-----------------------------------
                        // touche à droite
                        //-----------------------------------
                        horizontal_colision = 1;
                        horizontal_state = 20;
                        break;
                    }
                }
            }


            
            if (horizontal_state == 0)
            {
                horizontal_colision = 2;

            }

        }


        public override void Draw(GameTime gameTime, SpriteBatch _spritebatch)
        {
            if (loading_state == 1)
            {
                //------------------------
                // dessiner le donjons y compris la correction de position avec la camera
                //------------------------
                for (int i = 0; i < mapSize; i++)
                {
                    _spritebatch.Draw(map[i].text_sprite, map[i].rect_position_updated, map[i].rect_frame,
                        Color.White);

                }


                foreach (coin it in list_coins)
                {
                    Rectangle new_position = new Rectangle(
                     it.rect_position.X - (int)camera.X,
                      it.rect_position.Y - (int)camera.Y,
                      it.rect_position.Width,
                      it.rect_position.Height);
                    _spritebatch.Draw(it.text_sprite, new_position, it.rect_frame, Color.White);
                }


                foreach (Rectangle it in list_occlusion_ambiante)
                {
                    Rectangle new_position = new Rectangle(
                     it.X - (int)camera.X,
                      it.Y - (int)camera.Y,
                      it.Width,
                      it.Height);
                    _spritebatch.Draw(sprite_occlusion_ambiante, new_position, Color.White);
                }





                /*foreach (Rectangle it in list_spawn)
                {
                    Rectangle new_position = new Rectangle(
                     it.X - (int)camera.X,
                      it.Y - (int)camera.Y,
                      it.Width,
                      it.Height);

                    _spritebatch.Draw(coinsprite, new_position, Color.Red);
                }
                */

            }

            base.Draw(gameTime, _spritebatch);


        }
    }
}