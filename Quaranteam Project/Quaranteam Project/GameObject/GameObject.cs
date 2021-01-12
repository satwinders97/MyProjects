using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QuaranteamProject {
    public interface IGameObject {
        TileMap Level { get; set; }
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Acceleration { get; set; }
        ISprite Sprite { get; set; }
        Type TextureType { get; set; }
        bool ToBeRemoved { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void Init();
    }

    public abstract class CollidableObject : IGameObject {
        public TileMap Level { get; set; }
        public Vector2 Position {
            get => _position;
            set {
                _position = value;
                UpdateCollision(Level.ColliderGrid);
            }
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

        public bool ToBeRemoved { get; set; }
        public ISprite Sprite { get; set; }
        public Type TextureType { get; set; }
        public CollisionBox CollisionBox { get; set; }
        public bool CanCollide { get; set; } // determines if this object can interact via collisions at all
        public bool CanClip { get; set; } // determines if this object will receive a counter-force when colliding
        protected abstract Vector2 CollisionMin { get; }
        protected abstract Vector2 CollisionSize { get; }

        protected CollidableObject(TileMap level, Vector2 initPos) {
            Level = level;
            CollisionBox = new CollisionBox();
            Position = initPos;
            Velocity = new Vector2();
            Acceleration = level.GetGravity();
            CanCollide = true;
            CanClip = true;
        }
        protected CollidableObject(TileMap level, Vector2 initPos, Vector2 initVel, Vector2 initAccel) {
            Level = level;
            CollisionBox = new CollisionBox();
            Position = initPos;
            Velocity = initVel;
            Acceleration = initAccel;
            CanCollide = true;
            CanClip = true;
        }

        public void Draw(SpriteBatch spriteBatch) {
            Sprite.Draw(spriteBatch);
            CollisionBox.Draw(spriteBatch);
        }

        public virtual void Update(GameTime gameTime) {
            Velocity += Acceleration;
            Position += Velocity;

            Sprite.Update(gameTime);
        }

        public virtual void Init() {
            UpdateCollision(Level.ColliderGrid);
        }

        protected virtual void UpdateCollision(ColliderGrid colliderGrid) {
            CollisionBox newBox = new CollisionBox(CollisionMin, CollisionMin + CollisionSize);
            colliderGrid.Update(this, CollisionBox, newBox);
            CollisionBox.Min = newBox.Min;
            CollisionBox.Max = newBox.Max;
        }

        public abstract void CollideLeft(CollidableObject obj);
        public abstract void CollideRight(CollidableObject obj);
        public abstract void CollideTop(CollidableObject obj);
        public abstract void CollideBottom(CollidableObject obj);
    }
}
