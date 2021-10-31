using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImGuiNET;
using Nekinu.SystemCache;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nekinu.Editor
{
    public class ContentBrowser : IEditorPanel
    {
        private List<directory_info> directory_info;

        private string min_directory = @"./Asset/";

        private string current_directory;

        private int folder_image;
        private int item_image;

        private float padding = 15f;

        private bool something = true;
        
        private System.Numerics.Vector2 scale = new System.Numerics.Vector2(50, 50);
        
        public override void Init()
        {
            folder_image = Loader.loadTexture(@"./Resources/Images/Folder.png");
            item_image = Loader.loadTexture(@"./Resources/Images/Item.png");
            
            if (Directory.Exists(min_directory) == false)
            {
                DirectoryInfo info = Directory.CreateDirectory(min_directory);
            }

            current_directory = min_directory;

            directory_info = new List<directory_info>();
            
            update_directory();

            //delay_directory_update();
        }

        private void update_directory()
        {
            for (int i = 0; i < directory_info.Count; i++)
            {
                if(directory_info[i].remove_id_on_update)
                    Cache.RemoveTexture(directory_info[i].texture_id);
            }

            directory_info.Clear();
            directory_info = new List<directory_info>();
            
            string[] directories = Directory.GetDirectories(current_directory);
            
            string[] file = Directory.GetFiles(current_directory);

            List<string> all_files = new List<string>();
            
            all_files.AddRange(directories);
            all_files.AddRange(file);

            sort_dictionary(all_files);
        }
        
        public override void Render()
        {
            ImGui.Begin("Content Browser");

            bool open = true;
            
            if (ImGui.IsMouseClicked(ImGuiMouseButton.Right))
            {
                ImGui.OpenPopup("Add");
            }

            if (ImGui.BeginPopupModal("Add", ref open))
            {
                if (ImGui.Button("New Folder"))
                {
                    Directory.CreateDirectory(current_directory + "/New Folder");
                    ImGui.CloseCurrentPopup();
                    update_directory();
                }
                
                ImGui.EndPopup();
            }
            
            float cell_size = scale.X + padding;

            float panel_width = ImGui.GetContentRegionAvail().X;

            int column_count = (int) (panel_width / cell_size);

            ImGui.Columns(column_count, 0.ToString(), false);
            
            if (current_directory != min_directory)
            {
                if (ImGui.Button("Back", scale))
                {
                    List<string> lines = new List<string>();
                    lines.AddRange(current_directory.Replace('\\', '/').Split("/"));

                    string new_directory = "";
                    
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (i == lines.Count - 1)
                            break;
                        
                        new_directory += lines[i] + "/";
                    }

                    current_directory = new_directory;
                    update_directory();
                }
                
                ImGui.NextColumn();
            }
            
            for (int i = 0; i < directory_info.Count; i++)
            {
                string file = directory_info[i].file;

                int id = directory_info[i].texture_id;

                ImGui.ImageButton((IntPtr) id, scale);
                
                if (directory_info[i].is_directory)
                {
                    if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                    {
                        current_directory = file;
                        update_directory();
                    }
                }

                if (ImGui.IsItemHovered() && Input.isKeyPressed(Keys.Delete))
                {
                    if (directory_info[i].is_directory)
                    {
                        Directory.Delete(directory_info[i].file);
                    }
                    else
                    {
                        File.Delete(directory_info[i].file);
                    }
                    
                    update_directory();
                }
                
                string[] lines = file.Replace('\\', '/').Split("/");
                //ImGui.InputText("N", ref lines[lines.Length - 1], 300);
                //ImGui.Text(lines[lines.Length - 1]);
                ImGui.TextWrapped(lines[lines.Length - 1]);

                ImGui.NextColumn();
            }

            ImGui.Columns(1);

            ImGui.End();
        }

        private void sort_dictionary(List<string> all_files)
        {
            for (int i = 0; i < all_files.Count; i++)
            {
                if (check_if_file_in_dictionary(all_files[i]) == false)
                {
                    if (Directory.Exists(all_files[i]))
                    {
                        directory_info.Add(new directory_info(all_files[i], folder_image, true));
                    }
                    else
                    {
                        if (all_files[i].Contains(".png"))
                        {
                            int id = Loader.loadTexture(all_files[i]);

                            directory_info.Add(new directory_info(all_files[i], id, false));
                        }
                        else
                        {
                            directory_info.Add(new directory_info(all_files[i], item_image, false, false));
                        }
                    }
                }   
            }
        }
        
        private bool check_if_file_in_dictionary(string file)
        {
            if (directory_info.Count == 0)
                return false;
            
            
            for (int i = 0; i < directory_info.Count; i++)
            {
                if (file == directory_info[i].file)
                    return true;
            }

            return false;
        }

        private async Task<bool> delay_directory_update()
        {
            while (something)
            {
                await new WaitForSeconds(2).run();
                
                update_directory();
            }

            return true;
        }
    }

    internal class directory_info
    {
        internal string file { get; private set; }
        internal int texture_id { get; private set; }
        internal bool is_directory { get; private set; }
        internal bool remove_id_on_update { get; private set; }

        internal directory_info(string file, int id, bool isDirectory, bool remove_on_update = true)
        {
            this.file = file;
            texture_id = id;
            is_directory = isDirectory;

            if (isDirectory)
                remove_id_on_update = false;
            else
                remove_id_on_update = remove_on_update;
        }
    }
}