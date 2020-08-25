using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;

namespace MonoGameAndLua
{
    public class ModdingHandler : IDisposable
    {
        private Script _workingScript;
        public string ScriptFilePath { get; set; }

        private FileSystemWatcher _fileWatcher;
        private Closure _renderFunc;

        public ModdingHandler(string scriptFilePath)
        {
            ScriptFilePath = scriptFilePath;

            UserData.RegisterAssembly(Assembly.GetCallingAssembly());
            UserData.RegisterType(typeof(LuaImGuiWrapper), InteropAccessMode.Default, "ImGui");
            UserData.RegisterType(typeof(Texture2D), InteropAccessMode.Default, "Texture2D");
            UserData.RegisterType<IntPtr>();

            // ImGui
            UserData.RegisterType<ImGuiInputTextFlags>();
            UserData.RegisterType<ImGuiDataType>();
            UserData.RegisterType<ImGuiTreeNodeFlags>();
            UserData.RegisterType<ImGuiSelectableFlags>();
            UserData.RegisterType<ImGuiMouseCursor>();
            UserData.RegisterType<ImGuiCond>();
            UserData.RegisterType<ImGuiWindowFlags>();
            UserData.RegisterType<ImGuiDir>();
            UserData.RegisterType<ImGuiDragDropFlags>();
            UserData.RegisterType<ImGuiTabBarFlags>();
            UserData.RegisterType<ImGuiTabItemFlags>();
            UserData.RegisterType<ImGuiColorEditFlags>();
            UserData.RegisterType<ImGuiKey>();
            UserData.RegisterType<ImGuiCol>();
            UserData.RegisterType<ImGuiMouseButton>();

            // MonoGame
            UserData.RegisterType<Keys>();
            UserData.RegisterType<MouseButton>();

            RegisterAllVectors();

            ExecuteScript();

            _fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(ScriptFilePath));
            _fileWatcher.Changed += FileWatcherOnChanged;
            _fileWatcher.Created += FileWatcherOnChanged;
            _fileWatcher.Deleted += FileWatcherOnChanged;

            _fileWatcher.EnableRaisingEvents = true;
        }

        private void FileWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("FILE CHANGED: " + e.FullPath);
            ExecuteScript();
        }

        public void ExecuteScript()
        {
            _workingScript = new Script(CoreModules.Preset_HardSandbox);
            _workingScript.Globals["ImageLoader"] = typeof(ImageLoader);
            _workingScript.Globals["ImGuiTexture"] = typeof(ImGuiRendererTexture);
            try
            {
                var source = File.ReadAllText(ScriptFilePath);
                var result = _workingScript.DoString(source).Table;
                _renderFunc = result["render"] as Closure;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

            _workingScript.Globals["Input"] = typeof(InputManager);
            _workingScript.Globals["ImGui"] = typeof(ImGuiWrapper);
            
        }

        public void CallScript()
        {
            SetFrameState();
            try
            {
                _workingScript.Call(_workingScript.Globals["Render"]);
                _renderFunc.Call();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Dispose()
        {
            _fileWatcher.Dispose();
        }

        public void SetFrameState()
        {
            _workingScript.Globals["imgui_input_text_flags"] = typeof(ImGuiInputTextFlags);
            _workingScript.Globals["imgui_data_type"] = typeof(ImGuiDataType);
            _workingScript.Globals["imgui_tree_node_flags"] = typeof(ImGuiTreeNodeFlags);
            _workingScript.Globals["imgui_selectable_flags"] = typeof(ImGuiSelectableFlags);
            _workingScript.Globals["imgui_mouse_cursor"] = typeof(ImGuiMouseCursor);
            _workingScript.Globals["imgui_cond"] = typeof(ImGuiCond);
            _workingScript.Globals["imgui_window_flags"] = typeof(ImGuiWindowFlags);
            _workingScript.Globals["imgui_dir"] = typeof(ImGuiDir);
            _workingScript.Globals["imgui_drag_drop_flags"] = typeof(ImGuiDragDropFlags);
            _workingScript.Globals["imgui_tab_bar_flags"] = typeof(ImGuiTabBarFlags);
            _workingScript.Globals["imgui_tab_item_flags"] = typeof(ImGuiTabItemFlags);
            _workingScript.Globals["imgui_color_edit_flags"] = typeof(ImGuiColorEditFlags);
            _workingScript.Globals["imgui_key"] = typeof(ImGuiKey);
            _workingScript.Globals["imgui_col"] = typeof(ImGuiCol);

            _workingScript.Globals["Keys"] = typeof(Keys);
        }

        private void RegisterAllVectors()
        {
            // Vector 2
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector2),
                dynVal => {
                    var table = dynVal.Table;
                    var x = (float)(double)table[1];
                    var y = (float)(double)table[2];
                    return new Vector2(x, y);
                }
            );

            Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion<Vector2>(
                (script, vector) => {
                    var x = DynValue.NewNumber(vector.X);
                    var y = DynValue.NewNumber(vector.Y);
                    var dynVal = DynValue.NewTable(script, x, y);
                    return dynVal;
                }
            );

            // Vector3
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector3),
                dynVal => {
                    var table = dynVal.Table;
                    var x = (float)((double)table[1]);
                    var y = (float)((double)table[2]);
                    var z = (float)((double)table[3]);
                    return new Vector3(x, y, z);
                }
            );

            Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion<Vector3>(
                (script, vector) => {
                    var x = DynValue.NewNumber(vector.X);
                    var y = DynValue.NewNumber(vector.Y);
                    var z = DynValue.NewNumber(vector.Z);
                    var dynVal = DynValue.NewTable(script, x, y, z);
                    return dynVal;
                }
            );

            // Vector4
            Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector4),
                dynVal => {
                    var table = dynVal.Table;
                    var w = (float)((double)table[1]);
                    var x = (float)((double)table[2]);
                    var y = (float)((double)table[3]);
                    var z = (float)((double)table[4]);
                    return new Vector4(w, x, y, z);
                }
            );

            Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion<Vector4>(
                (script, vector) => {
                    var w = DynValue.NewNumber(vector.W);
                    var x = DynValue.NewNumber(vector.X);
                    var y = DynValue.NewNumber(vector.Y);
                    var z = DynValue.NewNumber(vector.Z);
                    var dynVal = DynValue.NewTable(script, w, x, y, z);
                    return dynVal;
                }
            );
        }
    }
}