using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class PowerUpSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private Texture2D texture;
        private PowerUp context;

        private int rowCnt, colCnt;

        private int currentFrame;

        /* Uncomment in future, if powerup animations are needed */
        /*
        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;
        private int totFrames, initFrame;
        */

        public PowerUpSprite(int rows, int columns, int startingFrame, PowerUp powerUpContext) { //int totalFrames,
            context = powerUpContext;
            rowCnt = rows;
            colCnt = columns;
            currentFrame = startingFrame;
            //initFrame = startingFrame;
            //totFrames = totalFrames;
        }

        public void Update(GameTime gameTime) { }

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
