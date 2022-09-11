using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace No_Brainer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    /// 
    public enum SCENE
    {
        SCENE_MAIN_MENU = 0,
        SCENE_GAME,
        SCENE_LOADING,
        SCENE_CUSTOM,
        SCENE_CONTINUE,
        SCENE_PROGRESS,
        SCENE_OPTIONS,
    }

    public enum GAME_TYPE
    {                                           
        GAME_CALCULATE = 0,      // Balanced    # 0          // solve         |Done
        GAME_GUESS_PERCENTAGE,   // Balanced    # 1          // solve         |Done
        GAME_MULTIPLY_BY,        // Balanced    # 2          // solve         |Done
        GAME_TRUE_FALSE,         // Balanced    # 3          // solve         |Done
        GAME_PATTERN,            // Balanced    # 4          // memory        |Done
        GAME_MEMORIZE_ITEMS,     // Balanced    # 5          // memory        |Done
        GAME_ORDER_CIRCLE,       // Balanced    # 6          // memory        |Done
        GAME_MEMORIZE_MATCH,     // Balanced    # 7          // memory        |Done
        GAME_FOCUS_ARROW,        // Balanced    # 8          // attention     |Done
        GAME_ORDER_NUMBERS,      // Balanced    # 9          // attention     |Done
        GAME_FALSE_CHARACTER,    // Balanced    # 10         // attention     |Done
        GAME_NUMBERS_CIRCLE,     // Balanced    # 11         // attention     |Done
        GMAE_ORDER_CAKES,        // Balanced    # 12         // speed         |Done
        GAME_MATCH_COLOR,        // Balanced    # 13         // speed         |Done
        GAME_FIGURES_MATCH,      // Balanced    # 14         // speed         |Done
        GAME_EVEN_OR,            // Balanced    # 15         // speed         |Done
    }

    public enum GAME_STATE
    {
        GAME_MENU = 0,
        GAME_TUTORIAL,
        GAME_RULES,
        GAME_CREATE,
        GAME_THINK,
        GAME_HIDE,
        GAME_PLAY,
        GAME_SHOW_RESULT,
        GAME_SHOW_TOTAL,
    }

    public enum TRANSIT_STATE
    {
        TRANSIT_FRONT = 0,
        TRANSIT_BACK,
        TRANSIT_NONE,
    }

    public enum MENU_STATE
    {
        MENU_CHOOSE = 0,
        MENU_TRANSIT,
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SceneManager scene_manager;
        

        public Game1()
        {
            Guide.SimulateTrialMode = true;


            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
            graphics.IsFullScreen = true;


            scene_manager = new SceneManager(this);

            Components.Add(scene_manager);


            scene_manager.AddScene(new MainMenuScene(scene_manager));
            scene_manager.AddScene(new GameScene(scene_manager));
            scene_manager.AddScene(new LoadingScene(scene_manager));
            scene_manager.AddScene(new CustomScene(scene_manager));
            scene_manager.AddScene(new ContinueScene(scene_manager));
            scene_manager.AddScene(new ProgressScene(scene_manager));
            scene_manager.AddScene(new OptionScene(scene_manager));

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
