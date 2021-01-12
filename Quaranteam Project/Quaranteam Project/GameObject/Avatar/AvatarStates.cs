using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public interface IAvatarPowerState {
        void SmallMario(Avatar context);
        void SuperMario(Avatar context);
        void FireMario(Avatar context);
        void DeadMario(Avatar context);
        Vector2 GetCollisionMin(Avatar context);
        Vector2 GetCollisionSize(Avatar context);
    }

    public interface IAvatarVerticalState {
        void UpAction(Avatar context);
        void DownAction(Avatar context);
    }

    public interface IAvatarHorizontalState {
        void LeftAction(Avatar context);
        void RightAction(Avatar context);
    }

    public class SmallState : IAvatarPowerState {
        public void SmallMario(Avatar context) { }

        public void SuperMario(Avatar context) {
            context.PowerState = new SuperState();
        }

        public void FireMario(Avatar context) {
            context.PowerState = new FireState();
        }

        public void DeadMario(Avatar context) {
            context.PowerState = new DeadState();
        }
        public Vector2 GetCollisionMin(Avatar context) {
            return new Vector2(context.Position.X + 11, context.Position.Y + 16);
        }
        public Vector2 GetCollisionSize(Avatar context) {
            return new Vector2(10, 16);
        }
    }

    public class SuperState : IAvatarPowerState {
        public void SmallMario(Avatar context) {
            context.PowerState = new SmallState();
        }

        public void SuperMario(Avatar context) { }

        public void FireMario(Avatar context) {
            context.PowerState = new FireState();
        }

        public void DeadMario(Avatar context) {
            context.PowerState = new DeadState();
        }
        public Vector2 GetCollisionMin(Avatar context) {
            return new Vector2(context.Position.X + 10, context.Position.Y + 7);
        }
        public Vector2 GetCollisionSize(Avatar context) {
            return new Vector2(12, 25);
        }
    }

    public class FireState : IAvatarPowerState {
        public void SmallMario(Avatar context) {
            context.PowerState = new SmallState();
        }

        public void SuperMario(Avatar context) {
            context.PowerState = new SuperState();
        }

        public void FireMario(Avatar context) { }

        public void DeadMario(Avatar context) {
            context.PowerState = new DeadState();
        }
        public Vector2 GetCollisionMin(Avatar context) {
            return new Vector2(context.Position.X + 10, context.Position.Y + 7);
        }
        public Vector2 GetCollisionSize(Avatar context) {
            return new Vector2(12, 25);
        }
    }

    public class DeadState : IAvatarPowerState {
        public void SmallMario(Avatar context) {
            context.PowerState = new SmallState();
        }

        public void SuperMario(Avatar context) {
            context.PowerState = new SuperState();
        }

        public void FireMario(Avatar context) {
            context.PowerState = new FireState();
        }

        public void DeadMario(Avatar context) { }
        public Vector2 GetCollisionMin(Avatar context) {
            return new Vector2(context.Position.X + 11, context.Position.Y + 16);
        }
        public Vector2 GetCollisionSize(Avatar context) {
            return new Vector2(10, 16);
        }
    }


    public class IdleAvatar : IAvatarVerticalState {
        public void UpAction(Avatar context) {
            context.VerticalState = new JumpingAvatar();
        }

        public void DownAction(Avatar context) {
            context.VerticalState = new CrouchingAvatar();
        }
    }

    public class CrouchingAvatar : IAvatarVerticalState {
        public void UpAction(Avatar context) {
            context.VerticalState = new IdleAvatar();
        }

        public void DownAction(Avatar context) { }
    }

    public class JumpingAvatar : IAvatarVerticalState {
        public void UpAction(Avatar context) { }

        public void DownAction(Avatar context) {
            context.VerticalState = new IdleAvatar();

        }
    }

    public class FallingAvatar : IAvatarVerticalState {
        public void UpAction(Avatar context) { }

        public void DownAction(Avatar context) { }
    }

    public class LeftAvatar : IAvatarHorizontalState {
        public void LeftAction(Avatar context) {
            context.HorizontalState = new RunningLeftAvatar();
            context.Velocity = new Vector2(-1, context.Velocity.Y);
        }

        public void RightAction(Avatar context) {
            context.HorizontalState = new RightAvatar();
        }
    }

    public class RunningLeftAvatar : IAvatarHorizontalState {
        public void LeftAction(Avatar context) { }

        public void RightAction(Avatar context) {
            context.HorizontalState = new LeftAvatar();
            context.Velocity = new Vector2(0, context.Velocity.Y);
        }
    }

    public class RightAvatar : IAvatarHorizontalState {
        public void LeftAction(Avatar context) {
            context.HorizontalState = new LeftAvatar();
        }

        public void RightAction(Avatar context) {
            context.HorizontalState = new RunningRightAvatar();
            context.Velocity = new Vector2(1, context.Velocity.Y);
        }
    }

    public class RunningRightAvatar : IAvatarHorizontalState {
        public void LeftAction(Avatar context) {
            context.HorizontalState = new RightAvatar();
            context.Velocity = new Vector2(0, context.Velocity.Y);
        }

        public void RightAction(Avatar context) { }
    }

}
