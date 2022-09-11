using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using Microsoft.Xna.Framework.Content;

namespace No_Brainer
{
    public abstract class GameInterface
    {
        protected ContentManager content = null;
        

        protected GameScene game_scene;
        protected byte score;
        protected byte right_count;
        protected byte right_count_row;

        protected GAME_STATE game_state;

        protected float _stat_right;
        protected float _stat_wrong;

        // Used for different info
        public virtual void Load(Game game)
        {

        }

        public virtual void Unload()
        {
            //content.Unload();
        }


        public virtual void Draw(SpriteBatch sp, SpriteFont font)
        {

        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Pressed(Vector2 p)
        {

        }

        public virtual void Moved(Vector2 p)
        {

        }

        public virtual void Released(Vector2 p)
        {

        }

        public float Score()
        {
            if (_stat_right == 0f)
                return 1f;
            if (_stat_wrong == 0)
                return _stat_right + 100f;

            return _stat_right + 100f / _stat_wrong;
        }

        public virtual string Description()
        {
            return "";
        }

        public GAME_STATE State
        {
            get { return game_state; }
            set { game_state = value; }
        }

    }
}
