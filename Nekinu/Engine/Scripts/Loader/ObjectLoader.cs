using System;
using System.Collections.Generic;
using System.IO;
using Assimp;
using Nekinu.Editor;
using Nekinu.EngineDebug;
using Scene = Assimp.Scene;

namespace Nekinu
{
    class ObjectLoader
    {
        public static Mesh loadOBJ(string objFile)
        {
            Mesh loadedMesh = Cache.MeshExists(objFile);
            if (loadedMesh != null)
            {
                return loadedMesh;
            }

            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + objFile);

            string line = "";
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            float[] verticesArray = new float[0];
            float[] normalsArray = new float[0];
            float[] texturesArray = new float[0];
            int[] indicesArray = new int[0];

            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("v "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));

                        vertices.Add(new Vertex(vertices.Count, vertex));

                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector2 texture = new Vector2(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 normal = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                        normals.Add(normal);
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] currentLine = line.Split(" ");
                        string[] vertex1 = currentLine[1].Split("/");
                        string[] vertex2 = currentLine[2].Split("/");
                        string[] vertex3 = currentLine[3].Split("/");

                        processVertex(vertex1, vertices, indices);
                        processVertex(vertex2, vertices, indices);
                        processVertex(vertex3, vertices, indices);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteError($"Couldn't read model file: {objFile}. Error {e}");
                return null;
            }

            reader.Close();

            removeUnusedVertices(vertices);
            verticesArray = new float[vertices.Count * 3];
            texturesArray = new float[vertices.Count * 2];
            normalsArray = new float[vertices.Count * 3];
            float furthest = convertDataToArrays(vertices, textures, normals, verticesArray, texturesArray, normalsArray);
            indicesArray = convertIndicesListToArray(indices);
            Mesh m = Loader.loadModel(objFile, verticesArray, texturesArray, normalsArray, indicesArray);

            return m;
        }

        public static Mesh loadAnimatedOBJ(string objFile)
        {
            Mesh loadedMesh = Cache.MeshExists(objFile);
            if (loadedMesh != null)
            {
                return loadedMesh;
            }

            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + objFile);

            string line = "";
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            float[] verticesArray = new float[0];
            float[] normalsArray = new float[0];
            float[] texturesArray = new float[0];
            int[] indicesArray = new int[0];

            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("v "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));

                        vertices.Add(new Vertex(vertices.Count, vertex));

                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector2 texture = new Vector2(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("vn "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 normal = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                        normals.Add(normal);
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] currentLine = line.Split(" ");
                        string[] vertex1 = currentLine[1].Split("/");
                        string[] vertex2 = currentLine[2].Split("/");
                        string[] vertex3 = currentLine[3].Split("/");

                        processVertex(vertex1, vertices, indices);
                        processVertex(vertex2, vertices, indices);
                        processVertex(vertex3, vertices, indices);
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteError($"Couldn't read model file: {objFile}. Error {e}");
                return null;
            }

            reader.Close();

            removeUnusedVertices(vertices);
            verticesArray = new float[vertices.Count * 3];
            texturesArray = new float[vertices.Count * 2];
            normalsArray = new float[vertices.Count * 3];
            float furthest = convertDataToArrays(vertices, textures, normals, verticesArray, texturesArray, normalsArray);
            indicesArray = convertIndicesListToArray(indices);
            Mesh m = new Mesh(objFile, indicesArray.Length);

            return m;
        }

        public static Mesh loadUIOBJ(string objFile)
        {
            Mesh loadedMesh = Cache.MeshExists(objFile);
            if (loadedMesh != null)
            {
                return loadedMesh;
            }

            StreamReader reader = new StreamReader(Directory.GetCurrentDirectory() + objFile);

            String line = "";
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();
            List<int> indices = new List<int>();

            float[] verticesArray = null;
            float[] texturesArray = null;
            int[] indicesArray = null;

            try
            {
                line = reader.ReadLine();

                while (line != null)
                {
                    line = reader.ReadLine();

                    if (line.StartsWith("v "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                        vertices.Add(new Vertex(vertices.Count, vertex));

                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector2 texture = new Vector2(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("f "))
                    {
                        while (line != null && line.StartsWith("f "))
                        {
                            string[] currentLine = line.Split(" ");
                            string[] vertex1 = currentLine[1].Split("/");
                            string[] vertex2 = currentLine[2].Split("/");
                            string[] vertex3 = currentLine[3].Split("/");

                            processVertex(vertex1, vertices, indices);
                            processVertex(vertex2, vertices, indices);
                            processVertex(vertex3, vertices, indices);

                            line = reader.ReadLine();
                        }
                    }
                }

                reader.Close();
            }
            catch (IOException e)
            {
                Debug.WriteError("Couldn't read model file: " + objFile);
                Environment.Exit(-1);
            }

            removeUnusedVertices(vertices);
            verticesArray = new float[vertices.Count * 3];
            texturesArray = new float[vertices.Count * 2];
            float furthest = convertDataToArrays(vertices, textures, verticesArray, texturesArray);
            indicesArray = convertIndicesListToArray(indices);
            Mesh m = Loader.loadModel(objFile, verticesArray, texturesArray, indicesArray);

            return m;
        }

        public static Mesh loadTextOBJ(string objFile)
        {
            Mesh loadedMesh = Cache.MeshExists(objFile);
            if (loadedMesh != null)
            {
                return loadedMesh;
            }

            StreamReader reader = new StreamReader(ProjectDetails.rootDirectory + objFile);

            String line = "";
            List<Vertex> vertices = new List<Vertex>();
            List<Vector2> textures = new List<Vector2>();

            float[] verticesArray = null;
            float[] texturesArray = null;

            float[] points = new float[6];

            try
            {
                line = reader.ReadLine();

                while (line != null)
                {
                    line = reader.ReadLine();

                    if (line.StartsWith("v "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector3 vertex = new Vector3(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]), float.Parse(currentLine[3]));

                        Console.WriteLine(points[0] + " " + points[1] + " " + points[2] + " " + points[3] + " " + points[4] + " " + points[5]);

                        vertices.Add(new Vertex(vertices.Count, vertex));

                    }
                    else if (line.StartsWith("vt "))
                    {
                        string[] currentLine = line.Split(" ");
                        Vector2 texture = new Vector2(float.Parse(currentLine[1]),
                                float.Parse(currentLine[2]));
                        textures.Add(texture);
                    }
                    else if (line.StartsWith("f "))
                    {
                        while (line != null && line.StartsWith("f "))
                        {
                            string[] currentLine = line.Split(" ");
                            string[] vertex1 = currentLine[1].Split("/");
                            string[] vertex2 = currentLine[2].Split("/");
                            string[] vertex3 = currentLine[3].Split("/");

                            line = reader.ReadLine();
                        }
                    }
                }

                reader.Close();
            }
            catch (IOException e)
            {
                Debug.WriteError("Couldn't read model file: " + objFile);
                Environment.Exit(-1);
            }

            removeUnusedVertices(vertices);
            verticesArray = new float[vertices.Count * 3];
            texturesArray = new float[vertices.Count * 2];
            float furthest = convertDataToArrays(vertices, textures, verticesArray, texturesArray);

            Mesh m = Loader.loadModel(objFile, verticesArray, texturesArray);

            return m;
        }

        private static Vertex processVertex(string[] vertex, List<Vertex> vertices, List<int> indices)
        {
            int index = int.Parse(vertex[0]) - 1;
            Vertex currentVertex = vertices[index];
            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;

            if (!currentVertex.isSet())
            {
                currentVertex.setTextureIndex(textureIndex);
                currentVertex.setNormalIndex(normalIndex);
                indices.Add(index);
                return currentVertex;
            }
            else
            {
                return dealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices, vertices);
            }
        }

        private static int[] convertIndicesListToArray(List<int> indices)
        {
            int[] indicesArray = new int[indices.Count];
            for (int i = 0; i < indicesArray.Length; i++)
            {
                indicesArray[i] = indices[i];
            }
            return indicesArray;
        }

        private static float convertDataToArrays(List<Vertex> vertices, List<Vector2> textures, List<Vector3> normals,
                                                 float[] verticesArray, float[] texturesArray, float[] normalsArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];

                if (currentVertex.getLength() > furthestPoint)
                {
                    furthestPoint = currentVertex.getLength();
                }

                Vector3 position = currentVertex.getPosition();
                Vector2 textureCoord = textures[currentVertex.getTextureIndex()];
                Vector3 normalVector = normals[currentVertex.getNormalIndex()];
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;
                normalsArray[i * 3] = normalVector.x;
                normalsArray[i * 3 + 1] = normalVector.y;
                normalsArray[i * 3 + 2] = normalVector.z;

            }
            return furthestPoint;
        }

        private static float convertDataToArrays(List<Vertex> vertices, List<Vector2> textures,
                                                 float[] verticesArray, float[] texturesArray)
        {
            float furthestPoint = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertex currentVertex = vertices[i];

                if (currentVertex.getLength() > furthestPoint)
                {
                    furthestPoint = currentVertex.getLength();
                }

                Vector3 position = currentVertex.getPosition();
                Vector2 textureCoord = textures[currentVertex.getTextureIndex()];
                verticesArray[i * 3] = position.x;
                verticesArray[i * 3 + 1] = position.y;
                verticesArray[i * 3 + 2] = position.z;
                texturesArray[i * 2] = textureCoord.x;
                texturesArray[i * 2 + 1] = 1 - textureCoord.y;

            }
            return furthestPoint;
        }

        private static Vertex dealWithAlreadyProcessedVertex(Vertex previousVertex, int newTextureIndex, int newNormalIndex,
                                                             List<int> indices, List<Vertex> vertices)
        {
            if (previousVertex.hasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertex.getIndex());
                return previousVertex;
            }
            else
            {
                Vertex anotherVertex = previousVertex.getDuplicateVertex();
                if (anotherVertex != null)
                {
                    return dealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex, indices,
                            vertices);
                }
                else
                {
                    Vertex duplicateVertex = new Vertex(vertices.Count, previousVertex.getPosition());
                    duplicateVertex.setTextureIndex(newTextureIndex);
                    duplicateVertex.setNormalIndex(newNormalIndex);
                    previousVertex.setDuplicateVertex(duplicateVertex);
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.getIndex());
                    return duplicateVertex;
                }
            }
        }

        private static void removeUnusedVertices(List<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                vertex.averageTangents();
                if (!vertex.isSet())
                {
                    vertex.setTextureIndex(0);
                    vertex.setNormalIndex(0);
                }
            }
        }

        public static void loadFBX(string name)
        {
            PostProcessSteps s =
                PostProcessSteps.FindInvalidData |
                PostProcessSteps.FlipUVs |
                PostProcessSteps.FlipWindingOrder |
                PostProcessSteps.JoinIdenticalVertices |
                PostProcessSteps.ImproveCacheLocality |
                PostProcessSteps.OptimizeMeshes |
                PostProcessSteps.RemoveRedundantMaterials |
                PostProcessSteps.Triangulate;
            
            using (var importer = new AssimpContext())
            {
                Scene scene = importer.ImportFile(ProjectDetails.rootDirectory + name, s);

                /*foreach (var mesh in scene.Meshes)
                {
                    foreach (var vertex in mesh.Vertices)
                    {
                        Debug.WriteLine(vertex.ToString());
                    }
                    foreach (var face in mesh.Faces)
                    {
                    }
                }*/
            }
        }
    }
}