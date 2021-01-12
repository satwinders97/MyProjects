using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public interface IBrickBlockState {
        void Bump(BrickBlock context);
        void Break(BrickBlock context);
        void Update(BrickBlock context, GameTime gameTime);
    }

    public class IdleBrickBlock : IBrickBlockState {
        public void Bump(BrickBlock context) {
            context.Velocity = new Vector2(context.Velocity.X, -1);
            if (context.Items.Count == 0) {
                context.State = new BumpedBrickBlock(false); // if it never had items to begin with, return to idle after
            } else {
                context.State = new BumpedBrickBlock(true); // else enable returning as a used block
            }
        }
        public void Break(BrickBlock context) {
            context.State = new BrokenBrickBlock();
        }
        public void Update(BrickBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }

    public class BumpedBrickBlock : IBrickBlockState {
        private bool toUsedBlock;
        public BumpedBrickBlock(bool toUsed) {
            toUsedBlock = toUsed;
        }
        public void Bump(BrickBlock context) { }
        public void Break(BrickBlock context) { }
        public void Update(BrickBlock context, GameTime gameTime) {
            if (context.Position.Y + context.Velocity.Y <= context.minY) {
                context.Velocity = new Vector2(context.Velocity.X, -context.Velocity.Y);
                if (context.Items.Count > 0) {
                    context.SpawnItem(); // spawn item at peak, so collision isn't messy
                }
            } else if (context.Position.Y + context.Velocity.Y >= context.maxY) {
                if (toUsedBlock && context.Items.Count == 0) {
                    context.State = new UsedBrickBlock();
                } else {
                    context.State = new IdleBrickBlock();
                }
            }
        }
    }

    public class UsedBrickBlock : IBrickBlockState {
        public void Bump(BrickBlock context) { }
        public void Break(BrickBlock context) { }
        public void Update(BrickBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }

    public class BrokenBrickBlock : IBrickBlockState {
        public void Bump(BrickBlock context) { }
        public void Break(BrickBlock context) { }
        public void Update(BrickBlock context, GameTime gameTime) {
            if (context.Position.Y <= context.Level.MapHeight) {
                context.Velocity = new Vector2(context.Velocity.X, 1);
            } else {
                context.ToBeRemoved = true;
            }
        }
    }

    public class HiddenBrickBlock : IBrickBlockState {
        public void Bump(BrickBlock context) {
            context.Velocity = new Vector2(context.Velocity.X, -1);
            if (context.Items.Count == 0) {
                context.State = new BumpedBrickBlock(false); // if it never had items to begin with, return to idle after
            } else {
                context.State = new BumpedBrickBlock(true); // else enable returning as a used block
            }
        }
        public void Break(BrickBlock context) {
            context.Velocity = new Vector2(context.Velocity.X, -1);
            if (context.Items.Count == 0) {
                context.State = new BumpedBrickBlock(false); // if it never had items to begin with, return to idle after
            } else {
                context.State = new BumpedBrickBlock(true); // else enable returning as a used block
            }
        }
        public void Update(BrickBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }
}
