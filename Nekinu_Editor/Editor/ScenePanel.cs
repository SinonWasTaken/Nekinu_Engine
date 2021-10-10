using ImGuiNET;
using System;
using Nekinu.Render;

namespace Nekinu
{
    [EditorType("Scene")]
    public class ScenePanel : Editor.Editor
    {
        public Texture texture;

        private ImGuiWindowFlags flags;

        public override void Init() 
        {
            texture = new Texture("/Resources/Textures/White.png");

            flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoTitleBar;
        }

        public override void Render()
        {
            ImGui.Begin("Scene Panel", flags);

            System.Numerics.Vector2 size = ImGui.GetWindowSize();

            ImGui.Image((IntPtr)MasterRenderer.buffer.colorBuffer, new System.Numerics.Vector2(size.X, size.Y), new System.Numerics.Vector2(1), new System.Numerics.Vector2(0));
            ImGui.End();
        }
    }
}

