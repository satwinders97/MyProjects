using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class FloorBlockSprite : BlockSprite {
        public override Vector2 Position {
            get => blockContext.Position;
            set => blockContext.Position = value;
        }
        private FloorBlock blockContext;
        public FloorBlockSprite(FloorBlock context) : base() {
            blockContext = context;
            frames[0] = blockContext.IsBlue ? 9 : 7;
        }
    }
}
