using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Fireball : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X + 4, Position.Y + 4);
        protected override Vector2 CollisionSize => new Vector2(7, 7);
        private bool isExploded = false;
        private TimeSpan timeExploded;

        public Fireball(TileMap level, Vector2 initPos, Vector2 initVel) : base(level, initPos, initVel, level.GetGravity()) {
            TextureType = typeof(Fireball);
            CollisionBox.ColorType = typeof(Fireball);

            Sprite = new FireballSprite(1, 5, 4, 1, true, this);
            CanClip = false;
        }

        public override void Update(GameTime gameTime) {
            if ((Position.X >= Level.Avatar.Position.X + (Game1.ScreenWidth / 2) + 50 && Position.X >= Game1.ScreenWidth) || (Position.X <= Level.Avatar.Position.X - (Game1.ScreenWidth / 2) - 50 && Position.X <= Level.MapWidth - Game1.ScreenWidth)) {
                ToBeRemoved = true;
            }
            if (isExploded) {
                if (timeExploded.TotalMilliseconds == 0) {
                    timeExploded = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > timeExploded + new TimeSpan(0, 0, 0, 0, 500)) {
                    ToBeRemoved = true;
                }
            } else {
                base.Update(gameTime);
            }
        }

        public override void CollideBottom(CollidableObject obj) {
            if (obj is Block) {
                _velocity.Y = -2;
            } else {
                Explode();
            }
        }

        public override void CollideLeft(CollidableObject obj) {
            Explode();
        }

        public override void CollideRight(CollidableObject obj) {
            Explode();
        }

        public override void CollideTop(CollidableObject obj) {
            Explode();
        }

        private void Explode() {
            if (!isExploded) {
                isExploded = true;
                (Sprite as FireballSprite).Explode();
                timeExploded = new TimeSpan();
            }
        }
    }
}
