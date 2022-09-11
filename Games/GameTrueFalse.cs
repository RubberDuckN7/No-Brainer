using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameTrueFalse : GameInterface
    {

        ButtonGeneral b_true;
        ButtonGeneral b_false;

        float cooldown;
        float cd_bonus;

        string expression;
        bool is_true;

        bool right;

        public GameTrueFalse(GameScene game)
        {
            game_scene = game;

            b_true = new ButtonGeneral(new Rectangle(190, 360, 200, 100));
            b_false = new ButtonGeneral(new Rectangle(410, 360, 200, 100));
        }

        public override void Load(Game game)
        {
            game_state = GAME_STATE.GAME_TUTORIAL;

            cooldown = 0f;
            cd_bonus = 0f;

            expression = "";

            is_true = false;
            right = false;

            CreateRound();

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            b_true = null;
            b_false = null;
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if (b_true.Pressed)
                b_true.Draw(sp, game_scene.Manager.TLeftTrue, Color.DarkGray);
            else
                b_true.Draw(sp, game_scene.Manager.TLeftTrue, Color.White);

            Vector2 pos = new Vector2();


            pos.X = b_true.X + 40;
            pos.Y = b_true.Y + 20;

            sp.DrawString(font, "True", pos, Color.White);

            if (b_false.Pressed)
                b_false.Draw(sp, game_scene.Manager.TRightFalse, Color.DarkGray);
            else
                b_false.Draw(sp, game_scene.Manager.TRightFalse, Color.White);

            pos.X = b_false.X + 40;
            pos.Y = b_false.Y + 20;

            sp.DrawString(font, "False", pos, Color.White);

            Rectangle b = new Rectangle();

            b.X = -50;
            b.Y = 150;

            b.Width = 950;
            b.Height = 100;

            b = new Rectangle(200, 150, 400, 100);

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            pos = new Vector2(300f, 180f);

            sp.DrawString(font, expression, pos, Color.Black);

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

        public override void Pressed(Vector2 p)
        {
            b_true.Pressed = false;
            b_false.Pressed = false;
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if(b_true.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_false.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            b_true.Pressed = false;
            b_false.Pressed = false;
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                b_true.Collide(p);
                b_false.Collide(p);
            }            
        }

        public override void Released(Vector2 p)
        {

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                if(b_true.Collide(p))
                {
                    press = true;

                    if (is_true)
                        right = true;
                    else
                        right = false;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;

                    
                }
                if (b_false.Collide(p))
                {
                    press = true;

                    if (is_true)
                        right = false;
                    else
                        right = true;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;
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

            b_true.Pressed = false;
            b_false.Pressed = false;

        }

        public override string Description()
        {
            return "Decide if the expression is true or false.";
        }

        private void CreateRound()
        {
            expression = "something vs something = this :D";

            int type = (int)(Utility.Random(0f, 4f));

            if (type == 0)
            {
                expression = CreateDevision();
            }
            else if (type == 1)
            {
                expression = CreateMultiply();
            }
            else if (type == 2)
            {
                expression = CreateAddition();
            }
            else
            {
                expression = Createsubstraction();
            }

            expression = Createsubstraction();
        }

        private string CreateDevision()
        {
            int nr1 = (int)(Utility.Random(8, 20));
            int nr2 = (int)(Utility.Random(5, 15));

            int d1 = nr1;
            int d2 = nr1 * nr2;

            is_true = true;

            if (Utility.RandomBool())
            {
                is_true = false;
                if(Utility.RandomBool())
                {
                    nr2 -= (int)(Utility.Random(2f, nr2 * 0.2f));
                }
                else
                {
                    nr2 += (int)(Utility.Random(2f, nr2 * 0.2f));
                }
            }

            return "" + d2 + " / " + d1 + " = " + nr2;
        }

        private string CreateMultiply()
        {
            int nr1 = (int)(Utility.Random(8, 20));
            int nr2 = (int)(Utility.Random(5, 15));

            int ans = nr1 * nr2;

            is_true = true;

            if (Utility.RandomBool())
            {
                is_true = false;
                if (Utility.RandomBool())
                {
                    ans -= (int)(Utility.Random(2f, 30f));
                }
                else
                {
                    ans += (int)(Utility.Random(2f, 30f));
                }
            }


            return "" + nr1 + " * " + nr2 + " = " + ans;
        }

        private string CreateAddition()
        {
            int nr1 = (int)(Utility.Random(15, 60));
            int nr2 = (int)(Utility.Random(10, 55));

            int ans = nr1 + nr2;

            is_true = true;

            if (Utility.RandomBool())
            {
                is_true = false;
                if (Utility.RandomBool())
                {
                    ans -= (int)(Utility.Random(2f, 30f));
                }
                else
                {
                    ans += (int)(Utility.Random(2f, 30f));
                }
            }


            return "" + nr1 + " + " + nr2 + " = " + ans;
        }

        private string Createsubstraction()
        {
            int nr1 = (int)(Utility.Random(15, 60));
            int nr2 = (int)(Utility.Random(10, 55));

            int ans = nr1 - nr2;

            is_true = true;

            if (Utility.RandomBool())
            {
                is_true = false;
                if (Utility.RandomBool())
                {
                    ans -= (int)(Utility.Random(2f, 30f));
                }
                else
                {
                    ans += (int)(Utility.Random(2f, 30f));
                }
            }

            return "" + nr1 + " - " + nr2 + " = " + ans;
        }
    }
}
