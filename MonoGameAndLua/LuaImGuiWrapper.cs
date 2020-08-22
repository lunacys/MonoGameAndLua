using System.Numerics;
using ImGuiNET;

namespace MonoGameAndLua
{
    public class LuaImGuiWrapper
    {
        public bool Begin(string name) => ImGui.Begin(name);
        public void End() => ImGui.End();
        public void InputText(string label, ref string input, uint maxLength) => ImGui.InputText(label, ref input, maxLength);
        public bool Button(string label) => ImGui.Button(label);
        public bool Button(string label, Vector2 size) => ImGui.Button(label, size);
    }
}