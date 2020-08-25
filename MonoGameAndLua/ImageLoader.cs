using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;

namespace MonoGameAndLua
{
    [MoonSharpUserData]
    [MoonSharpHideMember("GraphicsDevice")]
    public static class ImageLoader
    {
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static Texture2D Load(string path)
        {
            return Texture2D.FromFile(GraphicsDevice, path);
        }
    }
}