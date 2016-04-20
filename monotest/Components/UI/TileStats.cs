using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.World;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;

namespace monotest.Components.UI
{
    public class TileStats 
        : Component, IUpdatable
    {
        private UICanvas Canvas;

        private Window Layout;

        private Label XLabel;
        private Label YLabel;

        private Label XOffLabel;
        private Label YOffLabel;

        private Label BaseTypeLabel;
        private Label DetailTypeLabel;
        private Label Walkable;

        public override void onAddedToEntity()
        {
            
            Canvas =  entity.addComponent<UICanvas>() as UICanvas;
            Canvas.renderLayer = 999;
            
            Layout = new Window("stats", new WindowStyle());
            Layout.setPosition(250, 250);
            XLabel = new Label("X: ");
            YLabel = new Label("Y: ");
            XOffLabel = new Label("XOff: ");
            YOffLabel = new Label("YOff: ");
            BaseTypeLabel= new Label("Base Type: ");
            DetailTypeLabel = new Label("Detail Type: ");
            Walkable = new Label("Walkable: ");


            Canvas.stage.addElement(Layout);

            Layout.add(XLabel);
            Layout.row();
            Layout.add(YLabel);
            Layout.row();
            Layout.add(XOffLabel);
            Layout.row();
            Layout.add(YOffLabel);
            Layout.row();
            Layout.add(BaseTypeLabel);
            Layout.row();
            Layout.add(DetailTypeLabel);
            Layout.row();
            Layout.add(Walkable);
            Layout.row();
        }
        

        public void update()
        {
            TileData Data = ChunkManager.Instance.MouseTile;

            if (Data != null)
            {
                XLabel.setText("X: " + Data.TileX);
                YLabel.setText("Y: " + Data.TileY);
                XOffLabel.setText("XOFf: " + Data.ChunkTileOffsetX);
                YOffLabel.setText("YOff: " + Data.ChunkTileOffsetY);
                BaseTypeLabel.setText("Base Type: " + Data.TileBaseType);
                DetailTypeLabel.setText("Detail Type: " + Data.TileDetailType);
                Walkable.setText("Walkable: " + Data.IsTileWalkable);
            }
            
        }
    }
}
