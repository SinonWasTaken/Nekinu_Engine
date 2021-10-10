using ImGuiNET;
using System.Collections.Generic;
using Nekinu.EngineDebug;

namespace Nekinu.Editor
{
    [EditorType("Debug")]
    public class Debug : Editor
    {
        public override void Init() {}

        public override void Render()
        {
            List<DebugType> lines = EngineDebug.Debug.all_lines;
            ImGuiWindowFlags Debugflags = ImGuiWindowFlags.MenuBar;

            ImGui.Begin("Debug", Debugflags);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.Button("Clear"))
                {
                    EngineDebug.Debug.Clear();
                }

                ImGui.EndMenuBar();
            }

            if (lines.Count != 0)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].debug_type == DebugType.type.Red)
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(255, 0, 0, 255));
                    }
                    else
                    {
                        ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(255, 255, 255, 255));
                    }

                    ImGui.Text($"{lines[i].line}");
                    
                    ImGui.PopStyleColor(1);
                }
            }

            ImGui.End();
        }
    }
}
