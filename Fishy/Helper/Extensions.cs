using System.Numerics;

namespace Fishy.Helper
{
    public static class Vector3Extensions
    {
        public static float Dot(this Vector3 a, Vector3 b)
            =>  a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector3 Cross(this Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X
            );
        }

        public static float Magnitude(this Vector3 a)
            => (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);

        public static Vector3 Normalized(this Vector3 a)
        {
            float magnitude = a.Magnitude();
            if (magnitude == 0)
                throw new InvalidOperationException("Cannot normalize a zero vector.");
            return a / magnitude;
        }
    }

    public static class Vector2Extensions
    {
        public static float Magnitude(this Vector2 a)
            => (float)Math.Sqrt(a.X * a.X + a.Y * a.Y);


        public static Vector2 Normalized(this Vector2 a)
        {
            float magnitude = a.Magnitude();
            if (magnitude == 0)
                throw new InvalidOperationException("Cannot normalize a zero vector.");
            return new Vector2(a.X / magnitude, a.Y / magnitude);
        }

        public static float Angle(this Vector2 a)
            => (float)Math.Atan2(a.Y, a.X);

        public static float AngleInDegrees(this Vector2 a)
           => a.Angle() * (180f / (float)Math.PI); 


        public static Vector2 Rotate(this Vector2 a, float angle)
        {
            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            float newX = a.X * cosTheta - a.Y * sinTheta;
            float newY = a.X * sinTheta + a.Y * cosTheta;

            return new Vector2(newX, newY);
        }

        public static Vector2 RotateInDegrees(this Vector2 a, float angleDegrees)
        =>  a.Rotate(angleDegrees * ((float)Math.PI / 180f));
    }

}
