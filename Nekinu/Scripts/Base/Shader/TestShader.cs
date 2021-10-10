using OpenTK.Mathematics;
using System.Collections.Generic;
using Nekinu.Shaders;

namespace Nekinu
{
    class TestShader : Shader
    {
        private int location_cameraProjection;
        private int location_cameraView;
        private int location_entityTransformMatrix;

        private const int lightLimit = 4;

        private int location_SunPosition;
        private int location_SunColor;
        private int location_SunIntensity;

        private int[] location_LightPosition;
        private int[] location_LightColor;
        private int[] location_LightAttenuation;
        private int[] location_LightIntensity;

        public TestShader(string vertexShader, string fragmentShader) : base(vertexShader, fragmentShader)
        { }

        public override void BindAttributes()
        {
            BindAttribute(0, "position");
            BindAttribute(1, "textureCords");
            BindAttribute(2, "normals");
        }

        public override void GetAllUniformLocations()
        {
            location_LightPosition = new int[lightLimit];
            location_LightColor = new int[lightLimit];
            location_LightAttenuation = new int[lightLimit];
            location_LightIntensity = new int[lightLimit];

            for (int i = 0; i < lightLimit; i++)
            {
                location_LightPosition[i] = GetUniformLocation("lightPosition[" + i + "]");
                location_LightColor[i] = GetUniformLocation("lightColor[" + i + "]");
                location_LightAttenuation[i] = GetUniformLocation("lightAttenuation[" + i + "]");
                location_LightIntensity[i] = GetUniformLocation("lightIntensity[" + i + "]");
            }

            location_cameraProjection = GetUniformLocation("projection");
            location_cameraView = GetUniformLocation("view");
            location_entityTransformMatrix = GetUniformLocation("entity");
        }

        public void LoadSun(Entity sunObj)
        {
            if (sunObj == null)
            {
                Uniform3f(location_SunPosition, new Vector3(0, 5, 0));
                Uniform3f(location_SunColor, new Vector3(0,0,1));
                Uniform1f(location_SunIntensity, 1);
            }
            else
            {
                Sun sun = sunObj.GetComponent<Sun>();

                Uniform3f(location_SunPosition, sunObj.transform.position);
                Uniform3f(location_SunColor, sun.lightColor);
                Uniform1f(location_SunIntensity, sun.intensity);
            }
        }

        public void LoadLights(List<Entity> lights)
        {
            if (lights.Count != 0)
            {
                for (int i = 0; i < lightLimit; i++)
                {
                    if (i < lights.Count)
                    {
                        AreaLight light = lights[i].GetComponent<AreaLight>();

                        this.lights(location_LightPosition[i], location_LightColor[i], location_LightAttenuation[i], location_LightIntensity[i], lights[i].transform.position, light.lightColor, light.attenuation, light.intensity);
                    }
                    else
                    {
                        this.lights(location_LightPosition[i], location_LightColor[i], location_LightAttenuation[i], location_LightIntensity[i], new Vector3(0, 0, 1), new Vector3(0, 0, 1), Vector3.one, 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lightLimit; i++)
                {
                    this.lights(location_LightPosition[i], location_LightColor[i], location_LightAttenuation[i], location_LightIntensity[i], new Vector3(0, 0, 1), new Vector3(0, 0, 1), Vector3.one, 1);
                }
            }
        }

        private void lights(int location1, int location2, int location3, int location4, Vector3 pos, Vector3 color, Vector3 atten, float inten)
        {
            Uniform3f(location1, pos);
            Uniform3f(location2, color);
            Uniform3f(location3, atten);
            Uniform1f(location4, inten);
        }

        public void LoadTransformationMatrix(Matrix4 entity)
        {
            UniformMatrix4(location_entityTransformMatrix, entity);
        }

        public void LoadCameraView(Matrix4 view)
        {
            UniformMatrix4(location_cameraView, view);
        }

        public void LoadCameraProjection(Matrix4 projection)
        {
            UniformMatrix4(location_cameraProjection, projection);
        }

        public override void doShaderLoad(Entity self, List<Entity> scene_entities)
        {
            
        }

        public override void loadCamera(Camera camera)
        {
            throw new System.NotImplementedException();
        }
    }
}