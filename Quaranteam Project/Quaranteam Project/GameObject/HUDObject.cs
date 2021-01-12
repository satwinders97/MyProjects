using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace QuaranteamProject {
    public interface IHUDObject {
        Texture2D Texture { get; set; }
        Vector2 Position { get; set; }
        Rectangle SourceRectangle { get; set; }

        Color TextureColor { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }
    public class HUDObject : IHUDObject {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Color TextureColor { get; set; }
        public HUDObject(Texture2D texture, Vector2 position, Rectangle source, Color color) {
            Texture = texture;
            Position = position;
            SourceRectangle = source;
            TextureColor = color;
        }

        public HUDObject(Texture2D texture2D) {
            Texture = texture2D;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, SourceRectangle, TextureColor);
        }
    }
}