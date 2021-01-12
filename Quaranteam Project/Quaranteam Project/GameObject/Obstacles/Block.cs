using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public abstract class Block : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(16, 16);
        public Block(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Block);
            CollisionBox.ColorType = typeof(Block);

            _acceleration = new Vector2();
        }

        public override void CollideLeft(CollidableObject obj) { }
        public override void CollideRight(CollidableObject obj) { }
        public override void CollideTop(CollidableObject obj) { }
        public override void CollideBottom(CollidableObject obj) { }

        public virtual void BumpAction() { }
        public virtual void BreakAction() { }
    }
}
