using ImGuiNET;
using Nekinu;
using System;
using System.Windows;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nekinu.Editor;
using Nekinu.SceneManage;

internal class FilePicker : Editor
{
    private System.Numerics.Vector2 initalSize = new System.Numerics.Vector2(500, 500);

    private ImGuiWindowFlags windowFlags;

    private string currentDirectory;

    private string currentFile;

    private string[] fileTypesToLookFor = new string[0];

    [STAThread]
    public override void Init()
    {
        windowFlags = ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar;

        currentDirectory = Directory.GetCurrentDirectory();
    }

    public void setFilesTypes(params string[] fileTypes)
    {
        fileTypesToLookFor = fileTypes;
    }

    //DEBUG. Have a method to return the string of the file or directory
    public override void Render()
    {
        ImGui.Begin("File Picker", windowFlags);
        bool open = ImGui.TreeNodeEx("File");
        if (open)
        {
            string[] files = Directory.GetFiles(currentDirectory);
            string[] directories = Directory.GetDirectories(currentDirectory);

            for (int i = 0; i < directories.Length; i++)
            {
                ImGui.Text(directories[i]);

                if (ImGui.IsItemClicked())
                {
                    currentDirectory = directories[i];
                }
            }

            for (int i = 0; i < files.Length; i++)
            {
                if (isCorrectFileType(files[i]))
                {
                    ImGui.Text(files[i]);

                    if (ImGui.IsItemClicked())
                    {
                        currentFile = files[i];

                        SceneManager.LoadSceneInfo(currentFile);

                        EditorRenderer.removeEditor(this);
                    }
                }
            }


            if (ImGui.Button("X"))
            {
                EditorRenderer.removeEditor(this);
            }
        }

        ImGui.TreePop();
        ImGui.End();
    }

    private bool isCorrectFileType(string file)
    {
        if (fileTypesToLookFor.Length == 0)
            return true;

        for (int i = 0; i < fileTypesToLookFor.Length; i++)
        {
            if (file.Contains(fileTypesToLookFor[i]))
                return true;
        }

        return false;
    }
}