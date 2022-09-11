using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameFalseCharacter : GameInterface 
    {

        Texture2D t_circle;

        Vector2[] pos_one;
        Vector2[] pos_two;

        string[] circle_one;
        string[] circle_two;

        byte false_id;

        float cooldown;
        float cd_bonus;

        byte count_characters;

        byte selected_id;
        bool selected_left;

        bool right;

        bool part_one;

        public GameFalseCharacter(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionOrderNumbers");

            t_circle = content.Load<Texture2D>("order_number_circle");
            
            game_state = GAME_STATE.GAME_TUTORIAL;

            cooldown = 0f;
            cd_bonus = 0f;

            selected_id = 255;
            selected_left = false;

            game_scene.AddTimer(1, 0);

            count_characters = 8;

            CreateRound();
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            t_circle = null;

            pos_one = null;
            pos_two = null;

            circle_one = null;
            circle_two = null;
        }

        public override void Pressed(Vector2 p)
        {
            Rectangle b = new Rectangle(0, 0, 90, 90);

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < count_characters; i++)
                {
                    b.X = (int)(pos_one[i].X);
                    b.Y = (int)(pos_one[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_one[i] != "")
                    {
                        selected_id = i;
                        selected_left = true;

                        game_scene.Manager.PlayPress();
                    }

                    b.X = (int)(pos_two[i].X);
                    b.Y = (int)(pos_two[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_two[i] != "")
                    {
                        selected_id = i;
                        selected_left = false;

                        game_scene.Manager.PlayPress();
                    }

                }
            }
        }

        public override void Moved(Vector2 p)
        {
            Rectangle b = new Rectangle(0, 0, 90, 90);

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < count_characters; i++)
                {
                    b.X = (int)(pos_one[i].X);
                    b.Y = (int)(pos_one[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_one[i] != "")
                    {
                        selected_id = i;
                        selected_left = true;
                    }

                    b.X = (int)(pos_two[i].X);
                    b.Y = (int)(pos_two[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_two[i] != "")
                    {
                        selected_id = i;
                        selected_left = false;
                    }

                }
            }            
        }

        public override void Released(Vector2 p)
        {
            selected_id = 255;

            Rectangle b = new Rectangle(0, 0, 90, 90);

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                for (byte i = 0; i < count_characters; i++)
                {
                    b.X = (int)(pos_one[i].X);
                    b.Y = (int)(pos_one[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_one[i] != "")
                    {
                        if (part_one && false_id == i)
                        {
                            right = true;
                            game_state = GAME_STATE.GAME_SHOW_RESULT;
                            cooldown = 0f;

                            press = true;
                        }
                        else
                        {
                            right = false;
                            game_state = GAME_STATE.GAME_SHOW_RESULT;
                            cooldown = 0f;

                            press = true;
                        }
                    }

                    b.X = (int)(pos_two[i].X);
                    b.Y = (int)(pos_two[i].Y);

                    if (Utility.PointVsRectangle(b, p) && circle_two[i] != "")
                    {
                        if (!part_one && false_id == i)
                        {
                            right = true;
                            game_state = GAME_STATE.GAME_SHOW_RESULT;
                            cooldown = 0f;

                            press = true;
                        }
                        else
                        {
                            right = false;
                            game_state = GAME_STATE.GAME_SHOW_RESULT;
                            cooldown = 0f;

                            press = true;
                        }
                    }

                }

                if (press)
                {
                    cd_bonus = 0f;

                    if (right)
                    {
                        _stat_right += 1f;
                        if (cd_bonus < 2f)
                        {
                            _stat_right += (2f - cd_bonus) * 3f;
                        }
                    }
                    else
                    {
                        _stat_wrong += 1f;
                    }
                }
            }            
        }

        public override string Description()
        {
            return "Eeach side has numbers and letters, but there\nis one that is only on one side.\nSelect it as fast as you can.";
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(390, 0, 20, 480);

            sp.Draw(game_scene.Manager.TSliderTrail, b, Color.White);

            b.Width = 90;
            b.Height = 90;

            for (byte i = 0; i < count_characters; i++)
            {
                b.X = (int)(pos_one[i].X);
                b.Y = (int)(pos_one[i].Y);

                if (circle_one[i] != "")
                {
                    if (selected_id == i && selected_left)
                        sp.Draw(t_circle, b, Color.DarkGray);
                    else
                        sp.Draw(t_circle, b, Color.White);
                }

                Vector2 t = pos_one[i];

                t.X += 20f;
                t.Y += 20f;

                sp.DrawString(font, circle_one[i], t, Color.Black);



                b.X = (int)(pos_two[i].X);
                b.Y = (int)(pos_two[i].Y);

                if (circle_two[i] != "")
                {
                    if (selected_id == i && !selected_left)
                        sp.Draw(t_circle, b, Color.DarkGray);
                    else
                        sp.Draw(t_circle, b, Color.White);
                }

                t = pos_two[i];

                t.X += 20f;
                t.Y += 20f;

                sp.DrawString(font, circle_two[i], t, Color.Black);
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
                    cooldown = 0f;
                    game_state = GAME_STATE.GAME_CREATE;
                }

                cooldown += dt;
            }
        }

        private void CreateRound()
        {
            if (count_characters > 6)
            {
                if (!right)
                    count_characters -= 1;
            }
            else if (count_characters < 8)
            {
                if (right)
                    count_characters += 1;
            }

            circle_one = new string[count_characters];
            circle_two = new string[count_characters];

            pos_one = new Vector2[count_characters];
            pos_two = new Vector2[count_characters];


            byte id = (byte)(Utility.Random(0f, count_characters));

            Vector2 p1 = Vector2.Zero;
            Vector2 p2 = Vector2.Zero;

            for (byte i = 0; i < count_characters; i++)
            {
                if (i < count_characters * 0.5)
                {
                    p1.X = Utility.Random(0f, 150f);
                    p1.Y = (float)(i * 90f) + Utility.Random(-10f, 10f);

                    p2.X = Utility.Random(400f, 550f);
                    p2.Y = (float)(i * 90f) + Utility.Random(-10f, 10f);

                    pos_one[i] = p1;
                    pos_two[i] = p2;
                }
                else
                {

                    p1.X = Utility.Random(200f, 310f);
                    p1.Y = (float)((i - count_characters * 0.5) * 90f) + Utility.Random(-10f, 10f);

                    p2.X = Utility.Random(600f, 710f);
                    p2.Y = (float)((i - count_characters * 0.5) * 90f) + Utility.Random(-20f, 0f);

                    pos_one[i] = p1;
                    pos_two[i] = p2;
                }


                if (Utility.Random(0f, 100f) < 50f)
                {
                    string c = GenerateLetter();
                
                    circle_one[i] = c;
                    circle_two[i] = c;
                }
                else
                {
                    string c = "" + GenerateNumber();
                
                    circle_one[i] = c;
                    circle_two[i] = c;
                }


                if (id == i)
                {
                    false_id = id;


                    if (Utility.Random(0f, 100f) < 50f)
                    {
                        circle_one[i] = "";
                        part_one = false;
                    }
                    else
                    {
                        circle_two[i] = "";
                        part_one = true;
                    }
                }
            }

        }

        private string GenerateLetter()
        {
            int id = (int)(Utility.Random(0f, 26));

            switch (id)
            {
                case 0: return "Q";
                case 1: return "W";
                case 2: return "E";
                case 3: return "R";
                case 4: return "T";
                case 5: return "Y";
                case 6: return "U";
                case 7: return "I";
                case 8: return "O";
                case 9: return "P";
                case 10: return "A";
                case 11: return "S";
                case 12: return "D";
                case 13: return "F";
                case 14: return "G";
                case 15: return "H";
                case 16: return "J";
                case 17: return "K";
                case 18: return "L";
                case 19: return "Z";
                case 20: return "X";
                case 21: return "C";
                case 22: return "V";
                case 23: return "B";
                case 24: return "N";
                case 25: return "M";
            }


            return "G";
        }

        private int GenerateNumber()
        {
            return (int)(Utility.Random(1f, 300f));
        }

    }
}
