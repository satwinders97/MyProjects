using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Koopa : CollidableObject {
        protected override Vector2 CollisionMin {
            get {
                if (Type is GreenKoopa || Type is RedKoopa) {
                    return Position;
                } else {
                    return new Vector2(Position.X, Position.Y + 16);
                }
            }
        }
        protected override Vector2 CollisionSize {
            get {
                if (Type is GreenKoopa || Type is RedKoopa) {
                    return new Vector2(16, 32);
                } else {
                    return new Vector2(16, 16);
                }
            }
        }
        private bool isDead = false;
        private bool isGreenShell = false;
        private bool isRedShell = false;
        private bool isGreenProjectile = false;
        private bool isRedProjectile = false;
        private bool rRedCollide = false;
        private bool lRedCollide = false;
        private bool rGreenCollide = false;
        private bool lGreenCollide = false;
        private TimeSpan timeDied;
        private TimeSpan greenShellTime;
        private TimeSpan redShellTime;
        public IKoopa Type { get; set; }

        public Koopa(TileMap level, Vector2 initPos, IKoopa type) : base(level, initPos) {
            Type = type;
            TextureType = typeof(Koopa);
            CollisionBox.ColorType = typeof(Koopa);

            switch (Type) {
                case RedKoopa _:
                    Sprite = new KoopaSprite(2, 7, 9, 7, true, this);
                    break;
                case GreenKoopa _:
                    Sprite = new KoopaSprite(2, 7, 2, 0, true, this);
                    break;
                case RedShell _:
                    Sprite = new KoopaSprite(2, 7, 13, 9, true, this);
                    break;
                case GreenShell _:
                    Sprite = new KoopaSprite(2, 7, 7, 2, true, this);
                    break;
                case RedProjectile _:
                    Sprite = new KoopaSprite(2, 7, 13, 9, true, this);
                    break;
                case GreenProjectile _:
                    Sprite = new KoopaSprite(2, 7, 7, 2, true, this);
                    break;
            }
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
            if (obj is Avatar) {
                if (Type is GreenShell) {
                    GreenProjectile();
                    rGreenCollide = false;
                    lGreenCollide = true;
                } else if (Type is RedShell) {
                    RedProjectile();
                    rRedCollide = false;
                    lRedCollide = true;
                }
            }

            leftCol = true;
            rightCol = false;

            if (obj is Fireball) {
                Die();
            }
        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Avatar) {
                if (Type is GreenShell) {
                    GreenProjectile();
                    rGreenCollide = true;
                    lGreenCollide = false;
                } else if (Type is RedShell) {
                    RedProjectile();
                    rRedCollide = true;
                    lRedCollide = false;
                }
            }

            rightCol = true;
            leftCol = false;

            if (obj is Fireball) {
                Die();
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar) {
                if (Type is GreenKoopa) {
                    Type = new GreenShell();
                    GreenShell();

                } else if (Type is RedKoopa) {
                    Type = new RedShell();
                    RedShell();

                } else if (Type is GreenProjectile) {
                    GreenShell();
                } else if (Type is RedProjectile) {
                    RedShell();
                }

            } else if (obj is Fireball) {
                Die();
            }
        }

        protected void Die() {
            if (!isDead) {
                isDead = true;
                (Sprite as KoopaSprite).Die();
                timeDied = new TimeSpan();
            }
        }

        protected void GreenShell() {
            if (!isGreenShell) {
                isGreenShell = true;
                (Sprite as KoopaSprite).GreenShell();
                _velocity.X = 0;
                greenShellTime = new TimeSpan();
            }

        }

        protected void GreenKoopa() {
            (Sprite as KoopaSprite).GreenKoopa();
        }

        protected void RedShell() {
            if (!isRedShell) {
                isRedShell = true;
                (Sprite as KoopaSprite).RedShell();
                _velocity.X = 0;
                redShellTime = new TimeSpan();
            }
        }

        protected void RedKoopa() {
            (Sprite as KoopaSprite).RedKoopa();
        }

        protected void GreenProjectile() {
            if (!isGreenProjectile) {
                isGreenProjectile = true;
                (Sprite as KoopaSprite).GreenProjectile();
            }
        }

        protected void RedProjectile() {
            if (!isRedProjectile) {
                isRedProjectile = true;
                (Sprite as KoopaSprite).RedProjectile();
            }
        }

        public override void Update(GameTime gameTime) {
            if (isDead) {
                if (timeDied.TotalSeconds == 0) {
                    timeDied = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > timeDied + new TimeSpan(0, 0, 0, 0, 500)) {
                    ToBeRemoved = true;
                }
            }

            if (isGreenShell && !lGreenCollide && !rGreenCollide) {
                lGreenCollide = false;
                rGreenCollide = false;
                if (greenShellTime.TotalSeconds == 0) {
                    greenShellTime = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > greenShellTime + new TimeSpan(0, 0, 0, 0, 4000)) {
                    Type = new GreenKoopa();
                    GreenKoopa();
                    isGreenShell = false;
                }
            }
            if (isRedShell && !lRedCollide && !rRedCollide) {
                lRedCollide = false;
                rRedCollide = false;
                if (redShellTime.TotalSeconds == 0) {
                    redShellTime = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > redShellTime + new TimeSpan(0, 0, 0, 0, 4000)) {
                    Type = new RedKoopa();
                    RedKoopa();
                    isRedShell = false;
                }
            }
            if (isGreenProjectile && (lGreenCollide || rGreenCollide)) {
                Type = new GreenProjectile();
                GreenProjectile();
                if (lGreenCollide) {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                } else if (rGreenCollide) {
                    _velocity.X = -1;
                    (Sprite as KoopaSprite).FaceLeft();
                } else {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                }

                if (Position.X == 0) {
                    CollideLeft(this);
                }

                if (Position.X == (Level.MapWidth - 16)) {
                    CollideRight(this);
                }
                if (leftCol) {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                } else if (rightCol) {
                    _velocity.X = -1;
                    (Sprite as KoopaSprite).FaceLeft();
                } else {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                }
            }
            if (isRedShell && (lRedCollide || rRedCollide)) {
                Type = new RedProjectile();
                RedProjectile();
                if (lRedCollide == true) {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                } else if (rRedCollide == true) {
                    _velocity.X = -1;
                    (Sprite as KoopaSprite).FaceLeft();
                } else {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                }

                if (Position.X == 0) {
                    CollideLeft(this);
                }

                if (Position.X == (Level.MapWidth - 16)) {
                    CollideRight(this);
                }
                if (leftCol) {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                } else if (rightCol) {
                    _velocity.X = -1;
                    (Sprite as KoopaSprite).FaceLeft();
                } else {
                    _velocity.X = 1;
                    (Sprite as KoopaSprite).FaceRight();
                }

            }

            if (Type is GreenKoopa || Type is RedKoopa) {
                if ((Position.X >= Level.Avatar.Position.X + (Game1.ScreenWidth / 2) + 50 && Position.X >= Game1.ScreenWidth) || (Position.X <= Level.Avatar.Position.X - (Game1.ScreenWidth / 2) - 50 && Position.X <= Level.MapWidth - Game1.ScreenWidth)) {
                    _velocity.X = 0;
                } else {
                    if (Level.Avatar.Position.X <= Position.X && collisionCount == 0) {
                        CollideRight(this);
                        collisionCount++;
                    }

                    if (leftCol) {
                        _velocity.X = .5f;
                        (Sprite as KoopaSprite).FaceRight();
                    } else if (rightCol) {
                        _velocity.X = -.5f;
                        (Sprite as KoopaSprite).FaceLeft();
                    } else {
                        _velocity.X = .5f;
                        (Sprite as KoopaSprite).FaceRight();
                    }

                    if (Position.X == 0) {
                        CollideLeft(this);
                    }

                    if (Position.X == (Level.MapWidth - 16)) {
                        CollideRight(this);
                    }
                }
            }

            base.Update(gameTime);
        }
    }

    public interface IKoopa {

    }

    public class RedKoopa : IKoopa {

    }

    public class GreenKoopa : IKoopa {

    }

    public class GreenShell : IKoopa {

    }

    public class RedShell : IKoopa {

    }

    public class GreenProjectile : IKoopa {

    }

    public class RedProjectile : IKoopa {

    }
}
