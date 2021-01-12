using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class Castle : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(80, 80);
        public Castle(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Castle);
            CollisionBox.ColorType = typeof(Castle);

            Sprite = new CastleSprite(this);
        }

        public override void CollideBottom(CollidableObject obj) { }
        public override void CollideLeft(CollidableObject obj) { }
        public override void CollideRight(CollidableObject obj) { }
        public override void CollideTop(CollidableObject obj) { }

        public override void Update(GameTime gameTime) { }
    }


    public interface ICastle { }

    public class StartCastle : ICastle { }
    public class EndCastle : ICastle { }
}
