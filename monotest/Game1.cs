using System.Collections.Generic;
using monotest.Components.Mob;
using monotest.Components.UI;
using monotest.Components.Util;
using monotest.Rendering;
using monotest.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using monotest.Components.World;
using Nez.Sprites;

namespace monotest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Core
    {
        public static ContentManager ContentManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Scene MainScene;
        public static Entity MainPlayerEnt;
        public static Renderer MainSceneRenderer;

        public MainGame()
        {

#if WINDOWS
            // this line is only needed if your are on Windows!!!
            Window.ClientSizeChanged += Core.onClientSizeChanged;
#endif
            
            Content.RootDirectory = "Content";
        }

        public static bool DebugMode = false;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            MainScene = new Scene();
            MainScene.addRenderer(new RenderLayerRenderer(1, new []{0, 1}));
            MainScene.addRenderer(new ScreenSpaceRenderer(2, 999)); // UI!

            Core.scene = MainScene;
            Entity ChunkMan = new Entity("Managers");
            ChunkManager man = ChunkMan.addComponent<ChunkManager>() as ChunkManager;
            ChunkManager.CmEntity = ChunkMan;
            ChunkMan.addComponent<TileStats>();

            MainScene.addEntity(ChunkMan);

            MainPlayerEnt = PlayerMaker.CreatePlayerEntity(MainScene);
            MainPlayerEnt.transform.position = new Vector2(200f, 250f);
            MainScene.camera.position = MainPlayerEnt.transform.position;


            Entity TCursor = new Entity("Cursor");
            TCursor.addComponent<SelectedTile>();
            TCursor.addComponent(new Sprite(MainScene.contentManager.Load<Texture2D>("select16"))).renderLayer = 1;

            MainScene.addEntity(TCursor);
            base.Initialize();
        }
    }
}
