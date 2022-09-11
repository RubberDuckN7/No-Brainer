using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.GamerServices;

////////////////////////////////////////////////////
// Save statistic to file :O
////////////////////////////////////////////////////
using System.IO.IsolatedStorage;
using System.Collections.Generic;

namespace No_Brainer
{
    public class MainMenuScene : Scene
    {
        Texture2D texture_button;
        Texture2D texture_right;

        // Middle content
        ButtonGeneral b_new;
        ButtonGeneral b_continue;
        ButtonGeneral b_fullgame;


        // Bottom grid
        ButtonGeneral b_custom;
        ButtonGeneral b_help;
        ButtonGeneral b_statistic;

        ButtonGeneral b_cancel;
        ButtonGeneral b_accept;

        Vector2 grid_top;
        Vector2 grid_bottom;
        Vector2 grid_right;
        Vector2 grid_left;

        Vector2 grid_content;

        MENU_STATE menu_state;

        float cooldown;
        float cd_ask;

        /// <summary>
        /// 0 = new
        /// 1 = continue
        /// 2 = custom
        /// 255 = null
        /// </summary>
        byte button_pressed;
        byte transit_stage;

        /// <summary>
        /// 0 = pressed new
        /// 1 = start asking
        /// 2 = ask
        /// 3 = exit asking
        /// 255 = closed
        /// </summary>
        byte overwrite_save;

        bool save_exist;

        public MainMenuScene(SceneManager manager)
        {
            this.SceneManager = manager;
        }

        public override void LoadContent()
        {
            texture_background = manager.TMainBackground;
            texture_button = manager.TRectangle;

            texture_right = manager.TRectangle;

            Rectangle b = new Rectangle(310, 70, 130, 65);


            b_continue  = new ButtonGeneral(new Rectangle(300, 170, 200, 100));
            b_new       = new ButtonGeneral(new Rectangle(300, 310, 200, 100));

            b_fullgame = new ButtonGeneral(new Rectangle(650, 40, 80, 80));

            b_cancel = new ButtonGeneral(new Rectangle(70, 110, 80, 80));
            b_accept = new ButtonGeneral(new Rectangle(650, 110, 80, 80));

            b_help      = new ButtonGeneral(new Rectangle(18, 20, 64, 64));
            b_custom    = new ButtonGeneral(new Rectangle(18, 108, 64, 64));
            b_statistic = new ButtonGeneral(new Rectangle(18, 196, 64, 64));

            cooldown = 0f;
            cd_ask = 0f;

            transit_stage = 255;
            overwrite_save = 255;

            manager.GetStatistic = new Statistic();

            manager.SaveHeader = new byte[4];

            manager.SaveHeader[0] = 1; // Sound off/on
            manager.SaveHeader[1] = 1; // Version save file 
            manager.SaveHeader[2] = 0; // Reserved
            manager.SaveHeader[3] = 0; // Reserved

            if (manager.Trial == false)
            {

                using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (savegameStorage.FileExists("nb_statistic.dat"))
                    {
                        save_exist = true;
                        LoadStatistic();
                    }
                    else
                    {
                        save_exist = false;
                        manager.GetStatistic.StartDate = DateTime.Now;
                    }
                }
            }
            else
            {
                manager.GetStatistic.StartDate = DateTime.Now;
            }

        }

        private void ResetValues()
        {

            grid_bottom  = new Vector2(0f, 0f);
            grid_content = new Vector2(0f, 0f);
            grid_left    = new Vector2(0f, 0f);
            grid_right   = new Vector2(0f, 0f);
            grid_top     = new Vector2(0f, 0f);

        }

        public override void UnloadContent()
        {
            texture_button = null;
            texture_right = null;

            b_new = null;
            b_continue = null;

            b_fullgame = null;

            b_custom = null;
            b_help = null;
            b_statistic = null;

            b_accept = null;
            b_cancel = null;
        }

        public override void Initialize()
        {
            
        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            manager.SpriteBatch.Draw(texture_background, manager.GraphicsDevice.Viewport.Bounds, Color.White);

            Rectangle _b = new Rectangle(150, 10, 500, 500);

            manager.SpriteBatch.Draw(manager.TBrainLogoMain, _b, Color.White);

            if (manager.Trial)
            {
                _b.X = -50;
                _b.Y = 20;

                _b.Height = 130;
                _b.Width = 900;

                manager.SpriteBatch.Draw(manager.TInputRectangle, _b, Color.White);

                if (b_fullgame.Pressed)
                    b_fullgame.Draw(manager.SpriteBatch, manager.TAccept, Color.DarkGray);
                else
                    b_fullgame.Draw(manager.SpriteBatch, manager.TAccept, Color.White);

                Vector2 pos = Vector2.Zero;
                pos.X = 130f;
                pos.Y = 35f;

                manager.SpriteBatch.DrawString(manager.Font, "In trial mode you can play 3 games out \nof 16 and your progress wont be saved. \nBuy full game to unlock more games! ", pos, Color.Black);
            }


            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {
                DrawChoose(manager.SpriteBatch, manager.Font);
            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                DrawTransit(manager.SpriteBatch, manager.Font);
            }


            if (overwrite_save == 1)
            {
                Rectangle _tb = new Rectangle(-800, 150, 800, 10);

                if (transit_stage == 0)
                {
                    _tb.X = (int)(MathHelper.Lerp(-800f, 0f, cd_ask));
                }
                else if (transit_stage == 1)
                {
                    _tb.X = 0;
                    _tb.Height = (int)(MathHelper.Lerp(10f, 170f, cd_ask));
                    _tb.Y = (int)(150.0f - (_tb.Height * 0.5f) * (cd_ask));
                }

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);
            }
            else if (overwrite_save == 3)
            {
                Rectangle _tb = new Rectangle(-800, 150, 800, 10);

                if (transit_stage == 0)
                {
                    _tb.Height = (int)(MathHelper.Lerp(170f, 10f, cd_ask));

                    _tb.Y = (int)(150.0f - (_tb.Height * 0.5f) * (1f - cd_ask));
                    _tb.X = 0;
                }
                else if (transit_stage == 1)
                {
                    _tb.X = (int)(MathHelper.Lerp(0f, -800f, cd_ask));
                }

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);
            }
            else if (overwrite_save == 2)
            {
                Rectangle _tb = new Rectangle(0, 150, 800, 170);

                _tb.Y = (int)(150.0f - 85);

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);

                Vector2 pos = Vector2.Zero;

                pos.X = 170f;
                pos.Y = 110f;

                manager.SpriteBatch.DrawString(manager.Font, "Are you sure you want to \noverwrite existing progress?", pos, Color.White);

                if (b_accept.Pressed)
                    b_accept.Draw(manager.SpriteBatch, manager.TAccept, Color.DarkGray);
                else
                    b_accept.Draw(manager.SpriteBatch, manager.TAccept, Color.White);

                if (b_cancel.Pressed)
                    b_cancel.Draw(manager.SpriteBatch, manager.TCancel, Color.DarkGray);
                else
                    b_cancel.Draw(manager.SpriteBatch, manager.TCancel, Color.White);
            }

            manager.SpriteBatch.End();
        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                manager.ExitGame();

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {

                for (byte i = 0; i < manager.Touches.Count; i++)
                {

                    if (manager.Touches[i].State == TouchLocationState.Pressed)
                    {
                        Pressed(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Moved)
                    {
                        Moved(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Released)
                    {
                        Released(manager.Touches[i].Position);
                    }
                }

                if (overwrite_save == 1)
                {
                    if (transit_stage == 0)
                        cd_ask += dt * 2f;
                    else
                        cd_ask += dt * 5f;

                    if (cd_ask > 1.0f)
                    {
                        if (transit_stage == 0)
                        {
                            transit_stage = 1;
                            cd_ask = 0f;
                        }
                        else
                        {
                            overwrite_save = 2;
                            cd_ask = 0f;
                        }
                    }
                }
                else if (overwrite_save == 3)
                {
                    if (transit_stage == 0)
                        cd_ask += dt * 2f;
                    else
                        cd_ask += dt * 5f;

                    if (cd_ask > 1.0f)
                    {
                        if (transit_stage == 0)
                        {
                            transit_stage = 1;
                            cd_ask = 0f;
                        }
                        else
                        {
                            overwrite_save = 255;
                            transit_stage = 0;
                            cd_ask = 0f;
                        }
                    }
                }


            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                cooldown += dt * 2.4f;

                cooldown = 2f;

                if (cooldown > 1.0f)
                {
                    cooldown = 0f;

                    if (button_pressed == 0)
                    {
                        NewGame();
                    }
                    else if (button_pressed == 1)
                    {
                        ContinueGame();
                    }
                    else if (button_pressed == 2)
                    {
                        CustomGame();
                    }
                    else if (button_pressed == 3)
                    {
                        Options();
                    }
                    else if (button_pressed == 4)
                    {
                        ProgressGame();
                    }
                }
            }
        }

        public override void Activate()
        {
            pressed_touch = false;

            menu_state = MENU_STATE.MENU_CHOOSE;

            cooldown = 0f;

            ResetValues();
        }


        private void DrawTransit(SpriteBatch sp, SpriteFont font)
        {
            Rectangle _b = new Rectangle(160, 0, 480, 480);
            Vector2 v = new Vector2();

            _b.Width = b_continue.W;
            _b.Height = b_continue.H;


            _b.X = (int)(MathHelper.Lerp(b_continue.X, 800 * 1.2f, cooldown));
            _b.Y = b_continue.Y;

            b_continue.Draw(sp, manager.TRectangle, _b, Color.White);

            v.X = (float)(_b.X + 20);
            v.Y = (float)(_b.Y + 25);

            sp.DrawString(manager.Font24, "Continue", v, Color.White);



            if (cooldown > 0.2f)
                _b.X = (int)(MathHelper.Lerp(b_new.X, 800 * 1.2f, cooldown - 0.2f));
            else
                _b.X = b_new.X;

            _b.Y = b_new.Y;
            b_new.Draw(sp, manager.TRectangle, _b, Color.White);

            v.X = (float)(_b.X + 50);
            v.Y = (float)(_b.Y + 25);

            sp.DrawString(manager.Font24, "New", v, Color.White);

            // Bottom grid!!! :O
            _b.X = 0;
            _b.Y = (int)(MathHelper.Lerp(0, 480 * 1.4f, cooldown));

            _b.Width = 100;
            _b.Height = 480;

            sp.Draw(manager.TSliderTrail, _b, Color.White);

            _b.Width = b_help.W;
            _b.Height = b_help.H;

            _b.X = b_help.X;
            _b.Y = (int)(MathHelper.Lerp(b_help.Y, 700f, cooldown));
            b_help.Draw(sp, manager.TTile, _b, Color.White);
            sp.Draw(manager.TOptions, _b, Color.White);


            _b.X = b_custom.X;

            if(cooldown > 0.1f)
                _b.Y = (int)(MathHelper.Lerp(b_custom.Y, 700f, cooldown - 0.1f));
            else
                _b.Y = b_custom.Y;


            b_custom.Draw(sp, manager.TTile, _b, Color.White);
            sp.Draw(manager.TCustomGame, _b, Color.White);

            _b.X = b_statistic.X;

            if (cooldown > 0.2f)
                _b.Y = (int)(MathHelper.Lerp(b_statistic.Y, 700f, cooldown - 0.2f));
            else
                _b.Y = b_statistic.Y;

            b_statistic.Draw(sp, manager.TTile, _b, Color.White);
            sp.Draw(manager.TCustomGame, _b, Color.White);

        }

        private void DrawChoose(SpriteBatch sp, SpriteFont font)
        {
            Rectangle _b = new Rectangle(160, 0, 480, 480);
            Vector2 v = new Vector2();

            if (b_continue.Pressed)
                b_continue.Draw(manager.SpriteBatch, manager.TRectangle, Color.DarkGray);
            else
                b_continue.Draw(manager.SpriteBatch, manager.TRectangle);

            if (save_exist && !manager.Trial)
            {
                if (b_new.Pressed)
                    b_new.Draw(manager.SpriteBatch, manager.TRectangle, Color.DarkGray);
                else
                    b_new.Draw(manager.SpriteBatch, manager.TRectangle);
            }
            else
            {
                b_new.Draw(manager.SpriteBatch, manager.TRectangle, Color.DarkGray);
            }

            v.X = (float)(b_continue.X + 20);
            v.Y = (float)(b_continue.Y + 25);

            sp.DrawString(manager.Font24, "Continue", v, Color.White);


            v.X = (float)(b_new.X + 50);
            v.Y = (float)(b_new.Y + 25);

            sp.DrawString(manager.Font24, "New", v, Color.White);

            // Bottom
            _b.X = 0;
            _b.Y = (int)(MathHelper.Lerp(0, 480, cooldown));

            _b.Width = 100;
            _b.Height = 480;

            sp.Draw(manager.TSliderTrail, _b, Color.White);


            if (b_custom.Pressed)
                b_custom.Draw(manager.SpriteBatch, manager.TTile, Color.DarkGray);
            else
                b_custom.Draw(manager.SpriteBatch, manager.TTile);

            if (b_help.Pressed)
                b_help.Draw(manager.SpriteBatch, manager.TTile, Color.DarkGray);
            else
                b_help.Draw(manager.SpriteBatch, manager.TTile);

            if (b_statistic.Pressed)
                b_statistic.Draw(manager.SpriteBatch, manager.TTile, Color.DarkGray);
            else
                b_statistic.Draw(manager.SpriteBatch, manager.TTile);


            sp.Draw(manager.TOptions, b_help.Box, Color.White);
            sp.Draw(manager.TStatistic, b_statistic.Box, Color.White);
            sp.Draw(manager.TCustomGame, b_custom.Box, Color.White);

        }

        private void Pressed(Vector2 p)
        {
            if (pressed_touch == false)
            {
                if (overwrite_save == 2)
                {
                    if (b_cancel.Collide(p))
                    {

                    }
                    else if (b_accept.Collide(p))
                    {

                    }
                }


                if (b_continue.Collide(p))
                {
                    manager.PlayPress();
                }
                if (b_custom.Collide(p))
                {
                    manager.PlayPress();
                }
                if (b_new.Collide(p))
                {
                    manager.PlayPress();
                }
                if (b_help.Collide(p))
                {
                    manager.PlayPress();
                }
                if (b_statistic.Collide(p))
                {
                    manager.PlayPress();
                }
                if (b_fullgame.Collide(p))
                {
                    manager.PlayPress();
                }

                pressed_touch = true;
            }
        }

        public void Moved(Vector2 p)
        {

            if (overwrite_save == 2)
            {
                if (b_cancel.Collide(p))
                {

                }
                else if (b_accept.Collide(p))
                {

                }
            }

            if (b_continue.Collide(p))
            {

            }
            if (b_custom.Collide(p))
            {

            }
            if (b_new.Collide(p))
            {

            }
            if (b_help.Collide(p))
            {

            }
            if (b_statistic.Collide(p))
            {

            }
            if (b_fullgame.Collide(p))
            {

            }
        }

        private void Released(Vector2 p)
        {
            button_pressed = 255;

            if (overwrite_save != 255)
            {
                if (overwrite_save == 2)
                {
                    if (b_cancel.Collide(p))
                    {
                        overwrite_save = 3;
                    }
                    else if (b_accept.Collide(p))
                    {
                        overwrite_save = 3;

                        CreateNewProfile();
                    }

                    b_cancel.Pressed = false;
                    b_accept.Pressed = false;
                }

                return;
            }
            
            if (b_new.Collide(p))
            {
                b_new.Pressed = false;

                NewGame();
            }
            if (b_continue.Collide(p))
            {
                save_exist = true;
                if (save_exist)
                {
                    menu_state = MENU_STATE.MENU_TRANSIT;
                    button_pressed = 1;
                }

                b_continue.Pressed = false;
            }
            if (b_custom.Collide(p))
            {
                menu_state = MENU_STATE.MENU_TRANSIT;

                b_custom.Pressed = false;
                button_pressed = 2;
            }
            if (b_help.Collide(p))
            {
                menu_state = MENU_STATE.MENU_TRANSIT;

                b_help.Pressed = false;
                button_pressed = 3;
            }
            if (b_statistic.Collide(p))
            {
                menu_state = MENU_STATE.MENU_TRANSIT;

                b_statistic.Pressed = false;
                button_pressed = 4;
            }
            if (b_fullgame.Collide(p))
            {
                b_fullgame.Pressed = false;

                Guide.ShowMarketplace(PlayerIndex.One);
            }

            pressed_touch = false;
        }

        /// <summary>
        /// Overwrite existing file or create to let know that game has been started
        /// </summary>
        private void CreateNewFile()
        {
            DateTime current = DateTime.Now;

            manager.GetStatistic = new Statistic();
            manager.GetStatistic.StartDate = current;

            int start_day = current.Day;
            int start_month = current.Month;
            int start_year = current.Year;


            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream fs = null;
            using (fs = savegameStorage.CreateFile("nb_statistic.dat"))
            {
                if (fs != null)
                {
                    int days_count = 0; // Just new created file :O

                    byte[] byte_write;

                    fs.Write(manager.SaveHeader, 0, 4);

                    byte_write = System.BitConverter.GetBytes(start_day);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(start_month);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(start_year);
                    fs.Write(byte_write, 0, byte_write.Length);

                    byte_write = System.BitConverter.GetBytes(days_count);

                    fs.Write(byte_write, 0, byte_write.Length);


                } // if fs != null
            } // Using fs


        }

        /// <summary>
        /// Just temp random stuff to statistic to see if it works :O
        /// </summary>
        private void AddJunkStatistic()
        {
            for (int i = 0; i < 25; i++)
            {
                manager.GetStatistic.AddDay();

                int count = (int)(Utility.Random(3, 5));

                for (int j = 0; j < count; j++)
                {
                    int game_id = (int)(Utility.Random(1, 10));
                    float score = Utility.Random(0, 55f);

                    manager.GetStatistic.AddGameScore(game_id, score);
                }
            }
        }

        /// <summary>
        /// Save actual statistic struct to file
        /// </summary>
        private void SaveStatistic()
        {
            int start_day = manager.GetStatistic.StartDate.Day;
            int start_month = manager.GetStatistic.StartDate.Month;
            int start_year = manager.GetStatistic.StartDate.Year;


            IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream fs = null;
            using (fs = savegameStorage.CreateFile("nb_statistic.dat"))
            {
                if (fs != null)
                {
                    int days_count = manager.GetStatistic.DaysPlayed;

                    byte[] byte_write;

                    fs.Write(manager.SaveHeader, 0, 4);

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
                        int game_count = manager.GetStatistic.GameCount(i);

                        byte[] t = System.BitConverter.GetBytes(game_count);

                        fs.Write(t, 0, t.Length);

                        for (int j = 0; j < game_count; j++)
                        {
                            byte[] bt_w;

                            int game_id = manager.GetStatistic.GameID(i, j);
                            float score = manager.GetStatistic.Score(i, j);

                            bt_w = System.BitConverter.GetBytes(game_id);
                            fs.Write(bt_w, 0, bt_w.Length);

                            bt_w = System.BitConverter.GetBytes(score);
                            fs.Write(bt_w, 0, bt_w.Length);
                        }
                    }
                } // if fs != null
            } // Using fs
        }

        /// <summary>
        /// Load saved file
        /// </summary>
        private void LoadStatistic()
        {
            int start_day;
            int start_month;
            int start_year;

            int days_count;

            manager.GetStatistic = new Statistic();

            List<List<float>> load_data = new List<List<float>>();

            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (savegameStorage.FileExists("nb_statistic.dat"))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile("nb_statistic.dat", System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            byte[] saveBytes = new byte[4];

                            fs.Read(manager.SaveHeader, 0, 4);

                            // Load header info about save file
                            fs.Read(saveBytes, 0, 4);
                            start_day = System.BitConverter.ToInt32(saveBytes, 0);

                            saveBytes = new byte[4];
                            fs.Read(saveBytes, 0, 4);
                            start_month = System.BitConverter.ToInt32(saveBytes, 0);

                            saveBytes = new byte[4];
                            fs.Read(saveBytes, 0, 4);
                            start_year = System.BitConverter.ToInt32(saveBytes, 0);

                            saveBytes = new byte[4];
                            fs.Read(saveBytes, 0, 4);
                            days_count = System.BitConverter.ToInt32(saveBytes, 0);

                            manager.GetStatistic = new Statistic();

                            manager.GetStatistic.StartDate = new DateTime(start_year, start_month, start_day);

                            for (int i = 0; i < days_count; i++)
                            {
                                manager.GetStatistic.AddDay();

                                int game_count = 0;

                                fs.Read(saveBytes, 0, 4);
                                game_count = System.BitConverter.ToInt32(saveBytes, 0);

                                for (int g = 0; g < game_count; g++)
                                {
                                    int game_id;
                                    float score;

                                    fs.Read(saveBytes, 0, 4);
                                    game_id = System.BitConverter.ToInt32(saveBytes, 0);

                                    fs.Read(saveBytes, 0, 4);
                                    score = System.BitConverter.ToSingle(saveBytes, 0);

                                    manager.GetStatistic.AddGameScore(game_id, score);

                                } // for games count

                            } // for days count

                        }
                    }
                } // if(fs)

            } // Using

            if (manager.SaveHeader[0] == 0)
            {
                manager.SoundOn = false;
            }
            else
            {
                manager.SoundOn = true;
            }

        } // Load statistic

        private void Options()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_OPTIONS, SCENE.SCENE_MAIN_MENU, true);
            }
        }

        public void NewGame()
        {
            if (!manager.Trial && save_exist)
            {
                overwrite_save = 1;
                transit_stage = 0;
                cd_ask = 0f;
            }
        }

        public void CreateNewProfile()
        {
            manager.GetStatistic = new Statistic();
            manager.GetStatistic.StartDate = DateTime.Now;

            manager.SaveStatistic();
        }

        private void ContinueGame()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_CONTINUE, SCENE.SCENE_MAIN_MENU, true);
            }
        }

        private void ProgressGame()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_PROGRESS, SCENE.SCENE_MAIN_MENU, true);
            }
        }

        private void CustomGame()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_CUSTOM, SCENE.SCENE_MAIN_MENU, true);
            }
        }
    }
}
