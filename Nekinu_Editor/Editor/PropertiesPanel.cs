using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nekinu.Editor
{
    [EditorType("Other")]
    public class PropertiesPanel : Editor
    {
        private int selected;
        private bool buttonSelected;

        private List<Component> comp = new List<Component>();
        private List<string> string_comp = new List<string>();

        public override void Init()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type component in assembly.GetTypes().Where(my => my.IsClass && !my.IsAbstract && my.IsSubclassOf(typeof(Component))))
                {
                    string[] lines = component.ToString().Split(".");
                    string type_name = lines[lines.Length - 1];
                    string_comp.Add(type_name);
                    comp.Add((Component)Activator.CreateInstance(component));
                }
            }
        }

        public override void Render()
        {
            ImGui.Begin("Properties");

            if (SceneHeirarchyPanel.Instance != null)
            {
                if (SceneHeirarchyPanel.Instance.selectedEntity != null)
                {
                    Entity entity = SceneHeirarchyPanel.Instance.selectedEntity;
                    ImGui.InputText("Name", ref entity.transform.name, (uint)256);

                    ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow;

                    if (ImGui.TreeNodeEx("Transform", flags))
                    {
                        bool value = entity.isActive;
                        ImGui.Checkbox("Active", ref value);
                        entity.isActive = value;

                        System.Numerics.Vector3 v = new System.Numerics.Vector3(entity.transform.position.x, entity.transform.position.y, entity.transform.position.z);
                        ImGui.DragFloat3("Position", ref v);

                        entity.transform.position.FromSystemVector(v);

                        v = new System.Numerics.Vector3(entity.transform.rotation.x, entity.transform.rotation.y, entity.transform.rotation.z);
                        ImGui.DragFloat3("Rotation", ref v);

                        entity.transform.rotation.FromSystemVector(v);

                        v = new System.Numerics.Vector3(entity.transform.scale.x, entity.transform.scale.y, entity.transform.scale.z);
                        ImGui.DragFloat3("Scale", ref v);

                        entity.transform.scale.FromSystemVector(v);

                        ImGui.TreePop();
                    }

                    if(entity.components.Count != 0)
                    {
                        for (int i = 0; i < entity.components.Count; i++)
                        {
                            Component c = entity.components[i];

                            string[] lines = c.GetType().ToString().Split(".");

                            if (ImGui.TreeNodeEx(lines[lines.Length - 1], flags))
                            {
                                Type t = c.GetType();

                                drawFields(c, t);
                                drawPrivateField(c, t);
                                //drawProperties(c, t);

                                ImGui.TreePop();
                            }
                        }
                    }

                    if(ImGui.Button("Add Component", new System.Numerics.Vector2(50, 50)))
                    {
                        buttonSelected = !buttonSelected;
                    }

                    if (buttonSelected)
                    {
                        drawComponentAdd();
                        //buttonSelected = false;
                    }
                }
            }

            ImGui.End();
        }

        private void drawComponentAdd()
        {
            ImGui.Begin("Component Add");

            ImGui.Combo("Components", ref selected, string_comp.ToArray(), string_comp.Count);

            if(ImGui.Button("Add Component"))
            {
                SceneHeirarchyPanel.Instance.selectedEntity.AddComponent(comp[selected]);
                buttonSelected = false;
            }

            if (ImGui.Button("None"))
            {
                buttonSelected = false;
            }

            ImGui.End();
        }

        private void drawProperties(Component c, Type t)
        {
            for (int prop = 0; prop < t.GetProperties().Length; prop++)
            {
                PropertyInfo info = t.GetProperties()[prop];

                PropertyInfo[] infos = t.GetProperties();

                if (info.PropertyType == typeof(int))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        int v = (int)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                        ImGui.DragInt($"{info.Name}", ref v);

                        infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                    }
                }
                else if (info.PropertyType == typeof(string))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        string v = (string)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                        ImGui.LabelText($"{info.Name}", v);

                        infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                    }
                }
                else if (info.PropertyType == typeof(float))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        float v = (float)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                        ImGui.DragFloat($"{info.Name}", ref v, 0.1f);

                        infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                    }
                }
                else if (info.PropertyType == typeof(Vector2))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        Vector2 ve = (Vector2)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                        System.Numerics.Vector2 v = new System.Numerics.Vector2(ve.x, ve.y);
                        ImGui.DragFloat2($"{info.Name}", ref v, 0.1f);

                        ve.ConvertSystemVector(v);
                        infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                    }
                }
                else if (info.PropertyType == typeof(Vector3))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        Vector3 ve = (Vector3)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);
                        System.Numerics.Vector3 v = new System.Numerics.Vector3(ve.x, ve.y, ve.z);

                        ImGui.DragFloat3($"{info.Name}", ref v, 0.1f);

                        ve.FromSystemVector(v);
                        infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                    }
                }
                else if (info.PropertyType == typeof(Vector4))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        Vector4 ve = (Vector4)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);

                        System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                        ImGui.DragFloat4($"{info.Name}", ref v, 0.1f);

                        ve.ConvertSystemVector(v);

                        infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                    }
                }
                else if (info.PropertyType == typeof(Color4))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        Color4 ve = (Color4)infos.Single(pi => pi.Name == info.Name).GetValue(c, null);

                        System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                        ImGui.ColorPicker4($"{info.Name}", ref v);

                        ve.ConvertSystemVector(v);
                        infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                    }
                }
                else if (info.PropertyType == typeof(bool))
                {
                    if (infos.Single(pi => pi.Name == info.Name).GetSetMethod() != null)
                    {
                        bool value = (bool)infos.Single(pi => pi.Name == info.Name).GetValue(c);

                        ImGui.Checkbox($"{info.Name}", ref value);

                        infos.Single(pi => pi.Name == info.Name).SetValue(c, value);
                    }
                }
                else if (info.PropertyType == typeof(Enum))
                {
                    //ImGui.Combo()
                }
            }
        }

        private void checkFieldType(Component c, FieldInfo info, FieldInfo[] infos)
        {
            try
            {
                if (info.FieldType == typeof(int))
                {
                    int v = (int) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    ImGui.DragInt($"{info.Name}", ref v);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
                else if (info.FieldType == typeof(string))
                {
                    string v = (string) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    ImGui.InputText("Text", ref v, (uint) v.Length);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
                else if (info.FieldType == typeof(float))
                {
                    float v = (float) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    ImGui.DragFloat($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
                else if (info.FieldType == typeof(Vector2))
                {
                    Vector2 ve = (Vector2) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    System.Numerics.Vector2 v = new System.Numerics.Vector2(ve.x, ve.y);
                    ImGui.DragFloat2($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                    ve.ConvertSystemVector(v);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
                else if (info.FieldType == typeof(Vector3))
                {
                    Vector3 ve = (Vector3) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    System.Numerics.Vector3 v = new System.Numerics.Vector3(ve.x, ve.y, ve.z);
                    ImGui.DragFloat3($"{info.Name}", ref v, 0.001f, float.MinValue, float.MaxValue);

                    ve = new Vector3(v.X, v.Y, v.Z);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
                else if (info.FieldType == typeof(Vector4))
                {
                    Vector4 ve = (Vector4) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                    ImGui.DragFloat4($"{info.Name}", ref v, 0.001f, float.MaxValue, float.MaxValue);

                    ve.ConvertSystemVector(v);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
                else if (info.FieldType == typeof(Color4))
                {
                    Color4 ve = (Color4) infos.Single(pi => pi.Name == info.Name).GetValue(c);

                    System.Numerics.Vector4 v = new System.Numerics.Vector4(ve.x, ve.y, ve.z, ve.w);
                    ImGui.ColorPicker4($"{info.Name}", ref v);

                    ve.ConvertSystemVector(v);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, ve);
                }
                else if (info.FieldType == typeof(bool))
                {
                    bool value = (bool) infos.Single(pi => pi.Name == info.Name).GetValue(c);

                    ImGui.Checkbox($"{info.Name}", ref value);
                    infos.Single(pi => pi.Name == info.Name).SetValue(c, value);
                }
                else if (info.FieldType == typeof(Enum))
                {
                    Enum v = (Enum) infos.Single(pi => pi.Name == info.Name).GetValue(c);
                    Type type = v.GetType();
                    string[] enum_value = Enum.GetNames(type);
                    ImGui.Combo("Enum", ref selected, enum_value, enum_value.Length);

                    infos.Single(pi => pi.Name == info.Name).SetValue(c, v);
                }
            }
            catch (Exception e)
            {
                EngineDebug.Debug.WriteError($"Error drawing editor field: {e}");
            }
        }
        
        private void drawFields(Component c, Type t)
        {
            FieldInfo[] infos = t.GetFields();
            
            for (int prop = 0; prop < infos.Length; prop++)
            {
                FieldInfo info = infos[prop];

                checkFieldType(c, info, infos);
            }
        }
        
        private void drawPrivateField(Component component, Type type)
        {
            FieldInfo[] privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            
            for (int i = 0; i < privateFields.Length; i++)
            {
                FieldInfo info = privateFields[i];

                if (Attribute.IsDefined(info, typeof(SerializedPropertyAttribute)))
                {
                    checkFieldType(component, info, privateFields);
                }
            }

            type = component.GetType().BaseType;
            while (type != typeof(Component))
            {
                EngineDebug.Debug.WriteLine(type);
                
                privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                
                for (int i = 0; i < privateFields.Length; i++)
                {
                    FieldInfo info = privateFields[i];

                    if (Attribute.IsDefined(info, typeof(SerializedPropertyAttribute)))
                    {
                        checkFieldType(component, info, privateFields);
                    }
                }
                
                type = type.BaseType;
            }
        }
    }
}
