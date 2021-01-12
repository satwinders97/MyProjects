using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class MiniCovidSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private MiniCovid context;

        private Texture2D texture;

        private int rowCnt, colCnt;

        private int currentFrame;
        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;
        private int totFrames, initFrame;
        private bool animated = false;

        private bool isDying = false;


        public MiniCovidSprite(int rows, int columns, int totalFrames, int startingFrame, bool isAnimated, MiniCovid g) {
            rowCnt = rows;
            colCnt = columns;
            initFrame = startingFrame;
            currentFrame = startingFrame;
            totFrames = totalFrames;
            animated = isAnimated;
            context = g;
        }

        public void Update(GameTime gameTime) {
            if (animated && !(isDying && currentFrame == 4)) {
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
            totFrames = 3;
            animated = true;
            initFrame = 2;
            currentFrame = 2;
            isDying = true;
        }


        public void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[context.TextureType];

            int width = texture.Width / colCnt;
            int height = texture.Height / rowCnt;
            int row = (int)((float)currentFrame / colCnt);
            int column = currentFrame % colCnt;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
