using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameColorMatch : GameInterface
    {


        enum TYPE_QUESTION
        {
            SAME_OUTER_COLOR = 0,
            SAME_INNER_COLOR,
            DIFFERENT_OUTER_COLOR,
            DIFFERENT_INNER_COLOR,
        };


        Texture2D[] texture_figures;

        ButtonGeneral b_true;
        ButtonGeneral b_false;

        Color[] c_left;
        Color[] c_right;

        TYPE_QUESTION type_question;

        float cooldown;

        float cd_bonus;

        byte shape_one_in;
        byte shape_one_out;

        byte shape_two_in;
        byte shape_two_out;

        bool right_true;

        bool right;

        public GameColorMatch(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/SpeedMatchColor");

            texture_figures = new Texture2D[4];


            texture_figures[0] = content.Load<Texture2D>("shape_box");
            texture_figures[1] = content.Load<Texture2D>("shape_circle");
            texture_figures[2] = content.Load<Texture2D>("shape_star");
            texture_figures[3] = content.Load<Texture2D>("shape_triangle");

            c_left = new Color[2];
            c_right = new Color[2];

            b_true = new ButtonGeneral(new Rectangle(190, 360, 200, 100));
            b_false = new ButtonGeneral(new Rectangle(410, 360, 200, 100));


            game_state = GAME_STATE.GAME_TUTORIAL;

            cooldown = 0f;
            cd_bonus = 0f;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if (content != null)
                content.Unload();

            texture_figures[0] = null;
            texture_figures[1] = null;
            texture_figures[2] = null;
            texture_figures[3] = null;

            texture_figures = null;

            b_true = null;
            b_false = null;

            c_left = null;
            c_right = null;
        }

        public override void Pressed(Vector2 p)
        {
            b_true.Pressed = false;
            b_false.Pressed = false;

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
            b_true.Pressed = false;
            b_false.Pressed = false;

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

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                if (b_true.Collide(p))
                {
                    press = true;
                    b_true.Pressed = false;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;

                    cooldown = 0f;

                    if (right_true)
                        right = true;
                    else
                        right = false;
                }
                else if (b_false.Collide(p))
                {
                    press = true;
                    b_false.Pressed = false;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;

                    cooldown = 0f;

                    if (right_true)
                        right = false;
                    else
                        right = true;
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
            Rectangle b = new Rectangle(120, 40, 250, 250);

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);

            b.X = 430;

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);

            b.Width = 240;
            b.Height = 240;

            b.X = 125;
            b.Y = 45;

            sp.Draw(texture_figures[shape_one_out], b, c_left[0]);

            b.X = 435;
            sp.Draw(texture_figures[shape_two_out], b, c_right[0]);

            b.Width = 60;
            b.Height = 60;

            b.X = 215;
            b.Y = 135;

            sp.Draw(texture_figures[shape_one_in], b, c_left[1]);

            b.X = 526;
            sp.Draw(texture_figures[shape_two_in], b, c_right[1]);


            if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                Rectangle __b = new Rectangle(272, 50, 256, 256);

                if (right)
                    sp.Draw(game_scene.Manager.TTrue, __b, Color.White);
                else
                    sp.Draw(game_scene.Manager.TFalse, __b, Color.White);
            }

            b.X = -50;
            b.Y = 300;

            b.Width = 950;
            b.Height = 50;

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            Vector2 pos = new Vector2(100f, 300f);

            sp.DrawString(font, Message(), pos, Color.Black);

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

        private void CreateRound()
        {
            bool same_inner = false;
            bool same_outer = false;

            shape_one_in = (byte)(Utility.Random(0f, 4f));
            shape_one_out = (byte)(Utility.Random(0f, 4f));

            shape_two_in = (byte)(Utility.Random(0f, 4f));
            shape_two_out = (byte)(Utility.Random(0f, 4f));

            Color[] colors = new Color[8];

            colors[0] = Color.Green;
            colors[1] = Color.Yellow;
            colors[2] = Color.Red;
            colors[3] = Color.Orange;
            colors[4] = Color.Blue;
            colors[5] = Color.Brown;
            colors[6] = Color.BlueViolet;
            colors[7] = Color.Gray;

            for (byte i = 0; i < 8; i++)
            {
                Color swap = colors[i];
                int id = (int)(Utility.Random(0f, 8f));

                colors[i] = colors[id];
                colors[id] = swap;
            }


            int start = (int)(Utility.Random(0f, 4f));

            // Outer same or not
            if (Utility.Random(0f, 100f) < 50f)
            {
                c_left[0] = c_right[0] = colors[start];
                same_outer = true;
            }
            else
            {
                c_left[0] = colors[start];
                c_right[0] = colors[start + 1];
                same_outer = false;
            }

            start = (int)(Utility.Random(0f, 4f));
            // Inner same or not
            if (Utility.Random(0f, 100f) < 50f)
            {
                c_left[1] = c_right[1] = colors[start];
                same_inner = true;
            }
            else
            {
                c_left[1] = colors[start];
                c_right[1] = colors[start + 1];
                same_inner = false;
            }

            int __type = (int)(Utility.Random(0f, 4f));

            switch (__type)
            {
                case 0:

                    type_question = TYPE_QUESTION.SAME_OUTER_COLOR;

                    if (same_outer)
                        right_true = true;
                    else
                        right_true = false;

                    break;

                case 1:

                    type_question = TYPE_QUESTION.SAME_INNER_COLOR;

                    if (same_inner)
                        right_true = true;
                    else
                        right_true = false;

                    break;

                case 2:

                    type_question = TYPE_QUESTION.DIFFERENT_OUTER_COLOR;

                    if (same_outer)
                        right_true = false;
                    else
                        right_true = true;

                    break;

                case 3:

                    type_question = TYPE_QUESTION.DIFFERENT_INNER_COLOR;

                    if (same_inner)
                        right_true = false;
                    else
                        right_true = true;
                    
                    break;
            }

            switch (type_question)
            {
                case TYPE_QUESTION.SAME_OUTER_COLOR:

                    break;

                case TYPE_QUESTION.SAME_INNER_COLOR:

                    break;

                case TYPE_QUESTION.DIFFERENT_OUTER_COLOR:

                    break;

                case TYPE_QUESTION.DIFFERENT_INNER_COLOR:

                    break;
            }

        }

        public override string Description()
        {
            return "Press true or false, depending on\nexpression displayed below figures.";
        }

        private Color RandomColor()
        {
            Color c = Color.Black;

            int id = (int)(Utility.Random(0f, 7));

            switch (id)
            {
                case 0: return Color.Green;
                case 1: return Color.Yellow;
                case 2: return Color.Red;
                case 3: return Color.Orange;
                case 4: return Color.Blue;
                case 5: return Color.Brown;
                case 6: return Color.BlueViolet;
                case 7: return Color.Gray;
            }

            return c;
        }

        private string Message()
        {
            string msg = "";

            switch (type_question)
            {
                case TYPE_QUESTION.SAME_OUTER_COLOR:
                    msg = "Are outer figres same colored?";
                    break;
                case TYPE_QUESTION.SAME_INNER_COLOR:
                    msg = "Are inner figures same color?";
                    break;
                case TYPE_QUESTION.DIFFERENT_OUTER_COLOR:
                    msg = "Are outer figures different color?";
                    break;
                case TYPE_QUESTION.DIFFERENT_INNER_COLOR:
                    msg = "Are inner figures different color?";
                    break;
            }


            return msg;
        }

    }
}
