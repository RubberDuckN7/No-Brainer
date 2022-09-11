using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    public class CustomScene : Scene
    {
        ContentManager content;

        List<GAME_TYPE> types;

        ButtonGeneral b_back;
        ButtonGeneral b_play;

        Rectangle s_slider;

        MENU_STATE menu_state;

        float cooldown;

        float pos_new;
        float pos_old;

        float s_force;

        byte selected_id;

        public CustomScene(SceneManager manager)
        {
            this.SceneManager = manager;

            types = new List<GAME_TYPE>();

            types.Add(GAME_TYPE.GAME_CALCULATE);
            types.Add(GAME_TYPE.GAME_GUESS_PERCENTAGE);
            types.Add(GAME_TYPE.GAME_MULTIPLY_BY);   
            types.Add(GAME_TYPE.GAME_TRUE_FALSE);      
            types.Add(GAME_TYPE.GAME_PATTERN);        
            types.Add(GAME_TYPE.GAME_MEMORIZE_ITEMS); 
            types.Add(GAME_TYPE.GAME_ORDER_CIRCLE); 
            types.Add(GAME_TYPE.GAME_MEMORIZE_MATCH); 
            types.Add(GAME_TYPE.GAME_FOCUS_ARROW);    
            types.Add(GAME_TYPE.GAME_ORDER_NUMBERS);   
            types.Add(GAME_TYPE.GAME_FALSE_CHARACTER); 
            types.Add(GAME_TYPE.GAME_NUMBERS_CIRCLE);
            types.Add(GAME_TYPE.GMAE_ORDER_CAKES);    
            types.Add(GAME_TYPE.GAME_MATCH_COLOR);     
            types.Add(GAME_TYPE.GAME_FIGURES_MATCH);
            types.Add(GAME_TYPE.GAME_EVEN_OR);         

            b_back = new ButtonGeneral(new Rectangle(500, 50, 140, 70));
            b_play = new ButtonGeneral(new Rectangle(500, 180, 140, 70));


            s_slider = new Rectangle(0, 0, 50, 100);

            pos_new = 0f;
            pos_old = 0f;

            selected_id = 255;

            content = null;
        }

        // Clean new scene
        public override void Initialize()
        {
            //base.Initialize();
        }

        // Return from paus???
        public override void Activate()
        {
            pressed_touch = false;

            s_slider.X = 0;
            s_slider.Y = 0;


            cooldown = 0f;

            pos_new = 0f;
            pos_old = 0f;

            selected_id = 255;

            menu_state = MENU_STATE.MENU_CHOOSE;

            b_back.Pressed = false;
            b_play.Pressed = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(manager.Game.Services); 
        }

        public override void UnloadContent()
        {
            if(content != null)
                content.Unload();

            types.Clear();

            b_back = null;
            b_play = null;
        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                selected_id = 255;
                if (menu_state == MENU_STATE.MENU_CHOOSE)
                {
                    menu_state = MENU_STATE.MENU_TRANSIT;
                }
                else
                {
                    Back();
                }
                
            }

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {

                s_slider.Y += (int)(s_force * dt * 4f);

                if (s_slider.Y < 0)
                    s_slider.Y = 0;

                if (s_slider.Y + s_slider.Height > 480)
                    s_slider.Y = 480 - s_slider.Height;


                s_force -= s_force * dt * 7f;


                for (byte i = 0; i < manager.Touches.Count; i++)
                {

                    if (manager.Touches[i].State == TouchLocationState.Pressed)
                    {
                        Pressed(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Released)
                    {
                        Released(manager.Touches[i].Position);
                    }
                    if (manager.Touches[i].State == TouchLocationState.Moved)
                    {
                        Moved(manager.Touches[i].Position);
                    }
                }
            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                cooldown += dt * 2.4f;

                // Skip transit
                cooldown = 2f;

                if (cooldown > 1f)
                {
                    if (selected_id == 255)
                    {
                        Back();
                    }
                    else
                    {
                        // Clear games list.
                        manager.CustomGame = true;
                        manager.ClearGames();
                        manager.AddGame(types[selected_id]);
                        PlayGame();
                    }
                }
            }

        }

        private void Pressed(Vector2 p)
        {
            if (Utility.PointVsRectangle(s_slider, p))
            {
                pos_old = p.Y;
                manager.PlayPress();
            }
            else
            {
                bool found = false;
                if (b_play.Collide(p))
                {
                    manager.PlayPress();
                    found = true;
                }
                else if (b_back.Collide(p))
                {
                    manager.PlayPress();
                    found = true;
                }

                if(!found)
                    selected_id = 255;
                CheckSliderButtons(p, true);


            }

        }
        private void Moved(Vector2 p)
        {
            if (p.X < s_slider.Width)
            {
                s_force = pos_new - pos_old;

                pos_new = p.Y;
            }
            else
            {
                bool found = false;
                if (b_play.Collide(p))
                {
                    found = true;
                }
                else if (b_back.Collide(p))
                {
                    found = true;
                }

                if(!found)
                    selected_id = 255;
                CheckSliderButtons(p, false);


            }
        }
        private void Released(Vector2 p)
        {
            if (b_play.Collide(p))
            {
                if (selected_id != 255)
                    menu_state = MENU_STATE.MENU_TRANSIT;
                b_play.Pressed = false;
            }
            if (b_back.Collide(p))
            {
                selected_id = 255;
                menu_state = MENU_STATE.MENU_TRANSIT;
                b_back.Pressed = false;
            }

            CheckSliderButtons(p, false);
        }

        private void CheckSliderButtons(Vector2 p, bool sound)
        {
            Rectangle _b = new Rectangle();

            float lerp = (float)((float)(s_slider.Y) / (float)(480.0f - s_slider.Height));
            float temp = (80 * types.Count - 480) * lerp;


            _b.X = s_slider.Width + 100;
            _b.Y = -(int)temp;

            _b.Width = 240;
            _b.Height = 60;

            for (int i = 0; i < types.Count; i++)
            {
                if(Utility.PointVsRectangle(_b, p))
                {
                    if (Available((byte)i))
                    {
                        if(sound)
                            manager.PlayPress();

                        selected_id = (byte)i;
                    }

                }

                _b.Y += _b.Height + 20;
            }
        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            Vector2 t = Vector2.Zero;

            manager.SpriteBatch.Draw(manager.TMainBackground, manager.Game.GraphicsDevice.Viewport.Bounds, Color.White);

            if (menu_state == MENU_STATE.MENU_CHOOSE)
            {
                DrawChoose(manager.SpriteBatch, manager.Font);
            }
            else if (menu_state == MENU_STATE.MENU_TRANSIT)
            {
                DrawTransit(manager.SpriteBatch, manager.Font);
            }

            manager.SpriteBatch.End();
        }

        private void DrawChoose(SpriteBatch sp, SpriteFont font)
        {
            Rectangle _b = new Rectangle(s_slider.Width, 0, 400, 480);

            sp.Draw(manager.TSliderBackground, _b, Color.White);

            _b.X = 0;
            _b.Width = s_slider.Width;
            sp.Draw(manager.TSliderTrail, _b, Color.White);

            sp.Draw(manager.TSliderButton, s_slider, Color.White);

            float lerp = (float)((float)(s_slider.Y) / (float)(480.0f - s_slider.Height));
            float temp = (80 * types.Count - 480) * lerp;


            _b.X = s_slider.Width + 100;
            _b.Y = -(int)temp;

            _b.Width = 240;
            _b.Height = 60;

            for (int i = 0; i < types.Count; i++)
            {
                if (Available((byte)i))
                {
                    if (selected_id == i)
                        sp.Draw(manager.TRectangleLong, _b, Color.Yellow);
                    else
                        sp.Draw(manager.TRectangleLong, _b, Color.White);

                    sp.DrawString(font, Message((byte)i), new Vector2(_b.X + 15f, _b.Y + 7f), Color.White);
                }
                else
                {
                    sp.Draw(manager.TRectangleLong, _b, Color.White);
                    sp.DrawString(font, Message((byte)i), new Vector2(_b.X + 15f, _b.Y + 7f), Color.White);
                    sp.Draw(manager.TRectangleLock, _b, Color.White);
                }

                

                _b.Y += _b.Height + 20;
            }


            if(b_back.Pressed)
                b_back.Draw(sp, manager.TRectangle, Color.DarkGray);
            else
                b_back.Draw(sp, manager.TRectangle);

            sp.DrawString(font, "Back", new Vector2(b_back.X + 30f, b_back.Y + 15f), Color.White);

            if(b_play.Pressed)
                b_play.Draw(sp, manager.TRectangle, Color.DarkGray);
            else
                b_play.Draw(sp, manager.TRectangle);


            sp.DrawString(font, "Play", new Vector2(b_play.X + 30f, b_play.Y + 14f), Color.White);


        }

        private void DrawTransit(SpriteBatch sp, SpriteFont font)
        {
            float offset = MathHelper.Lerp(0, -400, cooldown);


            Rectangle _b = new Rectangle(s_slider.Width, 0, 400, 480);

            _b.X += (int)offset;

            sp.Draw(manager.TSliderBackground, _b, Color.White);

            _b.Width = s_slider.Width;
            sp.Draw(manager.TSliderTrail, _b, Color.White);

            s_slider.X = (int)offset;

            sp.Draw(manager.TSliderButton, s_slider, Color.White);

            float lerp = (float)((float)(s_slider.Y) / (float)(480.0f - s_slider.Height));
            float temp = (80 * types.Count - 480) * lerp;


            _b.X = s_slider.Width + 100 + (int)offset;
            _b.Y = -(int)temp;

            _b.Width = 240;
            _b.Height = 60;

            for (int i = 0; i < types.Count; i++)
            {
                if (Available((byte)i))
                {
                    if (selected_id == i)
                        sp.Draw(manager.TRectangleLong, _b, Color.Yellow);
                    else
                        sp.Draw(manager.TRectangleLong, _b, Color.White);
                }
                else
                {
                    sp.Draw(manager.TRectangleLong, _b, Color.White);
                    sp.Draw(manager.TRectangleLock, _b, Color.White);
                }

                sp.DrawString(font, Message((byte)i), new Vector2(_b.X + 10f, _b.Y + 5f), Color.White);

                _b.Y += _b.Height + 20;
            }


            _b.Width = b_back.W;
            _b.Height = b_back.H;

            _b.X = b_back.X - (int)offset;
            _b.Y = b_back.Y;

            b_back.Draw(sp, manager.TRectangle, _b, Color.White);

            sp.DrawString(font, "Back", new Vector2(_b.X + 15f, _b.Y + 10f), Color.White);

            _b.X = b_play.X - (int)offset;
            _b.Y = b_play.Y;

            b_play.Draw(sp, manager.TRectangle, _b, Color.White);

            sp.DrawString(font, "Play", new Vector2(_b.X + 15f, _b.Y + 10f), Color.White);


        }

        public void PlayGame()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_GAME, SCENE.SCENE_CUSTOM, true);
            }
        }

        public void Back()
        {
            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_MAIN_MENU, SCENE.SCENE_CUSTOM, true);
            }
        }

        private bool Available(byte type)
        {
            if (manager.Trial == false)
                return true;

            switch (types[type])
            {
                case GAME_TYPE.GAME_CALCULATE:
                    return true;
                case GAME_TYPE.GAME_GUESS_PERCENTAGE:
                    return false;
                case GAME_TYPE.GAME_MULTIPLY_BY:
                    return false;
                case GAME_TYPE.GAME_TRUE_FALSE:
                    return false;
                case GAME_TYPE.GAME_PATTERN:
                    return false;
                case GAME_TYPE.GAME_MEMORIZE_ITEMS:
                    return false;
                case GAME_TYPE.GAME_ORDER_CIRCLE:
                    return true;
                case GAME_TYPE.GAME_EVEN_OR:
                    return false;
                case GAME_TYPE.GAME_MEMORIZE_MATCH:
                    return false;
                case GAME_TYPE.GAME_FOCUS_ARROW:
                    return false;
                case GAME_TYPE.GAME_ORDER_NUMBERS:
                    return false;
                case GAME_TYPE.GAME_FALSE_CHARACTER:
                    return false;
                case GAME_TYPE.GAME_NUMBERS_CIRCLE:
                    return false;
                case GAME_TYPE.GMAE_ORDER_CAKES:
                    return false;
                case GAME_TYPE.GAME_MATCH_COLOR:
                    return true;
                case GAME_TYPE.GAME_FIGURES_MATCH:
                    return false;
            }

            return false;
        }

        private string Message(byte id)
        {


            switch (types[id])
            {
                case GAME_TYPE.GAME_CALCULATE:
                    return "Calculator";
                case GAME_TYPE.GAME_GUESS_PERCENTAGE:
                    return "Guess percent";
                case GAME_TYPE.GAME_MULTIPLY_BY:
                    return "Multiply by";
                case GAME_TYPE.GAME_TRUE_FALSE:
                    return "True or False";
                case GAME_TYPE.GAME_PATTERN:
                    return "Memorize Patter";
                case GAME_TYPE.GAME_MEMORIZE_ITEMS:
                    return "Memorize Items";
                case GAME_TYPE.GAME_ORDER_CIRCLE:
                    return "Order Circle";
                case GAME_TYPE.GAME_EVEN_OR:
                    return "Even or Vowel";
                case GAME_TYPE.GAME_MEMORIZE_MATCH:
                    return "Remember Match";
                case GAME_TYPE.GAME_FOCUS_ARROW:
                    return "Focus on arrow";
                case GAME_TYPE.GAME_ORDER_NUMBERS:
                    return "Ordered numbers";
                case GAME_TYPE.GAME_FALSE_CHARACTER:
                    return "False character";
                case GAME_TYPE.GAME_NUMBERS_CIRCLE:
                    return "Numbers in circle";
                case GAME_TYPE.GMAE_ORDER_CAKES:
                    return "Ordered cards";
                case GAME_TYPE.GAME_MATCH_COLOR:
                    return "Speed color";
                case GAME_TYPE.GAME_FIGURES_MATCH:
                    return "Figures match";

                default:
                    return "";
            }

        }

    }
}


