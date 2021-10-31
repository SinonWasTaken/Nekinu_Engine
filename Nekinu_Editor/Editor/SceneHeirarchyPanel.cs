using ImGuiNET;
using Nekinu.SceneManage;

namespace Nekinu.Editor
{
    [EditorType("Scene")]
    public class SceneHeirarchyPanel : IEditorPanel
    {
        public static SceneHeirarchyPanel Instance;

        public Entity selectedEntity { get; private set; }

        public override void Init()
        {
            Instance = this;
        }

        public override void Render()
        {
            ImGui.Begin("Scene Hierarchy");

            if (SceneManager.loadedScene != null)
            {
                if (SceneManager.loadedScene.sceneEntities.Count != 0)
                {
                    for (int i = 0; i < SceneManager.loadedScene.sceneEntities.Count; i++)
                    {
                        Entity entity = SceneManager.loadedScene.sceneEntities[i];

                        ImGuiTreeNodeFlags nodes = (selectedEntity == entity ? ImGuiTreeNodeFlags.Selected : 0) | ImGuiTreeNodeFlags.OpenOnArrow;
                        nodes |= ImGuiTreeNodeFlags.OpenOnDoubleClick;

                        bool isOpen = false;
                        
                        if (entity.children.Count > 0)
                        {
                            isOpen = ImGui.TreeNodeEx(entity.transform.name, nodes);
                        }
                        else
                        {
                            if (ImGui.Button(entity.transform.name))
                            {
                                if (ImGui.IsItemClicked())
                                {
                                    selectedEntity = entity;
                                }                                
                            }
                        }

                        if (ImGui.IsItemClicked())
                        {
                            selectedEntity = entity;
                        }
                        
                        if (isOpen)
                        {
                            setTreeDetail(entity);
                            ImGui.TreePop();
                        }

                        if (ImGui.IsMouseClicked(ImGuiMouseButton.Left) && ImGui.IsWindowHovered())
                            selectedEntity = null;
                    }
                }
            }

            ImGui.End();

            if (selectedEntity != null)
            {
                if (Input.isKeyPressed(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Delete))
                {
                    if (selectedEntity.parent != null)
                        selectedEntity.parent.RemoveChild(selectedEntity);
                    else
                        SceneManager.loadedScene?.RemoveEntity(selectedEntity);

                    selectedEntity = null;
                }
            }
        }

        private void setTreeDetail(Entity entity)
        {
            if (entity.children.Count != 0)
            {
                for (int c = 0; c < entity.children.Count; c++)
                {
                    ImGuiTreeNodeFlags nodes = (selectedEntity == entity.children[c] ? ImGuiTreeNodeFlags.Selected : 0) | ImGuiTreeNodeFlags.OpenOnArrow;

                    bool childOpen = ImGui.TreeNodeEx($"{entity.children[c].transform.name}", nodes);

                    if (ImGui.IsItemClicked())
                    {
                        selectedEntity = entity.children[c];
                    }

                    if (childOpen)
                    {
                        setTreeDetail(entity.children[c]);

                        ImGui.TreePop();
                    }
                }
            }
        }
    }
}