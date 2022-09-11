using System;
using Microsoft.Xna.Framework;

namespace No_Brainer
{
    public static class Utility
    {
        private static readonly Random random = new Random();

        public static float Random(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }

        public static bool RandomBool()
        {
            return random.Next(100) % 2 == 0;
        }

        public static bool PointVsRectangle(Rectangle b, Vector2 p)
        {
            if (p.X < b.X)
                return false;
            if (p.X > b.X + b.Width)
                return false;

            if (p.Y < b.Y)
                return false;
            if (p.Y > b.Y + b.Height)
                return false;

            return true;
        }

        public static bool PointVsRectangle(Rectangle b, float x, float y)
        {
            if (x < b.X)
                return false;
            if (x > b.X + b.Width)
                return false;

            if (y < b.Y)
                return false;
            if (y > b.Y + b.Height)
                return false;

            return true;
        }

        public static bool PointVsBox(Vector2 p_dot, Vector2 p_box, float size)
        {
            if (p_dot.X < p_box.X)
                return false;
            if (p_dot.X > p_box.X + size)
                return false;

            if (p_dot.Y < p_box.Y)
                return false;
            if (p_dot.Y > p_box.Y + size)
                return false;

            return true;
        }

        public static bool PointVsRectangle(Vector2 p_dot, Vector2 p_box, Vector2 p_bounds)
        {
            if (p_dot.X < p_box.X)
                return false;
            if (p_dot.X > p_box.X + p_bounds.X)
                return false;

            if (p_dot.Y < p_box.Y)
                return false;
            if (p_dot.Y > p_box.Y + p_bounds.Y)
                return false;


            return true;
        }

        public static bool BoxVsBox(Vector2 b_p1, float b_s1, Vector2 b_p2, float b_s2)
        {
            Vector2 b1 = new Vector2(b_p1.X, b_p1.Y);
            Vector2 b2 = new Vector2(b_p2.X, b_p2.Y);

            if (b1.X + b_s1 < b2.X)
                return false;

            if (b1.X > b2.X + b_s2)
                return false;

            if (b1.Y + b_s1 < b2.Y)
                return false;

            if (b1.Y > b2.Y + b_s2)
                return false;

            return true;
        }

        public static bool InCircle(Vector2 pos, float radie, Vector2 target)
        {
            float range = (pos - target).Length();

            return (range < radie) ? true : false;
        }


        public static bool CircleInRetangle(Vector2 pos_c, float radie, Vector2 pos_r, float w, float h)
        {
            Vector2 d = pos_c - pos_r;
            d.Normalize();

            Vector3 n = new Vector3(d.X, 0.0f, d.Y);
            Plane plane = new Plane(n, 50.0f);

            // box vs box for now

            float half_width = w * 0.5f,
                  half_height = h * 0.5f;

            if (pos_c.X < pos_r.X - half_width)
                return false;

            if (pos_c.X > pos_r.X + half_width)
                return false;

            if (pos_c.Y < pos_r.Y - half_height)
                return false;

            if (pos_c.Y > pos_r.Y + half_height)
                return false;

            return true;
        }

    }
}
