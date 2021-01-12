using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class FloorBlock : Block {
        public bool IsBlue { get; set; }
        public FloorBlock(TileMap level, Vector2 initPos, bool blue) : base(level, initPos) {
            IsBlue = blue;
            Sprite = new FloorBlockSprite(this);
        }
    }
}
