using System;
using System.Collections.Generic;
using System.Text;

namespace Nekinu
{
    public class LayerSystem
    {
        private static List<Layer> layerNames = new List<Layer>();

        public static void InitLayer()
        {
            layerNames.Add(new Layer("Default"));
        }

        public static void addLayer(string name)
        {
            if(!layerAlreadyExists(name))
                layerNames.Add(new Layer(name));
        }

        private static bool layerAlreadyExists(string name)
        {
            for (int i = 0; i < layerNames.Count; i++)
            {
                if(layerNames[i].layer_name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public static void removeLayer(Layer layer)
        {
            layerNames.Remove(layer);
        }

        public static Layer getLayer(string name)
        {
            for (int i = 0; i < layerNames.Count; i++)
            {
                if (layerNames[i].layer_name == name)
                    return layerNames[i];
            }

            return null;
        }
    }
}