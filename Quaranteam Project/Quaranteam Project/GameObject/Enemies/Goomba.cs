using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Goomba : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(16, 16);
        private bool isDead = false;
        private TimeSpan timeDied;

        public Goomba(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Goomba);
            CollisionBox.ColorType = typeof(Goomba);

            Sprite = new GoombaSprite(1, 3, 2, 0, true, this);
        }

        private bool rightCol = false;
        private bool leftCol = false;
        private int collisionCount = 0;

        public override void CollideBottom(CollidableObject obj) {
            if (obj is Block) {
                _velocity.Y = 0;
            } else if (obj is Fireball) {
                Die();
            }

        }
        public override void CollideLeft(CollidableObject obj) {
            leftCol = true;
            rightCol = false;
            if (obj is Fireball) {
                Die();
            } else if (obj is Koopa) {
                if ((obj as Koopa).Type is GreenProjectile || (obj as Koopa).Type is RedProjectile) {
                    Die();
                }
            }
        }
        public override void CollideRight(CollidableObject obj) {
            rightCol = true;
            leftCol = false;
            if (obj is Fireball) {
                Die();
            } else if (obj is Koopa) {
                if ((obj as Koopa).Type is GreenProjectile || (obj as Koopa).Type is RedProjectile) {
                    Die();
                }
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar || obj is Fireball) {
                Die();
            }
        }

        protected void Die() {
            if (!isDead) {
                isDead = true;
                (Sprite as GoombaSprite).Die();
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

            if ((Position.X >= Level.Avatar.Position.X + (Game1.ScreenWidth / 2) + 50 && Position.X >= Game1.ScreenWidth) || (Position.X <= Level.Avatar.Position.X - (Game1.ScreenWidth / 2) - 50 && Position.X <= Level.MapWidth - Game1.ScreenWidth)) {
                _velocity.X = 0;
            } else {
                if (Level.Avatar.Position.X <= Position.X && collisionCount == 0) {
                    CollideRight(this);
                    collisionCount++;
                }

                if (leftCol) {
                    _velocity.X = .3f;
                } else if (rightCol) {
                    _velocity.X = -.3f;

                } else {
                    _velocity.X = .3f;
                }

                if (Position.X == 0) {
                    CollideLeft(this);
                } else if (Position.X == (Level.MapWidth - 16)) {
                    CollideRight(this);
                }
            }

            base.Update(gameTime);
        }
    }
}
