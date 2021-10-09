using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProceduralDungeon.Data.scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProceduralDungeon.Data
{
    public class GameState
    {
        public enum sceneType
        {
            splashScreen, menu, level1
        }

        protected MainClass _mainclass;

        public Scene _currentScene { get; set; }

        public GameState(MainClass _main)
        {
            _mainclass = _main;
        }


        public void ChangeScene(sceneType _sceneWanted)
        {
            if (_currentScene != null)
            {
                _currentScene.Unload();
                _currentScene = null;
            }


            switch (_sceneWanted)
            {
                case sceneType.splashScreen:
                    // draw splashScreen screen
                    _currentScene = new SplashScreen(_mainclass);
                    /// _currentScene = new level_template(_mainclass);
                   /// _currentScene = new Level1(_mainclass);
                    break;

                case sceneType.menu:
                    // draw menu screen
                    _currentScene = new Mainscreen(_mainclass);
                    break;

                case sceneType.level1:
                    // draw level1 screen
                    _currentScene = new Level1(_mainclass);

                    break;
            }


            _currentScene.Load(_mainclass.Content);

        }


    }
}