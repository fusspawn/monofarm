using System.Collections.Generic;
using monotest.Objects;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monotest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        public static TileMap GameMap;
        public static Player MainPlayer;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static List<monotest.Interfaces.IUpdateable> UpdateList = new List<monotest.Interfaces.IUpdateable>();
        public static List<monotest.Interfaces.IDrawable> DrawList = new List<monotest.Interfaces.IDrawable>();  

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Noise2d.WriteDebugJSONForMap = true;
            // TODO: Add your initialization logic here

            GameMap = new TileMap();
            DrawList.Add(GameMap);
            UpdateList.Add(GameMap);

            DebugManager Debug = new DebugManager();
            DrawList.Add(Debug);
            UpdateList.Add(Debug);

            MainPlayer = new Player(250, 250);
            DrawList.Add(MainPlayer);
            UpdateList.Add(MainPlayer);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var comp in DrawList)
            {
                comp.LoadContent(Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            foreach(var comp in UpdateList)
                comp.Update(gameTime);

            GameMap.Offset = MainPlayer.Location;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            foreach (var comp in DrawList)
            {
                comp.Draw(spriteBatch, gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
