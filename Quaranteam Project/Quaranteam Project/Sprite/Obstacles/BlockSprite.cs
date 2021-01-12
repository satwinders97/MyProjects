using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public abstract class BlockSprite : ISprite {
        public abstract Vector2 Position { get; set; }
        public bool IsMasked { get; set; }
        protected Texture2D texture;

        protected int[] frames;
        protected int totFrames;
        protected int currFrame;

        protected int msSinceLastFrame = 0;
        protected int msPerFrame = 50;

        protected int colCnt = 10;
        protected int rowCnt = 1;
        protected int currRow, currCol;

        public BlockSprite() {
            frames = new int[colCnt];
            totFrames = 1;

            currRow = currCol = 0;
        }

        public virtual void Update(GameTime gameTime) {
            msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (msSinceLastFrame > msPerFrame) {
                msSinceLastFrame -= msPerFrame;

                currFrame++;
                msSinceLastFrame = 0;
                if (currFrame >= totFrames) {
                    currFrame = 0;
                }
            }

            currCol = frames[currFrame];
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[typeof(Block)];

            int width = texture.Width / colCnt;
            int height = texture.Height / rowCnt;

            Rectangle sourceRectangle = new Rectangle(width * currCol, height * currRow, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
