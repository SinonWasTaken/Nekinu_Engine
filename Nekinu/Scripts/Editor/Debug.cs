using ImGuiNET;
using System.Collections.Generic;

namespace Nekinu.Editor
{
    [EditorType("Other")]
    public class Debug : Editor
    {
        private static List<string> lines = new List<string>();

        public override void Init()
        {
            lines.Clear();
        }

        public override void Render()
        {
            ImGuiWindowFlags Debugflags = ImGuiWindowFlags.MenuBar;

            //ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(250, 0, 0, 255));

            ImGui.Begin("Debug", Debugflags);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.Button("Clear"))
                {
                    Clear();
                }

                ImGui.EndMenuBar();
            }

            if(lines.Count != 0)
            for (int i = 0; i < lines.Count; i++)
            {
                ImGui.Text($"{lines[i]}");
            }

            ImGui.End();

            //ImGui.PopStyleColor(1);
        }

        public static void WriteLine(int value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(uint value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(sbyte value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(byte value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(short value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(ushort value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(float value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(double value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(string value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(char value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(object value)
        {
            lines.Add($"{value}");
        }

        public static void WriteLine(Vector2 value)
        {
            lines.Add($"{value.x} : {value.y}");
        }

        public static void WriteLine(Vector3 value)
        {
            lines.Add($"{value.x} : {value.y} : {value.z}");
        }

        private void Clear()
        {
            lines.Clear();
        }
    }
}
