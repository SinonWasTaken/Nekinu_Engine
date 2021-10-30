using Nekinu.Editor;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;

namespace Nekinu
{
    public class Camera : Component
    {
        [JsonIgnore]
        public static Camera MainCamera { get; set; }

        public enum CameraProjection { Perspective, Orthographic }

        [JsonProperty]
        public CameraProjection cameraProjection { get; set; }

        [JsonProperty] [SerializedProperty] private float fov;

        [JsonProperty] [SerializedProperty] private float near;
        [JsonProperty] [SerializedProperty] private float far;

        [JsonProperty] [SerializedProperty] private float orthoSizeX;
        [JsonProperty] [SerializedProperty] private float orthoSizeY;

        [JsonIgnore]
        public Matrix4 projection { get; set; }

        private Matrix4 view;

        public Color4 color;

        public Camera() { }

        public Camera(float fov, float near, float far)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(fov * Math.ToRadians, Window.aspectRatio, near, far);
            cameraProjection = CameraProjection.Perspective;

            color = new Color4(20f / 255f, 97f / 255f, 161f / 255f, 1f);

            this.fov = fov;
            this.near = near;
            this.far = far;
        }

        public Camera(float fov, float near, float far, Color4 color)
        {
            projection = Matrix4.CreatePerspectiveFieldOfView(fov * Math.ToRadians, Window.aspectRatio, near, far);
            cameraProjection = CameraProjection.Perspective;

            this.color = color;

            this.fov = fov;
            this.near = near;
            this.far = far;
        }

        public Camera(float orthoSizeX, float orthoSizeY, float near, float far)
        {
            projection = Matrix4.CreateOrthographic(orthoSizeX, orthoSizeY, near, far);
            cameraProjection = CameraProjection.Orthographic;

            color = new Color4(20f / 255f, 97f / 255f, 161f / 255f, 1f);

            this.orthoSizeX = orthoSizeX; this.orthoSizeY = orthoSizeY;
            this.near = near;
            this.far = far;
        }

        public Camera(float orthoSizeX, float orthoSizeY, float near, float far, Color4 color)
        {
            projection = Matrix4.CreateOrthographic(orthoSizeX, orthoSizeY, near, far);
            cameraProjection = CameraProjection.Orthographic;

            this.color = color;

            this.orthoSizeX = orthoSizeX; this.orthoSizeY = orthoSizeY;
            this.near = near;
            this.far = far;
        }

        public override void Awake()
        {
            base.Awake();

            if (projection == Matrix4.Zero)
            {
                projection = cameraProjection == CameraProjection.Perspective ? Matrix4.CreatePerspectiveFieldOfView(fov * Math.ToRadians, Window.aspectRatio, near, far) : Matrix4.CreateOrthographic(orthoSizeX, orthoSizeY, near, far);
            }
        }

        [JsonIgnore][UpdateInEditor]
        public Matrix4 View
        {
            get
            {
                return Matrix4x4.cameraTransformationMatrix(parent.parent != null ? parent.parent : null, parent?.transform);
            }
        }
    }
}