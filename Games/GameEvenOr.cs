using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameEvenOr : GameInterface
    {
        enum TYPE_EXPRESSION
        {
            VOWEL = 0,
            CONSONANT,
            EVEN,
            ODD,
        }

        SpriteFont font_big;

        ButtonGeneral b_true;
        ButtonGeneral b_false;

        // A, E, I, O, U
        string[] vowels;

        // B, C, D, F, G, H, J, K, L, M, N, P, Q, R, S, T, V, W, X
        string[] consonants;     

        TYPE_EXPRESSION type_expression;
        TYPE_EXPRESSION answer;

        float cooldown;

        float cd_bonus;

        byte[] even;
        byte[] odd;

        byte id_letter;
        byte id_number;

        bool right;

        bool use_vowel;
        bool use_even;

        bool side;

        public GameEvenOr(GameScene game)
        {
            game_scene = game;
        }


        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/SpeedEvenOr");

            font_big = content.Load<SpriteFont>("BigFont");

            vowels = new string[] { "A", "E", "I", "O", "U" };
            consonants = new string[] { "B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X" };

            even = new byte[] { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 27, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48, 50 };
            odd = new byte[] { 3, 5, 7, 9, 11, 13, 15, 17, 19, 21, 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53 };

            b_true = new ButtonGeneral(new Rectangle(190, 360, 200, 100));
            b_false = new ButtonGeneral(new Rectangle(410, 360, 200, 100));

            cooldown = 0f;
            cd_bonus = 0f;

            game_state = GAME_STATE.GAME_TUTORIAL;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            font_big = null;

            b_true = null;
            b_false = null;

            vowels = null;
            consonants = null;

            even = null;
            odd = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_true.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                else if (b_false.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (game_state == GAME_STATE.GAME_PLAY)
                {
                    if (b_true.Collide(p))
                    {
                        
                    }
                    else if (b_false.Collide(p))
                    {
                        
                    }
                }
            }
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (game_state == GAME_STATE.GAME_PLAY)
                {
                    if (b_true.Collide(p))
                    {
                        Evaluate(true);

                        game_state = GAME_STATE.GAME_SHOW_RESULT;

                        cooldown = 0f;

                        b_true.Pressed = false;
                    }
                    else if (b_false.Collide(p))
                    {
                        Evaluate(false);

                        game_state = GAME_STATE.GAME_SHOW_RESULT;

                        cooldown = 0f;

                        b_false.Pressed = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(120, 40, 250, 250);

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);

            b.X = 430;

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);

            Vector2 pos = new Vector2();

            pos.X = 150f;
            pos.Y = 70;

            if (side)
            {
                if (use_vowel)
                    sp.DrawString(font_big, vowels[id_letter], pos, Color.White);
                else
                    sp.DrawString(font_big, consonants[id_letter], pos, Color.White);

                pos.X = 460;

                if (use_even)
                    sp.DrawString(font_big, "" + even[id_number], pos, Color.White);
                else
                    sp.DrawString(font_big, "" + odd[id_number], pos, Color.White);
            }
            else
            {
                if (use_even)
                    sp.DrawString(font_big, "" + even[id_number], pos, Color.White);
                else
                    sp.DrawString(font_big, "" + odd[id_number], pos, Color.White);

                pos.X = 460;

                if (use_vowel)
                    sp.DrawString(font_big, vowels[id_letter], pos, Color.White);
                else
                    sp.DrawString(font_big, consonants[id_letter], pos, Color.White);

            }


            if (b_true.Pressed)
                b_true.Draw(sp, game_scene.Manager.TLeftTrue, Color.DarkGray);
            else
                b_true.Draw(sp, game_scene.Manager.TLeftTrue, Color.White);

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

            b.X = -50;
            b.Y = 300;

            b.Width = 950;
            b.Height = 50;

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Vector2 message = new Vector2(300f, 300f);

                if (type_expression == TYPE_EXPRESSION.CONSONANT)
                {
                    sp.DrawString(font, "Is consonant?", message, Color.Black);
                }
                else if (type_expression == TYPE_EXPRESSION.VOWEL)
                {
                    sp.DrawString(font, "Is vowel?", message, Color.Black);
                }
                else if (type_expression == TYPE_EXPRESSION.EVEN)
                {
                    sp.DrawString(font, "Is even?", message, Color.Black);
                }
                else if (type_expression == TYPE_EXPRESSION.ODD)
                {
                    sp.DrawString(font, "Is odd?", message, Color.Black);
                }
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
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

                cooldown = 0f;
                cd_bonus = 0f;
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (cd_bonus > 2f)
                {

                }

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

        private void Evaluate(bool state)
        {
            if (state)
            {
                switch (type_expression)
                {
                    case TYPE_EXPRESSION.CONSONANT:

                        if (use_vowel)
                            right = false;
                        else
                            right = true;

                        break;
                    case TYPE_EXPRESSION.EVEN:

                        if (use_even)
                            right = true;
                        else
                            right = false;

                        break;
                    case TYPE_EXPRESSION.ODD:

                        if (use_even)
                            right = false;
                        else
                            right = true;

                        break;
                    case TYPE_EXPRESSION.VOWEL:

                        if (use_vowel)
                            right = true;
                        else
                            right = false;

                        break;

                    default:
                        right = false;
                        break;
                }
            }
            else
            {
                switch (type_expression)
                {
                    case TYPE_EXPRESSION.CONSONANT:

                        if (use_vowel)
                            right = true;
                        else
                            right = false;

                        break;
                    case TYPE_EXPRESSION.EVEN:

                        if (use_even)
                            right = false;
                        else
                            right = true;

                        break;
                    case TYPE_EXPRESSION.ODD:

                        if (use_even)
                            right = true;
                        else
                            right = false;

                        break;
                    case TYPE_EXPRESSION.VOWEL:

                        if (use_vowel)
                            right = false;
                        else
                            right = true;

                        break;

                    default:
                        right = false;
                        break;
                }
            }


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

        public override string Description()
        {
            return "Press true or false, depending on the \nexpression displayed. Answer as fast as you can.";
        }

        private void CreateRound()
        {
            byte type = 255;

            type = (byte)(Utility.Random(0f, 4f));

            if (type == 0)
            {
                type_expression = TYPE_EXPRESSION.CONSONANT;
            }
            else if (type == 1)
            {
                type_expression = TYPE_EXPRESSION.EVEN;
            }
            else if (type == 2)
            {
                type_expression = TYPE_EXPRESSION.ODD;
            }
            else if (type == 3)
            {
                type_expression = TYPE_EXPRESSION.VOWEL;
            }
            else
            {
                type_expression = TYPE_EXPRESSION.VOWEL;
            }

            if (Utility.Random(0f, 100f) < 50f)
            {
                use_even = true;
            }
            else
            {
                use_even = false;
            }

            if (Utility.Random(0f, 100f) < 50f)
            {
                use_vowel = true;
            }
            else
            {
                use_vowel = false;
            }

            if(use_vowel)
                id_letter = (byte)(Utility.Random(0f, 5f));
            else
                id_letter = (byte)(Utility.Random(0f, 19f));

            id_number = (byte)(Utility.Random(0f, 26f));

            if (Utility.Random(0f, 100f) < 50f)
                side = true;
        }
    }
}
