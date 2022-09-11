using System;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class GameSpeedCakes : GameInterface
    {
        Texture2D[] texture_cakes;

        byte[] id_cakes;
        byte count_cakes;


        float cooldown;

        byte selected_id;

        bool [] state_cake;

        bool right;

        public GameSpeedCakes(GameScene game)
        {
            game_scene = game;
        }


        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/SpeedCakes");


            texture_cakes = new Texture2D[9];

            state_cake = new bool[9];

            texture_cakes[0] = content.Load<Texture2D>("cake_one");
            texture_cakes[1] = content.Load<Texture2D>("cake_two");
            texture_cakes[2] = content.Load<Texture2D>("cake_three");

            texture_cakes[3] = content.Load<Texture2D>("cake_four");
            texture_cakes[4] = content.Load<Texture2D>("cake_five");
            texture_cakes[5] = content.Load<Texture2D>("cake_six");

            texture_cakes[6] = content.Load<Texture2D>("cake_seven");
            texture_cakes[7] = content.Load<Texture2D>("cake_eight");
            texture_cakes[8] = content.Load<Texture2D>("cake_nine");


            cooldown = 0f;

            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateRound();

            right = false;

            selected_id = 255;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            texture_cakes[0] = null;
            texture_cakes[1] = null;
            texture_cakes[2] = null;
            texture_cakes[3] = null;
            texture_cakes[4] = null;
            texture_cakes[5] = null;
            texture_cakes[6] = null;
            texture_cakes[7] = null;
            texture_cakes[8] = null;

            texture_cakes = null;

            id_cakes = null;
            state_cake = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(70, 200, 100, 200);

                for (byte i = 0; i < count_cakes; i++)
                {
                    if (state_cake[i] == false)
                    {
                        if (Utility.PointVsRectangle(b, p))
                        {
                            selected_id = i;

                            game_scene.Manager.PlayPress();
                        }
                    }

                    b.X += 110;
                } // for cakes
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(70, 200, 100, 200);

                selected_id = 255;

                for (byte i = 0; i < count_cakes; i++)
                {
                    if (state_cake[i] == false)
                    {
                        if (Utility.PointVsRectangle(b, p))
                        {

                            selected_id = i;
                        }
                    }

                    b.X += 110;
                } // for cakes
            }            
        }

        public override void Released(Vector2 p)
        {
            selected_id = 255;

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(70, 200, 100, 200);

                for (byte i = 0; i < count_cakes; i++)
                {
                    if (state_cake[i] == false)
                    {
                        if (Utility.PointVsRectangle(b, p))
                        {
                            byte lowest = 255;

                            byte count = 0;

                            for (byte c = 0; c < count_cakes; c++)
                            {

                                if (state_cake[c] == false)
                                {
                                    if (id_cakes[c] < lowest)
                                        lowest = id_cakes[c];

                                    count += 1;
                                }
                            }

                            if (lowest == id_cakes[i])
                            {
                                if (count <= 1)
                                {
                                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                                    cooldown = 0f;
                                    right = true;
                                }
                                else
                                {
                                    state_cake[i] = true;
                                }
                            }
                            else
                            {
                                game_state = GAME_STATE.GAME_SHOW_RESULT;
                                cooldown = 0f;
                                right = false;
                            }

                            if (right)
                            {
                                _stat_right += 2f * count_cakes * 0.4f;
                            }
                            else
                            {
                                _stat_wrong += 1f;
                            }
                        }
                    }

                    b.X += 110;
                } // for cakes
            }            
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            if (game_state != GAME_STATE.GAME_SHOW_RESULT)
            {
                Rectangle b = new Rectangle(70, 200, 100, 200);

                for (byte i = 0; i < count_cakes; i++)
                {
                    if (state_cake[i] == false)
                    {
                        if(selected_id == i)
                            sp.Draw(texture_cakes[id_cakes[i]], b, Color.DarkGray);
                        else
                            sp.Draw(texture_cakes[id_cakes[i]], b, Color.White);
                    }

                    b.X += 110;
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
                game_state = GAME_STATE.GAME_PLAY;

                CreateRound();
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
            return "Select one brick in ascending order \nuntill all are removed.";
        }

        private void CreateRound()
        {
            count_cakes = 6;

            id_cakes = new byte[count_cakes];

            state_cake = new bool[count_cakes];

            byte[] temp = new byte[9];

            for (byte i = 0; i < 9; i++)
            {
                temp[i] = i;
            }

            for (byte i = 0; i < 9; i++)
            {
                byte swap = temp[i];
                byte id = (byte)(Utility.Random(0f, 9));

                temp[i] = temp[id];
                temp[id] = swap;
            }

            for (byte i = 0; i < count_cakes; i++)
            {
                id_cakes[i] = temp[i];

                state_cake[i] = false;
            }
        }
    }
}
