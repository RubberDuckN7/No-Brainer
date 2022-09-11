using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameMemorizeItems : GameInterface
    {
        Texture2D[] texture_items;

        ButtonGeneral b_evaluate;

        Rectangle slider_pos;

        Vector2[] choose_pos;

        Vector2[] pos;

        float cooldown;
        float s_speed;


        // 26
        byte[] items_total;

        // current
        byte[] items_show;

        byte[] items_hide;

        byte count_hide;
        byte count_show;

        bool[] item_state;

        bool right;

        bool increase;

        public GameMemorizeItems(GameScene game_scene)
        {
            this.game_scene = game_scene;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/MemorizeItem");


            texture_items = new Texture2D[26];

            texture_items[0] = content.Load<Texture2D>("apple");
            texture_items[1] = content.Load<Texture2D>("bok_brown");
            texture_items[2] = content.Load<Texture2D>("chicken");
            texture_items[3] = content.Load<Texture2D>("cig");
            texture_items[4] = content.Load<Texture2D>("cigar");
            texture_items[5] = content.Load<Texture2D>("cow");
            texture_items[6] = content.Load<Texture2D>("egg");
            texture_items[7] = content.Load<Texture2D>("fish");

            texture_items[8] = content.Load<Texture2D>("fish_other");
            texture_items[9] = content.Load<Texture2D>("flower_red");
            texture_items[10] = content.Load<Texture2D>("flower_white");
            texture_items[11] = content.Load<Texture2D>("knife");
            texture_items[12] = content.Load<Texture2D>("letter");
            texture_items[13] = content.Load<Texture2D>("monk");
            texture_items[14] = content.Load<Texture2D>("mushroom_brown");
            texture_items[15] = content.Load<Texture2D>("mushroom_red");

            texture_items[16] = content.Load<Texture2D>("paper");
            texture_items[17] = content.Load<Texture2D>("pear");
            texture_items[18] = content.Load<Texture2D>("pen");
            texture_items[19] = content.Load<Texture2D>("pencil");
            texture_items[20] = content.Load<Texture2D>("pig");
            texture_items[21] = content.Load<Texture2D>("sandwich");
            texture_items[22] = content.Load<Texture2D>("scissors");
            texture_items[23] = content.Load<Texture2D>("time");

            texture_items[24] = content.Load<Texture2D>("watch");
            texture_items[25] = content.Load<Texture2D>("book_blue");

            choose_pos = new Vector2[26];

            item_state = new bool[26];

            b_evaluate = new ButtonGeneral(new Rectangle(100, 415, 200, 50));

            slider_pos = new Rectangle(750, 0, 50, 100);

            pos = new Vector2[26];

            Vector2 t = Vector2.Zero;

            Vector2 t2 = new Vector2(20f, 20f);

            cooldown = 0f;

            score           = 0;
            right_count     = 0;
            right_count_row = 0;
            count_show      = 4;

            byte count = 0;

            for (byte i = 0; i < 5; i++)
            {
                for (byte j = 0; j < 5; j++)
                {
                    pos[count] = t2;

                    t2.X += 70f;

                    count += 1;
                }
                t2.X = 20f;
                t2.Y += 70f;
            }

            for (byte i = 0; i < 13; i++)
            {
                t.X = 500f;
                t.Y = (float)(i * 100);

                choose_pos[i] = t;

                t.X = 600f;

                choose_pos[i + 13] = t;

                item_state[i] = false;
                item_state[i + 13] = false;
            }

            game_scene.AddTimer(1, 30);

            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateGame();
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            for (byte i = 0; i < 26; i++)
                texture_items[i] = null;

            texture_items = null;

            b_evaluate = null;

            choose_pos = null;

            pos = null;

            items_total = null;

            items_show = null;

            items_hide = null;

            item_state = null;            
        }


        private void DrawList(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(450, 0, 300, 480);

            sp.Draw(game_scene.Manager.TSliderBackground, b, Color.White);

            b = slider_pos;

            b.Y = 0;
            b.Height = 480;

            sp.Draw(game_scene.Manager.TSliderTrail, b, Color.White);

            sp.Draw(game_scene.Manager.TSliderButton, slider_pos, Color.White);

            b.Width = 64;
            b.Height = 64;

            float lerp = (float)slider_pos.Y / (float)(480f - slider_pos.Height);
            int temp = (int)(MathHelper.Lerp(0, 1300 - 480, lerp));

            for (byte i = 0; i < 26; i++)
            {
                b.X = (int)(choose_pos[i].X);
                b.Y = (int)(choose_pos[i].Y) - temp;

                if (b.Y + b.Height > 0 && b.Y < 480)
                {

                    if (item_state[i] == false)
                        sp.Draw(texture_items[i], b, Color.White);
                    else
                        sp.Draw(texture_items[i], b, Color.Gray);

                }
            }
        }

        private void DrawItemsRemember(SpriteBatch sp)
        {
            byte __step = 2;

            if (count_show > 13)
                __step = 1;

            Rectangle b = new Rectangle(0, 0, 64, 64);

            byte __pos_id = 0;

            for (byte i = 0; i < count_show; i++)
            {
                b.X = (int)(pos[__pos_id].X);
                b.Y = (int)(pos[__pos_id].Y);


                byte _found_id = 255;

                for (byte _h = 0; _h < count_hide; _h++)
                {
                    if (items_hide[_h] == i)
                        _found_id = i;
                }

                sp.Draw(texture_items[items_show[i]], b, Color.White);

                __pos_id += __step;
            }
        }

        private void DrawItemsResult(SpriteBatch sp)
        {
            byte __step = 2;

            if (count_show > 13)
                __step = 1;

            Rectangle b = new Rectangle(0, 0, 64, 64);

            byte __pos_id = 0;

            for (byte i = 0; i < count_show; i++)
            {
                byte _found_id = 255;

                for (byte _h = 0; _h < count_hide; _h++)
                {
                    if (items_hide[_h] == i)
                        _found_id = i;
                }

                if (_found_id == 255)
                {
                    b.X = (int)(pos[__pos_id].X);
                    b.Y = (int)(pos[__pos_id].Y);

                    sp.Draw(texture_items[items_show[i]], b, Color.White);
                }
                else
                {
                    b.X = (int)(pos[__pos_id].X);
                    b.Y = (int)(pos[__pos_id].Y);

                    bool t_right = false;

                    for (byte all = 0; all < 26; all++)
                    {
                        if (item_state[all])
                        {
                            if (items_show[_found_id] == all)
                                t_right = true;
                        }
                    }

                    if (t_right)
                        sp.Draw(texture_items[items_show[i]], b, Color.Green);
                    else
                        sp.Draw(texture_items[items_show[i]], b, Color.Red);
                }

                __pos_id += __step;
            }
        }

        private void DrawItemsHidden(SpriteBatch sp)
        {
            byte __step = 2;

            if (count_show > 13)
                __step = 1;

            Rectangle b = new Rectangle(0, 0, 64, 64);

            byte __pos_id = 0;

            for (byte i = 0; i < count_show; i++)
            {
                bool found = false;
                for (byte h = 0; h < count_hide; h++)
                {
                    if (items_hide[h] == i)
                        found = true;
                }

                if (items_show[i] != 255 && !found)
                {
                    b.X = (int)(pos[__pos_id].X);
                    b.Y = (int)(pos[__pos_id].Y);

                    sp.Draw(texture_items[items_show[i]], b, Color.White);
                }

                __pos_id += __step;
            }
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {

            DrawList(sp, font);
            DrawPlate(sp);

            if (game_state == GAME_STATE.GAME_CREATE)
            {
                
            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {
                DrawItemsRemember(sp);

                if (b_evaluate.Pressed)
                    b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.DarkGray);
                else
                    b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.White);

                sp.DrawString(game_scene.Manager.Font, "Start", new Vector2(b_evaluate.X + 60, b_evaluate.Y + 5), Color.Black);

            }
            else if (game_state == GAME_STATE.GAME_HIDE)
            {
                DrawItemsRemember(sp);

                Rectangle b = new Rectangle(-800, -800, 800, 800);

                b.X = (int)(MathHelper.Lerp(-800f, b.Width, cooldown));
                b.Y = (int)(MathHelper.Lerp(-800f, 480, cooldown));

                Color c = Color.DarkGray;

                if (cooldown > 0.8f)
                {
                    c.A = 255;
                }
                else
                {
                    c.A = (byte)(cooldown * 255.0f);
                }

                sp.Draw(game_scene.Manager.TPoint, game_scene.Manager.GraphicsDevice.Viewport.Bounds, c);

                c = Color.Transparent;

                c.A = 25;


                sp.Draw(game_scene.Manager.TFlashScreen, b, c);

                b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.White);

                sp.DrawString(game_scene.Manager.Font, "Start", new Vector2(b_evaluate.X + 60, b_evaluate.Y + 5), Color.Black);

            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                DrawItemsHidden(sp);

                if(b_evaluate.Pressed)
                    b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.DarkGray);
                else
                    b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.White);

                sp.DrawString(game_scene.Manager.Font, "Show result", new Vector2(b_evaluate.X + 30, b_evaluate.Y + 5), Color.Black);

            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                DrawItemsResult(sp);

                b_evaluate.Draw(sp, game_scene.Manager.TReady, Color.White);

                sp.DrawString(game_scene.Manager.Font, "Show result", new Vector2(b_evaluate.X + 30, b_evaluate.Y + 5), Color.Black);
            }
        }

        private int CollideList(Vector2 p)
        {
            Rectangle b = new Rectangle(500, 0, 64, 64);


            float lerp = (float)slider_pos.Y / (float)(480f - slider_pos.Height);
            int temp = (int)(MathHelper.Lerp(0, 1300 - 480, lerp));

            b.X = 500;
            b.Width = 64;
            b.Height = 64;
            
            for (byte i = 0; i < 26; i++)
            {
                b.X = (int)(choose_pos[i].X);
                b.Y = (int)(choose_pos[i].Y) - temp;
            
                if (Utility.PointVsRectangle(b, p))
                {
                    return (int)i;
                }
            
            }

            return -1;
        }


        public override void Pressed(Vector2 p)
        {
            b_evaluate.Pressed = false;

            if (game_state == GAME_STATE.GAME_PLAY)
            {

                int res = CollideList(p);

                if (res > -1)
                {
                    item_state[res] = !item_state[res];

                    game_scene.Manager.PlayPress();
                }

                if (b_evaluate.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }

            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {

                if (b_evaluate.Collide(p))
                {
                    
                }
            }
          
        }

        public override void Moved(Vector2 p)
        {
            b_evaluate.Pressed = false;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (p.X > slider_pos.X)
                {
                    s_speed = p.Y - (float)(slider_pos.Y + slider_pos.Height * 0.5f);
                }

                if (b_evaluate.Collide(p))
                {

                }

            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {
                if (b_evaluate.Collide(p))
                {
                    
                }
            }
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_evaluate.Collide(p))
                {
                    b_evaluate.Pressed = false;
                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                    Evaluate();
                    cooldown = 0f;
                }
            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {
                if (b_evaluate.Collide(p))
                {
                    b_evaluate.Pressed = false;
                    game_state = GAME_STATE.GAME_HIDE;
                    cooldown = 0f;
                }
            }
        }

        public override void Update(float dt)
        {
            if (game_state == GAME_STATE.GAME_CREATE)
            {
                CreateGame();

                game_state = GAME_STATE.GAME_THINK;

                cooldown = 0f;
            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {

            }
            else if (game_state == GAME_STATE.GAME_HIDE)
            {
                if (cooldown > 1.0f)
                {
                    game_state = GAME_STATE.GAME_PLAY;
                }

                cooldown += dt * 2.2f;
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                s_speed -= s_speed * 0.3f;

                slider_pos.Y += (int)(s_speed * dt * 10f);

                if (slider_pos.Y < 0)
                    slider_pos.Y = 0;
                if (slider_pos.Y + slider_pos.Height > 480)
                    slider_pos.Y = 480 - slider_pos.Height;


                cooldown = 0f;
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                // Show answers ...

                if (cooldown > 1f)
                {
                    game_state = GAME_STATE.GAME_CREATE;
                }

                cooldown += dt;
            }
        }


        private void CreateGame()
        {

            if (increase)
            {
                count_show += 1;

                items_show = new byte[count_show];

                if (count_show > 16)
                {
                    count_show = 16;
                }
            }

            for (byte i = 0; i < 26; i++)
            {
                item_state[i] = false;
            }

            byte[] temp = new byte[26];

            byte step = (byte)(26 / count_show);

            for (byte i = 0; i < 26; i++)
            {
                temp[i] = i;
            }

            for (byte i = 0; i < 26; i++)
            {
                byte swap = temp[i];
                byte id = (byte)(Utility.Random(0f, 26f));

                temp[i] = temp[id];
                temp[id] = swap;
            }

            byte temp_count = 0;

            if (count_show < 4)
            {
                temp_count = 0;
            }
            else if (count_show >= 4 && count_show < 8)
            {
                temp_count = 1;
            }
            else
            {
                temp_count = 2;
            }

            count_hide = (byte)(Utility.Random(0f, (float)(temp_count)) + 1);
            items_hide = new byte[count_hide];

            items_show = new byte[count_show];

            for (byte i = 0; i < count_show; i++)
            {
                temp_count += step;

                items_show[i] = temp[i];
            }


            byte[] _sort = new byte[count_show];

            for (byte i = 0; i < count_show; i++)
                _sort[i] = i;

            for (byte i = 0; i < count_show; i++)
            {
                byte _swap = _sort[i];
                byte id = (byte)(Utility.Random(0f, count_show));
                _sort[i] = _sort[id];
                _sort[id] = _swap;
            }

            for (byte i = 0; i < count_hide; i++)
            {
                items_hide[i] = _sort[i];
            }

        }

        private void Evaluate()
        {
            byte t_right = 0;

            increase = false;

            for (byte i = 0; i < count_hide; i++)
            {
                for (byte j = 0; j < 26; j++)
                {
                    if (item_state[j])
                    {
                        if (items_show[items_hide[i]] == j)
                        {
                            t_right += 1;
                        }
                    }
                }
            }

            if (t_right >= count_hide)
            {
                right_count += 1;

                right = true;

                _stat_right += 1f + count_show * 0.4f;


                if (right_count > 1)
                {
                    right_count = 0;

                    increase = true;

                }
            }
            else
            {
                right = false;

                _stat_wrong += 1f;
            }
        }

        public override string Description()
        {
            return "Memorize items to the left, when ready, press start.\nSelect item that has been removed from right side\n and press show results.";
        }

        private void DrawPlate(SpriteBatch sp)
        {
            Rectangle b = new Rectangle(0, 0, 400, 400);

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);
        }

    }
}
















