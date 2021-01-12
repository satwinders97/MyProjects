using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QuaranteamProject {

    public class Background : IGameObject {
        public TileMap Level { get; set; }
        public Vector2 Position {
            get => Sprite.Position;
            set => Sprite.Position = value;
        }
        public Vector2 Velocity { get; set; } = Vector2.Zero;
        public Vector2 Acceleration { get; set; } = Vector2.Zero;
        public ISprite Sprite { get; set; }
        public Type TextureType { get; set; }
        public bool ToBeRemoved { get; set; }


        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch) {
            Sprite.Draw(spriteBatch);
        }

        public void Init() { }
    }
}
