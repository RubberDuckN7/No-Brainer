using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameFocusArrow : GameInterface
    {
        Texture2D texture_arrow_up;
        Texture2D texture_arrow_down;
        Texture2D texture_arrow_left;
        Texture2D texture_arrow_right;

        Texture2D t_up;
        Texture2D t_down;
        Texture2D t_left;
        Texture2D t_right;

        ButtonGeneral b_up;
        ButtonGeneral b_down;
        ButtonGeneral b_left;
        ButtonGeneral b_right;

        Vector2[] pos;

        float cooldown;

        float cd_bonus;

        byte dir_all;
        byte dir_focus;

        bool right;

        public GameFocusArrow(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/AttentionFocusArrow");


            texture_arrow_up = content.Load<Texture2D>("arrow_up");
            texture_arrow_down = content.Load<Texture2D>("arrow_down");
            texture_arrow_left = content.Load<Texture2D>("arrow_left");
            texture_arrow_right = content.Load<Texture2D>("arrow_right");

            t_up = content.Load<Texture2D>("b_up");
            t_down = content.Load<Texture2D>("b_down");
            t_left = content.Load<Texture2D>("b_left");
            t_right = content.Load<Texture2D>("b_right");

            b_up = new ButtonGeneral(new Rectangle(120, 90, 100, 100));
            b_down = new ButtonGeneral(new Rectangle(120, 290, 100, 100));
            b_left = new ButtonGeneral(new Rectangle(20, 190, 100, 100));
            b_right = new ButtonGeneral(new Rectangle(220, 190, 100, 100));


            pos = new Vector2[5];

            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateRound();

            cooldown = 0f;

            right = false;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            texture_arrow_up = null;
            texture_arrow_down = null;
            texture_arrow_left = null;
            texture_arrow_right = null;

            t_up = null;
            t_down = null;
            t_left = null;
            t_right = null;

            b_up = null;
            b_down = null;
            b_left = null;
            b_right = null;

            pos = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_up.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_down.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_left.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
                if (b_right.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }

            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (b_up.Collide(p))
                {

                }
                if (b_down.Collide(p))
                {

                }
                if (b_left.Collide(p))
                {

                }
                if (b_right.Collide(p))
                {

                }

            }            
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                bool press = false;

                if (b_up.Collide(p))
                {
                    press = true;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                    cooldown = 0f;

                    if (dir_focus == 0)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }
                }
                if (b_down.Collide(p))
                {
                    press = true;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                    cooldown = 0f;

                    if (dir_focus == 2)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }
                }
                if (b_left.Collide(p))
                {
                    press = true;
                    
                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                    cooldown = 0f;

                    if (dir_focus == 3)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }
                }
                if (b_right.Collide(p))
                {
                    press = true;

                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                    cooldown = 0f;

                    if (dir_focus == 1)
                    {
                        right = true;
                    }
                    else
                    {
                        right = false;
                    }
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

            b_up.Pressed = false;
            b_down.Pressed = false;
            b_left.Pressed = false;
            b_right.Pressed = false;
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {

            Rectangle b = new Rectangle(20, 90, 300, 300);

            sp.Draw(game_scene.Manager.TBoxTransparent, b, Color.White);

            Color color = Color.White;

            if(b_up.Pressed)
                b_up.Draw(sp, t_up, Color.DarkGray);
            else
                b_up.Draw(sp, t_up, Color.White);

            if(b_down.Pressed)
                b_down.Draw(sp, t_down, Color.DarkGray);
            else
                b_down.Draw(sp, t_down, Color.White);
            
            if(b_left.Pressed)
                b_left.Draw(sp, t_left, Color.DarkGray);
            else
                b_left.Draw(sp, t_left, Color.White);
            
            if(b_right.Pressed)
                b_right.Draw(sp, t_right, Color.DarkGray);
            else
                b_right.Draw(sp, t_right, Color.White);

            if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                Rectangle __b = new Rectangle(272, 50, 256, 256);

                if (right)
                    sp.Draw(game_scene.Manager.TTrue, __b, Color.White);
                else
                    sp.Draw(game_scene.Manager.TFalse, __b, Color.White);

            }
            else if (game_state == GAME_STATE.GAME_TUTORIAL)
            {
               
            }

            b.Width = 64;
            b.Height = 64;

            for (byte i = 0; i < 5; i++)
            {
                b.X = (int)(pos[i].X);
                b.Y = (int)(pos[i].Y);

                if (i == 2)
                {
                    if (dir_focus == 0)
                    {
                        sp.Draw(texture_arrow_up, b, color);
                    }
                    else if (dir_focus == 1)
                    {
                        sp.Draw(texture_arrow_right, b, color);
                    }
                    else if (dir_focus == 2)
                    {
                        sp.Draw(texture_arrow_down, b, color);
                    }
                    else if (dir_focus == 3)
                    {
                        sp.Draw(texture_arrow_left, b, color);
                    }
                }
                else
                {
                    if (dir_all == 0)
                    {
                        sp.Draw(texture_arrow_up, b, color);
                    }
                    else if (dir_all == 1)
                    {
                        sp.Draw(texture_arrow_right, b, color);
                    }
                    else if (dir_all == 2)
                    {
                        sp.Draw(texture_arrow_down, b, color);
                    }
                    else if (dir_all == 3)
                    {
                        sp.Draw(texture_arrow_left, b, color);
                    }
                }
            }
        }

        public override string Description()
        {
            return "Select direction from left side, to which middle \narrow at the right side points to.";
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

                if (cooldown > 0.5f)
                {
                    game_state = GAME_STATE.GAME_CREATE;
                    cooldown = 0f;
                }

                cooldown += dt;
            }
        }

        private void CreateRound()
        {
            bool vertical = false;


            if (Utility.Random(0f, 100f) < 50f)
                vertical = true;

            Vector2 offset = new Vector2(Utility.Random(360f, 410f), Utility.Random(105f, 110f));

            dir_all = (byte)(Utility.Random(0f, 4f));

            dir_focus = (byte)(Utility.Random(0f, 4f));

            if (vertical)
            {
                offset.X += Utility.Random(0f, 250f);

                for (byte i = 0; i < 5; i++)
                {
                    pos[i] = new Vector2(0f, i * 70f) + offset;
                }
            }
            else
            {
                for (byte i = 0; i < 5; i++)
                {
                    pos[i] = new Vector2(i * 70f, 0f) + offset;
                }
            }

        }

    }
}
