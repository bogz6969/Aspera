using SFML.System;

namespace Aspera
{
    public static class MathHelper
    {
        public static float Lerp(float firstFloat, float secondFloat, float by)
        {
            return firstFloat * (1 - by) + secondFloat * by;
        }

        public static Vector2f Lerp(Vector2f firstVector, Vector2f secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2f(retX, retY);
        }

        public static bool CloseEnough(float value1, float value2, float acceptableDifference)
        {
            return MathF.Abs(value1 - value2) <= acceptableDifference;
        }

        internal static bool CloseEnough(Vector2f v1, Vector2f v2, float diff)
        {
            return CloseEnough(v1.X, v2.X, diff) && CloseEnough(v1.Y, v2.Y, diff);
        }
    }
}