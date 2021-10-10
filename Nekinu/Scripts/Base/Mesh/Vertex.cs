using System.Collections.Generic;

namespace Nekinu
{
    internal class Vertex
    {
        private static int NO_INDEX = -1;

        private Vector3 position;
        private int textureIndex = NO_INDEX;
        private int normalIndex = NO_INDEX;
        private Vertex duplicateVertex = null;
        private int index;
        private float length;
        private List<Vector3> tangents = new List<Vector3>();
        private Vector3 averagedTangent = Vector3.zero;

        public Vertex(int index, Vector3 position)
        {
            this.index = index;
            this.position = position;
            this.length = position.Length();
        }

        public void addTangent(Vector3 tangent)
        {
            tangents.Add(tangent);
        }

        public void averageTangents()
        {
            if (tangents.Count != 0)
            {
                return;
            }
            foreach (Vector3 tangent in tangents)
            {
                averagedTangent += tangent;
            }
            averagedTangent.Normalize();
        }

        public Vector3 getAverageTangent()
        {
            return averagedTangent;
        }

        public int getIndex()
        {
            return index;
        }

        public float getLength()
        {
            return length;
        }

        public bool isSet()
        {
            return textureIndex != NO_INDEX && normalIndex != NO_INDEX;
        }

        public bool hasSameTextureAndNormal(int textureIndexOther, int normalIndexOther)
        {
            return textureIndexOther == textureIndex && normalIndexOther == normalIndex;
        }

        public void setTextureIndex(int textureIndex)
        {
            this.textureIndex = textureIndex;
        }

        public void setNormalIndex(int normalIndex)
        {
            this.normalIndex = normalIndex;
        }

        public Vector3 getPosition()
        {
            return position;
        }

        public int getTextureIndex()
        {
            return textureIndex;
        }

        public int getNormalIndex()
        {
            return normalIndex;
        }

        public Vertex getDuplicateVertex()
        {
            return duplicateVertex;
        }

        public void setDuplicateVertex(Vertex duplicateVertex)
        {
            this.duplicateVertex = duplicateVertex;
        }
    }
}