using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameAndLua
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ImGuiRenderer _imGuiRenderer;
        private ModdingHandler _moddingHandler;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _imGuiRenderer = new ImGuiRenderer(this);
            _moddingHandler = new ModdingHandler("Lua/testImGui.lua");

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _imGuiRenderer.RebuildFontAtlas();

            _moddingHandler.ExecuteScript();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*if (InputManager.WasKeyPressed(Keys.Enter))
            {
                _moddingHandler.ExecuteScript();
            }*/

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _imGuiRenderer.BeforeLayout(gameTime);
            _moddingHandler.CallScript();
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }
    }
}
