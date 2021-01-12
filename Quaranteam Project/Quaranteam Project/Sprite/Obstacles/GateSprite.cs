using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class GateSprite : ISprite {
        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private Gate context;
        private Texture2D texture;

        public GateSprite(Gate g) {
            context = g;
        }

        public void Update(GameTime gameTime) {
        }

        public void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[context.TextureType];
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
