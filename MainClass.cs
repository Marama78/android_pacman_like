using Android.Content.PM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProceduralDungeon.Data;

namespace ProceduralDungeon
{
    public class MainClass : Game
    {
        public GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
      
        private GameState _gamestate_mgr;
        public SpriteFont _mainfont;

        public float previous_width;
        public float previous_height;


        public MainClass()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {

            previous_width = Window.ClientBounds.Width;
            previous_height = Window.ClientBounds.Height;

            _mainfont = Content.Load<SpriteFont>("mainfont");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _gamestate_mgr = new GameState(this);
            _gamestate_mgr.ChangeScene(GameState.sceneType.splashScreen);
            _gamestate_mgr._currentScene.Load(Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (_gamestate_mgr._currentScene != null)
            {
                _gamestate_mgr._currentScene.Update(gameTime);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            if (_gamestate_mgr._currentScene != null)
            {
                _gamestate_mgr._currentScene.Draw(gameTime, _spriteBatch);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LoadNewScene(GameState.sceneType _value)
        {
            _gamestate_mgr.ChangeScene(_value);
        }



        public SpriteBatch GetSpriteBatch()
        {
            return _spriteBatch;
        }

        public ContentManager GetContent()
        {
            return Content;
        }



    }
}
