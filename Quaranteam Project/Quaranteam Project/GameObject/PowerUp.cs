using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class PowerUp : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(16, 16);
        public IPowerUp State { get; set; }

        public PowerUp(TileMap level, Vector2 initPos, IPowerUp state) : base(level, initPos) {
            State = state;
            TextureType = State.GetTextureType();
            CollisionBox.ColorType = typeof(PowerUp);

            if (State is Flower) {
                Sprite = new PowerUpSprite(1, 2, 0, this);
            } else if (State is SuperMushroom) {
                Sprite = new PowerUpSprite(1, 2, 1, this);
            } else if (State is LifeMushroom) {
                Sprite = new PowerUpSprite(1, 2, 0, this);
            } else if (State is Starmans) {
                Sprite = new PowerUpSprite(1, 2, 1, this);
            } else if (State is Sanitizer) {
                Sprite = new PowerUpSprite(1, 1, 0, this);
                Acceleration = Vector2.Zero;
                CanClip = false;
            } else if (State is Vaccine) {
                Sprite = new PowerUpSprite(1, 1, 0, this);
                Acceleration = Vector2.Zero;
                CanClip = false;
            } else if (State is Mask) {
                Sprite = new PowerUpSprite(1, 1, 0, this);
                Acceleration = Vector2.Zero;
                CanClip = false;
            }
        }

        public override void CollideLeft(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                if (State is LifeMushroom) {
                    Game1.SoundMap["LifeShroom"].Play();
                } else if (State is SuperMushroom) {
                    Game1.SoundMap["PUpShroom"].Play();
                }
            } else {
                _velocity.X = .5f;
            }
        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                if (State is LifeMushroom) {
                    Game1.SoundMap["LifeShroom"].Play();
                } else if (State is SuperMushroom) {
                    Game1.SoundMap["PUpShroom"].Play();
                }
            } else {
                _velocity.X = -.5f;
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                if (State is LifeMushroom) {
                    Game1.SoundMap["LifeShroom"].Play();
                } else if (State is SuperMushroom) {
                    Game1.SoundMap["PUpShroom"].Play();
                }
            }
        }
        public override void CollideBottom(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                if (State is LifeMushroom) {
                    Game1.SoundMap["LifeShroom"].Play();
                } else if (State is SuperMushroom) {
                    Game1.SoundMap["PUpShroom"].Play();
                }
            } else {
                _velocity.Y = 0;
            }
        }
    }

    public interface IPowerUp {
        void CreateAction(PowerUp powerUpContext);
        Type GetTextureType();
    }

    public class Flower : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new Flower();
        }
        public Type GetTextureType() {
            return typeof(Flower);
        }
    }

    public class SuperMushroom : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new SuperMushroom();
        }
        public Type GetTextureType() {
            return typeof(SuperMushroom);
        }
    }
    public class LifeMushroom : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new LifeMushroom();
        }
        public Type GetTextureType() {
            return typeof(LifeMushroom);
        }
    }
    public class Starmans : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new Starmans();
        }
        public Type GetTextureType() {
            return typeof(Starmans);
        }
    }
    public class Sanitizer : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new Sanitizer();
        }
        public Type GetTextureType() {
            return typeof(Sanitizer);
        }
    }

    public class Vaccine : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new Vaccine();
        }
        public Type GetTextureType() {
            return typeof(Vaccine);
        }
    }
    public class Mask : IPowerUp {
        public void CreateAction(PowerUp context) {
            context.State = new Mask();
        }
        public Type GetTextureType() {
            return typeof(Mask);
        }
    }
}