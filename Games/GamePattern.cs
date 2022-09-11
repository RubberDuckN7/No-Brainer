using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GamePattern : GameInterface
    {
        Rectangle derp_rect;
        ButtonGeneral b_ready;

        Vector2 [][]pos;
        Vector2 offset;

        float cooldown;

        bool[][] matrix_current;
        bool[][] matrix_right;

        byte max_size;
        byte current_size;

        byte current;

        byte count_pattern;

        byte difficulty;

        byte selected_x;
        byte selected_y;

        bool right;

        public GamePattern(GameScene game_scene)
        {
            this.game_scene = game_scene;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/MemorizePattern");

            derp_rect = new Rectangle();

            b_ready = new ButtonGeneral(new Rectangle(570, 360, 200, 50));

            right_count     = 0;
            right_count_row = 0;
            score           = 0;
            max_size        = 6;
            current         = 0;
            count_pattern   = 15;
            current         = 0;
            difficulty      = 0;
            current_size    = 3;

            selected_x = 255;
            selected_y = 255;

            derp_rect.Width = 64;
            derp_rect.Height = 64;

            cooldown = 0f;
            
            right = false;

            pos = new Vector2[max_size][];

            matrix_current = new bool[max_size][];
            matrix_right = new bool[max_size][];

            for (byte i = 0; i < max_size; i++)
            {
                pos[i] = new Vector2[max_size];
                matrix_current[i] = new bool[max_size];
                matrix_right[i] = new bool[max_size];
            }

            offset = Vector2.Zero;

            Vector2 t = new Vector2(75f + 445f * 0.5f, 15f + 445f * 0.5f);

            for (byte i = 0; i < max_size; i++)
            {
                for (byte j = 0; j < max_size; j++)
                {
                    pos[i][j] = t;
                    t.X += derp_rect.Width + 5;

                    matrix_current[i][j] = false;
                    matrix_right[i][j] = false;

                }

                t.X = 75f + 445f * 0.5f;
                t.Y += derp_rect.Height + 5;
            }

            game_state = GAME_STATE.GAME_TUTORIAL;

            CreateMatrix();
        }

        public override void Unload()
        {
            if(content != null)
                content.Unload();

            b_ready = null;

            for (byte i = 0; i < max_size; i++)
            {
                pos[i] = null;
                matrix_current[i] = null;
                matrix_right[i] = null;
            }

            pos = null;
            matrix_current = null;
            matrix_right = null;
        }

        public override string Description()
        {
            return "Memorize blue blocks, \nwhen you're ready,\npress button, and select blocks.";
        }


        public override void Pressed(Vector2 p)
        {
            b_ready.Pressed = false;

            if(game_state == GAME_STATE.GAME_THINK)
            {
                if(b_ready.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {

                derp_rect.Width = 64;
                derp_rect.Height = 64;

                for (byte n = 0; n < current_size; n++)
                {
                    for (byte m = 0; m < current_size; m++)
                    {
                        derp_rect.X = (int)(pos[n][m].X + offset.X);
                        derp_rect.Y = (int)(pos[n][m].Y + offset.Y);

                        if (Utility.PointVsRectangle(derp_rect, p))
                        {
                            game_scene.Manager.PlayPress();

                            selected_x = n;
                            selected_y = m;
                        }
                    }
                }

                if(b_ready.Collide(p))
                {
                    game_scene.Manager.PlayPress();
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            b_ready.Pressed = false;

            if (game_state == GAME_STATE.GAME_THINK)
            {
                if (b_ready.Collide(p))
                {

                }
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {

                derp_rect.Width = 64;
                derp_rect.Height = 64;

                for (byte n = 0; n < current_size; n++)
                {
                    for (byte m = 0; m < current_size; m++)
                    {
                        derp_rect.X = (int)(pos[n][m].X + offset.X);
                        derp_rect.Y = (int)(pos[n][m].Y + offset.Y);

                        if (Utility.PointVsRectangle(derp_rect, p))
                        {
                            selected_x = n;
                            selected_y = m;
                        }
                    }
                }

                if (b_ready.Collide(p))
                {

                }
            }            
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_THINK)
            {
                if (b_ready.Collide(p))
                {
                    game_state = GAME_STATE.GAME_PLAY;
                }
            }
            else if (game_state == GAME_STATE.GAME_PLAY)
            {

                derp_rect.Width = 64;
                derp_rect.Height = 64;

                for (byte n = 0; n < current_size; n++)
                {
                    for (byte m = 0; m < current_size; m++)
                    {
                        derp_rect.X = (int)(pos[n][m].X + offset.X);
                        derp_rect.Y = (int)(pos[n][m].Y + offset.Y);

                        if (Utility.PointVsRectangle(derp_rect, p))
                        {
                            matrix_current[n][m] = !matrix_current[n][m];
                        }
                    }
                }

                if (b_ready.Collide(p))
                {
                    EvaluateAnswer();
                    game_state = GAME_STATE.GAME_SHOW_RESULT;
                }
            }

            b_ready.Pressed = false;
            selected_x = 255;
            selected_y = 255;
        }

        public override void Update(float dt)
        {
            if (game_state == GAME_STATE.GAME_CREATE)
            {
                CreateMatrix();
                game_state = GAME_STATE.GAME_THINK;

                cooldown = 0f;
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                if (cooldown > 1f)
                {
                    game_state = GAME_STATE.GAME_CREATE;
                }

                cooldown += dt;
            }
            
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {

            Color c = new Color(90, 90, 90);
            c.A = 250;

            derp_rect.X = 580;
            derp_rect.Y = 230;

            derp_rect.Width = 160;
            derp_rect.Height = 50;

            sp.Draw(game_scene.Manager.TInputRectangle, derp_rect, Color.White);

            sp.DrawString(font, "" + current + " / " + count_pattern, new Vector2(600f, 230f), Color.White);


            Rectangle b_sp = new Rectangle(75, 15, 445, 445);

            sp.Draw(game_scene.Manager.TBoxTransparent, b_sp, Color.White);

            derp_rect.Width = 64;
            derp_rect.Height = 64;

            for (byte i = 0; i < current_size; i++)
            {
                for (byte j = 0; j < current_size; j++)
                {
                    derp_rect.X = (int)(pos[i][j].X + offset.X);
                    derp_rect.Y = (int)(pos[i][j].Y + offset.Y);

                    if (game_state == GAME_STATE.GAME_THINK || game_state == GAME_STATE.GAME_TUTORIAL)
                    {
                        if (matrix_right[i][j])
                        {
                            sp.Draw(game_scene.Manager.TTile, derp_rect, Color.Blue);
                        }
                        else
                        {
                            sp.Draw(game_scene.Manager.TTile, derp_rect, Color.White);
                        }


                        if (b_ready.Pressed)
                            b_ready.Draw(sp, game_scene.Manager.TReady, Color.DarkGray);
                        else
                            b_ready.Draw(sp, game_scene.Manager.TReady, Color.White);

                        sp.DrawString(game_scene.Manager.Font, "Start", new Vector2(b_ready.X + 60, b_ready.Y + 5), Color.Black);


                    }
                    else if (game_state == GAME_STATE.GAME_PLAY)
                    {
                        if (matrix_current[i][j])
                        {
                            if(selected_x == i && selected_y == j)
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.DarkGray);
                            else
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.Gold);
                        }
                        else
                        {
                            if(selected_x == i && selected_y == j)
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.DarkGray);
                            else
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.White);
                        }


                        if (b_ready.Pressed)
                            b_ready.Draw(sp, game_scene.Manager.TReady, Color.DarkGray);
                        else
                            b_ready.Draw(sp, game_scene.Manager.TReady, Color.White);

                        sp.DrawString(game_scene.Manager.Font, "Show result", new Vector2(b_ready.X + 30, b_ready.Y + 5), Color.Black);

                    }
                    else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
                    {
                        
                        if (matrix_current[i][j] != matrix_right[i][j])
                        {
                            if(matrix_current[i][j])
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.Red);
                            else
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.Yellow);
                        }
                        else
                        {
                            if(matrix_current[i][j])
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.Green);
                            else
                                sp.Draw(game_scene.Manager.TTile, derp_rect, Color.White);
                        }

                        b_ready.Draw(sp, game_scene.Manager.TReady, Color.White);

                        sp.DrawString(game_scene.Manager.Font, "Show result", new Vector2(b_ready.X + 30, b_ready.Y + 5), Color.Black);

                    }

                }
            }

        }

        private void EvaluateAnswer()
        {
            right = true;

            for (byte i = 0; i < current_size; i++)
            {
                for (byte j = 0; j < current_size; j++)
                {
                    if (matrix_current[i][j] != matrix_right[i][j])
                    {
                        right = false;
                    }
                }
            }

            cooldown = 0f;

            current += 1;

            if (current >= count_pattern)
            {
                game_scene.ShowResults();
            }

            if (right)
            {
                _stat_right += 1f + current_size * 0.25f;
            }
            else
            {
                _stat_wrong += 1f;
            }
        }

        private void CreateMatrix()
        {

            if (right)
            {
                right_count += 1;

                if (right_count >= 2)
                {
                    right_count = 0;

                    difficulty += 1;

                    if (current_size < max_size)
                    {
                        current_size += 1;

                        if (score < current_size)
                        {
                            score = current_size;
                        }
                    }

                    if (score < current_size)
                    {
                        score = current_size;
                    }
                }
            }
            else
            {
                right_count = 0;

                if (difficulty > 0)
                    difficulty -= 1;

                if (current_size > 3)
                    current_size -= 1;
            }


            float _s = 64f * current_size + (current_size - 1) * 5f;

            _s *= 0.5f;

            offset.X = - _s;
            offset.Y = - _s;

            // Use current_size as difficulty
            float disttribution = 1f / (float)(current_size);

            int count = (int)(current_size + difficulty);

            for (byte i = 0; i < max_size; i++)
            {
                for (byte j = 0; j < max_size; j++)
                {
                    matrix_right[i][j] = false;
                }
            }

            // Just set out numbers
            for (byte i = 0; i < current_size; i++)
            {
                for (byte j = 0; j < current_size; j++)
                {
                    if (count > 0)
                    {
                        matrix_right[i][j] = true;
                        count--;
                    }
                }
            }

            // Generate random matrix

            int x = 0, y = 0;

            for (byte i = 0; i < current_size; i++)
            {
                for (byte j = 0; j < current_size; j++)
                {
                    x = (int)(Utility.Random(0, (float)(current_size)));
                    y = (int)(Utility.Random(0, (float)(current_size)));

                    bool temp = matrix_right[i][j];

                    matrix_right[i][j] = matrix_right[x][y];

                    matrix_right[x][y] = temp;
                }
            }

            for (byte i = 0; i < max_size; i++)
            {
                for (byte j = 0; j < max_size; j++)
                {
                    matrix_current[i][j] = false;
                }
            }

        }
    }
}
