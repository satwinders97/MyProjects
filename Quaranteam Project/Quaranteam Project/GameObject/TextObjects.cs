using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject.GameObject {
    public interface ITextObject {
        SpriteFont TextFont { get; set; }
        Vector2 Position { get; set; }
        Color FontColor { get; set; }
        float Rotation { get; set; }
        Vector2 Origin { get; set; }
        float Scale { get; set; }
        SpriteEffects Effects { get; set; }
        float LayerDepth { get; set; }
        string Text { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }

    public class TextObject : ITextObject {
        public SpriteFont TextFont { get; set; }
        public Vector2 Position { get; set; }
        public Color FontColor { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public SpriteEffects Effects { get; set; }
        public float LayerDepth { get; set; }
        public string Text { get; set; }

        public TextObject(SpriteFont sf, string text, Vector2 position, Color color, float rot, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) {
            TextFont = sf;
            Text = text;
            Position = position;
            FontColor = color;
            Rotation = rot;
            Origin = origin;
            Scale = scale;
            Effects = effects;
            LayerDepth = layerDepth;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(TextFont, Text, Position, FontColor, Rotation, Origin, Scale, Effects, LayerDepth);
        }
    }
}
