using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameCalculate : GameInterface
    {

        enum SIGN
        {
            ADD = 0,
            SUBSTRACT,
            DIVIDE,
            MULTIPLY,
        }


        ButtonGeneral [] b_numbers;
        ButtonGeneral b_check;
        ButtonGeneral b_cancel;


        Texture2D[] texture_numbers;

        SIGN sign;

        string input;
        string expression;

        float cooldown;

        float bonus_state;

        int value_one;
        int value_two;

        byte answered;
        byte cap;

        byte difficulty;

        bool right;

        public GameCalculate(GameScene game_scene)
        {
            this.game_scene = game_scene;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/SolveCalculate");

            texture_numbers = new Texture2D[10];

            texture_numbers[0] = content.Load<Texture2D>("zero");
            texture_numbers[1] = content.Load<Texture2D>("one");
            texture_numbers[2] = content.Load<Texture2D>("two");
            texture_numbers[3] = content.Load<Texture2D>("three");
            texture_numbers[4] = content.Load<Texture2D>("four");

            texture_numbers[5] = content.Load<Texture2D>("five");
            texture_numbers[6] = content.Load<Texture2D>("six");
            texture_numbers[7] = content.Load<Texture2D>("seven");
            texture_numbers[8] = content.Load<Texture2D>("eight");
            texture_numbers[9] = content.Load<Texture2D>("nine");

            b_numbers = new ButtonGeneral[10];

            int _tsize = 100;
            int _step = _tsize + 10;

            b_numbers[0] = new ButtonGeneral(new Rectangle(440, 25, _tsize, _tsize));
            b_numbers[1] = new ButtonGeneral(new Rectangle(440 + _step, 25, _tsize, _tsize));
            b_numbers[2] = new ButtonGeneral(new Rectangle(440 + 2 * _step, 25, _tsize, _tsize));

            b_numbers[3] = new ButtonGeneral(new Rectangle(440, 25 + _step, _tsize, _tsize));
            b_numbers[4] = new ButtonGeneral(new Rectangle(440 + _step, 25 + _step, _tsize, _tsize));
            b_numbers[5] = new ButtonGeneral(new Rectangle(440 + 2 * _step, 25 + _step, _tsize, _tsize));

            b_numbers[6] = new ButtonGeneral(new Rectangle(440, 25 + 2 * _step, _tsize, _tsize));
            b_numbers[7] = new ButtonGeneral(new Rectangle(440 + _step, 25 + 2 * _step, _tsize, _tsize));
            b_numbers[8] = new ButtonGeneral(new Rectangle(440 + 2 * _step, 25 + 2 * _step, _tsize, _tsize));

            b_numbers[9] = new ButtonGeneral(new Rectangle(440 + _step, 25 + 3 * _step, _tsize, _tsize));

            b_check = new ButtonGeneral(new Rectangle(440, 25 + 3 * _step, _tsize, _tsize));
            b_cancel = new ButtonGeneral(new Rectangle(440 + 2 * _step, 24 + 3 * _step, _tsize, _tsize));


            _stat_right = 0f;
            _stat_wrong = 0f;

            bonus_state = 0f;

            cooldown = 1f;

            input = "";
            
            score = 0;
            answered = 0;
            cap = 30;
            difficulty = 1;
            right_count = 0;
            right_count_row = 0;

            right = false;

            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateExpression();

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            for (byte i = 0; i < 10; i++)
            {
                b_numbers[i] = null;
                texture_numbers[i] = null;
            }
            b_numbers = null;
            b_check = null;
            b_cancel = null;
            texture_numbers = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {

                for (byte i = 0; i < 9; i++)
                {
                    if (b_numbers[i].Collide(p))
                    {
                        game_scene.Manager.PlayPress();
                    }
                }

                if (b_numbers[9].Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }

                if (b_cancel.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                else if (b_check.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }

            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {

                for (byte i = 0; i < 9; i++)
                {
                    if (b_numbers[i].Collide(p))
                    {

                    }
                }

                if (b_numbers[9].Collide(p))
                {

                }

                if (b_cancel.Collide(p))
                {

                }
                else if (b_check.Collide(p))
                {

                }

            }            
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {

                for (byte i = 0; i < 9; i++)
                {
                    if (b_numbers[i].Collide(p) && input.Length < 6)
                    {
                        input += "" + (int)(i + 1);
                    }

                    b_numbers[i].Pressed = false;
                }

                if (b_numbers[9].Collide(p))
                {
                    if (input != "" && input.Length < 6)
                    {
                        input += "0";
                    }
                    b_numbers[9].Pressed = false;
                }

                if (b_cancel.Collide(p))
                {
                    input = "";
                    
                }
                else if (b_check.Collide(p))
                {
                    
                    CheckAnswer();
                }

                b_cancel.Pressed = false;
                b_check.Pressed = false;

            }

        }

        public override void Update(float dt)
        {
            if (game_state == GAME_STATE.GAME_CREATE)
            {
                CreateExpression();
                game_state = GAME_STATE.GAME_PLAY;

                bonus_state = 0f;
            }
            else if(game_state == GAME_STATE.GAME_TUTORIAL)
            {

            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                bonus_state += dt;
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                cooldown += dt * 3f;

                if (cooldown > 1f)
                {

                    if (answered >= cap)
                    {
                        game_scene.ExitGame();
                    }
                    else
                    {
                        input = "";

                        game_state = GAME_STATE.GAME_CREATE;
                    }
                }
            }


        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if(game_state == GAME_STATE.GAME_TUTORIAL || game_state == GAME_STATE.GAME_PLAY)
            {

            }



            DrawNumPad(sp, font);

            Rectangle b = new Rectangle(0, 380, 800, 100);

            Vector2 wh_pos = new Vector2(670f, 340f);
            b = new Rectangle(50, 200, 320, 80);

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            b.X = 670;
            b.Y = 340;

            b.Width = 256;
            b.Height = 64;

            if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                Rectangle __b = new Rectangle(272, 50, 256, 256);

                if(right)
                    sp.Draw(game_scene.Manager.TTrue, __b, Color.White);
                else
                    sp.Draw(game_scene.Manager.TFalse, __b, Color.White);
            }
            else
            {

            }


            b = new Rectangle(45, 208, 256, 64);

            string whole = expression + " = " + input;

            wh_pos = new Vector2(70, 214);

            sp.DrawString(font, whole, wh_pos, Color.White);

        }

        public override string Description()
        {
            return "Calculate expression by using numpad.\nCanceling the game will count as skipped and \ncannot be replayed for this day.";
        }

        private void DrawButton(SpriteBatch sp, Rectangle b, Color c)
        {
            sp.Draw(game_scene.Manager.TTile, b, c);
        }

        private void DrawNumPad(SpriteBatch sp, SpriteFont font)
        {
            for (int i = 0; i < 9; i++)
            {
                if (b_numbers[i].Pressed)
                {
                    b_numbers[i].Draw(sp, game_scene.Manager.TTile, Color.DarkGray);
                }
                else
                {
                    b_numbers[i].Draw(sp, game_scene.Manager.TTile, Color.White);
                }

                sp.Draw(texture_numbers[i + 1], b_numbers[i].Box, Color.White);
            }

            if (b_numbers[9].Pressed)
            {
                b_numbers[9].Draw(sp, game_scene.Manager.TTile, Color.DarkGray);
            }
            else
            {
                b_numbers[9].Draw(sp, game_scene.Manager.TTile, Color.White);
            }

            sp.Draw(texture_numbers[0], b_numbers[9].Box, Color.White);



            if (b_cancel.Pressed)
            {
                b_cancel.Draw(sp, game_scene.Manager.TCancel, Color.DarkGray);
            }
            else
            {
                b_cancel.Draw(sp, game_scene.Manager.TCancel, Color.White);
            }

            if (b_check.Pressed)
            {
                b_check.Draw(sp, game_scene.Manager.TAccept, Color.DarkGray);
            }
            else
            {
                b_check.Draw(sp, game_scene.Manager.TAccept, Color.White);
            }



        }

        private void CreateExpression()
        {
            byte s = (byte)(Utility.Random(0f, 4f));

            if (s == 0)
            {
                value_one = (int)(Utility.Random(3 + difficulty, 7 + difficulty));
                value_two = (int)(Utility.Random(3 + difficulty, 7 + difficulty));

                sign = SIGN.MULTIPLY;

                expression = "" + value_one + "*" + value_two;
            }
            else if (s == 1)
            {
                value_one = (int)(Utility.Random(2f + difficulty, 5 + difficulty));
                value_two = (int)(Utility.Random(2f + difficulty, 5 + difficulty));

                value_one = value_one * value_two;

                sign = SIGN.DIVIDE;

                expression = "" + value_one + "/" + value_two;
            }
            else if (s == 2)
            {
                value_one = (int)(Utility.Random(3f + difficulty * 2, 5 + difficulty * 10));
                value_two = (int)(Utility.Random(3f + difficulty * 2, value_one - 2));

                sign = SIGN.SUBSTRACT;

                expression = "" + value_one + "-" + value_two;
            }
            else
            {
                value_one = (int)(Utility.Random(3f + difficulty * 3, difficulty * 7));
                value_two = (int)(Utility.Random(3f + difficulty * 3, difficulty * 7));

                sign = SIGN.ADD;

                expression = "" + value_one + "+" + value_two;
            }

        }

        private void CheckAnswer()
        {
            bool bonus = false;

            if (right)
                bonus = true;


            game_state = GAME_STATE.GAME_SHOW_RESULT;
            cooldown = 0f;

            int test = -1;

            int.TryParse(input, out test);

            switch (sign)
            {
                case SIGN.MULTIPLY:

                    if (value_one * value_two == test)
                        right = true;
                    else
                        right = false;
                    break;

                case SIGN.DIVIDE:
                    if (value_one / value_two == test)
                        right = true;
                    else
                        right = false;
                    break;

                case SIGN.SUBSTRACT:
                    if (value_one - value_two == test)
                        right = true;
                    else
                        right = false;
                    break;

                case SIGN.ADD:
                    if (value_one + value_two == test)
                        right = true;
                    else
                        right = false;
                    break;

            }

            answered += 1;

            if (right)
            {
                right_count += 1;

                if (right_count > 2)
                {
                    right_count = 0;
                    if (difficulty < 8)
                        difficulty += 1;
                }

                if (score < difficulty)
                {
                    score = difficulty;
                }

                _stat_right += 1f;
            }
            else
            {
                right_count = 0;
                _stat_wrong += 1f;
            }


            if (bonus && right)
            {
                if (bonus_state < 4.0f)
                    _stat_right += 2f;
            }


        }
    }
}
