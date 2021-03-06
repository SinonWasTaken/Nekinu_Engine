using ImGuiNET;
using Nekinu.SceneManage;
using System;
using System.Collections.Generic;
using System.IO;
using FileBrowser;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nekinu.Editor
{
    internal class DockPanel : IEditorPanel
    {
        private Dictionary<string, List<IEditorPanel>> editor_tabs;

        public static bool isPlaying = false;

        ImGuiWindowFlags window_flags;

        public override void Init() 
        {
            editor_tabs = new Dictionary<string, List<IEditorPanel>>();
            sortEditors();

            window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;

            window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            window_flags |= ImGuiWindowFlags.NoBackground;
        }

        public override void Render()
        {
            ImGuiViewportPtr port = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(port.Pos);
            ImGui.SetNextWindowSize(port.Size);

            ImGui.SetNextWindowViewport(port.ID);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, System.Numerics.Vector2.Zero);
            bool open = true;

            ImGui.Begin("DockSpace", ref open, window_flags);
            ImGui.PopStyleVar();
            ImGui.PopStyleVar(2);

            uint id = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(id);

            ImGui.DockSpaceOverViewport(port);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File", true))
                {
                    if (ImGui.MenuItem("Save"))
                    {
                        if (!isPlaying)
                            SceneManager.Save();
                        else
                            EngineDebug.Debug.WriteLine("Cannot save while game is running");
                    }
                    else if (ImGui.MenuItem("New Scene"))
                    {
                        SceneManager.NewScene();
                    }
                    else if (ImGui.MenuItem("Open Scene"))
                    {
                        Dictionary<Stream, String> file = OpenFile.getFile("Open scene", ".scene", Directory.GetCurrentDirectory());
                        
                        if (file != null)
                            SceneManager.LoadSceneInfo(file);
                    }

                    else if (ImGui.MenuItem("Scene List"))
                    {
                        //ADD SCENE LIST EDITOR
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Edit", true))
                {
                    if(ImGui.MenuItem("Project"))
                    {
                        EditorRenderer.addEditor(new ProjectInfo());
                    }
                    ImGui.EndMenu();
                }
                if(ImGui.BeginMenu("Entity", true))
                {
                    if(ImGui.MenuItem("Add empty entity"))
                    {
                        if(SceneManager.loadedScene != null)
                        {
                            if (SceneHeirarchyPanel.Instance.selectedEntity != null)
                            {
                                Entity entity = SceneHeirarchyPanel.Instance.selectedEntity;
                                entity.AddChild(new Entity(new Transform("child")));
                            }
                            else
                            {
                                SceneManager.loadedScene.AddEntity(new Entity(new Transform("Empty")));
                            }
                        }
                    }

                    if (ImGui.MenuItem("Cube"))
                    {
                        Entity entity = new Entity(new Transform("Entity"));
                        
                        Mesh mesh = new Mesh();
                        mesh.Location = $@"./Resources/Models/Cube.txt";
                        
                        entity.AddComponent(mesh);

                        if (SceneHeirarchyPanel.Instance.selectedEntity != null)
                        {
                            SceneHeirarchyPanel.Instance.selectedEntity.AddChild(entity);
                        }
                        else
                        {
                            SceneManager.loadedScene?.AddEntity(entity);
                        }
                    }

                    ImGui.EndMenu();
                }
                if(ImGui.BeginMenu("Windows", true))
                {
                    foreach (string key in editor_tabs.Keys)
                    {
                        List<IEditorPanel> tab = editor_tabs.GetValueOrDefault(key);

                        if (ImGui.TreeNodeEx(key))
                        {
                            for (int i = 0; i < tab?.Count; i++)
                            {
                                if (ImGui.Button(tab?[i].GetType().Name))
                                {
                                    if (!EditorRenderer.hasEditor(tab?[i]))
                                    {
                                        EditorRenderer.addEditor(tab?[i]);
                                    }
                                }
                            }

                            ImGui.TreePop();
                        }
                    }
                }
            }

            if (Input.isKeyDown(Keys.LeftControl) && Input.isKeyPressed(Keys.P))
            {
                isPlaying = !isPlaying;

                if (isPlaying)
                {
                    SceneManager.BeingPlay();
                }
                else
                {
                    SceneManager.EndPlay();
                }
            }

            if(Input.isKeyDown(Keys.LeftControl) && Input.isKeyPressed(Keys.S))
            {
                if (SceneManager.state != SceneState.Playing)
                    SceneManager.Save();
                else
                    EngineDebug.Debug.WriteLine("Cannot save while game is running");
            }

            ImGui.End();
        }

        private void sortEditors()
        {
            for (int i = 0; i < EditorList.allEditors.Count; i++)
            {
                EditorTypeAttribute type = (EditorTypeAttribute)Attribute.GetCustomAttribute(EditorList.allEditors[i].GetType(), typeof(EditorTypeAttribute));

                if(type != null)
                {
                    string typeString = type.editorType;

                    List<IEditorPanel> tab = editor_tabs.GetValueOrDefault(typeString);

                    if (tab == null)
                    {
                        List<IEditorPanel> newTab = new List<IEditorPanel>();
                        newTab.Add(EditorList.allEditors[i]);
                        editor_tabs.Add(typeString, newTab);
                    }
                    else
                    {
                        tab.Add(EditorList.allEditors[i]);
                    }
                }
                else
                {
                    List<IEditorPanel> tab = editor_tabs.GetValueOrDefault("None");

                    if (tab == null)
                    {
                        List<IEditorPanel> newTab = new List<IEditorPanel>();
                        newTab.Add(EditorList.allEditors[i]);
                        editor_tabs.Add("None", newTab);
                    }
                    else
                    {
                        tab.Add(EditorList.allEditors[i]);
                    }
                }
            }
        }
    }
}
