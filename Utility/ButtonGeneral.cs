using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input.Touch;

namespace No_Brainer
{
    class ButtonGeneral
    {
        Rectangle box;

        Rectangle box_original;

        float scale;
        float rotate;

        bool pressed;

        public ButtonGeneral()
        {
            this.box_original = this.box = new Rectangle();

            this.scale = 1.0f;
            this.pressed = false;
        }

        public ButtonGeneral(Rectangle box)
        {
            this.box_original = this.box = box;

            this.scale = 1.0f;
            this.pressed = false;
        }

        public ButtonGeneral(Rectangle box, float scale)
        {
            this.box_original = this.box = box;

            this.scale = scale;
            this.pressed = false;
        }

        public void ScaleButton(float s)
        {
            s += scale;

            float sx = (float)(box.X + box_original.Width - (box_original.Width * scale) * 0.5f);
            float sy = (float)(box.Y + box_original.Height - (box_original.Height * scale) * 0.5f);

            float sw = (float)(box_original.Width * scale);
            float sh = (float)(box_original.Height * scale);

            box.X = (int)sx;
            box.Y = (int)sy;

            box.Width = (int)sw;
            box.Height = (int)sh;
        }


        public void Draw(SpriteBatch sp, Texture2D texture)
        {
            sp.Draw(texture, box, Color.White);
        }

        public void Draw(SpriteBatch sp, Texture2D texture, Color color)
        {
            sp.Draw(texture, box, color);
        }

        public void Draw(SpriteBatch sp, Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            sp.Draw(texture, destinationRectangle, color);
        }

        public void Draw(SpriteBatch sp, Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            sp.Draw(texture, destinationRectangle, sourceRectangle, color);
        }
        
        public void Draw(SpriteBatch sp, Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            sp.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public bool Collide(Vector2 p)
        {
            if (p.X < box.X)
            {
                pressed = false;
                return false;
            }


            if (p.X > box.X + box.Width)
            {
                pressed = false;
                return false;
            }

            if (p.Y < box.Y)
            {
                pressed = false;
                return false;
            }

            if (p.Y > box.Y + box.Height)
            {
                pressed = false;
                return false;
            }

            pressed = true;

            return true;
        }

        public Rectangle Box
        {
            get { return box; }
            set { box = value; }
        }

        public int X
        {
            get { return box.X; }
            set { box.X = value; }
        }

        public int Y
        {
            get { return box.Y; }
            set { box.Y = value; }
        }

        public int W
        {
            get { return box.Width; }
            set { box.Width = value; }
        }

        public int H
        {
            get { return box.Height; }
            set { box.Height = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }
    }
}
