using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class StairBlock : Block {
        public bool IsDoor { get; set; }
        public StairBlock(TileMap level, Vector2 initPos) : base(level, initPos) {
            Sprite = new StairBlockSprite(this);
        }
    }
}
