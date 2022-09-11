using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameOrderCircle : GameInterface
    {

        Texture2D[] texture_circle;

        float cooldown;

        byte[] order_id;

        byte difficulty;

        byte play_id;

        byte current;

        byte pressed_part_id;

        byte count_answered;

        bool right;

        bool flash;

        public GameOrderCircle(GameScene game)
        {
            this.game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/MemorizeOrder");

            texture_circle = new Texture2D[8];


            texture_circle[0] = content.Load<Texture2D>("circle_part_one");
            texture_circle[1] = content.Load<Texture2D>("circle_part_two");
            texture_circle[2] = content.Load<Texture2D>("circle_part_three");
            texture_circle[3] = content.Load<Texture2D>("circle_part_four");

            texture_circle[4] = content.Load<Texture2D>("circle_part_five");
            texture_circle[5] = content.Load<Texture2D>("circle_part_six");
            texture_circle[6] = content.Load<Texture2D>("circle_part_seven");
            texture_circle[7] = content.Load<Texture2D>("circle_part_eight");


            game_state = GAME_STATE.GAME_TUTORIAL;

            difficulty      = 2;
            current         = 0;
            play_id         = 0;
            count_answered  = 0;
            pressed_part_id = 255;

            right = false;

            _stat_right = 0f;
            _stat_wrong = 0f;
            cooldown = 0f;

            game_scene.AddTimer(1, 0);
        }

        public override void Unload()
        {
            content.Unload();

            texture_circle[0] = null;
            texture_circle[1] = null;
            texture_circle[2] = null;
            texture_circle[3] = null;
            texture_circle[4] = null;
            texture_circle[5] = null;
            texture_circle[6] = null;
            texture_circle[7] = null;

            order_id = null;
        }

        public override void Pressed(Vector2 p)
        {
            pressed_part_id = 255;

            Vector2 center = new Vector2(400f, 240f);

            Vector2 dir = p - center;

            float length = dir.Length();

            byte orientation = 255;

            if (dir.X < 0f && dir.Y < 0f)
            {
                orientation = 1;
            }
            else if (dir.X > 0f && dir.Y < 0f)
            {
                orientation = 2;
            }
            else if (dir.X < 0f && dir.Y > 0f)
            {
                orientation = 3;
            }
            else if (dir.X > 0f && dir.Y > 0f)
            {
                orientation = 4;
            }

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (pressed_part_id == 255)
                {
                    if (length < 146f)
                    {
                        // Inner
                        if (orientation == 1)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 4;
                        }
                        else if (orientation == 2)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 5;
                        }
                        else if (orientation == 3)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 6;
                        }
                        else if (orientation == 4)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 7;
                        }

                    }
                    else if (length > 170f && length < 240f)
                    {
                        // Outer
                        if (orientation == 1)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 0;
                        }
                        else if (orientation == 2)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 1;
                        }
                        else if (orientation == 3)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 2;
                        }
                        else if (orientation == 4)
                        {
                            game_scene.Manager.PlayPress();
                            pressed_part_id = 3;
                        }
                    }
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            pressed_part_id = 255;

            Vector2 center = new Vector2(400f, 240f);

            Vector2 dir = p - center;

            float length = dir.Length();

            byte orientation = 255;

            if (dir.X < 0f && dir.Y < 0f)
            {
                orientation = 1;
            }
            else if (dir.X > 0f && dir.Y < 0f)
            {
                orientation = 2;
            }
            else if (dir.X < 0f && dir.Y > 0f)
            {
                orientation = 3;
            }
            else if (dir.X > 0f && dir.Y > 0f)
            {
                orientation = 4;
            }

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (pressed_part_id == 255)
                {
                    if (length < 146f)
                    {
                        // Inner
                        if (orientation == 1)
                        {
                            pressed_part_id = 4;
                        }
                        else if (orientation == 2)
                        {
                            pressed_part_id = 5;
                        }
                        else if (orientation == 3)
                        {
                            pressed_part_id = 6;
                        }
                        else if (orientation == 4)
                        {
                            pressed_part_id = 7;
                        }

                    }
                    else if (length > 170f && length < 240f)
                    {
                        // Outer
                        if (orientation == 1)
                        {
                            pressed_part_id = 0;
                        }
                        else if (orientation == 2)
                        {
                            pressed_part_id = 1;
                        }
                        else if (orientation == 3)
                        {
                            pressed_part_id = 2;
                        }
                        else if (orientation == 4)
                        {
                            pressed_part_id = 3;
                        }
                    }
                }
            }            
        }

        public override void Released(Vector2 p)
        {
            pressed_part_id = 255;

            Vector2 center = new Vector2(400f, 240f);

            Vector2 dir = p - center;

            float length = dir.Length();

            byte orientation = 255;

            if (dir.X < 0f && dir.Y < 0f)
            {
                orientation = 1;
            }
            else if (dir.X > 0f && dir.Y < 0f)
            {
                orientation = 2;
            }
            else if (dir.X < 0f && dir.Y > 0f)
            {
                orientation = 3;
            }
            else if (dir.X > 0f && dir.Y > 0f)
            {
                orientation = 4;
            }

            if (game_state == GAME_STATE.GAME_PLAY)
            {
                if (pressed_part_id == 255)
                {
                    if (length < 146f)
                    {
                        // Inner
                        if (orientation == 1)
                        {
                            pressed_part_id = 4;
                        }
                        else if (orientation == 2)
                        {
                            pressed_part_id = 5;
                        }
                        else if (orientation == 3)
                        {
                            pressed_part_id = 6;
                        }
                        else if (orientation == 4)
                        {
                            pressed_part_id = 7;
                        }

                    }
                    else if (length > 170f && length < 240f)
                    {
                        // Outer
                        if (orientation == 1)
                        {
                            pressed_part_id = 0;
                        }
                        else if (orientation == 2)
                        {
                            pressed_part_id = 1;
                        }
                        else if (orientation == 3)
                        {
                            pressed_part_id = 2;
                        }
                        else if (orientation == 4)
                        {
                            pressed_part_id = 3;
                        }
                    }


                    if (order_id[current] == pressed_part_id)
                    {
                        right = true;

                        _stat_right += 1f + difficulty * 0.5f;

                        if (current < difficulty - 1)
                        {
                            current += 1;
                        }
                        else
                        {
                            game_state = GAME_STATE.GAME_SHOW_RESULT;

                            cooldown = 0f;
                        }

                    }
                    else if (pressed_part_id != 255)
                    {
                        right = false;

                        _stat_wrong += 1f;

                        game_state = GAME_STATE.GAME_SHOW_RESULT;

                        cooldown = 0f;
                    }
                }
            }

            pressed_part_id = 255;
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle circle = new Rectangle(160, 0, 480, 480);

            Color __c = Color.Black;
            __c.A = 90;

            sp.Draw(game_scene.Manager.TPoint, game_scene.ViewBounds, __c);

            if (game_state == GAME_STATE.GAME_TUTORIAL)
            {
                DrawCircle(sp, circle);
            }
            else if (game_state == GAME_STATE.GAME_CREATE)
            {
                DrawCircle(sp, circle);
            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {
                DrawCircle(sp, circle);

                Color c = Color.White;

                if (cooldown > 1f)
                    c.A = 255;
                else
                    c.A = (byte)(cooldown * 255);

                if (cooldown > 0.6f)
                    c = Color.Transparent;

                sp.Draw(texture_circle[order_id[play_id]], circle, c);

            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {
                DrawCircle(sp, circle);
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {

                DrawCircle(sp, circle);

                Rectangle b = new Rectangle(200, 20, 400, 400);

                if (cooldown < 0.4f)
                {

                    if (right)
                    {
                        sp.Draw(game_scene.Manager.TTrue, b, Color.White);
                    }
                    else
                    {
                        sp.Draw(game_scene.Manager.TFalse, b, Color.White);
                    }

                }

            }

        }

        public override void Update(float dt)
        {
            if (game_state == GAME_STATE.GAME_TUTORIAL)
            {
                cooldown = 0.3f;
            }
            else if (game_state == GAME_STATE.GAME_CREATE)
            {


                if (cooldown < 0f)
                {
                    CreateRound();
                    cooldown = 0f;

                    game_state = GAME_STATE.GAME_THINK;
                }


                cooldown -= dt;
            }
            else if (game_state == GAME_STATE.GAME_THINK)
            {
                if (cooldown > 1f)
                {
                    cooldown = 0f;

                    if (play_id < difficulty-1)
                    {
                        play_id += 1;
                    }
                    else
                    {
                        game_state = GAME_STATE.GAME_PLAY;
                    }
                }

                cooldown += dt * 1.5f;
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {

            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                if (cooldown > 0.6f)
                {
                    if (count_answered < 15)
                    {
                        game_state = GAME_STATE.GAME_CREATE;
                        cooldown = 0f;

                        count_answered += 1;
                    }
                    else
                    {
                        game_scene.ExitGame();
                    }
                }

                cooldown += dt;
            }
        }

        private void CreateRound()
        {
            if (right)
            {
                if (difficulty < 250)
                    difficulty += 1;
            }
            else
            {
                if (difficulty > 2)
                    difficulty -= 1;
            }

            order_id = new byte[difficulty];

            play_id = 0;
            current = 0;

            byte id = 0;

            for (byte i = 0; i < difficulty; i++)
            {
                id = (byte)(Utility.Random(0f, 8f));

                order_id[i] = id;
            }
        }

        public void EvaluateRound()
        {
            // May not be needed
        }

        public override string Description()
        {
            return "Remember the order which circle parts\nare highlighted.";
        }

        private void DrawCircle(SpriteBatch sp, Rectangle b)
        {
            if(pressed_part_id == 0)
                sp.Draw(texture_circle[0], b, Color.Black);
            else
                sp.Draw(texture_circle[0], b, Color.Red);

            if(pressed_part_id == 1)
                sp.Draw(texture_circle[1], b, Color.Black);
            else
                sp.Draw(texture_circle[1], b, Color.Blue);

            if(pressed_part_id == 2)
                sp.Draw(texture_circle[2], b, Color.Black);
            else
                sp.Draw(texture_circle[2], b, Color.Green);

            if(pressed_part_id == 3)
                sp.Draw(texture_circle[3], b, Color.Black);
            else
                sp.Draw(texture_circle[3], b, Color.Orange);

            if(pressed_part_id == 4)
                sp.Draw(texture_circle[4], b, Color.Black);
            else
                sp.Draw(texture_circle[4], b, Color.Violet);
            
            if(pressed_part_id == 5)
                sp.Draw(texture_circle[5], b, Color.Black);
            else
                sp.Draw(texture_circle[5], b, Color.Cyan);

            if(pressed_part_id == 6)
                sp.Draw(texture_circle[6], b, Color.Black);
            else
                sp.Draw(texture_circle[6], b, Color.Gray);

            if(pressed_part_id == 7)
                sp.Draw(texture_circle[7], b, Color.Black);
            else
                sp.Draw(texture_circle[7], b, Color.Salmon);
        }
    }
}
