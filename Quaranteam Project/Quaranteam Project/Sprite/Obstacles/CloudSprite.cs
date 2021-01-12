using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject.Sprite.Obstacles {
    public abstract class CloudSprite : ISprite {
        public abstract Vector2 Position { get; set; }
        public bool IsMasked { get; set; }
        protected Texture2D texture;

        protected int[] frames;
        protected int totFrames;
        protected int currFrame;

        public CloudSprite() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[typeof(Cloud)];

            spriteBatch.Draw(texture, Position, Color.White);
        }

    }

    public class SingleCloudSprite : CloudSprite {
        public override Vector2 Position {
            get => cloudContext.Position;
            set => cloudContext.Position = value;
        }
        private SingleCloud cloudContext;

        public SingleCloudSprite(SingleCloud context) : base() {
            cloudContext = context;
        }

        public override void Update(GameTime gameTime) {
            if (Position.X >= Game1.ScreenWidth - 48 || Position.X < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X * -1, cloudContext.Velocity.Y);
            }

            if (Position.Y >= Game1.ScreenHeight - 32 || Position.Y < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X, cloudContext.Velocity.Y * -1);
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[typeof(SingleCloud)];

            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
    public class DoubleCloudSprite : CloudSprite {
        public override Vector2 Position {
            get => cloudContext.Position;
            set => cloudContext.Position = value;
        }

        private DoubleCloud cloudContext;

        public DoubleCloudSprite(DoubleCloud context) : base() {
            cloudContext = context;
        }
        public override void Update(GameTime gameTime) {
            if (Position.X >= Game1.ScreenWidth - 80 || Position.X < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X * -1, cloudContext.Velocity.Y);
            }

            if (Position.Y >= Game1.ScreenHeight - 32 || Position.Y < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X, cloudContext.Velocity.Y * -1);
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[typeof(DoubleCloud)];

            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
    public class TripleCloudSprite : CloudSprite {
        public override Vector2 Position {
            get => cloudContext.Position;
            set => cloudContext.Position = value;
        }

        private TripleCloud cloudContext;

        public TripleCloudSprite(TripleCloud context) : base() {
            cloudContext = context;
        }
        public override void Update(GameTime gameTime) {
            if (Position.X >= Game1.ScreenWidth - 112 || Position.X < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X * -1, cloudContext.Velocity.Y);
            }

            if (Position.Y >= Game1.ScreenHeight - 32 || Position.Y < 0) {
                cloudContext.Velocity = new Vector2(cloudContext.Velocity.X, cloudContext.Velocity.Y * -1);
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[typeof(TripleCloud)];

            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}
