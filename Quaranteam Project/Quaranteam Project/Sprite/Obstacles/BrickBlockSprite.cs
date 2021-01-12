using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class BrickBlockSprite : BlockSprite {
        public override Vector2 Position {
            get => blockContext.Position;
            set => blockContext.Position = value;
        }
        private BrickBlock blockContext;

        public BrickBlockSprite(BrickBlock context) : base() {
            blockContext = context;

            totFrames = 1;
            frames[0] = 4;
        }

        public override void Update(GameTime gameTime) {
            if (blockContext.IsBlue) {
                totFrames = 1;
                frames[0] = 8;
            } else {
                switch (blockContext.State) {
                    case IdleBrickBlock _:
                    case BumpedBrickBlock _:
                        totFrames = 1;
                        frames[0] = 4;
                        break;
                    case UsedBrickBlock _:
                        totFrames = 1;
                        frames[0] = 3;
                        break;
                    case BrokenBrickBlock _:
                        totFrames = 1;
                        frames[0] = 5;
                        break;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!(blockContext.State is HiddenBrickBlock)) {
                base.Draw(spriteBatch);
            }
        }
    }
}
