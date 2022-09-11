using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    class ContinueScene : Scene
    {

        ContentManager content;

        Texture2D t_brick_green;
        Texture2D t_brick_red;
        Texture2D t_brick_gray;
        Texture2D t_brick_background;
        Texture2D t_today;

        ButtonGeneral b_back;
        ButtonGeneral b_play;

        Rectangle slider_pos;

        MENU_STATE menu_state;

        int total_height;

        List<List<int>> games;

        float s_speed;

        int[] game_id = new int[5];

        int game_count;


        public ContinueScene(SceneManager manager)
        {
            this.SceneManager = manager;

            content = null;

            games = new List<List<int>>();

            for(int i = 0; i < 60; i++)
                games.Add(new List<int>());
            
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(manager.Game.Services, "Content/ContinueResources");


            t_brick_green = content.Load<Texture2D>("plate_cleared");
            t_brick_gray = content.Load<Texture2D>("plate_todo");
            t_brick_red = content.Load<Texture2D>("plate_skipped");
            t_brick_background = content.Load<Texture2D>("plate_background");
            t_today = content.Load<Texture2D>("today");

            slider_pos = new Rectangle(0, 0, 20, 50);

            b_back = new ButtonGeneral(new Rectangle(550 - 128 - 10, 350, 128, 64));
            b_play = new ButtonGeneral(new Rectangle(550 + 10, 350, 128, 64));

        }

        public override void UnloadContent()
        {
            if(content != null)
                content.Unload();

            t_brick_green = null;
            t_brick_red = null;
            t_brick_gray = null;
            t_brick_background = null;
            t_today = null;

            b_back = null;
            b_play = null;

            for (byte i = 0; i < games.Count; i++)
            {
                games[i].Clear();
            }

            games.Clear();

            game_id = null;
        }

        public override void Activate()
        {
            menu_state = MENU_STATE.MENU_CHOOSE;

            // Align slider to current day!
            total_height = 0;
            total_height = 60 * 60;

            int current_length = 60 * manager.GetStatistic.DaysPlayed;

            for (int d = 0; d < 60; d++)
            {
                SetDay(d);
                total_height += 30 * game_count;

                if (d < manager.GetStatistic.DaysPlayed - 1)
                    current_length += 30 * game_count;
            }

            float l = (float)current_length / (float)total_height;

            slider_pos.Y = (int)((float)(480f - slider_pos.Height) * l);

            b_back.Pressed = false;
            b_play.Pressed = false;

            // Check if one day is needed to be added
            TimeSpan time = DateTime.Now - manager.GetStatistic.StartDate;

            int difference = time.Days - manager.GetStatistic.DaysPlayed;

            int days = manager.GetStatistic.DaysPlayed - 1;

            if (difference > 0)
            {
                for (int d = 0; d < difference + 1; d++)
                {
                    if (manager.GetStatistic.DaysPlayed < 60)
                    {
                        SetDay(manager.GetStatistic.DaysPlayed);
                        manager.GetStatistic.AddDay();


                        float s = -1f;

                        if (d == difference)
                            s = 0f;
                        if (difference > 59 && d == 59)
                            s = 0f;


                        for (int g = 0; g < game_count; g++)
                        {
                            manager.GetStatistic.AddGameScore(game_id[g], s);
                        }
                    }
                }
            }
            else if (time.Days == 0)
            {
                if (manager.GetStatistic.DaysPlayed < 1)
                {
                    SetDay(0);

                    manager.GetStatistic.AddDay();

                    for (int g = 0; g < game_count; g++)
                    {
                        manager.GetStatistic.AddGameScore(game_id[g], 0f);
                    }
                }
            }
        }

        public override void Initialize()
        {

        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {
                if (manager.Trial)
                {
                    DrawTrial(manager.SpriteBatch, manager.Font);    
                }
                else
                {
                    DrawChoose(manager.SpriteBatch, manager.Font);
                }
            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                DrawTransit(manager.SpriteBatch, manager.Font);
            }


            manager.SpriteBatch.End();
        }

        private void DrawChoose(SpriteBatch sp, SpriteFont font)
        {
            sp.Draw(manager.TMainBackground, manager.GraphicsDevice.Viewport.Bounds, Color.White);

            DrawList(sp, font, Vector2.Zero);

            Rectangle b = new Rectangle(0, 0, 20, 480);

            sp.Draw(manager.TSliderTrail, b, Color.White);

            sp.Draw(manager.TSliderButton, slider_pos, Color.White);


            b.X = 300 + 250 - 225;
            b.Y = 10;

            b.Width = 450;
            b.Height = 450;

            sp.Draw(manager.TBoxTransparent, b, Color.White);

            b.X = 300 + 250 - 64;
            b.Y = 20;

            b.Width = 128;
            b.Height = 64;

            sp.Draw(t_today, b, Color.Gold);

            SetDay(manager.GetStatistic.DaysPlayed - 1);

            Vector2 t = new Vector2(450f, 70f);

            for (int i = 0; i < game_count; i++)
            {
                if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) > 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.Green);
                }
                else if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) == 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.White);
                }
                else if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) < 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.Red);
                }

                t.Y += 40f;
            }

            t.Y += 40f;
            sp.DrawString(font, "Day nr: " + manager.GetStatistic.DaysPlayed, t, Color.White);


            if(b_back.Pressed)
                b_back.Draw(sp, manager.TSideLeft, Color.DarkGray);
            else
                b_back.Draw(sp, manager.TSideLeft);

            if (b_play.Pressed)
                b_play.Draw(sp, manager.TSideRight, Color.DarkGray);
            else
                b_play.Draw(sp, manager.TSideRight);

            t.X = b_back.X + 40;
            t.Y = b_back.Y + 11;

            sp.DrawString(font, "Back", t, Color.White);

            t.X = b_play.X + 32;
            t.Y = b_play.Y + 11;

            sp.DrawString(font, "Play", t, Color.White);


        }

        private void DrawTrial(SpriteBatch sp, SpriteFont font)
        {
            sp.Draw(manager.TMainBackground, manager.GraphicsDevice.Viewport.Bounds, Color.White);

            DrawList(sp, font, Vector2.Zero);

            Rectangle b = new Rectangle(0, 0, 20, 480);

            sp.Draw(manager.TSliderTrail, b, Color.White);

            sp.Draw(manager.TSliderButton, slider_pos, Color.White);



            b.X = 300 + 250 - 225;
            b.Y = 10;

            b.Width = 450;
            b.Height = 450;

            sp.Draw(manager.TBoxTransparent, b, Color.White);

            b.X = 300 + 250 - 64;
            b.Y = 20;

            b.Width = 128;
            b.Height = 64;

            sp.Draw(t_today, b, Color.Gold);

            SetDay(90);

            Vector2 t = new Vector2(450f, 70f);

            for (int i = 0; i < game_count; i++)
            {
                if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) > 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.Green);
                }
                else if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) == 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.White);
                }
                else if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) < 0f)
                {
                    sp.DrawString(font, GetGameName((GAME_TYPE)game_id[i]), t, Color.Red);
                }

                t.Y += 40f;
            }


            t.Y += 40f;
            sp.DrawString(font, "Day nr: trial day", t, Color.White);


            if (b_back.Pressed)
                b_back.Draw(sp, manager.TSideLeft, Color.DarkGray);
            else
                b_back.Draw(sp, manager.TSideLeft);

            if (b_play.Pressed)
                b_play.Draw(sp, manager.TSideRight, Color.DarkGray);
            else
                b_play.Draw(sp, manager.TSideRight);

            t.X = b_back.X + 40;
            t.Y = b_back.Y + 11;

            sp.DrawString(font, "Back", t, Color.White);

            t.X = b_play.X + 32;
            t.Y = b_play.Y + 11;

            sp.DrawString(font, "Play", t, Color.White);
        }

        private void DrawTransit(SpriteBatch sp, SpriteFont font)
        {

        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Back();
            }

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {
                s_speed -= s_speed * 0.3f;

                slider_pos.Y += (int)(s_speed * dt * 10f);

                if (slider_pos.Y < 0)
                    slider_pos.Y = 0;
                if (slider_pos.Y + slider_pos.Height > 480)
                    slider_pos.Y = 480 - slider_pos.Height;

                for (byte i = 0; i < manager.Touches.Count; i++)
                {

                    if (manager.Touches[i].State == TouchLocationState.Pressed)
                    {
                        Pressed(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Released)
                    {
                        Released(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Moved)
                    {
                        Moved(manager.Touches[i].Position);
                    }
                }
            }

        }

        private void Pressed(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                manager.PlayPress();
            }
            if (b_play.Collide(p))
            {
                manager.PlayPress();
            }
        }

        private void Moved(Vector2 p)
        {
            if (p.X < slider_pos.X + slider_pos.Width)
            {
                s_speed = p.Y - (float)(slider_pos.Y + slider_pos.Height * 0.5f);
            }

            if (b_back.Collide(p))
            {

            }
            if (b_play.Collide(p))
            {

            }
        }

        private void Released(Vector2 p)
        {
            if (b_back.Collide(p))
            {
                Back();

                b_back.Pressed = false;
            }
            if (b_play.Collide(p))
            {

                PlayGame();

                b_play.Pressed = false;
            }
        }

        private void PlayGame()
        {
            manager.ClearGames();

            if (!manager.Trial)
            {

                SetDay(manager.GetStatistic.DaysPlayed - 1);

                bool found = false;

                for (int i = 0; i < game_count; i++)
                {
                    if (manager.GetStatistic.Score(manager.GetStatistic.DaysPlayed - 1, i) == 0f)
                    {
                        if (!found)
                        {
                            manager.SetGameID((byte)i);
                        }
                        found = true;

                        manager.CustomGame = false;
                        manager.AddGame((GAME_TYPE)game_id[i]);
                    }
                }

                if (!found)
                    return;

            }
            else
            {
                SetDay(90);

                for (byte i = 0; i < game_count; i++)
                {
                    manager.AddGame((GAME_TYPE)game_id[i]);
                }
            }

            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_GAME, SCENE.SCENE_CONTINUE, true);
            }
        }



        private void DrawList(SpriteBatch sp, SpriteFont font, Vector2 offset)
        {
            Rectangle day = new Rectangle(20, 0, 240, 60);
            Vector2 game = new Vector2(20f, 0f);
            Vector2 day_name = new Vector2(90f, 10f);

            float lerp = (float)slider_pos.Y / (float)(480f - (float)slider_pos.Height);

            int temp = (int)(MathHelper.Lerp(0, total_height - 480, lerp));
            temp = (int)(lerp * (float)(total_height - 480f));

            day.Y -= temp;
            day_name.Y -= temp;

            for(int i = 0; i < 60; i++)
            {
                SetDay(i);
                int next_day = 30 * game_count + day.Height;
                
                if (day.Y + day.Height > 0 && day.Y < 480)
                {
                    sp.Draw(manager.TRectangleLong, day, Color.White);
                    sp.DrawString(font, "Day: " + (i+1), day_name, Color.Black);
                }


                game.X = (float)day.X + 20;
                game.Y = (float)day.Y + (float)day.Height;

                for(int t = 0; t < game_count; t++)
                {
                    if (game.X + 25 > 0 && game.X < 480)
                    {
                        if (i < manager.GetStatistic.DaysPlayed)
                        {
                            if(manager.GetStatistic.Score(i, t) > 0f)
                                sp.DrawString(font, GetGameName((GAME_TYPE)game_id[t]), game, Color.Green);
                            else if(manager.GetStatistic.Score(i, t) == 0f)
                                sp.DrawString(font, GetGameName((GAME_TYPE)game_id[t]), game, Color.White);
                            else
                                sp.DrawString(font, GetGameName((GAME_TYPE)game_id[t]), game, Color.Red);
                        }
                        else
                        {
                            sp.DrawString(font, GetGameName((GAME_TYPE)game_id[t]), game, Color.DarkGray);
                        }
                    }

                    game.Y += 25f;
                }

                day_name.Y += next_day;
                day.Y += next_day;
            }
        }

        private void Back()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_MAIN_MENU, SCENE.SCENE_CONTINUE, true);
            }
        }

        private string GetGameName(GAME_TYPE type)
        {
            switch (type)
            {
                case GAME_TYPE.GAME_CALCULATE:
                    return "Calculator";
                case GAME_TYPE.GAME_GUESS_PERCENTAGE:
                    return "Guess percent";
                case GAME_TYPE.GAME_MULTIPLY_BY:
                    return "Multiply by";
                case GAME_TYPE.GAME_TRUE_FALSE:
                    return "True or False";
                case GAME_TYPE.GAME_PATTERN:
                    return "Memorize Patter";
                case GAME_TYPE.GAME_MEMORIZE_ITEMS:
                    return "Memorize Items";
                case GAME_TYPE.GAME_ORDER_CIRCLE:
                    return "Order Circle";
                case GAME_TYPE.GAME_EVEN_OR:
                    return "Even or Vowel";
                case GAME_TYPE.GAME_MEMORIZE_MATCH:
                    return "Remember Match";
                case GAME_TYPE.GAME_FOCUS_ARROW:
                    return "Focus on arrow";
                case GAME_TYPE.GAME_ORDER_NUMBERS:
                    return "Ordered numbers";
                case GAME_TYPE.GAME_FALSE_CHARACTER:
                    return "False character";
                case GAME_TYPE.GAME_NUMBERS_CIRCLE:
                    return "Numbers in circle";
                case GAME_TYPE.GMAE_ORDER_CAKES:
                    return "Ordered cards";
                case GAME_TYPE.GAME_MATCH_COLOR:
                    return "Speed color";
                case GAME_TYPE.GAME_FIGURES_MATCH:
                    return "Figures match";
                default:
                    return "Error :(";
            }
        }

        private void SetDay(int d)
        {
            switch (d)
            {

                case 0:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 5;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    break;
                case 1:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    game_id[3] = 13;
                    break;
                case 2:
                    game_count = 3;
                    game_id[0] = 0;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    break;
                case 3:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 8;
                    break;
                case 4:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 7;
                    game_id[2] = 8;
                    break;
                case 5:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    break;
                case 6:
                    game_count = 4;
                    game_id[0] = 1;
                    game_id[1] = 6;
                    game_id[2] = 8;
                    game_id[3] = 12;
                    break;
                case 7:
                    game_count = 5;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 8;
                    game_id[3] = 12;
                    game_id[4] = 14;
                    break;
                case 8:
                    game_count = 3;
                    game_id[0] = 0;
                    game_id[1] = 5;
                    game_id[2] = 8;
                    break;
                case 9:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 4;
                    game_id[2] = 9;
                    game_id[3] = 15;
                    game_id[4] = 1;
                    break;
                case 10:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 10;
                    break;
                case 11:
                    game_count = 5;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    game_id[3] = 13;
                    game_id[4] = 5;
                    break;
                case 12:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 7;
                    game_id[2] = 8;
                    break;
                case 13:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    break;
                case 14:
                    game_count = 4;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    game_id[3] = 13;
                    break;
                case 15:
                    game_count = 3;
                    game_id[0] = 1;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    break;
                case 16:
                    game_count = 3;
                    game_id[0] = 0;
                    game_id[1] = 6;
                    game_id[2] = 8;
                    break;
                case 17:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 5;
                    game_id[2] = 10;
                    game_id[3] = 14;
                    game_id[4] = 4;
                    break;
                case 18:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 5;
                    game_id[2] = 8;
                    game_id[3] = 13;
                    game_id[4] = 4;
                    break;
                case 19:
                    game_count = 5;
                    game_id[0] = 0;
                    game_id[1] = 4;
                    game_id[2] = 9;
                    game_id[3] = 15;
                    game_id[4] = 11;
                    break;
                case 20:
                    game_count = 4;
                    game_id[0] = 0;
                    game_id[1] = 4;
                    game_id[2] = 10;
                    game_id[3] = 15;
                    break;
                case 21:
                    game_count = 4;
                    game_id[0] = 0;
                    game_id[1] = 5;
                    game_id[2] = 9;
                    game_id[3] = 12;
                    break;
                case 22:
                    game_count = 4;
                    game_id[0] = 3;
                    game_id[1] = 5;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    break;
                case 23:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    break;
                case 24:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    break;
                case 25:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    break;
                case 26:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 6;
                    game_id[2] = 8;
                    game_id[3] = 14;
                    game_id[4] = 12;
                    break;
                case 27:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    game_id[3] = 13;
                    game_id[4] = 3;
                    break;
                case 28:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 7;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    game_id[4] = 13;
                    break;
                case 29:
                    game_count = 4;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 10;
                    game_id[3] = 14;
                    break;
                case 30:
                    game_count = 4;
                    game_id[0] = 0;
                    game_id[1] = 5;
                    game_id[2] = 8;
                    game_id[3] = 14;
                    break;
                case 31:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 7;
                    game_id[2] = 11;
                    game_id[3] = 13;
                    game_id[4] = 5;
                    break;
                case 32:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 5;
                    game_id[2] = 10;
                    game_id[3] = 13;
                    game_id[4] = 0;
                    break;
                case 33:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 5;
                    game_id[2] = 10;
                    game_id[3] = 14;
                    game_id[4] = 2;
                    break;
                case 34:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 10;
                    break;
                case 35:
                    game_count = 3;
                    game_id[0] = 0;
                    game_id[1] = 7;
                    game_id[2] = 9;
                    break;
                case 36:
                    game_count = 5;
                    game_id[0] = 0;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    game_id[3] = 13;
                    game_id[4] = 1;
                    break;
                case 37:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 7;
                    game_id[2] = 8;
                    break;
                case 38:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 8;
                    break;
                case 39:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    game_id[3] = 13;
                    game_id[4] = 2;
                    break;
                case 40:
                    game_count = 5;
                    game_id[0] = 3;
                    game_id[1] = 5;
                    game_id[2] = 9;
                    game_id[3] = 15;
                    game_id[4] = 10;
                    break;
                case 41:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 4;
                    game_id[2] = 8;
                    game_id[3] = 13;
                    break;
                case 42:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    game_id[3] = 12;
                    break;
                case 43:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    game_id[4] = 8;
                    break;
                case 44:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    break;
                case 45:
                    game_count = 4;
                    game_id[0] = 0;
                    game_id[1] = 5;
                    game_id[2] = 9;
                    game_id[3] = 13;
                    break;
                case 46:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 7;
                    game_id[2] = 9;
                    game_id[3] = 14;
                    game_id[4] = 6;
                    break;
                case 47:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 4;
                    game_id[2] = 11;
                    game_id[3] = 14;
                    break;
                case 48:
                    game_count = 5;
                    game_id[0] = 2;
                    game_id[1] = 7;
                    game_id[2] = 10;
                    game_id[3] = 12;
                    game_id[4] = 11;
                    break;
                case 49:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 5;
                    game_id[2] = 10;
                    break;
                case 50:
                    game_count = 4;
                    game_id[0] = 2;
                    game_id[1] = 5;
                    game_id[2] = 10;
                    game_id[3] = 12;
                    break;
                case 51:
                    game_count = 5;
                    game_id[0] = 0;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    game_id[3] = 15;
                    game_id[4] = 2;
                    break;
                case 52:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 10;
                    break;
                case 53:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    break;
                case 54:
                    game_count = 4;
                    game_id[0] = 3;
                    game_id[1] = 7;
                    game_id[2] = 10;
                    game_id[3] = 12;
                    break;
                case 55:
                    game_count = 4;
                    game_id[0] = 3;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    game_id[3] = 12;
                    break;
                case 56:
                    game_count = 3;
                    game_id[0] = 3;
                    game_id[1] = 4;
                    game_id[2] = 9;
                    break;
                case 57:
                    game_count = 3;
                    game_id[0] = 0;
                    game_id[1] = 5;
                    game_id[2] = 11;
                    break;
                case 58:
                    game_count = 5;
                    game_id[0] = 1;
                    game_id[1] = 6;
                    game_id[2] = 11;
                    game_id[3] = 15;
                    game_id[4] = 14;
                    break;
                case 59:
                    game_count = 3;
                    game_id[0] = 2;
                    game_id[1] = 6;
                    game_id[2] = 9;
                    break;

                case 90:
                    game_count = 3;
                    game_id[0] = (int)GAME_TYPE.GAME_CALCULATE;
                    game_id[1] = (int)GAME_TYPE.GAME_ORDER_CIRCLE;
                    game_id[2] = (int)GAME_TYPE.GAME_MATCH_COLOR;

                    break;
                default:

                    game_count = -1;
                    
                    return;

            } // switch
        } // SetDay
    }
}
