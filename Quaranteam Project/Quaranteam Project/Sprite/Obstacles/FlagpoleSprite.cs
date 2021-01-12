using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class FlagpoleSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private Flagpole context;
        private Texture2D texture;

        private int rowCnt, colCnt;
        private int currentFrame;
        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;
        private int totFrames;
        private bool animated = false;

        public FlagpoleSprite(int rows, int columns, int totalFrames, int startingFrame, bool isAnimated, Flagpole fp) {
            rowCnt = rows;
            colCnt = columns;
            currentFrame = startingFrame;
            totFrames = totalFrames;
            animated = isAnimated;
            context = fp;
        }

        public void Animate() {
            animated = true;
        }
        public virtual void Update(GameTime gameTime) {
            if (animated) {
                msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceLastFrame > msPerFrame) {
                    msSinceLastFrame -= msPerFrame;

                    currentFrame++;
                    msSinceLastFrame = 0;
                    if (currentFrame >= totFrames) {
                        //currentFrame = initFrame;
                        animated = false;
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
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

    public class FlagSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }

        private Flag context;
        private Texture2D texture;

        private int rowCnt, colCnt;
        private int currentFrame;
        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;
        private int totFrames;
        public bool animated = false;

        public FlagSprite(int rows, int columns, int totalFrames, int startingFrame, bool isAnimated, Flag f) {
            rowCnt = rows;
            colCnt = columns;
            currentFrame = startingFrame;
            totFrames = totalFrames;
            animated = isAnimated;
            context = f;
        }
        public void Animate() {
            animated = true;
        }
        public virtual void Update(GameTime gameTime) {
            if (animated) {
                msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceLastFrame > msPerFrame) {
                    msSinceLastFrame -= msPerFrame;

                    currentFrame++;
                    msSinceLastFrame = 0;
                    if (currentFrame >= totFrames) {
                        //currentFrame = initFrame;
                        animated = false;
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
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
