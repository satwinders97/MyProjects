using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuaranteamProject {
    public class AvatarSprite : ISprite {
        public Vector2 Position {
            get => avatar.Position;
            set => avatar.Position = value;
        }

        private Avatar avatar;
        public bool IsMasked { get; set; }
        private Texture2D texture;

        private int[] frames;
        private int totFrames;
        private int currFrame;

        private int msSinceLastFrame = 0;
        private int msPerFrame = 175;

        private bool isFacingLeft;

        private int rowCnt = 6;
        private int colCnt = 20;

        private int currRow, currColumn;

        float timeElapsed = 0f;

        public AvatarSprite(Avatar a) {
            frames = new int[colCnt];
            totFrames = 1;

            avatar = a;

            currColumn = 2;

            switch (a.PowerState) {
                case SmallState _:
                case DeadState _:
                    currRow = 0;
                    break;
                case SuperState _:
                    currRow = 1;
                    break;
                case FireState _:
                    currRow = 2;
                    break;
            }
        }

        public void Update(GameTime gameTime) {

            if (!IsMasked) {
                if ((avatar.HorizontalState is LeftAvatar) || (avatar.HorizontalState is RunningLeftAvatar)) {
                    isFacingLeft = true;
                } else {
                    isFacingLeft = false;
                }

                switch (avatar.PowerState) {
                    case SmallState _:
                    case DeadState _:
                        currRow = 0;
                        break;
                    case SuperState _:
                        currRow = 1;
                        break;
                    case FireState _:
                        currRow = 2;
                        break;
                }

                switch (avatar.HorizontalState) {
                    case RunningLeftAvatar _:
                        if (avatar.VerticalState is IdleAvatar) {
                            frames[0] = 4;
                            frames[1] = 2;
                            frames[2] = 3;

                            if (avatar.PowerState is SmallState) {
                                totFrames = 2;
                            } else {
                                totFrames = 3;
                            }
                        }
                        break;
                    case RunningRightAvatar _:
                        if (avatar.VerticalState is IdleAvatar) {
                            frames[0] = 4;
                            frames[1] = 2;
                            frames[2] = 3;

                            if (avatar.PowerState is SmallState) {
                                totFrames = 2;
                            } else {
                                totFrames = 3;
                            }
                        }
                        break;
                    default:
                        totFrames = 1;
                        break;
                }

                switch (avatar.VerticalState) {
                    case IdleAvatar _:
                        if ((avatar.HorizontalState is LeftAvatar) || (avatar.HorizontalState is RightAvatar)) {
                            totFrames = 1;
                            frames[0] = 2;
                        }
                        break;
                    case CrouchingAvatar _:
                        if (avatar.PowerState is SmallState) {
                            totFrames = 1;
                            frames[0] = 2;
                        } else {
                            totFrames = 1;
                            frames[0] = 11;
                        }
                        break;
                    case JumpingAvatar _:
                        totFrames = 1;
                        frames[0] = 6;
                        break;
                    case FallingAvatar _:
                        totFrames = 1;
                        frames[0] = 8;
                        break;
                }

                if (avatar.PowerState is DeadState) {
                    totFrames = 1;
                    frames[0] = 19;
                }

                msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceLastFrame > msPerFrame) {
                    msSinceLastFrame = 0;
                    currFrame++;

                    if (currFrame >= totFrames) {
                        currFrame = 0;
                    }
                }
                currColumn = frames[currFrame];
            } else {
                timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ((avatar.HorizontalState is LeftAvatar) || (avatar.HorizontalState is RunningLeftAvatar)) {
                    isFacingLeft = true;
                } else {
                    isFacingLeft = false;
                }

                switch (avatar.PowerState) {
                    case SmallState _:
                    case DeadState _:
                        currRow = 3;
                        break;
                    case SuperState _:
                        currRow = 4;
                        break;
                    case FireState _:
                        currRow = 5;
                        break;
                }

                switch (avatar.HorizontalState) {
                    case RunningLeftAvatar _:
                        if (avatar.VerticalState is IdleAvatar) {
                            frames[0] = 4;
                            frames[1] = 2;
                            frames[2] = 3;

                            if (avatar.PowerState is SmallState) {
                                totFrames = 2;
                            } else {
                                totFrames = 3;
                            }
                        }
                        break;
                    case RunningRightAvatar _:
                        if (avatar.VerticalState is IdleAvatar) {
                            frames[0] = 4;
                            frames[1] = 2;
                            frames[2] = 3;

                            if (avatar.PowerState is SmallState) {
                                totFrames = 2;
                            } else {
                                totFrames = 3;
                            }
                        }
                        break;
                    default:
                        totFrames = 1;
                        break;
                }

                switch (avatar.VerticalState) {
                    case IdleAvatar _:
                        if ((avatar.HorizontalState is LeftAvatar) || (avatar.HorizontalState is RightAvatar)) {
                            totFrames = 1;
                            frames[0] = 2;
                        }
                        break;
                    case CrouchingAvatar _:
                        if (avatar.PowerState is SmallState) {
                            totFrames = 1;
                            frames[0] = 2;
                        } else {
                            totFrames = 1;
                            frames[0] = 11;
                        }
                        break;
                    case JumpingAvatar _:
                        totFrames = 1;
                        frames[0] = 6;
                        break;
                    case FallingAvatar _:
                        totFrames = 1;
                        frames[0] = 8;
                        break;
                }

                if (avatar.PowerState is DeadState) {
                    totFrames = 1;
                    frames[0] = 19;
                }

                msSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (msSinceLastFrame > msPerFrame) {
                    msSinceLastFrame = 0;
                    currFrame++;

                    if (currFrame >= totFrames) {
                        currFrame = 0;
                    }
                }
                currColumn = frames[currFrame];
            }
            if (timeElapsed > 18) {
                IsMasked = false;
                timeElapsed = 0;
            }
        }


        public void Draw(SpriteBatch spriteBatch) {
            texture = Game1.TextureMap[avatar.TextureType];

            int width = texture.Width / colCnt;
            int height = texture.Height / rowCnt;

            Rectangle sourceRectangle = new Rectangle(width * currColumn, height * currRow, width, height);
            Rectangle destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White, 0, new Vector2(), isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
        }
    }

}
