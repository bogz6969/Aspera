using Microsoft.Xna.Framework;
using SFML.System;

namespace Aspera
{
    internal static class Extensions
    {
        public static Vector2 Convert(this Vector2f a)
        {
            return new Vector2(a.X, a.Y);
        }

        public static Vector2f Convert(this Vector2 a)
        {
            return new Vector2f(a.X, a.Y);
        }
    }
}