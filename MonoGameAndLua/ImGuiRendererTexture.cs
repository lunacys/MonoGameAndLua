using System;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;

namespace MonoGameAndLua
{
    [MoonSharpUserData]
    [MoonSharpHideMember("ImGuiRenderer")]
    public static class ImGuiRendererTexture
    {
        public static ImGuiRenderer ImGuiRenderer { get; set; }

        public static IntPtr BindTexture(Texture2D texture) => ImGuiRenderer.BindTexture(texture);
        public static void UnbindTexture(IntPtr texturePtr) => ImGuiRenderer.UnbindTexture(texturePtr);
    }
}