using Nekinu.Editor;
using Newtonsoft.Json;
using System;
using OpenTK;

namespace Nekinu
{
    public class Transform
    {
        public enum TransformTag { Default, Player, MainCamera }

        [JsonProperty]
        private TransformTag tag { get; set; }

        [JsonProperty]
        public string name;

        [JsonProperty]
        public Vector3 position { get; set; }
        [JsonProperty]
        public Vector3 rotation { get; set; }
        [JsonProperty]
        public Vector3 scale { get; set; }

        private Quaternion qRotation;

        public Transform()
        {
            initTransform("Entity", Vector3.zero, Vector3.zero, Vector3.one);
        }

        public Transform(Vector3 position)
        {
            initTransform("Entity", position, Vector3.zero, Vector3.one);
        }

        public Transform(Vector3 position, Vector3 rotation)
        {
            initTransform("Entity", position, rotation, Vector3.one);
        }

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            initTransform("Entity", position, rotation, scale);
        }

        public Transform(string name)
        {
            initTransform(name, Vector3.zero, Vector3.zero, Vector3.one);
        }

        public Transform(string name, Vector3 position)
        {
            initTransform(name, position, Vector3.zero, Vector3.one);
        }

        public Transform(string name, Vector3 position, Vector3 rotation)
        {
            initTransform(name, position, rotation, Vector3.one);
        }

        public Transform(string name, Vector3 position, Vector3 rotation, Vector3 scale)
        {
            initTransform(name, position, rotation, scale);
        }

        private void initTransform(string n, Vector3 p, Vector3 r, Vector3 s)
        {
            name = n;
            position = p;
            rotation = r;
            scale = s;

            qRotation = Quaternion.FromEulerAngles(rotation);
        }

        public Vector3 forward
        {
            get
            {
                float x = rotation.x;
                float y = rotation.y;
                float z = rotation.z;

                if (x < -360) 
                {
                    x %= -360;
                }
                if (y < -360)
                {
                    y %= -360;
                }
                if (z < -360)
                {
                    z %= -360;
                }

                if (x < 0)
                {
                    x = 360 + x;
                }
                if (y < 0)
                {
                    y = 360 + y;
                }
                if (z < 0)
                {
                    z = 360 + z;
                }

                qRotation = Quaternion.FromEulerAngles(new Vector3(x, y, z));

                return qRotation * Vector3.forward;
            }
        }

        public override string ToString()
        {
            return "Transform: Position " + position.ToString() + " Rotation " + rotation.ToString() + " Scale " + scale.ToString();
        }
    }
}