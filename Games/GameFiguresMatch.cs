using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameFiguresMatch : GameInterface
    {
        SpriteFont this_font;

        Texture2D[] textures;

        ButtonGeneral b_true;
        ButtonGeneral b_false;

        float cooldown;

        float cd_bonus;

        string char_one;
        string char_two;

        byte id_one;
        byte id_two;

        bool same;

        bool figure;

        bool right;

        public GameFiguresMatch(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/SpeedFiguresMatch");


            textures = new Texture2D[6];

            b_true = new ButtonGeneral(new Rectangle(190, 360, 200, 100));
            b_false = new ButtonGeneral(new Rectangle(410, 360, 200, 100));

            this_font = content.Load<SpriteFont>("FiguresMatchFont");

            textures[0] = content.Load<Texture2D>("shape_box");
            textures[1] = content.Load<Texture2D>("shape_circle");
            textures[2] = content.Load<Texture2D>("shape_kotte");

            textures[3] = content.Load<Texture2D>("shape_splash");
            textures[4] = content.Load<Texture2D>("shape_star");
            textures[5] = content.Load<Texture2D>("shape_triangle");

            cooldown = 0f;
            cd_bonus = 0f;

            same = false;
            figure = false;
            right = false;

            game_state = GAME_STATE.GAME_TUTORIAL;

            game_scene.AddTimer(1, 0);

            CreateRound();
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            this_font = null;

            textures[0] = null;
            textures[1] = null;
            textures[2] = null;
            textures[3] = null;
            textures[4] = null;
            textures[5] = null;

            textures = null;

            b_true = null;
            b_false = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_false.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_true.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_false.Collide(p))
                {

                }
                if (b_true.Collide(p))
                {
                    
                }
            }            
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;
                if (b_false.Collide(p))
                {
                    press = true;
                    if (same == false)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }

                    cooldown = 0f;
                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                }
                if (b_true.Collide(p))
                {
                    press = true;

                    if (same)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }

                    cooldown = 0f;
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

            b_false.Pressed = false;
            b_true.Pressed = false;
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

            sp.Draw(textures[id_one], b, Color.White);

            b.X = 435;
            sp.Draw(textures[id_two], b, Color.White);

            Vector2 pos_right = new Vector2(200, 70f);
            Vector2 pos_left = new Vector2(510f, 70f);


            sp.DrawString(this_font, "" + char_one, pos_left, Color.Black);
            sp.DrawString(this_font, "" + char_two, pos_right, Color.Black);

            string message = "";

            if (figure)
            {
                message = "Are figures the same?";
            }
            else
            {
                message = "Are Letters the same?";
            }

            b.X = -50;
            b.Y = 300;

            b.Width = 950;
            b.Height = 50;

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            Vector2 pos = new Vector2(150f, 300f);

            sp.DrawString(font, message, pos, Color.Black);

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

            if (game_state != GAME_STATE.GAME_SHOW_RESULT)
            {

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

        public override string Description()
        {
            return "Press true or false depending\non the statement.";
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

        private void CreateRound()
        {
            figure = true;


            if (Utility.Random(0f, 100f) < 50f)
                figure = false;

            figure = Utility.RandomBool();

            int id1 = (int)(Utility.Random(0f, 26));

            int id2 = GetNearNumber(id1);

            char_one = GenerateLetter(id1);
            char_two = GenerateLetter(id2);

            id_one = (byte)(Utility.Random(0f, 5f));
            id_two = (byte)(Utility.Random(0f, 5f));

            if (figure)
            {
                if (id_one == id_two)
                    same = true;
                else
                    same = false;
            }
            else
            {
                if (id1 == id2)
                    same = true;
                else
                    same = false;
            }
        }

        private int GetNearNumber(int id)
        {

            int res = (int)(Utility.Random(-3f, 3f)) + id;

            res = (int)(MathHelper.Clamp(res, 0f, 26f));

            return res;
        }

        private string GenerateLetter(int id)
        {
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

    }
}
