using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QuaranteamProject.GameObject;

namespace QuaranteamProject {
    public class Layer {
        private readonly Camera _camera;

        public bool Enabled { get; set; }

        public Layer(Camera camera) {
            _camera = camera;
            Parallax = Vector2.One;
            GameObjects = new List<IGameObject>();
            TextObjects = new List<ITextObject>();
            HUDObjects = new List<IHUDObject>();
            Enabled = false;
        }

        public Vector2 Parallax { get; set; }
        public List<IGameObject> GameObjects { get; set; }
        public List<ITextObject> TextObjects { get; set; }
        public List<IHUDObject> HUDObjects { get; set; }
        public SamplerState SamplerState { get; set; } = null;

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState, null, null, null, _camera.GetViewMatrix(Parallax));
            if (Enabled) {
                foreach (IGameObject gameObject in GameObjects) {
                    gameObject.Draw(spriteBatch);
                }

                foreach (ITextObject textObj in TextObjects) {
                    textObj.Draw(spriteBatch);
                }

                foreach (IHUDObject hudObj in HUDObjects) {
                    hudObj.Draw(spriteBatch);
                }
            }

        }

    }
}