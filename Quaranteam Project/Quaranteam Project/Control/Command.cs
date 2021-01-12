using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

namespace QuaranteamProject {


    public interface ICommand {
        void Execute();
    }

    public class ExitCommand : ICommand {
        protected Game1 receiver;

        public ExitCommand(Game1 receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.Exit();
        }
    }

    public class ResetCommand : ICommand {
        protected Game1 receiver;
        public ResetCommand(Game1 receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.ResetGameLevel(receiver.CurrentLevelName);
            if (receiver.IsGameOver) {
                receiver.IsGameOver = false;
                receiver.EmptyKeyBinds();
                receiver.GamePlayKeyBinds();
            }
        }
    }

    public class WarpCommand : ICommand {
        protected Game1 receiver;
        protected RawWarp warp;
        public WarpCommand(Game1 receiver, RawWarp warp) {
            this.receiver = receiver;
            this.warp = warp;
        }
        public void Execute() {
            IAvatarPowerState oldPower = receiver.Level.Avatar.PowerState;
            receiver.LoadGameLevel(warp.Level);
            if (warp.X != null && warp.Y != null) {
                receiver.Level.Avatar.Position = new Vector2((float)warp.X * 16, (float)warp.Y * 16);
                receiver.Level.CheckpointPosition = new Vector2((float)warp.X * 16, (float)warp.Y * 16);
            }
            receiver.Level.Avatar.PowerState = oldPower;
        }
    }

    public class PauseCommand : ICommand {
        protected Game1 receiver;
        public PauseCommand(Game1 receiver) {
            this.receiver = receiver;
        }
        public void Execute() {
            receiver.Paused = true;
            receiver.EmptyKeyBinds();
            receiver.PauseKeyBinds();
        }
    }

    public class UnpauseCommand : ICommand {
        protected Game1 receiver;
        public UnpauseCommand(Game1 receiver) {
            this.receiver = receiver;
        }
        public void Execute() {
            receiver.Paused = false;
            receiver.EmptyKeyBinds();
            receiver.GamePlayKeyBinds();
        }
    }

    public class UpCommand : ICommand {
        protected Avatar receiver;

        public UpCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.UpAction();
        }
    }

    public class CrouchCommand : ICommand {
        protected Avatar receiver;

        public CrouchCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.DownAction();
        }
    }

    public class LeftCommand : ICommand {
        protected Avatar receiver;

        public LeftCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.LeftAction();
        }
    }

    public class RightCommand : ICommand {
        protected Avatar receiver;
        public RightCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.RightAction();
        }
    }

    public class DashCommand : ICommand {
        protected Avatar receiver;

        public DashCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            // receiver.DashAction();
        }
    }

    public class QuestionBlockCommand : ICommand {
        protected Block receiver;

        public QuestionBlockCommand(Block receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.BumpAction();
        }
    }

    public class HiddenBlockCommand : ICommand {
        protected Block receiver;
        protected Avatar avatarState;

        public HiddenBlockCommand(Block receiver, Avatar avatarState) {
            this.receiver = receiver;
            this.avatarState = avatarState;
        }

        public void Execute() {
            if ((receiver is QuestionBlock && (receiver as QuestionBlock).State is HiddenQuestionBlock) ||
            (receiver is BrickBlock && (receiver as BrickBlock).State is HiddenBrickBlock)) {
                receiver.BumpAction();
            }
        }
    }

    public class BrickBlockCommand : ICommand {
        protected Block receiver;
        protected Avatar avatarState;

        public BrickBlockCommand(Block receiver, Avatar avatarState) {
            this.receiver = receiver;
            this.avatarState = avatarState;
        }

        public void Execute() {
            if (avatarState.PowerState is SmallState)   // || has any remaining items
            {
                // if small mario (if it has any items it does this regardless of marios size)
                receiver.BumpAction();
            } else {
                // if large mario
                receiver.BreakAction();
            }
        }
    }

    public class StandardMarioCommand : ICommand {
        protected Avatar receiver;

        public StandardMarioCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.SmallMario();
        }
    }

    public class SuperMarioCommand : ICommand {
        protected Avatar receiver;

        public SuperMarioCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.SuperMario();
        }
    }

    public class FireMarioCommand : ICommand {
        protected Avatar receiver;

        public FireMarioCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            receiver.FireMario();
        }
    }

    public class DamageCommand : ICommand {
        protected Avatar receiver;

        public DamageCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            if (receiver.PowerState is SmallState) {
                //receiver.DeadMario();
                receiver.Die();
            } else if (receiver.PowerState is SuperState) {
                receiver.SmallMario();
            } else if (receiver.PowerState is FireState) {
                receiver.SmallMario();
            }
        }
    }

    public class ToggleCollisionBoxCommand : ICommand {
        private readonly List<IGameObject> gameObject;
        private bool isVisible;

        public ToggleCollisionBoxCommand(List<IGameObject> obj) {
            gameObject = obj;
        }

        public void Execute() {
            isVisible = !isVisible;
            foreach (IGameObject obj in gameObject) {
                if (obj is CollidableObject) {
                    (obj as CollidableObject).CollisionBox.IsVisible = isVisible;
                }
            }
        }
    }

    public class SanitizerballCommand : ICommand {
        protected Avatar receiver;
        public SanitizerballCommand(Avatar receiver) {
            this.receiver = receiver;
        }

        public void Execute() {
            if (receiver.Level.GameObjects.Where(x => x is Sanitizerball).Count() > 1) { // don't spawn one if there's already 1 or more
                return;
            }

            if (receiver.ballCounter <= 0) { // don't spawn one if out of ammo
                return;
            }

            receiver.ballCounter -= 1;

            Vector2 initPos = new Vector2(receiver.Position.Y);
            Vector2 initVel = new Vector2();
            if (receiver.HorizontalState is RightAvatar || receiver.HorizontalState is RunningRightAvatar) {
                initPos.X = receiver.Position.X + 20;
                initVel.X = 2;
            } else {
                initPos.X = receiver.Position.X - 2;
                initVel.X = -2;
            }

            receiver.Level.GameObjects.Add(new Sanitizerball(receiver.Level, initPos, initVel));
        }
    }

    public class MuteCommand : ICommand {
        protected float receiver;

        public MuteCommand(float receiver) {
            this.receiver = receiver;
        }

        public void Execute() {


            if (MediaPlayer.IsMuted) {
                MediaPlayer.IsMuted = false;
                SoundEffect.MasterVolume = receiver;

            } else {
                MediaPlayer.IsMuted = true;
                SoundEffect.MasterVolume = 0;
            }
        }
    }
}
