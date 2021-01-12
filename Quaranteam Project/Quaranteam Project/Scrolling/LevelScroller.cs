using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using QuaranteamProject.GameObject;

namespace QuaranteamProject {

    public class BackgroundSprite : ISprite {
        private readonly Texture2D texture;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public bool IsMasked { get; set; }
        public Color Color { get; set; } = Color.White;
        public BackgroundSprite(Texture2D texture) {
            this.texture = texture;
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, Position, new Rectangle(0, 0, 4000, texture.Height), Color);
        }
    }

    public class LevelScroller {
        private readonly Camera camera;
        public List<Layer> layers = new List<Layer>();
        public TileMap level;

        public LevelScroller(Game1 game, List<IGameObject> gameObjects, TileMap tileMap) {
            level = tileMap;
            camera = new Camera(game.GraphicsDevice.Viewport) { Limits = new Rectangle(0, 0, level.MapWidth, level.MapHeight) };
            layers = new List<Layer> {
                new Layer(camera) { Parallax = new Vector2 (.1f, 0.0f), SamplerState = SamplerState.LinearWrap }, // cloud layer
                new Layer(camera) { Parallax = new Vector2 (.5f, 0.0f), SamplerState = SamplerState.LinearWrap }, // bush layer
                new Layer(camera) { Parallax = new Vector2(1.0f, 1.0f), GameObjects = gameObjects }, // Main game objects layer
                new Layer(camera) { Parallax = new Vector2(0f, 0f), SamplerState = SamplerState.LinearClamp }, // HUD layer
                new Layer(camera) { Parallax = new Vector2(0.0f, 0.0f), SamplerState = SamplerState.LinearClamp }, // Game over layer
                new Layer(camera) { Parallax = new Vector2(0.0f, 0.0f), SamplerState = SamplerState.LinearClamp } // Winner layer
            };

            AddBackground(game);
            AddHUD(game);
            AddGameOver(game);
            AddWinner(game);

            //layers[0].Enabled = level.IsOutside;
            //layers[1].Enabled = level.IsOutside;
        }

        private void AddBackground(Game1 game) {
            // adds clouds and bushes to layer
            layers[0].GameObjects.Add(new Background {
                Sprite = new BackgroundSprite(game.Content.Load<Texture2D>("Spritesheets/cloud")),
                Position = new Vector2(0, 37),
            });
            layers[1].GameObjects.Add(new Background {
                Sprite = new BackgroundSprite(game.Content.Load<Texture2D>("Spritesheets/cloud")) { Color = Color.Green },
                Position = new Vector2(0, 185),
            });
        }

        private void AddHUD(Game1 game) {
            //add HUD to layer
            layers[3].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "MARIO", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.Black, 0, new Vector2(222, 130), 0.9f, SpriteEffects.None, 0.5f));
            layers[3].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "WORLD", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.Black, 0, new Vector2(30, 130), 0.9f, SpriteEffects.None, 0.5f));
            layers[3].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "C-19", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.Black, 0, new Vector2(16, 110), 0.9f, SpriteEffects.None, 0.5f));
            layers[3].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "TOXICITY", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.Black, 0, new Vector2(-130, 130), 0.9f, SpriteEffects.None, 0.5f));

            Texture2D mario = game.Content.Load<Texture2D>("Spritesheets/mario");
            layers[3].HUDObjects.Add(new HUDObject(mario, new Vector2(80, -13), new Rectangle(mario.Width / 20 * 2, mario.Height / 3 * 0, mario.Width / 20, mario.Height / 3), Color.White));

            Texture2D drop = game.Content.Load<Texture2D>("Sprint 5 Sprites/drop");
            layers[3].HUDObjects.Add(new HUDObject(drop, new Vector2(77, 13), new Rectangle(drop.Width / 5 * 2 , drop.Height * 2 * 0, drop.Width / 5, drop.Height / 2), Color.White));
        }

        private void AddGameOver(Game1 game) {
            //adds game over to layer
            layers[4].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "GAME OVER", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(53, 20), 1.5f, SpriteEffects.None, 0.5f));
            layers[4].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "You contracted COVID-19!", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(100, 0), 1.0f, SpriteEffects.None, 0.5f));
            layers[4].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "Replay (R)", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(80, -20), 1.0f, SpriteEffects.None, 0.5f));
            layers[4].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "Quit (Q)", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(-30, -20), 1.0f, SpriteEffects.None, 0.5f));
        }

        private void AddWinner(Game1 game) {
            //adds winner to layer
            layers[5].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "WINNER", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(35, 40), 1.5f, SpriteEffects.None, 0.5f));
            layers[5].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "You saved Princess Peach!", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(102, 20), 1.0f, SpriteEffects.None, 0.5f));
            layers[5].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "Replay (R)", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(80, 0), 1.0f, SpriteEffects.None, 0.5f));
            layers[5].TextObjects.Add(new TextObject(game.Content.Load<SpriteFont>("Fonts/arial"), "Quit (Q)", new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2), Color.White, 0, new Vector2(-30, 0), 1.0f, SpriteEffects.None, 0.5f));
        }


        public void Update() {
            camera.LookAt(level.Avatar.Position);
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (Layer layer in layers) {
                layer.Draw(spriteBatch);
            }
        }
    }
}
