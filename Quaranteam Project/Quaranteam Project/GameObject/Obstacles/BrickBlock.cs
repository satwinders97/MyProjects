using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuaranteamProject {
    public class BrickBlock : Block {
        public IBrickBlockState State { get; set; }
        public List<IGameObject> Items { get; set; } // includes powerups and coins, but is not enforced
        public bool IsBlue { get; set; }
        public float minY, maxY;

        public BrickBlock(TileMap level, Vector2 initPos, IBrickBlockState initState, List<IGameObject> hiddenItems, bool blue) : base(level, initPos) {
            State = initState;
            Sprite = new BrickBlockSprite(this);
            minY = Position.Y - 10;
            maxY = Position.Y;
            Items = hiddenItems;
            IsBlue = blue;
            if (Items.Any(x => !(x is PowerUp || x is Item))) {
                throw new Exception("Bad item in brickblock (not powerup or coin)");
            }
        }

        public override void Update(GameTime gameTime) {
            State.Update(this, gameTime);
            base.Update(gameTime);
        }

        protected override void UpdateCollision(ColliderGrid colliderGrid) {
            if (State is BrokenBrickBlock) {
                CollisionBox newBox = new CollisionBox();
                colliderGrid.Update(this, CollisionBox, newBox);
                CollisionBox.Min = newBox.Min;
                CollisionBox.Max = newBox.Max;
            } else {
                base.UpdateCollision(colliderGrid);
            }
        }

        public override void BumpAction() {
            State.Bump(this);
            Game1.SoundMap["BrickBump"].Play();
        }
        public override void BreakAction() {
            State.Break(this);
            Game1.SoundMap["BrickBump"].Play();
        }

        public override void CollideBottom(CollidableObject obj) {
            if (obj is Avatar) {
                var powerState = (obj as Avatar).PowerState;
                if (powerState is SuperState || powerState is FireState) {
                    BreakAction();
                } else {
                    BumpAction();
                }
            }
        }

        // spawns the first item from Items above its position
        public void SpawnItem() {
            IGameObject nextItem = Items[0];
            Items.RemoveAt(0);

            nextItem.Position = new Vector2(Position.X, Position.Y - 18);
            if (nextItem is PowerUp) {
                if ((nextItem as PowerUp).State is SuperMushroom) {
                    if (Level.Avatar.Position.X >= Position.X) {
                        nextItem.Velocity = new Vector2(.5f, 0);
                    } else {
                        nextItem.Velocity = new Vector2(-.5f, 0);
                    }
                }
                if ((nextItem as PowerUp).State is LifeMushroom) {
                    if (Level.Avatar.Position.X >= Position.X) {
                        nextItem.Velocity = new Vector2(-.5f, 0);
                    } else {
                        nextItem.Velocity = new Vector2(.5f, 0);
                    }
                }
                if ((nextItem as PowerUp).State is Starmans) {
                    if (Level.Avatar.Position.X >= Position.X) {
                        nextItem.Velocity = new Vector2(-.5f, 0);
                    } else {
                        nextItem.Velocity = new Vector2(.5f, 0);
                    }
                }
            }
            Level.ToSpawn.Add(nextItem);
        }
    }
}
