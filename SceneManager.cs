using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.GamerServices;

using System.IO.IsolatedStorage;

using System.Collections.Generic;

using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace No_Brainer
{
    public class SceneManager : DrawableGameComponent
    {
        Scene[] scenes;
        Scene current_scene;

        SpriteBatch sprite_batch;
        SpriteFont font;
        SpriteFont font24;

        TouchCollection touches;

        SoundEffect s_press;
        SoundEffect s_flip;

        Texture2D t_accept;
        Texture2D t_box_transparent;
        Texture2D t_brain_logo_main;
        Texture2D t_cancel;
        Texture2D t_continue;
        Texture2D t_custom_game;
        Texture2D t_false;
        Texture2D t_flash_screen;
        Texture2D t_input_rectangle;
        Texture2D t_main_background;
        Texture2D t_message_alert;
        Texture2D t_message_box;
        Texture2D t_options;
        Texture2D t_point;
        Texture2D t_ready;
        Texture2D t_rectangle;
        Texture2D t_rectangle_long;
        Texture2D t_rectangle_lock;
        Texture2D t_side_left;
        Texture2D t_side_right;
        Texture2D t_slider_button;
        Texture2D t_slider_background;
        Texture2D t_slider_trail;
        Texture2D t_statistic;
        Texture2D t_tile;
        Texture2D t_timer;
        Texture2D t_true;

        Texture2D t_left_true;
        Texture2D t_right_false;

        Texture2D t_circle_small;
        Texture2D t_circle_big;

        Statistic stuff_save;

        List<GAME_TYPE> games;

        /// <summary>
        /// 0 = 1 - sound on, 0 - sound off
        /// 1 = file version
        /// 2 = reserve
        /// 3 = reserve
        /// </summary>
        byte[] save_header;

        byte game_current;

        byte scene_count;

        byte game_id;

        bool custom_game;

        bool __trial__;

        bool sound_on;

        public SceneManager(Game game)
            : base(game)
        {
            scene_count = 7;

            scenes = new Scene[scene_count];

            current_scene = null;

            game_current = 255;

            custom_game = false;

            __trial__ = true;

            stuff_save = new Statistic();

            games = new List<GAME_TYPE>();

            sound_on = true;

            game_id = 255;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            sprite_batch = new SpriteBatch(GraphicsDevice);

            font = content.Load<SpriteFont>("FontDefault");
            font24 = content.Load<SpriteFont>("Kootenay24");

            t_accept            = content.Load<Texture2D>("accept");
            t_box_transparent   = content.Load<Texture2D>("box_transparent");
            t_brain_logo_main   = content.Load<Texture2D>("brain_logo_main");
            t_cancel            = content.Load<Texture2D>("cancel");
            t_continue          = content.Load<Texture2D>("continue");
            t_custom_game       = content.Load<Texture2D>("custom_game");
            t_false             = content.Load<Texture2D>("false");
            t_flash_screen      = content.Load<Texture2D>("flash_screen");
            t_input_rectangle   = content.Load<Texture2D>("input_rectangle");
            t_main_background   = content.Load<Texture2D>("main_background");
            t_message_alert     = content.Load<Texture2D>("message_alert");
            t_message_box       = content.Load<Texture2D>("message_box");
            t_options           = content.Load<Texture2D>("options");
            t_point             = content.Load<Texture2D>("point");
            t_ready             = content.Load<Texture2D>("ready");
            t_rectangle         = content.Load<Texture2D>("rectangle");
            t_rectangle_long    = content.Load<Texture2D>("rectangle_long");
            t_rectangle_lock    = content.Load<Texture2D>("rectangle_lock");
            t_side_left         = content.Load<Texture2D>("side_left");
            t_side_right        = content.Load<Texture2D>("side_right");
            t_slider_button     = content.Load<Texture2D>("slider_button");
            t_slider_background = content.Load<Texture2D>("slider_background");
            t_slider_trail      = content.Load<Texture2D>("slider_trail");
            t_statistic         = content.Load<Texture2D>("statistic");
            t_tile              = content.Load<Texture2D>("tile");
            t_timer             = content.Load<Texture2D>("timer");
            t_true              = content.Load<Texture2D>("true");
            t_left_true         = content.Load<Texture2D>("left_true");
            t_right_false       = content.Load<Texture2D>("right_false");
            t_circle_small      = content.Load<Texture2D>("circle_small");
            t_circle_big        = content.Load<Texture2D>("circle_big");

            s_flip = content.Load<SoundEffect>("flip");
            s_press = content.Load<SoundEffect>("button_press");

            __trial__ = true;
            __trial__ = Guide.IsTrialMode;

            __trial__ = false;

            for (byte i = 0; i < scene_count; i++)
            {
                if (scenes[i] != null)
                {
                    scenes[i].LoadContent();
                }
            }

            ActivateScene(SCENE.SCENE_MAIN_MENU);
        }

        protected override void UnloadContent()
        {
            for (byte i = 0; i < scene_count; i++)
            {
                scenes[i].UnloadContent();
                scenes[i] = null;
            }

            scenes = null;
            current_scene = null;

            sprite_batch = null;
            font = null;
            font24 = null;

            s_press = null;
            s_flip = null;

            t_accept = null;
            t_box_transparent = null;
            t_brain_logo_main = null;
            t_cancel = null;
            t_continue = null;
            t_custom_game = null;
            t_false = null;
            t_flash_screen = null;
            t_input_rectangle = null;
            t_main_background = null;
            t_message_alert = null;
            t_message_box = null;
            t_options = null;
            t_point = null;
            t_ready = null;
            t_rectangle = null;
            t_rectangle_long = null;
            t_rectangle_lock = null;
            t_side_left = null;
            t_side_right = null;
            t_slider_button = null;
            t_slider_background = null;
            t_slider_trail = null;
            t_statistic = null;
            t_tile = null;
            t_timer = null;
            t_true = null;
            t_left_true = null;
            t_right_false = null;

            stuff_save = null;

            save_header = null;

            GC.Collect();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            touches = TouchPanel.GetState();

            if (current_scene != null)
            {
                current_scene.Update(dt);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Teal);

            if (current_scene != null)
            {
                current_scene.Draw();
            }
            else
            {
                sprite_batch.Begin();

                sprite_batch.DrawString(font, "LOADING...", Vector2.Zero, Color.White);

                sprite_batch.End();
            }
        }

        public void AddScene(Scene scene)
        {
            for (byte i = 0; i < scene_count; i++)
            {
                if (scenes[i] == null)
                {
                    scenes[i] = scene;

                    return;
                }
            }
        }

        public void ActivateScene(SCENE s)
        {
            if ((byte)(s) >= scene_count)
                return;

            current_scene = scenes[(byte)(s)];
            current_scene.Activate();
        }

        public void AddGame(GAME_TYPE type)
        {
            games.Add(type);
        }

        public void ClearGames()
        {
            game_current = 0;
            games.Clear();
        }

        public GAME_TYPE GameAt(int id)
        {
            return games[id];
        }

        public int GameCount
        {
            get { return games.Count; }
        }

        public void ExitGame()
        {
            if(!__trial__)
                SaveStatistic();
            Game.Exit();
        }

        /// <summary>
        /// Save actual statistic struct to file
        /// </summary>
        public void SaveStatistic()
        {
            int start_day = stuff_save.StartDate.Day;
            int start_month = stuff_save.StartDate.Month;
            int start_year = stuff_save.StartDate.Year;

            if (sound_on)
                save_header[0] = 1;
            else
                save_header[0] = 0;

            save_header[1] = 1;
            save_header[2] = 0;
            save_header[3] = 0;

            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream fs = null;
            using (fs = savegameStorage.CreateFile("nb_statistic.dat"))
            {
                if (fs != null)
                {
                    int days_count = stuff_save.DaysPlayed;

                    byte[] byte_write;

                    fs.Write(save_header, 0, 4);

                    byte_write = System.BitConverter.GetBytes(start_day);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(start_month);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(start_year);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(days_count);

                    fs.Write(byte_write, 0, byte_write.Length);

                    for (int i = 0; i < days_count; i++)
                    {
                        int game_count = stuff_save.GameCount(i);

                        byte[] t = System.BitConverter.GetBytes(game_count);

                        fs.Write(t, 0, t.Length);

                        for (int j = 0; j < game_count; j++)
                        {
                            byte[] bt_w;

                            int game_id = stuff_save.GameID(i, j);
                            float score = stuff_save.Score(i, j);

                            bt_w = System.BitConverter.GetBytes(game_id);
                            fs.Write(bt_w, 0, bt_w.Length);

                            bt_w = System.BitConverter.GetBytes(score);
                            fs.Write(bt_w, 0, bt_w.Length);
                        }


                    }


                } // if fs != null
            } // Using fs
        }

        public GAME_TYPE Type()
        {
            return (GAME_TYPE)(games[game_current]);
        }

        public bool FinishGame()
        {
            game_current++;

            if (game_current >= games.Count)
            {
                game_current = 0;
                games.Clear();

                return true;
            }


            return false;
        }

        public byte CurrentGame
        {
            get {   return game_current; }
        }

        public Scene Scene(SCENE s)
        {
            if ((byte)(s) >= scene_count)
                return null;

            return scenes[(byte)(s)];
        }

        public void SetScore(float score)
        {
            if (game_current < 255)
            {
                GetStatistic.SetScore(game_id + game_current, score);
            }
        }

        public void SetGameID(byte id)
        {
            game_id = id;
        }

        public bool CustomGame
        {
            get { return custom_game; }
            set { custom_game = value; }
        }

        public TouchCollection Touches
        {
            get { return touches; }
        }

        public Statistic GetStatistic
        {
            get { return stuff_save; }
            set { stuff_save = value; }
        }

        public bool SoundOn
        {
            get { return sound_on; }
            set { sound_on = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return sprite_batch; }
        }

        public SpriteFont Font
        {
            get { return font; }
        }

        public SpriteFont Font24
        {
            get { return font24; }
        }

        public Texture2D TAccept
        {
            get {  return t_accept;    } 
        }
        public Texture2D TBoxTransparent
        {
            get {  return t_box_transparent;     } 
        }
        public Texture2D TBrainLogoMain
        {
            get {  return t_brain_logo_main;     } 
        }
        public Texture2D TCancel
        {
            get {  return t_cancel;              } 
        }
        public Texture2D TContinue
        {
            get {  return t_continue;            } 
        }
        public Texture2D TCustomGame
        {
            get {  return t_custom_game;         } 
        }
        public Texture2D TFalse
        {
            get {  return t_false;               } 
        }
        public Texture2D TFlashScreen
        {
            get {  return t_flash_screen;        } 
        }
        public Texture2D TInputRectangle
        {
            get {  return t_input_rectangle;     } 
        }
        public Texture2D TMainBackground
        {
            get {  return t_main_background;     }
        }
        public Texture2D TMessageAlert
        {
            get {  return t_message_alert;       } 
        }
        public Texture2D TMessageBox
        {
            get {  return t_message_box;         } 
        }
        public Texture2D TOptions
        {
            get {  return t_options;             } 
        }
        public Texture2D TPoint
        {
            get {  return t_point;               } 
        }
        public Texture2D TReady
        {
            get {  return t_ready;               } 
        }
        public Texture2D TRectangle
        {
            get {  return t_rectangle;           } 
        }
        public Texture2D TRectangleLong
        {
            get { return t_rectangle_long; }
        }
        public Texture2D TRectangleLock
        {
            get {  return t_rectangle_lock;      } 
        }
        public Texture2D TSideLeft
        {
            get {  return t_side_left;           } 
        }
        public Texture2D TSideRight
        {
            get {  return t_side_right;          } 
        }
        public Texture2D TSliderButton
        {
            get {  return t_slider_button;       } 
        }
        public Texture2D TSliderBackground
        {
            get {  return t_slider_background;   } 
        }
        public Texture2D TSliderTrail
        {
            get {  return t_slider_trail;        } 
        }
        public Texture2D TStatistic
        {
            get {  return t_statistic;           }
        }
        public Texture2D TTile
        {
            get {  return t_tile;                } 
        }
        public Texture2D TTimer
        {
            get { return t_timer; }
        }
        public Texture2D TTrue
        {
            get {  return t_true;                } 
        }

        public Texture2D TLeftTrue
        {
            get { return t_left_true; }
        }

        public Texture2D TRightFalse
        {
            get { return t_right_false; }
        }

        public Texture2D TCircleSmall
        {
            get { return t_circle_small; }
        }

        public Texture2D TCircleBig
        {
            get { return t_circle_big; }
        }

        public void PlayFlip()
        {
            if (sound_on)
                s_flip.Play();
        }

        public void PlayPress()
        {
            if (sound_on)
                s_press.Play();
        }

        public byte[] SaveHeader
        {
            get { return save_header; }
            set { save_header = value; }
        }

        public bool Trial
        {
            get { return __trial__; }
        }

    }
}
