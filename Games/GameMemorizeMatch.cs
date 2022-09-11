using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public class GameMemorizeMatch : GameInterface
    {
        class TileState
        {
            float cooldown;

            float cd_slow;

            byte id_x;
            byte id_y;

            /// <summary>
            /// 255 = not active
            /// 0 = start shrinking tile
            /// 1 = start opening item
            /// 2 = show whole item, steady, aka completed showing item
            /// 3 = start shrinking item
            /// 4 = start showin tile
            /// 5 = show tile, aka completed reversing
            /// </summary>
            byte state;

            public TileState()
            {
                cooldown = 0f;

                cd_slow = 0f;

                id_x = 255;
                id_y = 255;

                state = 0;

            }

            public void Update(float dt)
            {
                if (state == 0)
                {
                    cooldown += dt;

                    if (cooldown > 1f)
                    {
                        cooldown = 0f;
                        state = 1;
                    }
                }
                else if (state == 1)
                {
                    cooldown += dt;

                    if (cooldown > 1f)
                    {
                        state = 2;
                        cooldown = 0f;
                    }
                }

                if (state == 3)
                {
                    if (cd_slow > 1.5f)
                    {
                        cooldown += dt;

                        if (cooldown > 1f)
                        {
                            state = 4;
                            cooldown = 0f;
                        }
                    }
                    else
                    {
                        cd_slow += dt;
                    }
                }
                else if (state == 4)
                {
                    cooldown += dt;

                    if (cooldown > 1f)
                    {
                        state = 5;
                        cooldown = 0f;
                    }
                }


            }

            public void Start(byte x, byte y)
            {
                cooldown = 0f;
                cd_slow = 0f;

                state = 0;

                id_x = x;
                id_y = y;

            }

            public void Reverse()
            {
                state = 3;
            }

            public void Kill()
            {
                cooldown = 0f;
                id_x = 255;
                id_y = 255;
            }

            public float Lerp
            {
                get { return cooldown; }
            }

            public byte X
            {
                get { return id_x; }
            }

            public byte Y
            {
                get { return id_y; } 
            }

            public bool OpeningTile
            {
                get { return state == 4; }
            }

            public bool OpeningItem
            {
                get { return state == 1; }
            }

            public bool ClosingTile
            {
                get { return state == 0; }
            }

            public bool ClosingItem
            {
                get { return state == 3; }
            }


            public bool CompletedOpening
            {
                get { return state == 2; }
            }

            public bool CompletedReverse
            {
                get { return state == 5; }
            }

            public bool Active
            {
                get { return (id_x != 255 && id_x != 255); }
            }

        }

        Texture2D[] t_items;
        Texture2D t_tile;

        TileState tile_one;
        TileState tile_two;

        Vector2 [][]pos;

        float time;

        float moves;

        byte[][] tile_id;

        byte selected_x;
        byte selected_y;

        byte cleared;

        /// <summary>
        /// true = flipped
        /// false = not flipped
        /// </summary>
        bool[][] tile_state;

        /// <summary>
        /// true = matched and done
        /// false = not answered
        /// </summary>
        bool[][] tile_done;


        public GameMemorizeMatch(GameScene game)
        {
            game_scene = game;
        }

        public override void Load(Game game)
        {
            if (content == null)
                content = new ContentManager(game.Services, "Content/GameResources/MemorizeMatch");

            t_items = new Texture2D[20];

            t_tile = content.Load<Texture2D>("memorize_tile"); // Asset name, origin name: tile.png

            t_items[0] = content.Load<Texture2D>("apple");
            t_items[1] = content.Load<Texture2D>("pear"); 
                                                          
            t_items[2] = content.Load<Texture2D>("cow");  
            t_items[3] = content.Load<Texture2D>("pig");  
                                                          
            t_items[4] = content.Load<Texture2D>("fish");    
            t_items[5] = content.Load<Texture2D>("fish_other");   
                                                      
            t_items[6] = content.Load<Texture2D>("flower_red");     
            t_items[7] = content.Load<Texture2D>("flower_white");   
                                                        
            t_items[8] = content.Load<Texture2D>("book_blue");   
            t_items[9] = content.Load<Texture2D>("bok_brown");   
                                                     
            t_items[10] = content.Load<Texture2D>("pen");  
            t_items[11] = content.Load<Texture2D>("pencil");    
                                                       
            t_items[12] = content.Load<Texture2D>("sandwich"); 
            t_items[13] = content.Load<Texture2D>("egg");  
                                                       
            t_items[14] = content.Load<Texture2D>("time");   
            t_items[15] = content.Load<Texture2D>("watch");  
                                                        
            t_items[16] = content.Load<Texture2D>("cig");    
            t_items[17] = content.Load<Texture2D>("cigar");  
                                                         
            t_items[18] = content.Load<Texture2D>("mushroom_red");    
            t_items[19] = content.Load<Texture2D>("mushroom_brown");

            tile_one = new TileState();
            tile_two = new TileState();

            cleared = 0;

            time = 0f;

            pos = new Vector2[5][];
            tile_id = new byte[5][];
            tile_state = new bool[5][];
            tile_done = new bool[5][];

            for (byte i = 0; i < 5; i++)
            {
                pos[i] = new Vector2[8];
                tile_id[i] = new byte[8];
                tile_state[i] = new bool[8];
                tile_done[i] = new bool[8];
            }

            Vector2 _p = Vector2.Zero;

            float step_h = 480 / 90;
            float step_w = 800 / 80;

            _p.X = step_w * 0.5f;
            _p.Y = step_h * 0.5f;

            byte count = 0;

            for (byte x = 0; x < 5; x++)
            {
                for (byte y = 0; y < 8; y++)
                {
                    tile_id[x][y] = count;
                    tile_state[x][y] = false;
                    tile_done[x][y] = false;

                    if (count == 19)
                        count = 0;
                    else
                        count += 1;



                    pos[x][y] = _p;

                    _p.X += step_w + 90f;
                }

                _p.X = step_w * 0.5f;
                _p.Y += step_h + 90f;
            }

            selected_x = 255;
            selected_y = 255;

            CreateRound();

            game_state = GAME_STATE.GAME_TUTORIAL;

        }

        public override void Unload()
        {
            if (content != null)
                content.Unload();

            for (byte i = 0; i < 20; i++)
                t_items[i] = null;

            t_items = null;

            t_tile = null;

            tile_one = null;
            tile_two = null;

            for (byte x = 0; x < 5; x++)
            {
                pos[x] = null;
                tile_id[x] = null;
                tile_state[x] = null;
                tile_done[x] = null;
            }

            pos = null;
            tile_id = null;
            tile_state = null;
            tile_done = null;
        }

        public override void Pressed(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                for (byte x = 0; x < 5; x++)
                {
                    for (byte y = 0; y < 8; y++)
                    {
                        b.X = (int)pos[x][y].X;
                        b.Y = (int)pos[x][y].Y;

                        if (!tile_state[x][y] && !tile_done[x][y])
                        {
                            if (Utility.PointVsRectangle(b, p))
                            {
                                game_scene.Manager.PlayPress();

                                selected_x = x;
                                selected_y = y;

                                return;
                            }
                        }

                    }
                }
            }
        }

        public override void Moved(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                for (byte x = 0; x < 5; x++)
                {
                    for (byte y = 0; y < 8; y++)
                    {
                        if (!tile_state[x][y] && !tile_done[x][y])
                        {
                            b.X = (int)pos[x][y].X;
                            b.Y = (int)pos[x][y].Y;

                            if(Utility.PointVsRectangle(b, p))
                            {
                                selected_x = x;
                                selected_y = y;

                                return;
                            }
                        }
                    }
                }
            }          
        }

        public override void Released(Vector2 p)
        {
            if (game_state == GAME_STATE.GAME_PLAY)
            {
                Rectangle b = new Rectangle(0, 0, 90, 90);

                for (byte x = 0; x < 5; x++)
                {
                    for (byte y = 0; y < 8; y++)
                    {
                        b.X = (int)pos[x][y].X;
                        b.Y = (int)pos[x][y].Y;

                        if (!tile_state[x][y] && !tile_done[x][y])
                        {
                            if (Utility.PointVsRectangle(b, p))
                            {
                                game_scene.Manager.PlayFlip();

                                if (!tile_one.Active)
                                {
                                    tile_state[x][y] = true;
                                    tile_one.Start(x, y);

                                    moves += 1f;
                                }
                                else if (!tile_two.Active)
                                {
                                    tile_state[x][y] = true;
                                    tile_two.Start(x, y);

                                    moves += 1f;

                                    return;
                                }



                            }
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sp, SpriteFont font)
        {
            Rectangle b = new Rectangle(0, 0, 90, 90);

            byte tx_one = tile_one.X;
            byte ty_one = tile_one.Y;

            byte tx_two = tile_two.X;
            byte ty_two = tile_two.Y;

            for (byte x = 0; x < 5; x++)
            {
                for (byte y = 0; y < 8; y++)
                {
                    b.X = (int)pos[x][y].X;
                    b.Y = (int)pos[x][y].Y;

                    //////////////////////////////////////
                    // Pain in a** starts here -><-
                    //////////////////////////////////////
                    if (tx_one == x && ty_one == y)
                    {
                        Rectangle _b = b;

                        if (tile_one.ClosingTile)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), 1f - tile_one.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, 1f - tile_one.Lerp));
                            sp.Draw(t_tile, _b, Color.White);
                        }
                        else if (tile_one.OpeningItem)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), tile_one.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, tile_one.Lerp));
                            sp.Draw(t_items[tile_id[x][y]], _b, Color.White);
                        }
                        else if (tile_one.ClosingItem)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), 1f - tile_one.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, 1f - tile_one.Lerp));
                            sp.Draw(t_items[tile_id[x][y]], _b, Color.White);
                        }
                        else if (tile_one.OpeningTile)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), tile_one.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, tile_one.Lerp));
                            sp.Draw(t_tile, _b, Color.White);
                        }
                        else if(tile_one.CompletedOpening)
                            sp.Draw(t_items[tile_id[x][y]], b, Color.White);

                    }
                    else if (tx_two == x && ty_two == y)
                    {
                        Rectangle _b = b;

                        if (tile_two.ClosingTile)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), 1f - tile_two.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, 1f - tile_two.Lerp));
                            sp.Draw(t_tile, _b, Color.White);
                        }
                        else if (tile_two.OpeningItem)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), tile_two.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, tile_two.Lerp));
                            sp.Draw(t_items[tile_id[x][y]], _b, Color.White);
                        }
                        else if (tile_two.ClosingItem)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), 1f - tile_two.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, 1f - tile_two.Lerp));
                            sp.Draw(t_items[tile_id[x][y]], _b, Color.White);
                        }
                        else if (tile_two.OpeningTile)
                        {
                            _b.Height = (int)(MathHelper.Lerp(0f, (float)(b.Height), tile_two.Lerp));
                            _b.Y += (int)(MathHelper.Lerp((float)(b.Height) * 0.5f, 0f, tile_two.Lerp));
                            sp.Draw(t_tile, _b, Color.White);
                        }
                        else if (tile_two.CompletedOpening)
                            sp.Draw(t_items[tile_id[x][y]], b, Color.White);
                    }
                    else
                    {
                        ///////////////////////////////////////
                        // IF not affected by TileState class
                        ///////////////////////////////////////
                        if (!tile_done[x][y])
                        {
                            if (tile_state[x][y])
                                sp.Draw(t_items[tile_id[x][y]], b, Color.White);
                            else
                                sp.Draw(t_tile, b, Color.White);
                        }
                    }
                }
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
                time += dt;

                if (tile_one.Active)
                {
                    tile_one.Update(dt * 5f);
                }
                if (tile_two.Active)
                {
                    tile_two.Update(dt * 5f);

                    if (tile_two.CompletedOpening)
                    {
                        if (CheckMatch())
                        {
                            tile_done[tile_one.X][tile_one.Y] = true;
                            tile_done[tile_two.X][tile_two.Y] = true;

                            tile_one.Kill();
                            tile_two.Kill();

                            cleared += 1;

                            if (cleared >= 20)
                            {
                                _stat_right = 100f / (time + moves);
                                _stat_wrong = 1f;
                                game_scene.ShowResults();
                            }
                        }
                        else
                        {
                            tile_one.Reverse();
                            tile_two.Reverse();
                        }
                    }
                    else if (tile_two.CompletedReverse)
                    {
                        tile_state[tile_one.X][tile_one.Y] = false;
                        tile_state[tile_two.X][tile_two.Y] = false;

                        tile_one.Kill();
                        tile_two.Kill();
                    }
                }
            }
            else if (game_state == GAME_STATE.GAME_SHOW_RESULT)
            {
                game_scene.ShowResults();
            } 
        }

        public override string Description()
        {
            return "Find matching item for each tile, \nwith no time limit.";
        }

        private bool CheckMatch()
        {
            return tile_id[tile_one.X][tile_one.Y] == tile_id[tile_two.X][tile_two.Y];
        }

        private void CreateRound()
        {

            for (byte x = 0; x < 5; x++)
            {
                for (byte y = 0; y < 8; y++)
                {
                    int id_x = (int)(Utility.Random(0f, 5f));
                    int id_y = (int)(Utility.Random(0f, 8f));

                    byte swap = tile_id[x][y];

                    tile_id[x][y] = tile_id[id_x][id_y];
                    tile_id[id_x][id_y] = swap;
                }

            }

        }
    }
}
