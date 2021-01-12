using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Piranah : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(20, 20);
        private bool isDead = false;
        private TimeSpan timeDied;

        public Piranah(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Piranah);
            CollisionBox.ColorType = typeof(Piranah);
            Acceleration = Vector2.Zero;

            Sprite = new PiranahSprite(1, 2, 2, 0, true, this);
        }

        public bool visible = false;

        public override void CollideBottom(CollidableObject obj) {
            if (obj is Fireball) {
                Die();
            }

        }
        public override void CollideLeft(CollidableObject obj) {
            if (obj is Fireball) {
                Die();
            }
        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Fireball) {
                Die();
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Fireball) {
                Die();
            }
        }

        protected void Die() {
            if (!isDead) {
                isDead = true;
                (Sprite as PiranahSprite).Die();
                timeDied = new TimeSpan();
                Game1.HUDMap["Score"] += 100;
            }
        }

        public override void Update(GameTime gameTime) {
            if (isDead) {
                if (timeDied.TotalMilliseconds == 0) {
                    timeDied = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > timeDied + new TimeSpan(0, 0, 0, 0, 500)) {
                    ToBeRemoved = true;
                }
            }

            if ((Position.X <= Level.Avatar.Position.X + 96) && (Position.X >= Level.Avatar.Position.X - 96)) {
                visible = true;
            }

            base.Update(gameTime);
        }
    }
}
