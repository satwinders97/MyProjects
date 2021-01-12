using Microsoft.Xna.Framework;
using System;

namespace QuaranteamProject {
    public class Collision {
        private CollidableObject obj1, obj2;

        public Collision(CollidableObject a, CollidableObject b) {
            obj1 = a;
            obj2 = b;
        }

        public void UpdatePostCollision() {
            if (HiddenBlockSpecialCase(obj1, obj2)) {
                return; // ignore hidden block special cases
            }

            Vector2 initObj1Pos = new Vector2(obj1.Position.X, obj1.Position.Y);
            Vector2 initObj2Pos = new Vector2(obj2.Position.X, obj2.Position.Y);

            // determine whether to fix obj1 or obj2 first (slower gets reverted first)
            if (obj1.Velocity.Length() < obj2.Velocity.Length()) {
                if (!TryFixObj(obj1, obj2)) {
                    if (!TryFixObj(obj2, obj1)) {
                        FixBothObjs();
                    }
                }
            } else {
                if (!TryFixObj(obj2, obj1)) {
                    if (!TryFixObj(obj1, obj2)) {
                        FixBothObjs();
                    }
                }
            }

            CollisionBox box1 = obj1.CollisionBox;
            CollisionBox box2 = obj2.CollisionBox;

            if (box1.LeftOf(box2)) {
                obj1.CollideRight(obj2);
                obj2.CollideLeft(obj1);
            } else if (box1.RightOf(box2)) {
                obj1.CollideLeft(obj2);
                obj2.CollideRight(obj1);
            } else if (box1.TopOf(box2)) {
                obj1.CollideBottom(obj2);
                obj2.CollideTop(obj1);
            } else if (box1.BottomOf(box2)) {
                obj1.CollideTop(obj2);
                obj2.CollideBottom(obj1);
            }

            if (!obj1.CanClip || !obj2.CanClip) {
                obj1.Position = initObj1Pos;
                obj2.Position = initObj2Pos;
            }
        }

        // returns true if only obj1's position needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryFixObj(CollidableObject obj1, CollidableObject obj2) {
            // determine whether to fix X or Y first
            if (Math.Abs(obj1.Velocity.X) < Math.Abs(obj1.Velocity.Y)) {
                if (!TryUpdateX(obj1, obj2)) {
                    if (!TryUpdateY(obj1, obj2)) {
                        return TryUpdateXAndY(obj1, obj2);
                    }
                }
            } else {
                if (!TryUpdateY(obj1, obj2)) {
                    if (!TryUpdateX(obj1, obj2)) {
                        return TryUpdateXAndY(obj1, obj2);
                    }
                }
            }

            return false;
        }

        private void FixBothObjs() {
            // determine whether to fix X or Y first
            if (Math.Abs(obj1.Velocity.X - obj2.Velocity.X) < Math.Abs(obj1.Velocity.Y - obj2.Velocity.Y)) {
                if (!TryUpdateBothX(obj1, obj2)) {
                    if (!TryUpdateBothY(obj1, obj2)) {
                        UpdateBothXAndY(obj1, obj2);
                    }
                }
            } else {
                if (!TryUpdateBothY(obj1, obj2)) {
                    if (!TryUpdateBothX(obj1, obj2)) {
                        UpdateBothXAndY(obj1, obj2);
                    }
                }
            }
        }

        // returns true if only obj1's x needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryUpdateX(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position = new Vector2(obj1.Position.X - obj1.Velocity.X, obj1.Position.Y);

            if (!obj1.CollisionBox.Intersects(obj2.CollisionBox)) {
                return true;
            } else {
                obj1.Position = new Vector2(obj1.Position.X + obj1.Velocity.X, obj1.Position.Y);
                return false;
            }
        }

        // returns true if only obj1's y needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryUpdateY(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position = new Vector2(obj1.Position.X, obj1.Position.Y - obj1.Velocity.Y);

            if (!obj1.CollisionBox.Intersects(obj2.CollisionBox)) {
                return true;
            } else {
                obj1.Position = new Vector2(obj1.Position.X, obj1.Position.Y + obj1.Velocity.Y);
                return false;
            }
        }

        // returns true if only obj1's position needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryUpdateXAndY(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position -= obj1.Velocity;

            if (!obj1.CollisionBox.Intersects(obj2.CollisionBox)) {
                return true;
            } else {
                obj1.Position += obj1.Velocity;
                return false;
            }
        }

        // returns true if both obj's x's needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryUpdateBothX(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position = new Vector2(obj1.Position.X - obj1.Velocity.X, obj1.Position.Y);
            obj2.Position = new Vector2(obj2.Position.X - obj2.Velocity.X, obj2.Position.Y);

            if (!obj1.CollisionBox.Intersects(obj2.CollisionBox)) {
                return true;
            } else {
                obj1.Position = new Vector2(obj1.Position.X + obj1.Velocity.X, obj1.Position.Y);
                obj2.Position = new Vector2(obj2.Position.X + obj2.Velocity.X, obj2.Position.Y);
                return false;
            }
        }

        // returns true if both obj's y's needs to change to revert intersection
        // if fails, then reverts its changes
        private static bool TryUpdateBothY(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position = new Vector2(obj1.Position.X, obj1.Position.Y - obj1.Velocity.Y);
            obj2.Position = new Vector2(obj2.Position.X, obj2.Position.Y - obj2.Velocity.Y);

            if (!obj1.CollisionBox.Intersects(obj2.CollisionBox)) {
                return true;
            } else {
                obj1.Position = new Vector2(obj1.Position.X, obj1.Position.Y + obj1.Velocity.Y);
                obj2.Position = new Vector2(obj2.Position.X, obj2.Position.Y + obj2.Velocity.Y);
                return false;
            }
        }

        // returns true if both obj's position needs to change to revert intersection
        // if fails, then reverts its changes
        private static void UpdateBothXAndY(CollidableObject obj1, CollidableObject obj2) {
            obj1.Position -= obj1.Velocity;
            obj2.Position -= obj2.Velocity;
        }

        // returns true if the collision is with a hiddenblock on its left, right, or top side
        public static bool HiddenBlockSpecialCase(CollidableObject objA, CollidableObject objB) {
            CollisionBox tempABox = new CollisionBox(objA.CollisionBox.Min - objA.Velocity, objA.CollisionBox.Max - objA.Velocity);
            CollisionBox tempBBox = new CollisionBox(objB.CollisionBox.Min - objB.Velocity, objB.CollisionBox.Max - objB.Velocity);
            if (objA is QuestionBlock && (objA as QuestionBlock).State is HiddenQuestionBlock) {
                return !tempABox.TopOf(tempBBox);
            } else if (objA is BrickBlock && (objA as BrickBlock).State is HiddenBrickBlock) {
                return !tempABox.TopOf(tempBBox);
            } else if (objB is QuestionBlock && (objB as QuestionBlock).State is HiddenQuestionBlock) {
                return !tempABox.BottomOf(tempBBox);
            } else if (objB is BrickBlock && (objB as BrickBlock).State is HiddenBrickBlock) {
                return !tempABox.BottomOf(tempBBox);
            } else {
                return false;
            }
        }
    }
}