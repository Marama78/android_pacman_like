using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data.scenes
{

    interface Isprite
    {
        public Rectangle rect_position { get; set; }
        public Texture2D text_sprite { get; set; }
    }

    interface IBuilding
    {
        public Rectangle rect_position { get; set; }
        public Rectangle rect_position_updated { get; set; }
        public Texture2D text_sprite { get; set; }
        public Rectangle colideBox { get; set; }

        public void Update_RectPosition(Vector2 __camera);
    }

    interface IAnimatedSprite
    {
        public Rectangle rect_position { get; set; }
        public Rectangle rect_frame { get; set; }
        public Texture2D text_sprite { get; set; }
        public int number_frames { get; set; }
        public int animState { get; set; }
    }

    interface IBone
    {
        public float size { get; set; }
        public Vector2 anchor { get; set; }
        public Vector2 next_anchor { get; set; }
        public Sprite_object body { get; set; }
        public string name { get; set; }
    }


    class Tile : IBuilding
    {
        public Rectangle rect_position { get; set; }
        public Rectangle rect_position_updated { get; set; }
        public Rectangle rect_frame { get; set; }
        public Texture2D text_sprite { get; set; }
        public Rectangle colideBox { get; set; }

        public void Update_RectPosition(Vector2 __camera)
        {
            rect_position_updated = new Rectangle(
                   rect_position.X - (int)__camera.X,
                  rect_position.Y - (int)__camera.Y,
                   rect_position.Width,
                   rect_position.Height);
        }

    }
    /// <summary>
    /// cette structure gère un joystick virtuel 
    /// </summary>
    public struct joystick
    {
        // public Rectangle rect_finger_sensible;
        public Rectangle rect_joy_back;
        public Rectangle rect_joy_btn;
        public Rectangle default_btn_position;
        public float axisX, axisY, screen_dim, maxX, maxY, minX, minY;
        public Texture2D back_joy;
        public Texture2D btn;

    }

    /// <summary>
    /// cette structure gère un bouton virtuel
    /// </summary>
    public struct my_button : Isprite
    {
        public Rectangle rect_position { get; set; }
        public string label;
        public Texture2D text_sprite { get; set; }


    }



    public class Player : IAnimatedSprite
    {
        public Rectangle rect_position { get; set; }
        public Rectangle rect_frame { get; set; }
        public Texture2D text_sprite { get; set; }
        public int number_frames { get; set; }
        public int animState { get; set; }

        public int width, height, current_direction, shoot_rate;

        public SpriteEffects flip_horizontal;

        private int old_animstate = 10;
        private float rate_anim = 0.2f;
        private float chronoAnim = 0;
        private int isRepeat = 0;
        public void PlayAnim(int _animState, int totalFrames, float rate, int _isRepeat)
        {
            isRepeat = _isRepeat;


            if (old_animstate != _animState)
            {
                Console.WriteLine("is walking");

                chronoAnim = 0;
                animState = _animState;
                number_frames = totalFrames;
                rate_anim = rate;
                old_animstate = _animState;
            }



        }

        public void UpdateAnim()
        {
            chronoAnim += rate_anim;

            if (isRepeat == 0)
            {
                if (chronoAnim >= number_frames)
                {
                    chronoAnim = 0;
                }
            }
            else if (isRepeat == 1)
            {
                if (chronoAnim >= number_frames)
                {
                    chronoAnim = number_frames + 1;
                    isRepeat = 2;
                }
            }

            if (isRepeat != 2)
            {
                rect_frame = new Rectangle((int)chronoAnim * rect_frame.Width, (int)animState * rect_frame.Height, rect_frame.Width, rect_frame.Height);
            }
        }


    }


    public class bullet : Isprite
    {
        public Rectangle rect_position { get; set; }
        public Texture2D text_sprite { get; set; }
        public int velocity, damage, state, direction;

        public bullet(Texture2D _sprite)
        {
            text_sprite = _sprite;

        }

        public void SetDirection(int my_playerDirection)
        {
            direction = my_playerDirection;
            state = 1;
        }
        public void UpdateMovement()
        {
            if (state == 1)
            {

                switch (direction)
                {

                    case 0:
                        rect_position = new Rectangle(
                           rect_position.X,
                            rect_position.Y - velocity,
                            rect_position.Width,
                            rect_position.Height);
                        Console.WriteLine("pouw : " + rect_position);

                        break;


                    case 1:
                        rect_position = new Rectangle(
                             rect_position.X + velocity,
                             rect_position.Y,
                             rect_position.Width,
                             rect_position.Height);
                        break;


                    case 2:
                        rect_position = new Rectangle(
                            rect_position.X,
                            rect_position.Y + velocity,
                            rect_position.Width,
                            rect_position.Height);
                        break;


                    case 3:
                        rect_position = new Rectangle(
                           rect_position.X - velocity,
                          rect_position.Y,
                           rect_position.Width,
                           rect_position.Height);
                        break;

                    default:
                        rect_position = new Rectangle(
                            rect_position.X,
                            rect_position.Y - velocity,
                            rect_position.Width,
                            rect_position.Height);
                        break;

                }
            }
        }
    }

    public class alien
    {
        public Rectangle rect_position, rect_static;
        public Texture2D sprite;
        public int velocity, life;
        public int state, style_movement;
        public float chrono_move, chrono_tir, rate;
        public Texture2D bullet_sprite;
        public float start_move = 0;
        private Vector2 vector_move;
        private float axisX, axisY;
        public int move_state, horizontal_state;
        private float chrono_dance = 5;
        private float chrono_horizontal_move, move_alien;
        public int ID;
        public alien(Texture2D _bullet_sprite, Texture2D _alien_sprite)
        {
            bullet_sprite = _bullet_sprite;
            sprite = _alien_sprite;
        }

        public bullet GetAttack(Texture2D _sprite)
        {
            return new bullet(_sprite)
            {
                rect_position = new Rectangle(this.rect_position.X + this.rect_position.Width / 2,
                this.rect_position.Y + this.rect_position.Height / 2,
                8, 12),
                velocity = 3,
            };
        }


        public void Load()
        {
        }
        public void Update(ref List<bullet> _list_tirs_alien)
        {

            if (move_state == 4)
            {
                chrono_move += 0.2f;
                if (chrono_move > 1)
                {
                    if (move_alien > rect_position.Width)
                    {
                        horizontal_state = 1;
                        rect_position = new Rectangle(rect_static.X + rect_position.Width, rect_static.Y, rect_static.Width, rect_static.Width);

                    }
                    if (move_alien < -rect_position.Width)
                    {
                        horizontal_state = 0;
                        rect_position = new Rectangle(rect_static.X - rect_position.Width, rect_static.Y, rect_static.Width, rect_static.Width);

                    }


                    if (horizontal_state == 0)
                    {
                        rect_position = new Rectangle(rect_static.X + (int)move_alien, rect_static.Y, rect_static.Width, rect_static.Width);
                        move_alien += 1;
                    }
                    if (horizontal_state == 1)
                    {
                        rect_position = new Rectangle(rect_static.X + (int)move_alien, rect_static.Y, rect_static.Width, rect_static.Width);
                        move_alien -= 1;
                    }


                    chrono_tir += rate;
                    if (chrono_tir > 30)
                    {
                        bullet mybullet = new bullet(bullet_sprite);
                        mybullet.rect_position = new Rectangle(rect_position.X + rect_position.Width / 2,
                        rect_position.Y + rect_position.Height / 2,
                        8, 12);
                        mybullet.velocity = 4;
                        _list_tirs_alien.Add(GetAttack(bullet_sprite));
                        chrono_tir = 0;

                    }
                }
            }

            if (chrono_move <= start_move && move_state != 3)
            {
                chrono_move += 0.2f;
            }
            else if (chrono_move > start_move && move_state != 3)
            {
                if (move_state != 2)
                {
                    chrono_dance -= 0.015f;
                    if (chrono_dance > 3 && move_state == 0)
                    {
                        axisX = 2f;
                        axisY = 2f;
                    }
                    else if (chrono_dance > 2 && move_state == 0)
                    {
                        move_state = 1;
                        axisX = 0;
                        axisY = 0;
                    }
                    else if (chrono_dance > 1 && move_state == 1)
                    {
                        axisX = -2f;
                        axisY = 2f;
                    }
                    else if (chrono_dance > 0 && move_state == 1)
                    {
                        move_state = 2;
                        axisX = 0;
                        axisY = 0;
                    }
                }
                else if (move_state == 2)
                {

                    if (rect_position.X == rect_static.X)
                    {
                        axisX = 0;
                    }
                    else if (rect_position.X < rect_static.X)
                    {
                        axisX = 2f;
                    }
                    else if (rect_position.X > rect_static.X)
                    {
                        axisX = -2f;
                    }

                    if (rect_position.Y == rect_static.Y)
                    {
                        axisY = 0;
                    }
                    else if (rect_position.Y < rect_static.Y)
                    {
                        axisY = 2f;
                    }
                    else if (rect_position.Y > rect_static.Y)
                    {
                        axisY = -2f;
                    }

                    if (rect_position.Y == rect_static.Y && rect_position.X == rect_static.X)
                    {
                        move_state = 3;
                        chrono_move = 0;
                        chrono_dance = 0;
                    }

                }

                if (move_state != 3)
                {
                    vector_move = new Vector2(axisX, axisY);

                    rect_position = new Rectangle(rect_position.X + (int)vector_move.X, rect_position.Y + (int)vector_move.Y, rect_position.Width, rect_position.Height);
                }


            }
        }


    }



    public class coin : IAnimatedSprite
    {
        public Rectangle rect_position { get; set; }
        public Texture2D text_sprite { get; set; }

        public Rectangle rect_frame { get; set; }
        public int number_frames { get; set; }
        public int animState { get; set; }


        private float rate_anim = 0.2f;
        private float chronoAnim = 0;
        public coin()
        {
            chronoAnim = 0;
            number_frames = 4;
            rate_anim = 0.05f;
        }

        public void UpdateAnim()
        {
            chronoAnim += rate_anim;


            if (chronoAnim >= number_frames)
            {
                chronoAnim = 0;
            }


            rect_frame = new Rectangle((int)chronoAnim * rect_frame.Width, (int)animState * rect_frame.Height, rect_frame.Width, rect_frame.Height);

        }


    }

    public struct Sprite_object : Isprite
    {
        public Rectangle rect_position { get; set; }
        public Texture2D text_sprite { get; set; }
    }


    public class level_template : Scene
    {
        /// <summary>
        /// Ce programme gère les comportements suivant :
        ///     * entrées joueur (joystick et bouton(s))
        ///     * caméra
        ///     * avatar et pnj
        /// </summary>

        protected int isDisplayJoystick = 0;
        protected joystick joy_L, joy_R;
        protected my_button fire_btn;
        protected List<bullet> list_my_bullets;
        protected List<bullet> list_bullets_ennemies;
        protected List<alien> list_aliens;
        protected Player my_player;
        protected Texture2D bullet_sprite;
        protected float move_speed;
        protected float square_screen_dim;
        protected float chrono_tir_player = 0;
        protected int tir_player_state = 0;
        protected Random rand;
        protected int score;

        protected bool restartGame = false;
        protected float chrono_restart;
        protected Texture2D[] sprite_alien;

        protected SoundEffect hit, fire;
        protected Song music;

        // protége le code et force à charger les assets
        protected int loading_state = 0;

        public List<coin> list_coins;

        protected int[] map;
        protected int tileW = 15;
        protected int tileH = 10;
        protected int size_map;

        protected int old_moveX, old_moveY;
        protected int direction_locker_colision_state = -1;

        protected int horizontal_colision = -1;
        protected int vertical_colision = -1;
        protected Texture2D coinsprite;




















        protected SoundEffect snd_get_coin;


        public level_template(MainClass _main) : base(_main)
        {


        }



        public void CreateAlienLine2(Vector2 _position, int _width, int _height, Texture2D _bullet_sprite, Texture2D _alien_sprite)
        {
            for (int i = 0; i < 8; i++)
            {
                alien it = new alien(_bullet_sprite, _alien_sprite);
                it.rect_static = new Rectangle(Convert.ToInt32(_position.X) + i * _width + _width * 5, Convert.ToInt32(_position.Y) + _height * 2, _width, _height);
                it.rect_position = new Rectangle(screenW / 2 - _width / 2, -_height, _width, _height);
                it.start_move = i * 10;
                it.chrono_move -= 200;

                it.rate = rand.Next(2, 20) / 100;
                it.ID = 2;
                list_aliens.Add(it);
            }


        }


        public override void Load(ContentManager _content)
        {

            base.Load(_content);


            list_coins = new List<coin>();
            coinsprite = _content.Load<Texture2D>("coin");
            snd_get_coin = _content.Load<SoundEffect>("get_coin");

            my_player = new Player();



            music = _content.Load<Song>("chapter 1");
            fire = _content.Load<SoundEffect>("laser");
            hit = _content.Load<SoundEffect>("hites");

            //--------------------------------
            // Préparer les nombres aléatoires
            //--------------------------------
            rand = new Random();

            //--------------------------------
            // On attribue les images aux Texture2D
            //--------------------------------
            joy_L.back_joy = _content.Load<Texture2D>("btn/back_joy");
            joy_L.btn = _content.Load<Texture2D>("btn/btn");

            //--------------------------------
            // En s'appuyant UNIQUEMENT sur la largeur screenW
            // et la hauteur screenH de l'écran
            // on décide de positionner notre joystick
            // * attention le bouton doit être centré et plus petit que le back
            //--------------------------------
            joy_L.rect_joy_back = new Rectangle(
                Convert.ToInt32(screenW / 40) + Convert.ToInt32(screenW / 20),
                Convert.ToInt32((screenH / 6) * 4),
                Convert.ToInt32(screenW / 20) * 3,
                Convert.ToInt32(screenW / 20) * 3);
            joy_L.rect_joy_btn = new Rectangle(
                joy_L.rect_joy_back.X + joy_L.rect_joy_back.Width / 2 - Convert.ToInt32(screenW / 20),
                joy_L.rect_joy_back.Y + joy_L.rect_joy_back.Height / 2 - Convert.ToInt32(screenW / 20),
                Convert.ToInt32(screenW / 20) * 2,
                Convert.ToInt32(screenW / 20) * 2);
            /*joy_L.rect_finger_sensible = new Rectangle(
                Convert.ToInt32(screenW / 40) + Convert.ToInt32(screenW / 20) - Convert.ToInt32(screenW / 20),
                Convert.ToInt32((screenH / 6) * 4) - Convert.ToInt32(screenW / 20),
                Convert.ToInt32(screenW / 20) * 5,
                Convert.ToInt32(screenW / 20) * 5);*/
            //--------------------------------
            // On n'oublie pas de sauvegarder la position initiale
            //--------------------------------
            joy_L.default_btn_position = joy_L.rect_joy_btn;

            //--------------------------------
            // On initialise les variables utilitaires
            //--------------------------------
            joy_L.maxX = joy_L.rect_joy_back.Width / 2;
            joy_L.minX = -joy_L.maxX;

            joy_L.maxY = joy_L.rect_joy_back.Height / 2;
            joy_L.minY = -joy_L.maxY;

            joy_L.screen_dim = joy_L.rect_joy_back.Width;

            square_screen_dim = joy_L.screen_dim * joy_L.screen_dim;

            //--------------------------------
            // Le bouton de tir
            //--------------------------------
            move_speed = 25;//15;// 30;

            //--------------------------------
            // Le bouton de tir
            //--------------------------------
            fire_btn.text_sprite = _content.Load<Texture2D>("btn/back_joy");
            fire_btn.rect_position = joy_L.default_btn_position;
            fire_btn.rect_position = new Rectangle(
                fire_btn.rect_position.X + 2 * screenW / 3,
                fire_btn.rect_position.Y,
                fire_btn.rect_position.Width,
                fire_btn.rect_position.Height);
            fire_btn.label = "FIRE";

            //--------------------------------
            // les balles
            //--------------------------------
            list_my_bullets = new List<bullet>();
            list_bullets_ennemies = new List<bullet>();

            bullet_sprite = _content.Load<Texture2D>("fire");

            //--------------------------------
            // le joueur
            //--------------------------------
            my_player.text_sprite = _content.Load<Texture2D>("player");
            SetPlayerObject();

            //--------------------------------
            // les aliens
            //--------------------------------
            list_aliens = new List<alien>();
            sprite_alien = new Texture2D[3];

            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

            loading_state = 1;

        }



        public void SetPlayerObject()
        {
            int rectW = screenW / 10;// screenW / 15;
            int rectH = screenW / 10; //screenW / 15;
            my_player.rect_position = new Rectangle((screenW / 2) - rectW/2, (screenH / 2) - rectH/2, rectW, rectH);
            /// my_player.rect_position = new Rectangle((screenW / 2), (screenH / 2), rectW, rectH);
           // my_player.rect_frame = new Rectangle(0, 0, 48, 48);
            my_player.rect_frame = new Rectangle(0, 0, 240, 240);
            my_player.number_frames = 4;
            my_player.PlayAnim(0, 10, 0.02f, 0);

        }

        public void Attack()
        {
            bullet it = new bullet(bullet_sprite)
            {
                text_sprite = bullet_sprite,
                damage = 10,
                velocity = 4,

            };


            switch (my_player.current_direction)
            {
                case 0:
                    it.rect_position = new Rectangle(my_player.rect_position.X, my_player.rect_position.Y, 8, 12);
                    break;

                case 1:
                    it.rect_position = new Rectangle(my_player.rect_position.X + (my_player.rect_position.Width), my_player.rect_position.Y + (my_player.rect_position.Height / 2), 12, 8);
                    break;
                case 2:
                    it.rect_position = new Rectangle(my_player.rect_position.X + (my_player.rect_position.Width / 2), my_player.rect_position.Y + my_player.rect_position.Height, 8, 12);
                    break;
                case 3:
                    it.rect_position = new Rectangle(my_player.rect_position.X - 10, my_player.rect_position.Y + (my_player.rect_position.Height / 2), 12, 8);
                    break;
                default:
                    it.rect_position = new Rectangle(my_player.rect_position.X, my_player.rect_position.Y, 8, 12);
                    break;

            }

            it.rect_position = new Rectangle(it.rect_position.X - my_player.rect_position.Width,
                it.rect_position.Y - my_player.rect_position.Height,
                it.rect_position.Width,
                it.rect_position.Height);

            it.SetDirection(my_player.current_direction);
            list_my_bullets.Add(it);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var touchstate = TouchPanel.GetState();

            my_player.UpdateAnim();


            //--------------------------------
            // lire les entrées du joueur
            //--------------------------------
            if (TouchPanel.GetCapabilities().IsConnected)
            {
                // trick is to check collide with de background button
                var result = CheckTouch(joy_L.rect_joy_back, touchstate);
                if (result.state == 1)
                {

                    //--------------------------------
                    // déplacer le bouton
                    //--------------------------------
                    // le doigt se déplace dans un cercle imaginaire de rayon
                    // .. égal à joy_L.screen_dim
                    //--------------------------------
                    joy_L.rect_joy_btn = new Rectangle(
                        Convert.ToInt32(result.touchposition.X - joy_L.rect_joy_btn.Width / 2),
                        Convert.ToInt32(result.touchposition.Y - joy_L.rect_joy_btn.Height / 2),
                        joy_L.rect_joy_btn.Width,
                        joy_L.rect_joy_btn.Height);


                }
                else if (Restore_Button_Position(result.state, touchstate) == 1)
                {
                    //--------------------------------
                    // lorsque le joueur retire son doigt de l'écran
                    // ramener à sa position initiale le bouton
                    //--------------------------------
                    joy_L.rect_joy_btn = joy_L.default_btn_position;
                }


            }

            //--------------------------------
            // manager les tirs du joueur
            //--------------------------------
            if (TouchPanel.GetCapabilities().IsConnected)
            {
                if (tir_player_state == 0)
                {
                    //--------------------------------
                    // rythmer les tirs
                    //--------------------------------
                    chrono_tir_player += 0.05f;
                    if (chrono_tir_player >= 1)
                    {
                        tir_player_state = 1;
                    }
                }
                else if (tir_player_state == 1)
                {
                    //--------------------------------
                    // attaquer
                    //--------------------------------
                    if (CheckTouch(fire_btn.rect_position, touchstate).state == 1)
                    {
                        fire.Play();
                        Attack();
                        tir_player_state = 0;
                        chrono_tir_player = 0;
                    }
                }
            }


            if (!restartGame)
            { //--------------------------------
              // déplacer les tirs
              //--------------------------------

                // trouver la direction du joueur


                if (list_my_bullets.Count > 0)
                {
                    int limitUp = 10;
                    int limitDown = screenH - 10;
                    int limitRight = screenW;
                    int limitLeft = 0;

                    foreach (var it in list_my_bullets)
                    {

                        //--------------------------------
                        // activer l'auto-destruction des tirs du joueur qui dépassent l'écran
                        //--------------------------------

                        if (it.rect_position.Y < limitUp
                            || it.rect_position.Y > limitDown
                            || it.rect_position.X < limitLeft + 20
                            || it.rect_position.X > limitRight - 20)
                        {
                            it.state = 10; // 10 => çà veut dire que l'objet passe en mode : à détruire!!!!
                        }

                        //--------------------------------
                        // activer l'auto-destruction des tirs du joueur touchent un alien
                        //--------------------------------

                        if (list_aliens != null)
                        {
                            foreach (alien target in list_aliens)
                            {
                                if (target.rect_position.Contains(it.rect_position))
                                {
                                    hit.Play();
                                    target.state = 1;
                                    it.state = 10;
                                    score += 10;
                                }
                            }
                        }


                    }

                    //--------------------------------
                    // Déplacer les tirs
                    //--------------------------------
                    list_my_bullets.ForEach(it => it.UpdateMovement());

                    //--------------------------------
                    // Nettoyer la liste des tirs
                    //--------------------------------
                    list_my_bullets.RemoveAll(it => it.state == 10);
                }




                //--------------------------------
                // Update les aliens
                //--------------------------------
                if (list_aliens.Count > 0)
                {
                    int total_ID_zero = 0;
                    int total_ID_zero_valid = 0;
                    int total_ID_one = 0;
                    int total_ID_one_valid = 0;
                    int total_ID_two = 0;
                    int total_ID_two_valid = 0;

                    foreach (var it in list_aliens)
                    {
                        it.Update(ref list_bullets_ennemies);

                        if (it.rect_position.Y > (screenH + 20))
                        {
                            it.rect_position = new Rectangle(it.rect_position.X, 0 - it.rect_position.Height, it.rect_position.Width, it.rect_position.Height);
                        }

                        if (it.rect_position.X < joy_L.rect_joy_back.X + joy_L.rect_joy_back.Width)
                        {
                            it.rect_position = new Rectangle(fire_btn.rect_position.X, it.rect_position.Y, it.rect_position.Width, it.rect_position.Height);

                        }

                        if (it.rect_position.X > fire_btn.rect_position.X)
                        {
                            it.rect_position = new Rectangle(joy_L.rect_joy_back.X + joy_L.rect_joy_back.Width, it.rect_position.Y, it.rect_position.Width, it.rect_position.Height);

                        }

                        if (it.ID == 0)
                        {
                            total_ID_zero += 1;

                        }
                        if (it.ID == 1)
                        {
                            total_ID_one += 1;
                        }

                        if (it.ID == 2)
                        {
                            total_ID_two += 1;
                        }


                        if (it.ID == 0 && it.move_state == 3)
                        {
                            total_ID_zero_valid += 1;

                        }
                        if (it.ID == 1 && it.move_state == 3)
                        {
                            total_ID_one_valid += 1;
                        }

                        if (it.ID == 2 && it.move_state == 3)
                        {
                            total_ID_two_valid += 1;
                        }
                    }


                    if (total_ID_zero == total_ID_zero_valid && total_ID_one == total_ID_one_valid && total_ID_two == total_ID_two_valid)
                    {
                        foreach (alien it in list_aliens)
                        {
                            it.move_state = 4;

                            /* if (it.ID == 0)
                             {
                                 it.move_state = 4;
                             }*/
                        }
                    }

                    /*  if (total_ID_one == total_ID_one_valid)
                      {
                          foreach (alien it in list_aliens)
                          {
                              if (it.ID == 1)
                              {
                                  it.move_state = 4;
                              }
                          }
                      }

                      if (total_ID_two == total_ID_two_valid)
                      {
                          foreach (alien it in list_aliens)
                          {
                              if (it.ID == 2)
                              {
                                  it.move_state = 4;
                              }
                          }
                      }*/

                }

                //--------------------------------
                // Update les aliens
                //--------------------------------
                if (list_bullets_ennemies.Count > 0)
                {
                    foreach (bullet it in list_bullets_ennemies)
                    {
                        it.rect_position = new Rectangle(it.rect_position.X, it.rect_position.Y + it.velocity, it.rect_position.Width, it.rect_position.Height);

                        if (it.rect_position.Y > (screenH - 16))
                        {
                            it.state = 1;
                        }
                    }

                    //--------------------------------
                    // Nettoyer la liste des tirs
                    //--------------------------------
                    list_bullets_ennemies.RemoveAll(it => it.state == 1);

                }

                //--------------------------------
                // Nettoyer la liste des aliens
                //--------------------------------
                list_aliens.RemoveAll(it => it.state == 1);




                if (list_aliens.Count <= 0)
                {
                    restartGame = true;
                }
            }
            else
            {
                chrono_restart += 0.02f;
                if (chrono_restart > 1)
                {

                    CreateAlienLine2(new Vector2((screenW / 20), 100), (screenW / 20), (screenW / 20), bullet_sprite, sprite_alien[2]);
                    restartGame = false;
                    chrono_restart = 0;


                }
            }

            //--------------------------------
            // les pièces de monnaies
            //--------------------------------





        }


        public (int state, Vector2 touchposition) CheckTouch(Rectangle target, TouchCollection _touch)
        {
            if (_touch.Count > 0)
            {
                foreach (var touch in _touch)
                {
                    if (target.Contains(touch.Position))
                    {
                        if (touch.State == TouchLocationState.Moved)//TouchLocationState.Pressed)
                        {
                            return (1, touch.Position);
                        }
                    }
                }
            }
            return (0, Vector2.Zero);
        }

        public int Restore_Button_Position(int _touch_state, TouchCollection _touch)
        {
            foreach (var touch in _touch)
            {
                if (touch.State == TouchLocationState.Released)
                {
                    return 1;
                }
            }
            return 0;
        }

        public float GetSquareDistanceJoystick()
        {
            float deltaX = joy_L.rect_joy_btn.X - joy_L.default_btn_position.X;
            float deltaY = joy_L.rect_joy_btn.Y - joy_L.default_btn_position.Y;

            float distance = deltaX * deltaX + deltaY * deltaY;


            return distance;
        }

        public float GetAxisX()
        {
            return joy_L.rect_joy_btn.X - joy_L.default_btn_position.X;
        }

        public float GetAxisY()
        {
            return joy_L.rect_joy_btn.Y - joy_L.default_btn_position.Y;
            // return joy_L.rect_joy_back.Width;
        }



        public (Vector2 camera_move, Rectangle rect_position) MovePlayer(Rectangle rect_position)
        {
            //--------------------------------
            // EXPLICATIONS :
            //   ** les rectangles de chacun des objets : rect_position, ne chagent jamais
            //   ** axisX et axisY sont l'ordre de déplacement du joueur
            //   ** ont additionne ou bien on soustrait ces valeurs aux rect_position
            //   **  C EST TOUT!!!
            //--------------------------------



            int axisX = (int)((GetAxisX() / joy_L.screen_dim) * move_speed);
            int axisY = (int)((GetAxisY() / joy_L.screen_dim) * move_speed);

            float reported_axisX = GetAxisX();
            float reported_axisY = GetAxisY();

            //-----------------------------
            // adapter l'animation du personnage en fonction de sa direction
            //-----------------------------
          ///  if (Math.Abs(GetAxisX()) > 20 || Math.Abs(GetAxisY()) > 20)
            if (Math.Abs(GetAxisX()) > 0 || Math.Abs(GetAxisY()) > 0)
                {
                my_player.PlayAnim(1, 10, 0.2f, 0);
            }
            else
            {
                my_player.PlayAnim(0, 10, 0.02f, 0);
            }

            //-----------------------------
            // le coût de déplacement
            //-----------------------------
            int moveX = rect_position.X;
            int moveY = rect_position.Y;
            switch (horizontal_colision)
            {
                case 0:
                    //-----------------------------
                    // bloqué à gauche
                    //-----------------------------

                    if (GetAxisX() < 0)
                    {
                        moveX = old_moveX;
                    }
                    else
                    {
                        moveX = axisX + rect_position.X;

                    }
                    break;
                case 1:
                    //-----------------------------
                    // bloqué à droite
                    //-----------------------------

                    if (GetAxisX() > 0)
                    {
                        moveX = old_moveX;
                    }
                    else
                    {
                        moveX = axisX + rect_position.X;
                    }
                    break;

                case 2:
                    //-----------------------------
                    // pas de colision horizontale
                    //-----------------------------
                    moveX = axisX + rect_position.X;
                    break;

                default:
                    moveX = axisX + rect_position.X;

                    break;
            }

            switch (vertical_colision)
            {
                case 0:
                    //-----------------------------
                    // bloqué en haut
                    //-----------------------------

                    if (GetAxisY() < 0)
                    {
                        moveY = old_moveY;
                    }
                    else
                    {
                        moveY = axisY + rect_position.Y;
                    }
                    break;
                case 1:
                    //-----------------------------
                    // bloqué en bas
                    //-----------------------------
                    if (GetAxisY() > 0)
                    {
                        moveY = old_moveY;
                    }
                    else
                    {
                        moveY = axisY + rect_position.Y;
                    }
                    break;
                case 2:
                    //-----------------------------
                    // pas de colision verticale
                    //-----------------------------
                    moveY = axisY + rect_position.Y;
                    break;

                default:
                    moveY = axisY + rect_position.Y;
                    break;
            }





            //-----------------------------
            // Renseigner le programme sur la direction du joueur
            // en fonction du mouvement 
            // ... projet de rpg ...
            //-----------------------------
            if (reported_axisX > 0)
            {
                if (Math.Abs(reported_axisY) > reported_axisX)
                {
                    if (reported_axisY > 0)
                    {
                        //-----------------------------
                        // le joueur descend
                        //-----------------------------
                        my_player.current_direction = 2;
                    }
                    if (reported_axisY < 0)
                    {
                        //-----------------------------
                        // le joueur monte
                        //-----------------------------
                        my_player.current_direction = 0;
                    }
                }
                else if (Math.Abs(reported_axisY) < reported_axisX)
                {
                    //-----------------------------
                    // le joueur part à droite
                    //-----------------------------
                    my_player.current_direction = 1;
                    my_player.flip_horizontal = SpriteEffects.None;
                }
            }
            else if (reported_axisX < 0)
            {
                if (Math.Abs(reported_axisX) < reported_axisY)
                {
                    if (reported_axisY > 0)
                    {
                        //-----------------------------
                        // le joueur descend
                        //-----------------------------
                        my_player.current_direction = 2;
                    }
                    if (reported_axisY < 0)
                    {
                        //-----------------------------
                        // le joueur monte
                        //-----------------------------
                        my_player.current_direction = 0;
                    }
                }
                else
                {
                    //-----------------------------
                    // le joueur part à gauche
                    //-----------------------------
                    my_player.current_direction = 3;
                    my_player.flip_horizontal = SpriteEffects.FlipHorizontally;

                }
            }

            old_moveX = moveX;
            old_moveY = moveY;

            return (new Vector2(moveX, moveY), new Rectangle(moveX, moveY, rect_position.Width, rect_position.Height));
        }

        public override void Draw(GameTime gameTime, SpriteBatch _spritebatch)
        {
            if (loading_state == 1)
            {
                //-----------------------------
                // Dessiner le joystick virtuel
                //-----------------------------

                _spritebatch.Draw(joy_L.back_joy, joy_L.rect_joy_back, Color.White);
                _spritebatch.Draw(joy_L.btn, joy_L.rect_joy_btn, Color.White);

                //-----------------------------
                // Dessiner l'interface
                //-----------------------------
                _spritebatch.DrawString(_mainclass._mainfont, "SCORE : ", new Vector2(10, 40), Color.Yellow);
                _spritebatch.DrawString(_mainclass._mainfont, "" + score, new Vector2(250, 100), Color.White);

                //-----------------------------
                // Dessiner le bouton virtuel
                //-----------------------------
                _spritebatch.Draw(fire_btn.text_sprite, fire_btn.rect_position, Color.White);
                //  _spritebatch.DrawString(_mainclass._mainfont, fire_btn.label, new Vector2(fire_btn.rect_position.X, fire_btn.rect_position.Y), Color.White);

                //-----------------------------
                // Dessiner l'avatar du joueur
                //-----------------------------
                if (my_player.text_sprite != null)
                {
                    _spritebatch.Draw(
                        my_player.text_sprite,
                        my_player.rect_position,
                        my_player.rect_frame,
                        Color.White,
                        0,
                        new Vector2(my_player.rect_position.Width / 2, my_player.rect_position.Width / 2),
                        //new Vector2(0,0),
                        my_player.flip_horizontal,
                        0);

                }

                /*  switch(my_player.current_direction)
                  {
                      case 0:
                          //-----------------------------
                          // le joueur monter
                          //-----------------------------
                          _spritebatch.DrawString(_mainclass._mainfont, "player monte", new Vector2(550, 40), Color.White);

                          break;


                      case 1:
                          //-----------------------------
                          // le joueur part à droite
                          //-----------------------------
                          _spritebatch.DrawString(_mainclass._mainfont, "player part a droite", new Vector2(550,  40), Color.White);

                          break;

                      case 2:
                          //-----------------------------
                          // le joueur descend
                          //-----------------------------
                          _spritebatch.DrawString(_mainclass._mainfont, "player descend", new Vector2(550,  40), Color.White);

                          break;

                      case 3:
                          //-----------------------------
                          // le joueur part à gauche
                          //-----------------------------
                          _spritebatch.DrawString(_mainclass._mainfont, "player part a gauche", new Vector2(550,  40), Color.White);

                          break;
                  }
                */
                //-----------------------------
                // Afficher les tirs du joueur
                //-----------------------------
                if (list_my_bullets != null)
                {
                    _spritebatch.DrawString(_mainclass._mainfont, "nombre de balles : " + list_my_bullets.Count(), new Vector2(550, 90), Color.White);

                    foreach (bullet it in list_my_bullets)
                    {
                        _spritebatch.Draw(it.text_sprite, it.rect_position, Color.White);
                    }
                }

                //-----------------------------
                // Afficher les tirs des aliens
                //-----------------------------
                if (list_bullets_ennemies != null)
                {
                    foreach (bullet it in list_bullets_ennemies)
                    {
                        _spritebatch.Draw(it.text_sprite, it.rect_position, Color.White);
                    }
                }

                //-----------------------------
                // Afficher les aliens
                //-----------------------------
                /* if (list_aliens != null)
                 {
                     foreach (alien it in list_aliens)
                     {
                         _spritebatch.Draw(it.sprite, it.rect_position, Color.White);
                     }
                 }*/


            }

            base.Draw(gameTime, _spritebatch);
        }
    }

}