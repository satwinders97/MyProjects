using Microsoft.Xna.Framework;
using QuaranteamProject.Sprite.Obstacles;

namespace QuaranteamProject {
    public abstract class Cloud : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X + 12, Position.Y + 14);
        protected override Vector2 CollisionSize => new Vector2(48, 32);

        public Cloud(TileMap level, Vector2 initPos, Vector2 initVel, Vector2 initAccel) : base(level, initPos, initVel, initAccel) {
            TextureType = typeof(Cloud);
            CollisionBox.ColorType = typeof(Cloud);
        }

        public override void CollideLeft(CollidableObject obj) {
            if (!(obj is Avatar)) {
                _velocity.X *= -1;
            }
        }

        public override void CollideRight(CollidableObject obj) {
            if (!(obj is Avatar)) {
                _velocity.X *= -1;
            }
        }

        public override void CollideTop(CollidableObject obj) {
            if (!(obj is Avatar)) {
                _velocity.Y *= -1;
            }
        }

        public override void CollideBottom(CollidableObject obj) {
            if (!(obj is Avatar)) {
                _velocity.Y *= -1;
            }
        }
    }



    public class SingleCloud : Cloud {
        protected override Vector2 CollisionSize => new Vector2(44, 24);
        public SingleCloud(TileMap level, Vector2 initPos, Vector2 initVel, Vector2 initAccel) : base(level, initPos, initVel, initAccel) {
            TextureType = typeof(SingleCloud);
            Sprite = new SingleCloudSprite(this);
        }

    }

    public class DoubleCloud : Cloud {
        protected override Vector2 CollisionSize => new Vector2(44 * 2 - 12, 24);
        public DoubleCloud(TileMap level, Vector2 initPos, Vector2 initVel, Vector2 initAccel) : base(level, initPos, initVel, initAccel) {
            TextureType = typeof(DoubleCloud);
            Sprite = new DoubleCloudSprite(this);
        }
    }

    public class TripleCloud : Cloud {
        protected override Vector2 CollisionSize => new Vector2(44 * 3 - 24, 24);
        public TripleCloud(TileMap level, Vector2 initPos, Vector2 initVel, Vector2 initAccel) : base(level, initPos, initVel, initAccel) {
            TextureType = typeof(TripleCloud);
            Sprite = new TripleCloudSprite(this);
        }
    }
}
