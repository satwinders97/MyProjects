using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class Peach : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X + 3, Position.Y + 5);
        protected override Vector2 CollisionSize => new Vector2(20, 37);

        public Peach(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Peach);
            CollisionBox.ColorType = typeof(Peach);

            Sprite = new PeachSprite(this);
        }

        public override void CollideBottom(CollidableObject obj) {
            if (obj is Avatar) {
                Game1.HUDMap["Score"] += 500;
            }
        }
        public override void CollideLeft(CollidableObject obj) {
            if (obj is Avatar) {
                Game1.HUDMap["Score"] += 500;
            }
        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Avatar) {
                Game1.HUDMap["Score"] += 500;
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar) {
                Game1.HUDMap["Score"] += 500;
            }
        }

        public override void Update(GameTime gameTime) { }
    }
}
