using System;
using System.Text;
using NLua;

namespace MonoGameAndLua
{
    public class ModdingHandler : IDisposable
    {
        public Lua LuaState { get; private set; }
        private LuaImGuiWrapper _luaImGuiWrapper;

        public ModdingHandler()
        {
            LuaState = new Lua();
            LuaState.State.Encoding = Encoding.UTF8;
            _luaImGuiWrapper = new LuaImGuiWrapper();
            LuaState["ImGui"] = _luaImGuiWrapper;
        }

        public object[] ExecuteScript(string filePath)
        {
            return LuaState.DoFile(filePath);
        }

        public void Dispose()
        {
            LuaState.Dispose();
        }
    }
}