using System;

using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{

    public class GameScene : Scene
    {
        /// <summary>
        /// Timer used for games, by GameScene only.
        /// SetTime - also starts timer
        /// </summary>
        class Timer
        {
            Rectangle box;
            Vector2 text_pos;

            float cooldown;

            byte min;
            byte sec;

            bool finished;

            public Timer(Rectangle box)
            {
                this.box = box;

                text_pos.X = (float)box.X + 25f;
                text_pos.Y = (float)box.Y + 1.5f;

                min = 255;
                sec = 255;

                finished = false;

                cooldown = 0f;
            }

            public void Set(byte min, byte sec)
            {
                this.min = min;
                this.sec = sec;

                finished = false;

                cooldown = 0f;
            }

            public void Tick(float dt)
            {
                if (cooldown > 1f)
                {
                    if (sec > 0)
                    {
                        sec -= 1;
                    }
                    else
                    {
                        if (min > 0)
                        {
                            min -= 1;
                            sec = 60;
                        }
                        else
                        {
                            min = 0;
                            sec = 0;
                            finished = true;
                        }
                    }

                    cooldown = 0f;
                }

                cooldown += dt;
            }

            public byte Min
            {
                get { return min; }
            }

            public byte Sec
            {
                get { return sec; }
            }

            public bool Finished
            {
                get { return finished; }
                set { finished = value; }
            }

            public void Draw(SpriteBatch sp, SpriteFont font, Texture2D texture)
            {
                sp.Draw(texture, box, Color.White);
                sp.DrawString(font, "" + min + " : " + sec, text_pos, Color.White);
            }
        }
        
        GameInterface game;

        ContentManager content;

        Timer timer;

        ButtonGeneral b_start;
        ButtonGeneral b_continue;

        GAME_STATE game_state;
        TRANSIT_STATE transit_state;

        List<float> score_total;

        float cooldown;

        float time_rotation;

        float transit;

        byte transit_stage;

        bool start_state;

        bool use_time;

        enum GAME_SCENE_STATE
        {
            START_TRANSIT = 0,
            TUTORIAL_TRANSIT,
            SHOW_TUTORIAL,
            TUTORIAL_EXIT,
            GAME_PLAY,
            SHOW_RESULT,
            EXIT_SCENE,
        }

        GAME_SCENE_STATE state;

        public GameScene(SceneManager manager)
        {
            this.SceneManager = manager;   

            time_rotation = 0f;
            transit = 0f;

            start_state = true;

            timer = new Timer(new Rectangle(336, 0, 128, 40));

            use_time = false;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(manager.Game.Services, "Content/GameResources");

            b_start = new ButtonGeneral(new Rectangle(650, 100, 100, 100));
            b_continue = new ButtonGeneral(new Rectangle(272, 340, 256, 64));

            game = null;

            transit_stage = 255;

            use_time = true;

            score_total = new List<float>();
        }

        public override void UnloadContent()
        {
            if (content != null)
                content.Unload();

            game = null;

            timer = null;

            b_start = null;
            b_continue = null;

            score_total.Clear();
        }

        public override void Initialize()
        {

        }

        public override void Draw()
        {
            manager.SpriteBatch.Begin();

            manager.SpriteBatch.Draw(manager.TMainBackground, manager.GraphicsDevice.Viewport.Bounds, Color.White);
            
            game.Draw(manager.SpriteBatch, manager.Font24);



            if (state == GAME_SCENE_STATE.START_TRANSIT)
            {
                Rectangle _tb = new Rectangle(-800, 150, 800, 10);

                if (transit_stage == 0)
                {
                    _tb.X = (int)(MathHelper.Lerp(-800f, 0f, cooldown));
                }
                else if(transit_stage == 1)
                {
                    _tb.X = 0;
                    _tb.Height = (int)(MathHelper.Lerp(10f, 170f, cooldown));
                    _tb.Y = (int)(150.0f - (_tb.Height * 0.5f) * (cooldown));
                }

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);
            }
            else if (state == GAME_SCENE_STATE.TUTORIAL_EXIT)
            {
                Rectangle _tb = new Rectangle(-800, 150, 800, 10);

                if (transit_stage == 0)
                {
                    _tb.Height = (int)(MathHelper.Lerp(170f, 10f, cooldown));

                    _tb.Y = (int)(150.0f - (_tb.Height * 0.5f) * (1f - cooldown));
                    _tb.X = 0;
                }
                else if (transit_stage == 1)
                {
                    _tb.X = (int)(MathHelper.Lerp(0f, -800f, cooldown));
                }

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);
            }
            else if (state == GAME_SCENE_STATE.SHOW_TUTORIAL)
            {
                Rectangle _tb = new Rectangle(0, 150, 800, 170);

                _tb.Y = (int)(150.0f - 85);

                manager.SpriteBatch.Draw(manager.TMessageAlert, _tb, Color.White);

                if (b_start.Pressed)
                {
                    b_start.Draw(manager.SpriteBatch, manager.TAccept, Color.DarkGray);
                }
                else
                {
                    b_start.Draw(manager.SpriteBatch, manager.TAccept);
                }

                manager.SpriteBatch.DrawString(manager.Font, game.Description(), new Vector2(20, 100), Color.White);
            }
            else if (state == GAME_SCENE_STATE.SHOW_RESULT)
            {
                Rectangle _tb = new Rectangle(200, 40, 400, 400);

                manager.SpriteBatch.Draw(manager.TMessageBox, _tb, Color.White);

                Vector2 t = new Vector2(240, 80);

                manager.SpriteBatch.DrawString(manager.Font24, "Score: ", t, Color.Black);

                t.Y += 15f;

                for (byte i = 0; i < score_total.Count; i++)
                {
                    t.Y += 30f;

                    manager.SpriteBatch.DrawString(manager.Font, "" + score_total[i], t, Color.Black);
                }

                if(b_continue.Pressed)
                    b_continue.Draw(manager.SpriteBatch, manager.TContinue, Color.DarkGray);
                else
                    b_continue.Draw(manager.SpriteBatch, manager.TContinue);
            }

            if (use_time)
            {
                timer.Draw(manager.SpriteBatch, manager.Font, manager.TTimer);
            }

            manager.SpriteBatch.End();
        }

        public override void Update(float dt)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (game.State != GAME_STATE.GAME_TUTORIAL)
                {
                    if(!manager.CustomGame && !manager.Trial)
                        manager.SetScore(-1f);
                }
                ExitGame();
            }

            time_rotation += dt;

            if (time_rotation > 360f)
                time_rotation = 0f;

            for (byte i = 0; i < manager.Touches.Count; i++)
            {

                if (manager.Touches[i].State == TouchLocationState.Pressed)
                    Pressed(manager.Touches[i].Position);
                if (manager.Touches[i].State == TouchLocationState.Moved)
                    Moved(manager.Touches[i].Position);
                if (manager.Touches[i].State == TouchLocationState.Released)
                    Released(manager.Touches[i].Position);

            }

            if (state == GAME_SCENE_STATE.GAME_PLAY)
            {
                game.Update(dt);

                if (use_time)
                {
                    timer.Tick(dt);

                    if (timer.Finished)
                    {
                        ShowResults();
                    }
                }

            }
            else if (state == GAME_SCENE_STATE.START_TRANSIT)
            {
                if (transit_stage == 0)
                    cooldown += dt * 2f;
                else
                    cooldown += dt * 5f;

                if (cooldown > 1.0f)
                {
                    if (transit_stage == 0)
                    {
                        transit_stage = 1;
                        cooldown = 0f;
                    }
                    else
                    {
                        state = GAME_SCENE_STATE.SHOW_TUTORIAL;
                        cooldown = 0f;
                    }
                }
            }
            else if (state == GAME_SCENE_STATE.TUTORIAL_EXIT)
            {
                if (transit_stage == 0)
                    cooldown += dt * 2f;
                else
                    cooldown += dt * 5f;

                if (cooldown > 1.0f)
                {
                    if (transit_stage == 0)
                    {
                        transit_stage = 1;
                        cooldown = 0f;
                    }
                    else
                    {
                        state = GAME_SCENE_STATE.GAME_PLAY;
                        game.State = GAME_STATE.GAME_CREATE;
                        cooldown = 0f;
                    }
                }
            }

        }

        public void ShowResults()
        {
            score_total.Add(game.Score());

            if (!manager.CustomGame && !manager.Trial)
                manager.SetScore(game.Score());

            if (manager.FinishGame())
            {
                state = GAME_SCENE_STATE.SHOW_RESULT;
            }
            else
            {
                Activate();
            }


        }

        private void Pressed(Vector2 p)
        {
            if (state == GAME_SCENE_STATE.GAME_PLAY)
                game.Pressed(p);
            else if (state == GAME_SCENE_STATE.SHOW_TUTORIAL)
            {
                if (b_start.Collide(p))
                {

                }                
            }
            else if (state == GAME_SCENE_STATE.SHOW_RESULT)
            {
                b_continue.Collide(p);
            }
        }

        private void Moved(Vector2 p)
        {
            if (state == GAME_SCENE_STATE.GAME_PLAY)
                game.Moved(p);
            else if (state == GAME_SCENE_STATE.SHOW_TUTORIAL)
            {
                if (b_start.Collide(p))
                {

                }
            }
            else if (state == GAME_SCENE_STATE.SHOW_RESULT)
            {
                b_continue.Collide(p);
            }
        }

        private void Released(Vector2 p)
        {
            if(state == GAME_SCENE_STATE.GAME_PLAY)
                game.Released(p);
            else if (state == GAME_SCENE_STATE.SHOW_TUTORIAL)
            {
                if (b_start.Collide(p))
                {
                    state = GAME_SCENE_STATE.TUTORIAL_EXIT;
                    cooldown = 0.0f;
                    transit_stage = 0;
                    b_start.Pressed = false;
                }
            }
            else if (state == GAME_SCENE_STATE.SHOW_RESULT)
            {
                if (b_continue.Collide(p))
                {
                    ExitGame();
                }
                b_continue.Pressed = false;
            }
        }

        public override void Activate()
        {
            if (game != null)
            {
                game.Unload();
                game = null;
            }

            use_time = false;

            state = GAME_SCENE_STATE.START_TRANSIT;
            cooldown = 0f;
            transit_stage = 0;
            

            switch (manager.Type())
            {
                case GAME_TYPE.GAME_CALCULATE:
                    game = new GameCalculate(this);
                    break;
                case GAME_TYPE.GAME_MEMORIZE_ITEMS:
                    game = new GameMemorizeItems(this);
                    break;
                case GAME_TYPE.GAME_PATTERN:
                    game = new GamePattern(this);
                    break;
                case GAME_TYPE.GAME_ORDER_CIRCLE:
                    game = new GameOrderCircle(this);
                    break;
                case GAME_TYPE.GAME_EVEN_OR:
                    game = new GameEvenOr(this);
                    break;
                case GAME_TYPE.GAME_MEMORIZE_MATCH:
                    game = new GameMemorizeMatch(this);
                    break;
                case GAME_TYPE.GAME_FOCUS_ARROW:
                    game = new GameFocusArrow(this);
                    break;
                case GAME_TYPE.GAME_NUMBERS_CIRCLE:
                    game = new GameNumbersCircle(this);
                    break;
                case GAME_TYPE.GAME_ORDER_NUMBERS:
                    game = new GameOrderNumbers(this);
                    break;
                case GAME_TYPE.GAME_FALSE_CHARACTER:
                    game = new GameFalseCharacter(this);
                    break;
                case GAME_TYPE.GAME_FIGURES_MATCH:
                    game = new GameFiguresMatch(this);
                    break;
                case GAME_TYPE.GMAE_ORDER_CAKES:
                    game = new GameSpeedCakes(this);
                    break;
                case GAME_TYPE.GAME_MATCH_COLOR:
                    game = new GameColorMatch(this);
                    break;
                case GAME_TYPE.GAME_GUESS_PERCENTAGE:
                    game = new GameGuessPercentage(this);
                    break;
                case GAME_TYPE.GAME_TRUE_FALSE:
                    game = new GameTrueFalse(this);
                    break;
                case GAME_TYPE.GAME_MULTIPLY_BY:
                    game = new GameMultiplyBy(this);
                    break;
                default:
                    game = new GameCalculate(this);
                    break;
            }

            game.Load(manager.Game);
        }

        public void ExitGame()
        {
            state = GAME_SCENE_STATE.EXIT_SCENE;

            if (game != null)
            {
                game.Unload();
                game = null;
            }

            score_total.Clear();
            manager.ClearGames();

            GC.Collect();

            LoadingScene s = manager.Scene(SCENE.SCENE_LOADING) as LoadingScene;

            if (s != null)
            {
                s.Switch(SCENE.SCENE_MAIN_MENU, SCENE.SCENE_GAME, true);
            }
        }

        public void AddTimer(byte min, byte sec)
        {
            timer.Set(min, sec);
            use_time = true;
        }

        public void StartGameCurrent()
        {
            game.Load(manager.Game);
        }

        public SceneManager Manager
        {
            get { return manager; }
        }

        public Rectangle ViewBounds
        {
            get { return manager.Game.GraphicsDevice.Viewport.Bounds; }
        }
    }
}
