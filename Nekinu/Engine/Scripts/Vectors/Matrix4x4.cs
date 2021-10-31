using Nekinu.EngineDebug;
using OpenTK.Mathematics;

namespace Nekinu
{
    class Matrix4x4
    {
        public static Matrix4 entityTransformationMatrix(Entity Parent, Transform transform)
        {
            Matrix4 result;

            Matrix4 transformMatrix = Matrix4.CreateTranslation(transform.position.x, transform.position.y, transform.position.z);

            Matrix4 rot = CreateRotationMatrix(transform.qRotation);

            Debug.WriteLine(rot);
            
            Matrix4 scale = Matrix4.CreateScale(transform.scale.x, transform.scale.y, transform.scale.z);

            result = scale * rot * transformMatrix;

            if (Parent != null)
            {
                result *= Parent.transformationMatrix;
            }

            return result;
        }

        public static Matrix4 CreateRotationMatrix(OpenTK.Quaternion quaternion_rotation)
        {
            Debug.WriteLine(quaternion_rotation);
            Matrix4 rotation = new Matrix4();
            
            rotation.Row0.X = quaternion_rotation.W;
            rotation.Row0.Y = quaternion_rotation.Z;
            rotation.Row0.Z = -quaternion_rotation.Y;
            rotation.Row0.W = quaternion_rotation.X;

            rotation.Row1.X = -quaternion_rotation.Z;
            rotation.Row1.Y = quaternion_rotation.W;
            rotation.Row1.Z = quaternion_rotation.X;
            rotation.Row1.W = quaternion_rotation.Y;

            rotation.Row2.X = quaternion_rotation.Y;
            rotation.Row2.Y = -quaternion_rotation.X;
            rotation.Row2.Z = quaternion_rotation.W;
            rotation.Row2.W = quaternion_rotation.Z;
            
            rotation.Row3.X = -quaternion_rotation.X;
            rotation.Row3.Y = -quaternion_rotation.Y;
            rotation.Row3.Z = -quaternion_rotation.Z;
            rotation.Row3.W = quaternion_rotation.W;
            
            Matrix4 rotation2 = new Matrix4();
            
            rotation2.Row0.X = quaternion_rotation.W;
            rotation2.Row0.Y = quaternion_rotation.Z;
            rotation2.Row0.Z = -quaternion_rotation.Y;
            rotation2.Row0.W = -quaternion_rotation.X;

            rotation2.Row1.X = -quaternion_rotation.Z;
            rotation2.Row1.Y = quaternion_rotation.W;
            rotation2.Row1.Z = quaternion_rotation.X;
            rotation2.Row1.W = -quaternion_rotation.Y;

            rotation2.Row2.X = quaternion_rotation.Y;
            rotation2.Row2.Y = -quaternion_rotation.X;
            rotation2.Row2.Z = quaternion_rotation.W;
            rotation2.Row2.W = -quaternion_rotation.Z;
            
            rotation2.Row3.X = quaternion_rotation.X;
            rotation2.Row3.Y = quaternion_rotation.Y;
            rotation2.Row3.Z = quaternion_rotation.Z;
            rotation2.Row3.W = quaternion_rotation.W;
            
            return Matrix4.Mult(rotation, rotation2);
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
