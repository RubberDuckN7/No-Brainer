using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameGuessPercentage : GameInterface
    {
        Texture2D t_circle;

        ButtonGeneral b_one;
        ButtonGeneral b_two;
        ButtonGeneral b_three;
        
        float cooldown;
        float cd_bonus;

        int percent;
        int var_one;
        int var_two;

        int difficulty;

        string circle_one;
        string circle_two;
        string circle_three;

        byte true_id;

        bool right;

        public GameGuessPercentage(GameScene game)
        {
            game_scene = game;

        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionOrderNumbers");


            t_circle = content.Load<Texture2D>("order_number_circle");

            game_state = GAME_STATE.GAME_TUTORIAL;

            b_one = new ButtonGeneral(new Rectangle(120, 250, 160, 160));
            b_two = new ButtonGeneral(new Rectangle(320, 250, 160, 160));
            b_three = new ButtonGeneral(new Rectangle(520, 250, 160, 160));

            cooldown = 0f;
            cd_bonus = 0f;

            percent = 0;
            var_one = 0;
            var_two = 0;

            difficulty = 5;

            circle_one = "";
            circle_two = "";
            circle_three = "";

            true_id = 255;

            right = false; ;

            game_scene.AddTimer(1, 0);

            CreateRound();
        }

        public override void Unload()
        {
            if (content != null)
                content.Unload();

            t_circle = null;

            b_one = null;
            b_two = null;
            b_three = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_one.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_two.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if(b_three.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                b_one.Collide(p);
                b_two.Collide(p);
                b_three.Collide(p);
            }            
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                if(b_one.Collide(p))
                {
                    press = true;

                    if (true_id == 0)
                        right = true;
                    else
                        right = false;
                }
                if(b_two.Collide(p))
                {
                    press = true;

                    if (true_id == 1)
                        right = true;
                    else
                        right = false;
                }
                if(b_three.Collide(p))
                {
                    press = true;

                    if (true_id == 2)
                        right = true;
                    else
                        right = false;
                }


                if (press)
                {
                    game_state = GAME_STATE.GAME_SHOW_RESULT;

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

            b_one.Pressed = false;
            b_two.Pressed = false;
            b_three.Pressed = false;
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(-50, 100, 900, 50);

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            Vector2 pos = new Vector2(380f, 105f);

            sp.DrawString(font, "" + percent + " %", pos, Color.Black);

            if (b_one.Pressed)
                b_one.Draw(sp, t_circle, Color.DarkGray);
            else
                b_one.Draw(sp, t_circle, Color.White);

            if (b_two.Pressed)
                b_two.Draw(sp, t_circle, Color.DarkGray);
            else
                b_two.Draw(sp, t_circle, Color.White);

            if (b_three.Pressed)
                b_three.Draw(sp, t_circle, Color.DarkGray);
            else
                b_three.Draw(sp, t_circle, Color.White);

            pos.X = b_one.X + 20f;
            pos.Y = b_one.Y + 60f;

            sp.DrawString(font, circle_one, pos, Color.Black);

            pos.X = b_two.X + 20f;

            sp.DrawString(font, circle_two, pos, Color.Black);

            pos.X = b_three.X + 20f;

            sp.DrawString(font, circle_three, pos, Color.Black);




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
                game_state = GAME_STATE.GAME_PLAY;
                CreateRound();

                cooldown = 0f;
                cd_bonus = 0f;
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                cd_bonus += dt;
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                cooldown += dt;

                if (cooldown > 0.4f)
                {
                    game_state = GAME_STATE.GAME_CREATE;

                    cooldown = 0f;
                }
            }            
        }

        public override string Description()
        {
            return "Select one of three circles which correspond to\npercentage displayed in the middle.";
        }

        private void CreateRound()
        {
            SetExpression();

            byte id = (byte)(Utility.Random(0f, 2f));

            int false_x = var_one + (int)(Utility.Random(2f, difficulty * 0.5f));
            int false_y = var_two + (int)(Utility.Random(2f, difficulty));

            circle_one = "" + false_x + " / " + false_y;

            false_x = var_one + (int)(Utility.Random(2f, difficulty * 0.5f));
            false_y = var_two + (int)(Utility.Random(2f, difficulty));

            circle_two = "" + false_x + " / " + false_y;

            false_x = var_one + (int)(Utility.Random(2f, difficulty * 0.5f));
            false_y = var_two + (int)(Utility.Random(2f, difficulty));

            circle_three = "" + false_x + " / " + false_y;


            if (id == 0)
            {
                circle_one = "" + var_one + " / " + var_two;
            }
            else if (id == 1)
            {
                circle_two = "" + var_one + " / " + var_two;
            }
            else
            {
                circle_three = "" + var_one + " / " + var_two;
            }

            true_id = id;

        }

        private void SetExpression()
        {
            int type = (int)(Utility.Random(0f, 54));



            switch (type)
            {
                case 0:
                    percent = 4;
                    var_one = 1;
                    var_two = 25;
                    break;

                case 1:
                    percent = 5;
                    var_one = 1;
                    var_two = 20;
                    break;

                case 2:
                    percent = 8;
                    var_one = 2;
                    var_two = 25;
                    break;

                case 3:
                    percent = 10;
                    var_one = 1;
                    var_two = 10;
                    break;

                case 4:
                    percent = 12;
                    var_one = 3;
                    var_two = 25;
                    break;

                case 5:
                    percent = 14;
                    var_one = 7;
                    var_two = 50;
                    break;

                case 6:
                    percent = 15;
                    var_one = 3;
                    var_two = 20;
                    break;

                case 7:
                    percent = 16;
                    var_one = 4;
                    var_two = 25;
                    break;

                case 8:
                    percent = 20;
                    var_one = 1;
                    var_two = 5;
                    break;

                case 9:
                    percent = 22;
                    var_one = 11;
                    var_two = 50;
                    break;

                case 10:
                    percent = 24;
                    var_one = 6;
                    var_two = 25;
                    break;

                case 11:
                    percent = 25;
                    var_one = 1;
                    var_two = 4;
                    break;

                case 12:
                    percent = 26;
                    var_one = 13;
                    var_two = 50;
                    break;

                case 13:
                    percent = 28;
                    var_one = 7;
                    var_two = 25;
                    break;

                case 14:
                    percent = 30;
                    var_one = 3;
                    var_two = 10;
                    break;

                case 15:
                    percent = 32;
                    var_one = 8;
                    var_two = 25;
                    break;

                case 16:
                    percent = 34;
                    var_one = 17;
                    var_two = 50;
                    break;

                case 17:
                    percent = 35;
                    var_one = 7;
                    var_two = 20;
                    break;

                case 18:
                    percent = 36;
                    var_one = 9;
                    var_two = 25;
                    break;

                case 19:
                    percent = 38;
                    var_one = 19;
                    var_two = 30;
                    break;

                case 20:
                    percent = 40;
                    var_one = 2;
                    var_two = 5;
                    break;

                case 21:
                    percent = 42;
                    var_one = 21;
                    var_two = 50;
                    break;

                case 22:
                    percent = 44;
                    var_one = 11;
                    var_two = 25;
                    break;

                case 23:
                    percent = 45;
                    var_one = 9;
                    var_two = 20;
                    break;

                case 24:
                    percent = 46;
                    var_one = 23;
                    var_two = 50;
                    break;

                case 25:
                    percent = 48;
                    var_one = 12;
                    var_two = 25;
                    break;

                case 26:
                    percent = 50;
                    var_one = 1;
                    var_two = 2;
                    break;

                case 27:
                    percent = 52;
                    var_one = 13;
                    var_two = 25;
                    break;

                case 28:
                    percent = 54;
                    var_one = 27;
                    var_two = 50;
                    break;

                case 29:
                    percent = 55;
                    var_one = 11;
                    var_two = 20;
                    break;

                case 30:
                    percent = 56;
                    var_one = 14;
                    var_two = 25;
                    break;

                case 31:
                    percent = 58;
                    var_one = 29;
                    var_two = 50;
                    break;

                case 32:
                    percent = 60;
                    var_one = 3;
                    var_two = 5;
                    break;

                case 33:
                    percent = 62;
                    var_one = 31;
                    var_two = 50;
                    break;

                case 34:
                    percent = 64;
                    var_one = 16;
                    var_two = 25;
                    break;

                case 35:
                    percent = 65;
                    var_one = 13;
                    var_two = 20;
                    break;

                case 36:
                    percent = 68;
                    var_one = 17;
                    var_two = 25;
                    break;

                case 37:
                    percent = 70;
                    var_one = 7;
                    var_two = 10;
                    break;

                case 38:
                    percent = 72;
                    var_one = 18;
                    var_two = 25;
                    break;

                case 39:
                    percent = 74;
                    var_one = 37;
                    var_two = 50;
                    break;

                case 40:
                    percent = 75;
                    var_one = 3;
                    var_two = 4;
                    break;

                case 41:
                    percent = 76;
                    var_one = 19;
                    var_two = 25;
                    break;

                case 42:
                    percent = 78;
                    var_one = 39;
                    var_two = 50;
                    break;

                case 43:
                    percent = 80;
                    var_one = 4;
                    var_two = 5;
                    break;

                case 44:
                    percent = 82;
                    var_one = 41;
                    var_two = 50;
                    break;

                case 45:
                    percent = 84;
                    var_one = 21;
                    var_two = 25;
                    break;

                case 46:
                    percent = 85;
                    var_one = 17;
                    var_two = 20;
                    break;

                case 47:
                    percent = 86;
                    var_one = 43;
                    var_two = 50;
                    break;

                case 48:
                    percent = 88;
                    var_one = 22;
                    var_two = 25;
                    break;

                case 49:
                    percent = 90;
                    var_one = 9;
                    var_two = 10;
                    break;

                case 50:
                    percent = 92;
                    var_one = 23;
                    var_two = 25;
                    break;

                case 51:
                    percent = 95;
                    var_one = 19;
                    var_two = 20;
                    break;

                case 52:
                    percent = 96;
                    var_one = 24;
                    var_two = 25;
                    break;

                case 53:
                    percent = 98;
                    var_one = 49;
                    var_two = 50;
                    break;

                case 54:
                    percent = 100;
                    var_one = 1;
                    var_two = 1;
                    break;

                default:
                    percent = 100;
                    var_one = 1;
                    var_two = 1;
                    break;
            }

        }

    }
}
