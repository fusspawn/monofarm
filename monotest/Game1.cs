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
using Monotest.Components.Util;

namespace monotest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Core
    {
        public static ContentManager ContentManager;
        public static MainGame Instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Scene MainScene;
        public static Entity MainPlayerEnt;
        public static Renderer MainSceneRenderer;

        public MainGame()
        {
            Instance = this;
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

             //Make the spatial hash grid match chunk size.

            Core.scene = MainScene;
            Entity ChunkMan = new Entity("Managers");
            ChunkManager man = ChunkMan.addComponent<ChunkManager>() as ChunkManager;
            ChunkManager.Instance.CmEntity = ChunkMan;
            Physics.spatialHashCellSize = ChunkManager.Instance.ChunkWidth * ChunkManager.Instance.TileXPixels;

            Entity UI = new Entity("UI");
            UI.addComponent<TileStats>();
            UI.transform.position = new Vector2(200, 200);


            MainScene.addEntity(UI);
            MainScene.addEntity(ChunkMan);

            MainPlayerEnt = PlayerMaker.CreatePlayerEntity(MainScene);
            MainPlayerEnt.transform.position =
                ChunkManager.Instance.ChunkToWorld(ChunkManager.Instance.MaxXChunks / 2,
                ChunkManager.Instance.MaxYChunks / 2).ToVector2();
            MainScene.camera.position = MainPlayerEnt.transform.position;


            Entity TCursor = new Entity("Cursor");
            TCursor.addComponent<SelectedTile>();
            TCursor.addComponent(new Sprite(MainScene.contentManager.Load<Texture2D>("select16"))).renderLayer = 1;
            TCursor.getComponent<Sprite>().origin = Vector2.Zero;
            

            MainScene.addEntity(TCursor);
            base.Initialize();
        }
    }
}
