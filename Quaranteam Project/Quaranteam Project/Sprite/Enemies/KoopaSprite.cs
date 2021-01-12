using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class KoopaSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private Koopa context;

        private Texture2D texture;

        private int rowCnt, colCnt;

        private int currentFrame;
        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;
        private int totFrames, initFrame;
        private bool animated = false;

        public bool isFacingLeft = false;

        public KoopaSprite(int rows, int columns, int totalFrames, int startingFrame, bool isAnimated, Koopa k) {
            rowCnt = rows;
            colCnt = columns;
            initFrame = startingFrame;
            currentFrame = startingFrame;
            totFrames = totalFrames;
            animated = isAnimated;
            context = k;
        }

        public void Update(GameTime gameTime) {
            if (animated) {
                msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceLastFrame > msPerFrame) {
                    msSinceLastFrame -= msPerFrame;

                    currentFrame++;
                    msSinceLastFrame = 0;
                    if (currentFrame >= totFrames) {
                        currentFrame = initFrame;
                    }
                }
            }
        }

        public void Die() {
            totFrames = 1;
            animated = false;
            initFrame = 6;
            currentFrame = 6;
        }

        public void GreenShell() {

            totFrames = 1;
            animated = false;
            initFrame = 2;
            currentFrame = 2;
        }
        public void RedShell() {

            totFrames = 1;
            animated = false;
            initFrame = 9;
            currentFrame = 9;
        }
        public void GreenKoopa() {

            totFrames = 2;
            animated = true;
            initFrame = 0;
            currentFrame = 1;
        }
        public void RedKoopa() {

            totFrames = 2;
            animated = true;
            initFrame = 7;
            currentFrame = 8;
        }
        public void GreenProjectile() {

            totFrames = 5;
            animated = true;
            initFrame = 2;
            currentFrame = 6;
        }
        public void RedProjectile() {

            totFrames = 5;
            animated = true;
            initFrame = 9;
            currentFrame = 13;
        }

        public void FaceLeft() {
            isFacingLeft = true;
        }

        public void FaceRight() {
            isFacingLeft = false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[context.TextureType];

            int width = texture.Width / colCnt;
            int height = texture.Height / rowCnt;
            int row = (int)((float)currentFrame / colCnt);
            int column = currentFrame % colCnt;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(), isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }
}
