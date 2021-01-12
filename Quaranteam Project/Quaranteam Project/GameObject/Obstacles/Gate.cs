using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class Gate : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X, Position.Y);
        protected override Vector2 CollisionSize => new Vector2(84, 80);

        public Gate(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Gate);
            CollisionBox.ColorType = typeof(Gate);

            Sprite = new GateSprite(this);
        }

        public override void CollideBottom(CollidableObject obj) { }

        public override void CollideLeft(CollidableObject obj) { }

        public override void CollideRight(CollidableObject obj) { }

        public override void CollideTop(CollidableObject obj) { }

        public override void Update(GameTime gameTime) { }
    }
}
