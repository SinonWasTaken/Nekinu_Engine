namespace Nekinu
{
    public class Vector2
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2()
        {
            x = 0;
            y = 0;
        }

        public Vector2(float value)
        {
            x = value;
            y = value;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 zero => new Vector2();

        public static Vector2 one => new Vector2(1);

        public float Length()
        {
            return (float)System.Math.Sqrt(x * x + y * y);
        }

        public Vector2 Normalize()
        {
            Vector2 thisVector = this;
            float length = Magnitude();

            if (length > 9.99999974737875E-06)
            {
                thisVector /= length;
            }
            else
            {
                thisVector = new Vector2();
            }

            return thisVector;
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float distance = 0;

            Vector2 c = new Vector2(a.x - b.x, a.y - b.y);

            distance = (float)System.Math.Sqrt(System.Math.Pow(c.x, 2f) + System.Math.Pow(c.y, 2f));

            return distance;
        }

        public float Magnitude()
        {
            return (float)System.Math.Sqrt(Dot());
        }

        public float Dot()
        {
            return x * x + y * y;
        }

        public Vector2 Negative()
        {
            return new Vector2(-x, -y);
        }

        public void ConvertSystemVector(System.Numerics.Vector2 v)
        {
            x = v.X;
            y = v.Y;
        }

        public static float Dot(Vector2 a, Vector2 b)
        {
            return (a.x * b.x) + (a.y * b.y);
        }

        public static Vector2 operator +(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x + right.x, left.y + right.y);
        }
        public static Vector2 operator +(Vector2 left, float right)
        {
            return new Vector2(left.x + right, left.y + right);
        }
        public static Vector2 operator +(float a, Vector2 b) => new Vector2(a + b.x, a + b.y);

        public static Vector2 operator -(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x - right.x, left.y - right.y);
        }
        public static Vector2 operator -(Vector2 left, float right)
        {
            return new Vector2(left.x - right, left.y - right);
        }
        public static Vector2 operator -(float a, Vector2 b) => new Vector2(a - b.x, a - b.y);

        public static Vector2 operator *(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x * right.x, left.y * right.y);
        }
        public static Vector2 operator *(Vector2 left, float right)
        {
            return new Vector2(left.x * right, left.y * right);
        }
        public static Vector2 operator *(float a, Vector2 b) => new Vector2(a * b.x, a * b.y);

        public static Vector2 operator /(Vector2 left, Vector2 right)
        {
            return new Vector2(left.x / right.x, left.y / right.y);
        }
        public static Vector2 operator /(Vector2 left, float right)
        {
            return new Vector2(left.x / right, left.y / right);
        }
        public static Vector2 operator /(float a, Vector2 b) => new Vector2(a / b.x, a / b.y);

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.x == right.x && left.y == right.y;
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return left.x != right.x || left.y != right.y;
        }

        public override string ToString()
        {
            return "x=" + x + ", y=" + y;
        }
    }
}