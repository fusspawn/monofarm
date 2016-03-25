using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using monotest.Components.World;
using Nez;
using Nez.UI;

namespace monotest.Components.UI
{
    public class TileStats 
        : Component, IUpdatable
    {
        private UICanvas Canvas;

        private Table Layout;

        private Label XLabel;
        private Label YLabel;
        private Label BaseTypeLabel;
        private Label DetailTypeLabel;
        private Label Walkable;

        public override void onAddedToEntity()
        {
            
            Canvas =(UICanvas) entity.addComponent<UICanvas>();
            Canvas.renderLayer = 999;
            Layout = new Table();
            XLabel = new Label("X: ");
            YLabel = new Label("Y: ");
            BaseTypeLabel= new Label("Base Type: ");
            DetailTypeLabel = new Label("Detail Type: ");
            Walkable = new Label("Walkable: ");


            Canvas.stage.addElement(Layout);

            Layout.addElement(XLabel);
            Layout.row();
            Layout.addElement(YLabel);
            Layout.row();
            Layout.addElement(BaseTypeLabel);
            Layout.row();
            Layout.addElement(DetailTypeLabel);
            Layout.row();
            Layout.addElement(Walkable);
            Layout.row();
        }
        

        public void update()
        {
            TileData Data = ChunkManager.MouseTile;

            if (Data != null)
            {
                XLabel.setText("X: " + Data.TileX);
                YLabel.setText("Y: " + Data.TileY);
                BaseTypeLabel.setText("Base Type: " + Data.TileBaseType);
                DetailTypeLabel.setText("Detail Type: " + Data.TileDetailType);
                Walkable.setText("Walkable: " + Data.IsTileWalkable);
            }
        }
    }
}
