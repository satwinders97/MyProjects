using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QuaranteamProject {
    public class Flagpole : CollidableObject {
        protected override Vector2 CollisionMin => new Vector2(Position.X + 4, Position.Y + 7);
        protected override Vector2 CollisionSize => new Vector2(8, 176);
        private bool isFalling;
        public bool Animate { get; set; }
        public Flagpole(TileMap level, Vector2 initPos) : base(level, initPos) {
            TextureType = typeof(Flagpole);
            CollisionBox.ColorType = typeof(Flagpole);

            Sprite = new FlagpoleSprite(1, 1, 1, 0, false, this);
        }

        public override void CollideBottom(CollidableObject obj) { }
        public override void CollideLeft(CollidableObject obj) {
            if (obj is Avatar) {
                if (!isFalling) {
                    if (Level.Avatar.Position.Y > 0 && Level.Avatar.Position.Y <= 6) {
                        Game1.HUDMap["Lives"]++;
                        isFalling = true;
                    } else if (Level.Avatar.Position.Y > 6 && Level.Avatar.Position.Y <= 32) {
                        Game1.HUDMap["Score"] += 4000;
                        isFalling = true;
                    } else if (Level.Avatar.Position.Y > 32 && Level.Avatar.Position.Y <= 78) {
                        Game1.HUDMap["Score"] += 2000;
                        isFalling = true;
                    } else if (Level.Avatar.Position.Y > 78 && Level.Avatar.Position.Y <= 102) {
                        Game1.HUDMap["Score"] += 800;
                        isFalling = true;
                    } else if (Level.Avatar.Position.Y > 102 && Level.Avatar.Position.Y <= 142) {
                        Game1.HUDMap["Score"] += 400;
                        isFalling = true;
                    } else if (Level.Avatar.Position.Y > 142 && Level.Avatar.Position.Y <= 160) {
                        Game1.HUDMap["Score"] += 100;
                        isFalling = true;
                    }
                    Game1.HUDMap["Score"] += (Game1.HUDMap["Time"] * 2);
                    Animate = true;
                }
            }
        }

        public override void CollideRight(CollidableObject obj) {
            if (obj is Avatar) {
                if (obj is Avatar) {
                    if (!isFalling) {
                        if (Level.Avatar.Position.Y > 0 && Level.Avatar.Position.Y <= 6) {
                            Game1.HUDMap["Lives"]++;
                            isFalling = true;
                        } else if (Level.Avatar.Position.Y > 6 && Level.Avatar.Position.Y <= 32) {
                            Game1.HUDMap["Score"] += 4000;
                            isFalling = true;
                        } else if (Level.Avatar.Position.Y > 32 && Level.Avatar.Position.Y <= 78) {
                            Game1.HUDMap["Score"] += 2000;
                            isFalling = true;
                        } else if (Level.Avatar.Position.Y > 78 && Level.Avatar.Position.Y <= 102) {
                            Game1.HUDMap["Score"] += 1000;
                            isFalling = true;
                        } else if (Level.Avatar.Position.Y > 102 && Level.Avatar.Position.Y <= 142) {
                            Game1.HUDMap["Score"] += 400;
                            isFalling = true;
                        } else if (Level.Avatar.Position.Y > 142 && Level.Avatar.Position.Y <= 160) {
                            Game1.HUDMap["Score"] += 100;
                            isFalling = true;
                        }

                        Animate = true;
                    }
                }
            }
        }

        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar) {
                if (!isFalling) {
                    Game1.HUDMap["Lives"]++;
                    isFalling = true;
                    Animate = true;
                }
            }
        }

        public override void Update(GameTime gameTime) { }
    }

    public class Flag : IGameObject {
        public TileMap Level { get; set; }
        public Vector2 Position {
            get => _position;
            set => _position = value;
        }
        protected Vector2 _position;
        public Vector2 Velocity {
            get => _velocity;
            set => _velocity = value;
        }
        protected Vector2 _velocity;
        public Vector2 Acceleration {
            get => _acceleration;
            set => _acceleration = value;
        }
        protected Vector2 _acceleration;

        public ISprite Sprite { get; set; }
        public Type TextureType { get; set; }
        public bool ToBeRemoved { get; set; }

        private readonly Flagpole flagpole;

        public Flag(TileMap level, Flagpole pole) {
            TextureType = typeof(Flag);

            Level = level;
            flagpole = pole;

            Position = new Vector2(flagpole.Position.X - 8, flagpole.Position.Y + 18);
            Velocity = new Vector2();
            Acceleration = new Vector2();

            Sprite = new FlagSprite(1, 1, 1, 0, false, this);
        }

        public void Update(GameTime gameTime) {
            if (flagpole.Animate) {
                if (Position.Y <= flagpole.Position.Y + 145) {
                    _position.Y += 0.5f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            Sprite.Draw(spriteBatch);
        }
        public void Init() { }
    }
}
