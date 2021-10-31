using ImGuiNET;
using Nekinu.Render;

namespace Nekinu.Editor
{
    [EditorType("Editor")]
    public class Statistics : IEditorPanel
    {
        public override void Init(){ }

        public override void Render()
        {
            ImGui.Begin("Statistics");
            ImGui.Text($"FPS: {Time.fps}");
            ImGui.Text($"Draw calls {StandardRenderer.DRAW_CALLS}");
            ImGui.Text($"Vertex count {StandardRenderer.VERTEX_COUNT}");
            ImGui.End();
        }
    }
}
