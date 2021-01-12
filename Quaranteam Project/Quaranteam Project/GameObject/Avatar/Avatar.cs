using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;

namespace QuaranteamProject {
    public class Avatar : CollidableObject {
        public int Toxicity { get; set; }

        public IAvatarPowerState PowerState { get; set; }
        public IAvatarVerticalState VerticalState { get; set; }
        public IAvatarHorizontalState HorizontalState { get; set; }

        public int ballCounter = 1;

        protected override Vector2 CollisionMin {
            get {
                if (PowerState == null) {
                    return Vector2.Zero;
                } else {
                    return PowerState.GetCollisionMin(this);
                }
            }
        }
        protected override Vector2 CollisionSize {
            get {
                if (PowerState == null) {
                    return Vector2.Zero;
                } else {
                    return PowerState.GetCollisionSize(this);
                }
            }
        }

        public TimeSpan TimeDied { get; set; }
        public TimeSpan TimeWon { get; set; }

        public bool IsDead { get; set; }
        public bool OnFlagpole { get; set; }
        public bool ReachedGate { get; set; }

        private bool canWarp;
        private bool warpInitiated;
        private TimeSpan warpAnimStart;
        private Vector2 savedAccel;
        private WarpPipe toWarp;

        public Avatar(TileMap level, Vector2 initPos) : base(level, initPos) {
            Init(new FireState(), new IdleAvatar(), new RightAvatar());
        }

        public Avatar(TileMap level, Vector2 initPos, IAvatarPowerState state, IAvatarVerticalState action, IAvatarHorizontalState orientation) : base(level, initPos) {
            Init(state, action, orientation);
        }

        private void Init(IAvatarPowerState state, IAvatarVerticalState action, IAvatarHorizontalState orientation) {
            TextureType = typeof(Avatar);
            CollisionBox.ColorType = typeof(Avatar);

            PowerState = state;
            VerticalState = action;
            HorizontalState = orientation;

            Sprite = new AvatarSprite(this);
        }

        public override void Update(GameTime gameTime) {
            //if (Position.Y >= Level.MapHeight - 34) {
            //    PowerState = new DeadState();
            //}

            if (Position.X <= 0) {
                _position.X++;
            } else if (Position.X >= Level.MapWidth - 16) {
                _position.X--;
            }

            if (PowerState is DeadState) {
                Velocity = Vector2.Zero;
            }

            if (Velocity.Y > 1) {
                VerticalState = new FallingAvatar();
            } else if (Velocity.Y < -1) {
                VerticalState = new JumpingAvatar();
            }

            if (Velocity.Y > -0.1f && Velocity.Y < 0.1f) {
                canWarp = true;
            } else {
                canWarp = false;
            }

            if (warpInitiated) {
                if (warpAnimStart.TotalMilliseconds == 0) {
                    warpAnimStart = gameTime.TotalGameTime;
                } else if (gameTime.TotalGameTime > warpAnimStart + new TimeSpan(0, 0, 0, 0, 500)) {
                    warpInitiated = false;
                    Warp(toWarp, savedAccel);
                }
            }
            if (Toxicity < 100 && Toxicity >= 51) {
                SmallMario();
            }
            if (Toxicity < 51 && Toxicity >= 26) {
                SuperMario();
            }
            if (Toxicity < 26 && Toxicity >= 0) {
                FireMario();
            }

            base.Update(gameTime);
        }

        public void UpAction() {
            if (VerticalState is CrouchingAvatar) {
                VerticalState = new IdleAvatar();
            } else if (!(VerticalState is JumpingAvatar || VerticalState is FallingAvatar)) {
                VerticalState.UpAction(this);
                if (PowerState is SuperState || PowerState is FireState) {
                    Game1.SoundMap["SuperJump"].Play();
                } else {
                    Game1.SoundMap["NormalJump"].Play();
                }

                _velocity.Y = -3.2f;
            }
        }

        public void DownAction() {
            VerticalState.DownAction(this);
            if (HorizontalState is RunningLeftAvatar || HorizontalState is RunningRightAvatar) {
                VerticalState = new IdleAvatar();
            }
        }

        public void LeftAction() {
            HorizontalState.LeftAction(this);
            if (VerticalState is CrouchingAvatar) {
                VerticalState = new IdleAvatar();
            }
        }

        public void RightAction() {
            HorizontalState.RightAction(this);
            if (VerticalState is CrouchingAvatar) {
                VerticalState = new IdleAvatar();
            }
        }

        public void SmallMario() {
            PowerState.SmallMario(this);
        }
        public void SuperMario() {
            PowerState.SuperMario(this);
        }
        public void FireMario() {
            PowerState.FireMario(this);
        }
        public void DeadMario() {
            PowerState.DeadMario(this);
        }
        public void TakeDamage() {
            if (PowerState is SuperState || PowerState is FireState) {
                PowerState = new SmallState();
            } else {
                Die();
            }
        }
        public void TakeToxicDamage(int amount) {
            Toxicity += amount;
            if (Toxicity >= 120) {
                CovidDie();
            }
        }
        public void Die() {
            CanCollide = false;
            TimeDied = new TimeSpan();
            PowerState = new DeadState();
            MediaPlayer.Stop();

            IsDead = true;
        }
        public void CovidDie() { // for toxicity-based death
            DeadMario();
            IsDead = true;
        }
        public void Respawn() {
            SmallMario();
            Position = new Vector2(Level.CheckpointPosition.X, Level.CheckpointPosition.Y);
            IsDead = false;
            CanCollide = true;
            HorizontalState = new RightAvatar();
            Toxicity = 0;
        }

        public override void CollideLeft(CollidableObject obj) {


            if ((obj is MiniCovid || obj is CovidBoss) && !Sprite.IsMasked) {
                TakeToxicDamage(18);
            }
            if (obj is Block || obj is WarpPipe || obj is Cloud) {
                RightAction();
            } else if (obj is Flagpole) {
                Game1.SoundMap["FlagPole"].Play();
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                _velocity.Y = 0.5f;
                Acceleration = new Vector2(0, 0);
                RightAction();
            } else if (obj is Peach) {
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                Acceleration = new Vector2(0, 0);
                RightAction();
            } else if (obj is PowerUp) {
                CollidePowerUp(obj as PowerUp);
            } else if (obj is Gate) {
                ReachedGate = true;
            }
        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Block || obj is Cloud) {
                LeftAction();
            } else if (obj is WarpPipe) {
                bool canFit = CollisionBox.Min.Y >= (obj as WarpPipe).CollisionBox.Min.Y && CollisionBox.Max.Y <= (obj as WarpPipe).CollisionBox.Max.Y;
                if (HorizontalState is RunningRightAvatar && (obj as WarpPipe).Type is Side && canFit) {
                    InitWarpRight(obj as WarpPipe);
                } else {
                    LeftAction();
                }
            } else if (obj is Flagpole) {
                Game1.SoundMap["FlagPole"].Play();
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                _velocity.Y = 0.5f;
                Acceleration = new Vector2(0, 0);
                LeftAction();
            } else if (obj is Peach) {
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                Acceleration = new Vector2(0, 0);
                LeftAction();
            } else if (obj is PowerUp) {
                CollidePowerUp(obj as PowerUp);
            } else if (obj is Gate) {
                ReachedGate = true;
            }
            if ((obj is MiniCovid || obj is CovidBoss) && !Sprite.IsMasked) {
                TakeToxicDamage(18);
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Block || obj is WarpPipe || obj is Cloud) {
                DownAction();
                _velocity.Y = 0;
            } else if (obj is Flagpole) {
                Game1.SoundMap["FlagPole"].Play();
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                _velocity.Y = 0.5f;
                Acceleration = new Vector2(0, 0);
                DownAction();

            } else if (obj is Peach) {
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                Acceleration = new Vector2(0, 0);
                DownAction();
            } else if (obj is PowerUp) {
                CollidePowerUp(obj as PowerUp);
            }

            if ((obj is MiniCovid || obj is CovidBoss) && !Sprite.IsMasked) {
                TakeToxicDamage(18);
                DownAction();
            }
        }
        public override void CollideBottom(CollidableObject obj) {
            if (obj is PowerUp) {
                CollidePowerUp(obj as PowerUp);
            } else if (obj is Goomba || obj is Koopa) {
                Game1.SoundMap["Stomp"].Play();
                _velocity.Y = -1;
            } else if (obj is Peach) {
                TimeWon = new TimeSpan();
                OnFlagpole = true;
                Velocity = new Vector2(0, -1);
                Acceleration = new Vector2(0, 0);
                UpAction();
            } else if (obj is Cloud) {
                if (VerticalState is IdleAvatar) {
                    if ((HorizontalState is LeftAvatar || HorizontalState is RightAvatar)) {
                        _velocity.X = obj.Velocity.X;
                    }
                    _velocity.Y = obj.Velocity.Y;
                } else {
                    _velocity.Y = 0;
                }

                if (!(VerticalState is CrouchingAvatar)) {
                    VerticalState = new IdleAvatar();
                }
            } else if (obj is Block) {
                _velocity.Y = 0;
                if (!(VerticalState is CrouchingAvatar)) {
                    VerticalState = new IdleAvatar();
                }
            } else if (obj is WarpPipe) {
                _velocity.Y = 0;
                bool canFit = CollisionBox.Min.X >= (obj as WarpPipe).CollisionBox.Min.X && CollisionBox.Max.X <= (obj as WarpPipe).CollisionBox.Max.X;
                if (VerticalState is CrouchingAvatar && (obj as WarpPipe).Type is End && canFit) {
                    InitWarpDown(obj as WarpPipe);
                } else {
                    VerticalState = new IdleAvatar();
                }
            }

            if ((obj is MiniCovid || obj is CovidBoss) && !Sprite.IsMasked) {
                TakeToxicDamage(18);
                _velocity.Y = -1.75f; // bounce higher here than for goomba

            } else if (obj is MiniCovid || obj is CovidBoss) { _velocity.Y = -1.75f; }

        }

        private void CollidePowerUp(PowerUp powerUp) {
            if (powerUp.State is Flower) {
                FireMario();
                Game1.HUDMap["Score"] += 1000;
            } else if (powerUp.State is SuperMushroom) {
                SuperMario();
                Game1.HUDMap["Score"] += 1000;
            } else if (powerUp.State is LifeMushroom) {
                Game1.HUDMap["Lives"]++;
            } else if (powerUp.State is Vaccine) {
                Toxicity -= 40;
                if (Toxicity < 0) {
                    Toxicity = 0;
                }
            } else if (powerUp.State is Mask) {
                Sprite.IsMasked = true;
            } else if (powerUp.State is Sanitizer) {
                ballCounter += 6; // we can change back to 5 if we want
            }
        }

        private void InitWarpDown(WarpPipe pipe) {
            if (!pipe.CanWarp || !canWarp) {
                return;
            }

            savedAccel = Acceleration;
            toWarp = pipe;
            CanCollide = false;
            Acceleration = Vector2.Zero;
            Velocity = new Vector2(0, 1);

            Game1.SoundMap["PipeTravel"].Play();

            warpInitiated = true;
            warpAnimStart = new TimeSpan();
        }

        private void InitWarpRight(WarpPipe pipe) {
            if (!pipe.CanWarp || !canWarp) {
                return;
            }

            savedAccel = Acceleration;
            toWarp = pipe;
            CanCollide = false;
            Acceleration = Vector2.Zero;
            Velocity = new Vector2(1, 0);

            Game1.SoundMap["PipeTravel"].Play();

            warpInitiated = true;
            warpAnimStart = new TimeSpan();
        }

        private void Warp(WarpPipe pipe, Vector2 accel) {
            pipe.Warp();
            CanCollide = true;
            Acceleration = accel;
        }
    }
}
