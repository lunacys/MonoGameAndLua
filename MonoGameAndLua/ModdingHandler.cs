using System;
using System.IO;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;

namespace MonoGameAndLua
{
    public class ModdingHandler : IDisposable
    {
        private Script _workingScript;
        public string ScriptFilePath { get; set; }

        private FileSystemWatcher _fileWatcher;

        public ModdingHandler(string scriptFilePath)
        {
            ScriptFilePath = scriptFilePath;

            UserData.RegisterAssembly(Assembly.GetCallingAssembly());
            // UserData.RegisterType(typeof(LuaImGuiWrapper), InteropAccessMode.Default, "ImGui");

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

            try
            {
                var source = File.ReadAllText(ScriptFilePath);
                _workingScript.DoString(source);
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

    }
}