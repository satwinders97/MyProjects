using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace QuaranteamProject {
    public class CollisionBox {
        public Vector2 Min {
            get => _min;
            set {
                if (value.X >= Max.X) {
                    _max.X = value.X + 1;
                }

                if (value.Y >= Max.Y) {
                    _max.Y = value.Y + 1;
                }

                _min = value;
            }
        }
        private Vector2 _min;
        public Vector2 Max {
            get => _max;
            set {
                if (value.X <= Min.X) {
                    _min.X = value.X + 1;
                }

                if (value.Y <= Min.Y) {
                    _min.Y = value.Y + 1;
                }

                _max = value;
            }
        }
        private Vector2 _max;

        public Type ColorType { get; set; }
        public bool IsVisible { get; set; }

        public CollisionBox() {
            Init(new Vector2(0, 0), new Vector2(1, 1), false);
        }
        public CollisionBox(Vector2 min, Vector2 max) {
            Init(min, max, false);
        }

        private void Init(Vector2 min, Vector2 max, bool isVis) {
            Min = min;
            Max = max;
            IsVisible = isVis;
        }

        // returns true if this intersects b anywhere
        public bool Intersects(CollisionBox b) {
            return Max.X > b.Min.X && Min.X < b.Max.X && Max.Y > b.Min.Y && Min.Y < b.Max.Y;
        }

        // returns true if this contacts b on the left side of b
        public bool LeftOf(CollisionBox b) {
            return Max.X <= b.Min.X;
        }

        // returns true if this contacts b on the right side of b
        public bool RightOf(CollisionBox b) {
            return Min.X >= b.Max.X;
        }

        // returns true if this contacts b on the top side of b
        public bool TopOf(CollisionBox b) {
            return Max.Y <= b.Min.Y;
        }

        // returns true if this contacts b on the bottom side of b
        public bool BottomOf(CollisionBox b) {
            return Min.Y >= b.Max.Y;
        }

        public void ToggleCollisionBox() {
            IsVisible = !IsVisible;
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (IsVisible) {
                Color c = Game1.ColorMap[ColorType];

                spriteBatch.Draw(Game1.BlankTexture, new Rectangle((int)Min.X, (int)Min.Y, (int)(Max.X - Min.X + 1), 1), c);
                spriteBatch.Draw(Game1.BlankTexture, new Rectangle((int)Min.X, (int)Min.Y, 1, (int)(Max.Y - Min.Y + 1)), c);
                spriteBatch.Draw(Game1.BlankTexture, new Rectangle((int)Max.X, (int)Min.Y, 1, (int)(Max.Y - Min.Y + 1)), c);
                spriteBatch.Draw(Game1.BlankTexture, new Rectangle((int)Min.X, (int)Max.Y, (int)(Max.X - Min.X + 1), 1), c);
            }
        }
    }
}
