using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameOrderNumbers : GameInterface
    {

        Texture2D t_circle;

        Vector2[] pos;

        int[] numbers;


        float cooldown;
        float cd_bonus;

        byte count_numbers;

        byte false_id;

        byte selected_id;

        bool right;

        public GameOrderNumbers(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionOrderNumbers");

            t_circle = content.Load<Texture2D>("order_number_circle");


            pos = new Vector2[24];

            Vector2 _p = Vector2.Zero;
            _p.X = 5f;

            _p.Y += 50f;

            byte id = 0;

            for (byte i = 0; i < 3; i++)
            {
                for (byte j = 0; j < 8; j++)
                {

                    pos[id] = _p;

                    _p.X += 100f;

                    id += 1;
                }

                _p.X = 5f;
                _p.Y += 100f;
                
            }

            game_state = GAME_STATE.GAME_TUTORIAL;

            cooldown = 0f;
            right = false;

            count_numbers = 8;

            game_scene.AddTimer(1, 0);

            selected_id = 255;

            CreateRound();
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            t_circle = null;

            pos = null;

            numbers = null;
        }

        public override void Pressed(Vector2 p)
        {
            selected_id = 255;
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                for (byte i = 0; i < count_numbers; i++)
                {
                    b.X = (int)(pos[i].X);
                    b.Y = (int)(pos[i].Y);

                    if (Utility.PointVsRectangle(b, p))
                    {
                        selected_id = i;

                        game_scene.Manager.PlayPress();
                    }
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            selected_id = 255;
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                for (byte i = 0; i < count_numbers; i++)
                {
                    b.X = (int)(pos[i].X);
                    b.Y = (int)(pos[i].Y);

                    if (Utility.PointVsRectangle(b, p))
                    {
                        selected_id = i;
                    }
                }
            }            
        }

        public override void Released(Vector2 p)
        {
            selected_id = 255;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                bool press = false;

                for (byte i = 0; i < count_numbers; i++)
                {
                    b.X = (int)(pos[i].X);
                    b.Y = (int)(pos[i].Y);

                    if (Utility.PointVsRectangle(b, p))
                    {
                        press = true;

                        if (i == false_id)
                        {
                            right = true;

                            right_count += 1;
                        }
                        else
                        {
                            right = false;
                        }

                        game_state = GAME_STATE.GAME_SHOW_RESULT;
                    }
                }

                if (press)
                {
                    if (right)
                    {
                        _stat_right += 1f;

                        if (cd_bonus < 2f)
                        {
                            _stat_right += 4f * (2f - cd_bonus) + 1f;
                        }
                    }
                    else
                    {
                        _stat_wrong += 1f;
                    }
                }

            }            
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(0, 0, 90, 90);

            Vector2 t = new Vector2();

            for (byte i = 0; i < count_numbers; i++)
            {
                b.X = (int)(pos[i].X);
                b.Y = (int)(pos[i].Y);

                t = pos[i];

                // Align text to center
                t.X += 15f;
                t.Y += 20f;

                if (selected_id == i)
                    sp.Draw(t_circle, b, Color.DarkGray);
                else
                    sp.Draw(t_circle, b, Color.White);

                sp.DrawString(font, "" + numbers[i], t, Color.Black);
            }

            if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                Rectangle __b = new Rectangle(272, 50, 256, 256);

                if (right)
                    sp.Draw(game_scene.Manager.TTrue, __b, Color.White);
                else
                    sp.Draw(game_scene.Manager.TFalse, __b, Color.White);
            }
        }

        public override string Description()
        {
            return "One of the circles presented is out of\norder, select it.";
        }

        public override void Update(float dt)
        {
            if (game_state == GAME_STATE.GAME_CREATE)
            {
                CreateRound();

                game_state = GAME_STATE.GAME_PLAY;
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                cd_bonus += dt;   
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                if (cooldown > 0.4f)
                {
                    game_state = GAME_STATE.GAME_CREATE;

                    cooldown = 0f;
                }

                cooldown += dt;
            }
        }

        private void CreateRound()
        {
            if (right_count > 2)
            {
                if (count_numbers < 24)
                {
                    count_numbers += 8;
                }

                right_count = 0;
            }

            numbers = new int[count_numbers];

            int start = (int)(Utility.Random(-150f, 500f));

            int step = (int)(Utility.Random(1f, 5f));


            for(byte i = 0; i < count_numbers; i++)
            {
                numbers[i] = start;

                start += step;
            }

            false_id = (byte)(Utility.Random(0f, count_numbers - 1));

            numbers[false_id] = numbers[false_id] + (int)(Utility.Random(1f, 25f));
        }
    }
}
