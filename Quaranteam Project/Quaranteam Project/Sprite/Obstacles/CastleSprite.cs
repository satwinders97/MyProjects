using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class CastleSprite : ISprite {

        public Vector2 Position {
            get => context.Position;
            set => context.Position = value;
        }
        public bool IsMasked { get; set; }
        private Castle context;
        private Texture2D texture;

        public CastleSprite(Castle c) {
            context = c;
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[context.TextureType];

            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
