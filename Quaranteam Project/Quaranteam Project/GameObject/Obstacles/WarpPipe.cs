using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public class WarpPipe : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => Type == null ? Vector2.Zero : Type.GetCollisionSize(this);
        public IWarpPipeType Type { get; set; }

        public bool CanWarp => warpCmd != null;
        private WarpCommand warpCmd;

        public WarpPipe(TileMap level, Vector2 initPos, IWarpPipeType type, RawWarp warp) : base(level, initPos) {
            Type = type;
            TextureType = typeof(WarpPipe);
            CollisionBox.ColorType = typeof(WarpPipe);

            Sprite = Type.GetSprite(this);

            if (warp != null) {
                warpCmd = new WarpCommand(level.Game, warp);
            }
        }

        public override void CollideBottom(CollidableObject obj) { }
        public override void CollideLeft(CollidableObject obj) { }
        public override void CollideRight(CollidableObject obj) { }
        public override void CollideTop(CollidableObject obj) { }

        public override void Update(GameTime gameTime) { }

        public void Warp() {
            if (CanWarp) {
                warpCmd.Execute();
            }
        }
    }

    public interface IWarpPipeType {
        ISprite GetSprite(WarpPipe context);
        Vector2 GetCollisionSize(WarpPipe context);
    }
    public class End : IWarpPipeType {
        public ISprite GetSprite(WarpPipe context) {
            return new WarpPipeSprite(5, 2, 1, 2, 1, context);
        }

        public Vector2 GetCollisionSize(WarpPipe context) {
            return new Vector2(32, 32);
        }
    }
    public class Middle : IWarpPipeType {
        public ISprite GetSprite(WarpPipe context) {
            return new WarpPipeSprite(5, 2, 1, 1, 5, context);
        }

        public Vector2 GetCollisionSize(WarpPipe context) {
            return new Vector2(32, 16);
        }
    }
    public class Side : IWarpPipeType {
        public ISprite GetSprite(WarpPipe context) {
            return new WarpPipeSprite(5, 2, 2, 2, 6, context);
        }

        public Vector2 GetCollisionSize(WarpPipe context) {
            return new Vector2(64, 32);
        }
    }
}
