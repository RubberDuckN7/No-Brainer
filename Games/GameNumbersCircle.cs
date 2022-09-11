using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameNumbersCircle : GameInterface
    {

        SpriteFont big_font;

        Texture2D texture_circle;

        Rectangle[] circles;

        int[] numbers;

        bool []answered;

        float cooldown;

        byte count_numbers;

        byte current;

        byte id_answer;

        byte selected_id;

        bool right;

        public GameNumbersCircle(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionNumbersCircle");


            big_font = content.Load<SpriteFont>("NumbersCircleFont");
            texture_circle = content.Load<Texture2D>("number_circle");


            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateRound();

            cooldown = 0f;

            right = false;

            selected_id = 255;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            big_font = null;

            texture_circle = null;

            circles = null;

            numbers = null;

            answered = null;
        }


        public override void Pressed(Vector2 p)
        {
            selected_id = 255;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < count_numbers; i++)
                {
                    if (answered[i] == false)
                    {
                        if (Utility.PointVsRectangle(circles[i], p))
                        {
                            selected_id = i;

                            game_scene.Manager.PlayPress();
                        }
                    }

                } // if answered
            }
        }

        public override void Moved(Vector2 p)
        {
            selected_id = 255;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < count_numbers; i++)
                {
                    if (answered[i] == false)
                    {
                        if (Utility.PointVsRectangle(circles[i], p))
                        {
                            selected_id = i;
                        }
                    }

                } // if answered
            }            
        }

        public override void Released(Vector2 p)
        {
            selected_id = 255;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < count_numbers; i++)
                {
                    if (answered[i] == false)
                    {
                        if (Utility.PointVsRectangle(circles[i], p))
                        {
                            int lowest = 1000;
                            byte id = 255;

                            for (byte n = 0; n < count_numbers; n++)
                            {
                                if (n != i && answered[n] == false)
                                {
                                    if (lowest > numbers[n])
                                    {
                                        lowest = numbers[n];
                                        id = n;
                                    }
                                }
                            }

                            if (lowest >= numbers[i])
                            {
                                right = true;
                            }
                            else
                            {
                                right = false;
                            }

                            if (current < count_numbers - 1 && right)
                            {
                                current += 1;

                                answered[i] = true;
                            }
                            else
                            {
                                cooldown = 0f;

                                id_answer = i;

                                game_state = GAME_STATE.GAME_SHOW_RESULT;
                            }

                            if (right)
                                _stat_right += 1f + count_numbers * 0.2f;
                            else
                                _stat_wrong += 1f;
                        }
                    }

                } // if answered
            }            
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if (game_state != GAME_STATE.GAME_SHOW_RESULT)
            {
                for (byte i = 0; i < count_numbers; i++)
                {
                    if (answered[i] == false)
                    {
                        if(selected_id == i)
                            sp.Draw(texture_circle, circles[i], Color.DarkGray);
                        else
                            sp.Draw(texture_circle, circles[i], Color.White);

                        sp.DrawString(big_font, "" + numbers[i], new Vector2(circles[i].X + 15f, circles[i].Y + 15f), Color.Black);
                    }
                }
            }
            else
            {
                Rectangle __b = new Rectangle(272, 50, 256, 256);

                if (right)
                    sp.Draw(game_scene.Manager.TTrue, __b, Color.White);
                else
                    sp.Draw(game_scene.Manager.TFalse, __b, Color.White);

            }
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

        public override string Description()
        {
            return "Select lowest number from the circles\npresented here until all disappear.";
        }

        private void CreateRound()
        {
            count_numbers = (byte)(Utility.Random(4f, 8f));

            current = 0;

            circles = new Rectangle[count_numbers];

            numbers = new int[count_numbers];

            answered = new bool[count_numbers];

            int start = 20;

            for (byte i = 0; i < count_numbers; i++)
            {
                numbers[i] = (int)(Utility.Random(-300, 300));

                int size = 90 + (int)(Utility.Random(-10f, 10f));

                Rectangle b = new Rectangle(start, (int)(Utility.Random(0f, 410f)), size, size);


                circles[i] = b;

                start += b.Width + (int)(Utility.Random(0f, 30f));

                answered[i] = false;
            }
        }
    }
}
