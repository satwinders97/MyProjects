using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class QuestionBlockSprite : BlockSprite {
        public override Vector2 Position {
            get => blockContext.Position;
            set => blockContext.Position = value;
        }
        private QuestionBlock blockContext;

        public QuestionBlockSprite(QuestionBlock context) : base() {
            blockContext = context;

            msPerFrame = 300;

            totFrames = 1;
            frames[0] = 0;
        }

        public override void Update(GameTime gameTime) {
            switch (blockContext.State) {
                case IdleQuestionBlock _:
                case BumpedQuestionBlock _:
                    totFrames = 3;
                    frames[0] = 2;
                    frames[1] = 1;
                    frames[2] = 0;
                    break;
                case UsedQuestionBlock _:
                    totFrames = 1;
                    frames[0] = 3;
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!(blockContext.State is HiddenQuestionBlock)) {
                base.Draw(spriteBatch);
            }
        }
    }
}
