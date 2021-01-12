using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class CovidBoss : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X + 32, Position.Y + 32);
        protected override Vector2 CollisionSize => new Vector2(32, 33); // y=33, not 32, so that he can't escape his boss room

        private bool isDead = false;
        private bool isInvuln = false;
        private TimeSpan timeInvuln;

        public bool isFacingLeft;

        private int health = 10;

        private const float horizontalSpeed = 0.6f;
        private const float verticalSpeed = 0.2f;

        private Random rng = new Random();

        private int msSinceLastSpawn;
        private const int msPerSpawn = 7500;

        public CovidBoss(TileMap level, Vector2 initPos) : base(level, initPos, new Vector2(horizontalSpeed, verticalSpeed), Vector2.Zero) {
            TextureType = typeof(CovidBoss);
            CollisionBox.ColorType = typeof(CovidBoss);

            CanClip = false;

            Sprite = new CovidBossSprite(1, 4, 4, 0, true, this);
        }

        public override void CollideBottom(CollidableObject obj) {
            _velocity.Y = -verticalSpeed;
            if (obj is Sanitizerball) {
                TakeDamage();
            }
        }
        public override void CollideLeft(CollidableObject obj) {
            _velocity.X = horizontalSpeed;
            isFacingLeft = false;
            if (obj is Sanitizerball) {
                TakeDamage();
                Game1.SoundMap["BatScreech"].Play();
            }
        }
        public override void CollideRight(CollidableObject obj) {
            _velocity.X = -horizontalSpeed;
            isFacingLeft = true;
            if (obj is Sanitizerball) {
                TakeDamage();
                Game1.SoundMap["BatScreech"].Play();
            }
        }
        public override void CollideTop(CollidableObject obj) {
            _velocity.Y = verticalSpeed;
            if (obj is Sanitizerball) {
                TakeDamage();
            }
        }

        protected void TakeDamage() {
            if (!isInvuln) {
                health--;
                if (health <= 0) {
                    Die();
                } else {
                    isInvuln = true;
                    timeInvuln = new TimeSpan();
                }
            }
        }

        protected void Die() {
            CanCollide = false;
            isDead = true;
            (Sprite as CovidBossSprite).isDying = true;
            Level.OpenPeach();
        }

        protected void SpawnMiniCovid() {
            float spawnX, spawnY;
            float velX, velY;

            if (isFacingLeft) {
                spawnX = CollisionMin.X - 18;
                velX = -1;
            } else {
                spawnX = CollisionMin.X + CollisionSize.X + 18;
                velX = 1;
            }

            spawnY = CollisionMin.Y + CollisionSize.Y / 2;
            velY = -0.5f;

            MiniCovid newSpawn = new MiniCovid(Level, new Vector2(spawnX, spawnY)) { Velocity = new Vector2(velX, velY) };

            Level.ToSpawn.Add(newSpawn);
        }

        public override void Update(GameTime gameTime) {
            if (!isDead) {
                if (Position.Y < 0) {
                    CollideTop(this);
                }

                if (isInvuln) {
                    if (timeInvuln.TotalMilliseconds == 0) {
                        timeInvuln = gameTime.TotalGameTime;
                    } else if (gameTime.TotalGameTime > timeInvuln + new TimeSpan(0, 0, 0, 0, 400)) {
                        isInvuln = false;
                    }
                }

                if (rng.NextDouble() < .005) {
                    _velocity.X = -_velocity.X;
                    isFacingLeft = !isFacingLeft;
                }
                if (rng.NextDouble() < .01) {
                    _velocity.Y = -_velocity.Y;
                }

                if ((Position.X >= Level.Avatar.Position.X + (Game1.ScreenWidth / 2) + 50 && Position.X >= Game1.ScreenWidth) || (Position.X <= Level.Avatar.Position.X - (Game1.ScreenWidth / 2) - 50 && Position.X <= Level.MapWidth - Game1.ScreenWidth)) {
                    _velocity.X = 0;
                } else { // only spawn miniCovids when on-screen
                    msSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
                    if (msSinceLastSpawn > msPerSpawn) {
                        msSinceLastSpawn = 0;
                        SpawnMiniCovid();
                    }
                }
            } else {
                _velocity.Y = 2;
            }

            base.Update(gameTime);
        }
    }
}
