using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuaranteamProject {
    public class QuestionBlock : Block {
        public IQuestionBlockState State { get; set; }
        public List<IGameObject> Items { get; set; } // includes powerups and coins, but is not enforced
        public float minY, maxY;

        public QuestionBlock(TileMap level, Vector2 initPos, IQuestionBlockState initState, List<IGameObject> hiddenItems) : base(level, initPos) {
            State = initState;
            Sprite = new QuestionBlockSprite(this);
            minY = Position.Y - 10;
            maxY = Position.Y;
            Items = hiddenItems;
            if (Items.Any(x => !(x is PowerUp || x is Item))) {
                throw new Exception("Bad item in questionblock (not powerup or coin)");
            }
        }

        public override void Update(GameTime gameTime) {
            State.Update(this, gameTime);
            base.Update(gameTime);
        }

        public override void BumpAction() {
            State.Bump(this);
            Game1.SoundMap["BrickBump"].Play();
        }

        public override void CollideBottom(CollidableObject obj) {
            BumpAction();
        }

        // spawns the first item from Items above its position
        public void SpawnItem() {
            IGameObject nextItem = Items[0];
            Items.RemoveAt(0);

            nextItem.Position = new Vector2(Position.X, Position.Y - 18);
            if (nextItem is PowerUp) {
                Game1.SoundMap["PowerUpAppears"].Play();
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
