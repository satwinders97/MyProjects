using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class StairBlockSprite : BlockSprite {
        public override Vector2 Position {
            get => blockContext.Position;
            set => blockContext.Position = value;
        }
        private readonly StairBlock blockContext;
        public StairBlockSprite(StairBlock context) : base() {
            blockContext = context;
            frames[0] = 6;
        }
    }
}
