using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Item : CollidableObject {
        protected override Vector2 CollisionMin => Position;
        protected override Vector2 CollisionSize => new Vector2(16, 16);
        public IItem State { get; set; }

        public Item(TileMap level, Vector2 initPos, IItem state) : base(level, initPos, Vector2.Zero, Vector2.Zero) {
            State = state;
            TextureType = State.GetTextureType();
            CollisionBox.ColorType = State.GetColorType();

            if (State is Coin) {
                Sprite = new ItemSprite(1, 4, 4, 0, true, this);
            }

            CanClip = false;
        }

        public override void CollideBottom(CollidableObject obj) {
            _velocity.Y = 0;
            if (obj is Avatar) {
                ToBeRemoved = true;
                Game1.SoundMap["CoinCollect"].Play();
                Game1.HUDMap["Coins"]++;
                Game1.HUDMap["Score"] += 200;
                if (Game1.HUDMap["Coins"] % 10 == 0) {
                    Game1.HUDMap["Lives"]++;
                }
            }

        }
        public override void CollideLeft(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                Game1.SoundMap["CoinCollect"].Play();
                Game1.HUDMap["Coins"]++;
                Game1.HUDMap["Score"] += 200;
                if (Game1.HUDMap["Coins"] % 10 == 0) {
                    Game1.HUDMap["Lives"]++;
                }
            }


        }
        public override void CollideRight(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                Game1.SoundMap["CoinCollect"].Play();
                Game1.HUDMap["Coins"]++;
                Game1.HUDMap["Score"] += 200;
                if (Game1.HUDMap["Coins"] % 10 == 0) {
                    Game1.HUDMap["Lives"]++;
                }
            }
        }
        public override void CollideTop(CollidableObject obj) {
            if (obj is Avatar) {
                ToBeRemoved = true;
                Game1.SoundMap["CoinCollect"].Play();
                Game1.HUDMap["Coins"]++;
                Game1.HUDMap["Score"] += 200;
                if (Game1.HUDMap["Coins"] % 10 == 0) {
                    Game1.HUDMap["Lives"]++;
                }
            }
        }
    }

    public interface IItem {
        Type GetTextureType();
        Type GetColorType();
    }

    public class Coin : IItem {
        public Type GetTextureType() {
            return typeof(Coin);
        }
        public Type GetColorType() {
            return typeof(Coin);
        }
    }
}
