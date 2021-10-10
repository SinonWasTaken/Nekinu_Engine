using ImGuiNET;

namespace Nekinu.Editor
{
    public class ProjectInfo : Editor
    {
        private string project_name;
        private string project_developer;
        public override void Init()
        {
            project_name = ProjectDetails.projectName;
            project_developer = ProjectDetails.projectDeveloper;
        }

        public override void Render()
        {
            ImGui.Begin("Project details");
            
            ImGui.InputText("Project name", ref project_name, 256);
            ImGui.InputText("Project developer", ref project_developer, 256);

            if (ImGui.Button("Save"))
            {
                ProjectDetails.projectName = project_name;
                ProjectDetails.projectDeveloper = project_developer;
            }

            if (ImGui.Button("Exit"))
            {
                EditorRenderer.removeEditor(this);
            }

            ImGui.End();
        }
    }
}