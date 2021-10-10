using OpenTK.Mathematics;

namespace Nekinu
{
    public class Vector4
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public float w { get; set; }

        public Vector4()
        {
            x = 0;
            y = 0;
            z = 0;
            w = 0;
        }

        public Vector4(float value)
        {
            x = value;
            y = value;
            z = value;
            w = value;
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector4 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
            w = other.w;
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float percent)
        {
            Vector4 lerp;

            lerp = (b - a);
            lerp *= percent;

            lerp += a;

            return new Vector4(System.Math.Abs(lerp.x), System.Math.Abs(lerp.y), System.Math.Abs(lerp.z), System.Math.Abs(lerp.w));
        }

        public static Vector4 zero => new Vector4();

        public static Vector4 one => new Vector4(1);

        public float Dot()
        {
            return x * x + y * y + z * z + w * w;
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(Dot());
        }

        public float Dot(Vector4 a)
        {
            return x * a.x + y * a.y + z * a.z + w * a.w;
        }

        public static float Dot(Vector4 a, Vector4 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public void ConvertSystemVector(System.Numerics.Vector4 v)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
            w = v.W;
        }

        public Vector4 Normalize()
        {
            Vector4 thisVector = new Vector4(x, y, z, w);
            float length = thisVector.Magnitude();

            if (length > 9.99999974737875E-06)
            {
                thisVector /= length;
            }
            else
            {
                thisVector = new Vector4();
            }

            return thisVector;
        }

        public static Vector4 Normalize(Vector4 vec)
        {
            return vec.Normalize();
        }

        public static Vector4 Negative(Vector4 vector)
        {
            return new Vector4(-vector.x, -vector.y, -vector.z, -vector.w);
        }

        public static Vector4 Up()
        {
            return new Vector4(0, 1, 0, 0);
        }

        public static Vector4 Forward()
        {
            return new Vector4(0, 0, 1, 0);
        }

        public static float Distance(Vector4 a, Vector4 b)
        {
            float distance = 0;

            Vector4 c = new Vector4(b.x - a.x, b.y - a.y, b.z - a.z, b.w - a.w);

            float x = (float)System.Math.Pow(c.x, 2f);
            float y = (float)System.Math.Pow(c.y, 2f);
            float z = (float)System.Math.Pow(c.z, 2f);

            distance = (float)System.Math.Sqrt(x + y + z);

            return distance;
        }

        public static Vector4 mulMat4(Matrix4 mat, Vector4 vec)
        {
            float x, y, z, w;
            x = mat.Column0.X * vec.x + mat.Column1.X * vec.y + mat.Column2.X * vec.z + mat.Column3.X * vec.w;
            y = mat.Column0.Y * vec.x + mat.Column1.Y * vec.y + mat.Column2.Y * vec.z + mat.Column3.Y * vec.w;
            z = mat.Column0.Z * vec.x + mat.Column1.Z * vec.y + mat.Column2.Z * vec.z + mat.Column3.Z * vec.w;
            w = mat.Column0.W * vec.x + mat.Column1.W * vec.y + mat.Column2.W * vec.z + mat.Column3.W * vec.w;

            return new Vector4(x, y, z, w);
        }

        /***
         * This method doesn't seem to work. Look into later
         * @param a
         * @param b
         * @return
         */
        /*public static float angle(Vector3 a, Vector3 b)
        {
            Vector3 one = Vector3.multiply(a, b);

            float two = Math.sqrt(Math.pow(a.x, 2) + Math.pow(a.y, 2) + Math.pow(a.z, 2));
            float three = Math.sqrt(Math.pow(b.x, 2) + Math.pow(b.y, 2) + Math.pow(b.z, 2));

            float four = Vector3.dot(a, b);

            float five = four / (two * three);

            return Math.toDegrees((float)java.lang.Math.acos(five));

            /*float num = Math.sqrt(a.dot() * b.dot());

            return num < 1.00000000362749E-15 ? 0.0f : (float) (java.lang.Math.acos(Math.clamp(Vector3.dot(a, b) / num, -1, 1)) * 57.29578f);
        }*/

        public float Magnitude()
        {
            return (float)System.Math.Sqrt(Dot());
        }

        public static Vector4 Abs(Vector4 values)
        {
            return new Vector4(System.Math.Abs(values.x), System.Math.Abs(values.y), System.Math.Abs(values.z), System.Math.Abs(values.w));
        }

        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x + right.x, left.y + right.y, left.z + right.z, left.w + right.w);
        }
        public static Vector4 operator +(Vector4 left, float right)
        {
            return new Vector4(left.x + right, left.y + right, left.z + right, left.w + right);
        }

        public static Vector4 operator -(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x - right.x, left.y - right.y, left.z - right.z, left.w - right.w);
        }
        public static Vector4 operator -(Vector4 left, float right)
        {
            return new Vector4(left.x - right, left.y - right, left.z - right, left.w - right);
        }

        public static Vector4 operator *(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x * right.x, left.y * right.y, left.z * right.z, left.w * right.w);
        }
        public static Vector4 operator *(Vector4 left, float right)
        {
            return new Vector4(left.x * right, left.y * right, left.z * right, left.w * right);
        }

        public static Vector4 operator /(Vector4 left, Vector4 right)
        {
            return new Vector4(left.x / right.x, left.y / right.y, left.z / right.z, left.w / right.w);
        }
        public static Vector4 operator /(Vector4 left, float right)
        {
            return new Vector4(left.x / right, left.y / right, left.z / right, left.w / right);
        }

        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.x == right.x && left.y == right.y && left.w == right.w;
        }

        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return left.x != right.x || left.y != right.y || left.w != right.w;
        }

        public override string ToString()
        {
            return "x=" + x + ", y=" + y + ", z=" + z + ", w=" + w;
        }
    }
}