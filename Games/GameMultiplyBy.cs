using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameMultiplyBy : GameInterface
    {
        Texture2D t_circle;

        ButtonGeneral[] b_numbers;

        int[] numbers;

        int true_id;

        float cooldown;
        float cd_bonus;

        string expression;

        bool right;

        public GameMultiplyBy(GameScene game)
        {
            game_scene = game;
        }


        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionOrderNumbers");

            t_circle = content.Load<Texture2D>("order_number_circle");


            b_numbers = new ButtonGeneral[4];

            b_numbers[0] = new ButtonGeneral(new Rectangle(100, 180, 90, 90));
            b_numbers[1] = new ButtonGeneral(new Rectangle(200, 320, 90, 90));
            b_numbers[2] = new ButtonGeneral(new Rectangle(400, 200, 90, 90));
            b_numbers[3] = new ButtonGeneral(new Rectangle(600, 190, 90, 90));

            numbers = new int[4];

            game_state = GAME_STATE.GAME_TUTORIAL;

            game_scene.AddTimer(1, 0);

            true_id = 100;

            cooldown = 0f;
            cd_bonus = 0f;

            expression = "";

            right = false;

            CreateRound();
        }

        public override void Unload()
        {
            if (content != null)
                content.Unload();

            t_circle = null;

            b_numbers[0] = null;
            b_numbers[1] = null;
            b_numbers[2] = null;
            b_numbers[3] = null;

            b_numbers = null;

            numbers = null;
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

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(-50, 100, 900, 50);

            sp.Draw(game_scene.Manager.TInputRectangle, b, Color.White);

            Vector2 pos = new Vector2(270f, 105f);

            sp.DrawString(font, expression, pos, Color.Black);

            pos.Y = b_numbers[0].Y + 60f;

            for (byte i = 0; i < 4; i++)
            {
                if (b_numbers[i].Pressed)
                    b_numbers[i].Draw(sp, t_circle, Color.DarkGray);
                else
                    b_numbers[i].Draw(sp, t_circle, Color.White);

                
                pos.X = b_numbers[i].X + 18f;
                pos.Y = b_numbers[i].Y + 20f;

                sp.DrawString(font, "" + numbers[i], pos, Color.Black);

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

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < 4; i++)
                {
                    if (b_numbers[i].Collide(p))
                    {
                        game_scene.Manager.PlayPress();
                    }
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                for (byte i = 0; i < 4; i++)
                {
                    b_numbers[i].Collide(p);
                }
            }        
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                for (byte i = 0; i < 4; i++)
                {
                    if(b_numbers[i].Collide(p))
                    {
                        press = true;

                        if (true_id == i)
                            right = true;
                        else
                            right = false;
                    }
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

            b_numbers[0].Pressed = false;
            b_numbers[1].Pressed = false;
            b_numbers[2].Pressed = false;
            b_numbers[3].Pressed = false;
        }

        public override string Description()
        {
            return "Select one of the numbers in circles to\ncomplete equation.";
        }


        private void CreateRound()
        {
            int x = (int)(Utility.Random(3f, 12f));
            int y = (int)(Utility.Random(3f, 12f));

            int id = (int)(Utility.Random(0f, 3f));

            expression = "" + x + " * __ = " + x * y;


            for (byte i = 0; i < 4; i++)
            {
                numbers[i] = y + (int)(Utility.Random(1f, 7f));

                if (id == i)
                {
                    true_id = i;
                    numbers[i] = y;
                }
            }

        }

    }
}
