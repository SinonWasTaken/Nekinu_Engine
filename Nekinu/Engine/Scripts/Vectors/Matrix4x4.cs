using OpenTK.Mathematics;

namespace Nekinu
{
    class Matrix4x4
    {
        public static Matrix4 entityTransformationMatrix(Entity Parent, Transform transform)
        {
            Matrix4 result;

            Matrix4 transformMatrix = Matrix4.CreateTranslation(transform.position.x, transform.position.y, transform.position.z);

            Matrix4 rot = CreateRotationMatrix(transform.rotation.x, transform.rotation.y, transform.rotation.z);

            Matrix4 scale = Matrix4.CreateScale(transform.scale.x, transform.scale.y, transform.scale.z);

            result = scale * rot * transformMatrix;

            if (Parent != null)
            {
                result *= Parent.transformationMatrix;
            }

            return result;
        }

        public static Matrix4 CreateRotationMatrix(float x, float y, float z)
        {
            Matrix4 rotx = Matrix4.CreateRotationX(x * Math.ToRadians);
            Matrix4 roty = Matrix4.CreateRotationY(y * Math.ToRadians);
            Matrix4 rotz = Matrix4.CreateRotationZ(z * Math.ToRadians);

            return rotx * roty * rotz;
        }

        public static Matrix4 entityTransformationMatrix(Transform transform)
        {
            return entityTransformationMatrix(null, transform);
        }

        public static Matrix4 cameraTransformationMatrix(Transform transform)
        {
            return cameraTransformationMatrix(null, transform);
        }

        public static Matrix4 cameraTransformationMatrix(Entity Parent, Transform transform)
        {
            Matrix4 result;

            Matrix4 transformMatrix = Matrix4.CreateTranslation(-transform.position.x, -transform.position.y, -transform.position.z);

            Matrix4 rotx = Matrix4.CreateRotationX(-transform.rotation.x * Math.ToRadians);
            Matrix4 roty = Matrix4.CreateRotationY(-(transform.rotation.y - 180) * Math.ToRadians);
            Matrix4 rotz = Matrix4.CreateRotationZ(-transform.rotation.z * Math.ToRadians);

            Matrix4 rot = rotx * roty * rotz;

            result = transformMatrix * rot;

            return result;
        }
    }
}