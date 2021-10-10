namespace Nekinu
{
    public class Vector3
    {
        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(float value)
        {
            x = value;
            y = value;
            z = value;
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float percent)
        {
            Vector3 lerp;

            lerp = (b - a);
            lerp *= percent;

            lerp += a;

            return new Vector3(System.Math.Abs(lerp.x), System.Math.Abs(lerp.y), System.Math.Abs(lerp.z));
        }

        public static Vector3 zero => new Vector3();

        public static Vector3 one => new Vector3(1);

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            float x = a.y * b.z - a.z * b.y;
            float y = a.z * b.x - a.x * b.z;
            float z = a.x * b.y - a.y * b.x;

            return new Vector3(x, y, z);
        }

        public float Dot()
        {
            return x * x + y * y + z * z;
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(Dot());
        }

        public float LengthSquared => (x * x) + (y * y) + (z * z);

        public float Magnitude()
        {
            return (float)System.Math.Sqrt(Dot());
        }

        public float Dot(Vector3 a)
        {
            return x * a.x + y * a.y + z * a.z;
        }

        public static float Dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public Vector3 Normalize()
        {
            Vector3 thisVector = new Vector3(x, y, z);
            float length = thisVector.Magnitude();

            if (length > 9.99999974737875E-06)
            {
                thisVector /= length;
            }
            else
            {
                thisVector = new Vector3();
            }

            return thisVector;
        }

        public void FromOpenTKVector3(OpenTK.Mathematics.Vector3 v)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }

        public void FromSystemVector(System.Numerics.Vector3 v)
        {
            x = v.X;
            y = v.Y;
            z = v.Z;
        }

        public OpenTK.Mathematics.Vector3 ToOpenTKVector3()
        {
            return new OpenTK.Mathematics.Vector3(x, y, z);
        }

        public static OpenTK.Mathematics.Vector3 ToOpenTKVector3(Vector3 v)
        {
            return new OpenTK.Mathematics.Vector3(v.x, v.y, v.z);
        }

        public static Vector3 Normalize(Vector3 vec)
        {
            return vec.Normalize();
        }

        public static Vector3 negative(Vector3 vector)
        {
            return new Vector3(-vector.x, -vector.y, -vector.z);
        }

        public static Vector3 up() => new Vector3(0, 1, 0);

        public static Vector3 forward => new Vector3(0, 0, 1);

        public static float Distance(Vector3 a, Vector3 b)
        {
            float distance = 0;

            Vector3 c = new Vector3(b.x - a.x, b.y - a.y, b.z - a.z);

            float x = (float)System.Math.Pow(c.x, 2f);
            float y = (float)System.Math.Pow(c.y, 2f);
            float z = (float)System.Math.Pow(c.z, 2f);

            distance = (float)System.Math.Sqrt(x + y + z);

            return distance;
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

        //FROM UNITY
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, Vector3 currentVelocity, float smoothTime, float maxSpeed)
        {
            smoothTime = System.Math.Max(0.0001f, smoothTime);
            float num1 = 2f / smoothTime;
            float num2 = num1 * Time.deltaTime;
            float num3 = (float)(1.0 / (1.0 + (double)num2 + 0.479999989271164 * (double)num2 * (double)num2 + 0.234999999403954 * (double)num2 * (double)num2 * (double)num2));
            float num4 = current.x - target.x;
            float num5 = current.y - target.y;
            float num6 = current.z - target.z;
            Vector3 vector3 = target;
            float num7 = maxSpeed * smoothTime;
            float num8 = num7 * num7;
            float num9 = (float)((double)num4 * (double)num4 + (double)num5 * (double)num5 + (double)num6 * (double)num6);
            if ((double)num9 > (double)num8)
            {
                float num10 = (float)System.Math.Sqrt(num9);
                num4 = num4 / num10 * num7;
                num5 = num5 / num10 * num7;
                num6 = num6 / num10 * num7;
            }
            target.x = current.x - num4;
            target.y = current.y - num5;
            target.z = current.z - num6;
            float num11 = (currentVelocity.x + num1 * num4) * Time.deltaTime;
            float num12 = (currentVelocity.y + num1 * num5) * Time.deltaTime;
            float num13 = (currentVelocity.z + num1 * num6) * Time.deltaTime;
            currentVelocity.x = (currentVelocity.x - num1 * num11) * num3;
            currentVelocity.y = (currentVelocity.y - num1 * num12) * num3;
            currentVelocity.z = (currentVelocity.z - num1 * num13) * num3;
            float x = target.x + (num4 + num11) * num3;
            float y = target.y + (num5 + num12) * num3;
            float z = target.z + (num6 + num13) * num3;
            float num14 = vector3.x - current.x;
            float num15 = vector3.y - current.y;
            float num16 = vector3.z - current.z;
            float num17 = x - vector3.x;
            float num18 = y - vector3.y;
            float num19 = z - vector3.z;
            if ((double)num14 * (double)num17 + (double)num15 * (double)num18 + (double)num16 * (double)num19 > 0.0)
            {
                x = vector3.x;
                y = vector3.y;
                z = vector3.z;
                currentVelocity.x = (x - vector3.x) / Time.deltaTime;
                currentVelocity.y = (y - vector3.y) / Time.deltaTime;
                currentVelocity.z = (z - vector3.z) / Time.deltaTime;
            }
            return new Vector3(x, y, z);
        }

        //FROM UNITY
        public static Vector3 SmoothDamp(Vector3 current, Vector3 target, Vector3 currentVelocity, float smoothTime)
        {
            float maxSpeed = float.PositiveInfinity;
            return SmoothDamp(current, target, currentVelocity, smoothTime, maxSpeed);
        }

        public static Vector3 Abs(Vector3 values)
        {
            return new Vector3(System.Math.Abs(values.x), System.Math.Abs(values.y), System.Math.Abs(values.z));
        }

        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x + right.x, left.y + right.y, left.z + right.z);
        }
        public static Vector3 operator +(Vector3 left, float right)
        {
            return new Vector3(left.x + right, left.y + right, left.z + right);
        }

        public static Vector3 operator -(Vector3 left)
        {
            return new Vector3(-left.x, -left.y, -left.z);
        }
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x - right.x, left.y - right.y, left.z - right.z);
        }
        public static Vector3 operator -(Vector3 left, float right)
        {
            return new Vector3(left.x - right, left.y - right, left.z - right); ;
        }

        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
        }
        public static Vector3 operator *(Vector3 left, float right)
        {
            return new Vector3(left.x * right, left.y * right, left.z * right);
        }
        public static Vector3 operator *(float left, Vector3 right)
        {
            return new Vector3(left * right.x, left * right.y, left * right.z);
        }

        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            return new Vector3(left.x / right.x, left.y / right.y, left.z / right.z);
        }
        public static Vector3 operator /(Vector3 left, float right)
        {
            return new Vector3(left.x / right, left.y / right, left.z / right);
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.x == right.x && left.y == right.y;
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return left.x != right.x || left.y != right.y;
        }

        public override string ToString()
        {
            return "x=" + x + ", y=" + y + ", z=" + z;
        }
    }
}