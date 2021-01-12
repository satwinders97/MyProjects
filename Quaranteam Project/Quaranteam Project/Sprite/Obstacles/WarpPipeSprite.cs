using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class WarpPipeSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private WarpPipe context;
        private Texture2D texture;

        private int rowCnt, colCnt;

        private int currentFrame, frameHeight, frameWidth;

        public WarpPipeSprite(int rows, int columns, int fWidth, int fHeight, int startingFrame, WarpPipe wp) {
            rowCnt = rows;
            colCnt = columns;
            frameHeight = fHeight;
            frameWidth = fWidth;
            currentFrame = startingFrame;
            context = wp;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[context.TextureType];

            int width = texture.Width / colCnt;
            int height = texture.Height / rowCnt;
            int row = (int)((float)currentFrame / colCnt);
            int column = currentFrame % colCnt;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width * frameWidth, height * frameHeight);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width * frameWidth, height * frameHeight);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
