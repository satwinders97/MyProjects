using System;
using System.Collections.Generic;

namespace QuaranteamProject {
    public class ColliderGrid {
        private List<CollidableObject>[,] grid;
        private int squareSize;

        public ColliderGrid(int xLen, int yLen, int sSize) {
            squareSize = sSize;
            xLen /= sSize;
            yLen /= sSize;
            grid = new List<CollidableObject>[xLen, yLen];
            for (int i = 0; i < xLen; i++) {
                for (int j = 0; j < yLen; j++) {
                    grid[i, j] = new List<CollidableObject>();
                }
            }
        }

        // removes all instances of obj from the grid
        public void Remove(CollidableObject obj) {
            foreach (var objList in grid) {
                objList.Remove(obj);
            }
        }

        // updates this grid based on the motion of obj
        public void Update(CollidableObject obj, CollisionBox oldBox, CollisionBox newBox) {
            int oldMinX = (int)(oldBox.Min.X / squareSize);
            int oldMaxX = (int)(oldBox.Max.X / squareSize);
            int oldMinY = (int)(oldBox.Min.Y / squareSize);
            int oldMaxY = (int)(oldBox.Max.Y / squareSize);
            int newMinX = (int)(newBox.Min.X / squareSize);
            int newMaxX = (int)(newBox.Max.X / squareSize);
            int newMinY = (int)(newBox.Min.Y / squareSize);
            int newMaxY = (int)(newBox.Max.Y / squareSize);

            for (int x = oldMinX; x <= oldMaxX; x++) {
                for (int y = oldMinY; y <= oldMaxY; y++) {
                    if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1)) {
                        grid[x, y].Remove(obj);
                    }
                }
            }
            for (int x = newMinX; x <= newMaxX; x++) {
                for (int y = newMinY; y <= newMaxY; y++) {
                    if (x >= 0 && y >= 0 && x < grid.GetLength(0) && y < grid.GetLength(1)) {
                        grid[x, y].Add(obj);
                    }
                }
            }
        }

        public HashSet<Tuple<CollidableObject, CollidableObject>> GetPossibleCollisionPairs(HashSet<CollidableObject> movingObjects) {
            HashSet<Tuple<CollidableObject, CollidableObject>> allCollisions = new HashSet<Tuple<CollidableObject, CollidableObject>>();

            foreach (var tile in grid) {
                for (int i = 0; i < tile.Count - 1; i++) {
                    for (int j = i + 1; j < tile.Count; j++) {
                        if (movingObjects.Contains(tile[i]) || movingObjects.Contains(tile[j])) { // only check collisions where there is movement (non-moving objects won't collide)
                            if (tile[i].CanCollide && tile[j].CanCollide) { // only check collisions where the objects are marked as collidable
                                allCollisions.Add(new Tuple<CollidableObject, CollidableObject>(tile[i], tile[j]));
                            }
                        }
                    }
                }
            }

            return allCollisions;
        }
    }
}
