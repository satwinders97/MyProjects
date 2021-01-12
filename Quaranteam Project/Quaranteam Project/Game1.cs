using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuaranteamProject {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game {
        public const int ScreenWidth = 400;
        public const int ScreenHeight = 240;
        public static Dictionary<Type, Color> ColorMap { get; set; }
        public static Dictionary<Type, Texture2D> TextureMap { get; set; }
        public static Texture2D BlankTexture { get; set; }
        public static Dictionary<string, SoundEffect> SoundMap { get; set; }
        public static Dictionary<string, int> HUDMap { get; set; }
        public SpriteFont TextFont;

        protected Dictionary<string, TileMap> levelMap = new Dictionary<string, TileMap>();
        private Dictionary<string, string> levelFiles = new Dictionary<string, string>(); // key is levelName, value is fileName

        protected SpriteBatch spriteBatch;

        public string CurrentLevelName { get; set; }

        public TileMap Level { get; private set; }

        public HUD GameHUD { get; set; }

        public LevelScroller LevelScroller { get; set; }

        protected ColliderGrid ColliderGrid => Level.ColliderGrid;
        protected List<IGameObject> GameObjects => Level.GameObjects;
        protected Avatar Mario => Level.Avatar;

        protected Song backGroundMusic;
        public const float SoundLevel = 0.4f;

        protected List<IController> controllers;
        public bool Paused { get; set; }
        public bool IsGameOver { get; set; }

        protected GraphicsDeviceManager graphics;

        public Game1() {
            graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = ScreenWidth,
                PreferredBackBufferHeight = ScreenHeight
            };

            Content.RootDirectory = "Content";

            SoundEffect.MasterVolume = SoundLevel;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.json", SearchOption.AllDirectories);

            foreach (var file in files) {
                InitializeGameLevel(Path.GetFileNameWithoutExtension(file), file);
            }

            LoadGameLevel("hospital");

            base.Initialize();
        }

        public void InitializeGameLevel(string levelName, string fileName) {
            levelMap.Add(levelName, new TileMap(fileName, this));
            levelFiles.Add(levelName, fileName);
        }

        public void ResetGameLevel(string levelName) {
            levelMap[levelName] = new TileMap(levelFiles[levelName], this);
            if (levelName == "1-1") {
                ResetGameLevel("1-1_secret");
            }

            LoadGameLevel(levelName);
            MediaPlayer.Play(backGroundMusic);
            HUDMap["Score"] = 0;
            HUDMap["Lives"] = 3;
            HUDMap["Coins"] = 0;
            HUDMap["Time"] = 400;
        }

        public void GameOver() {
            IsGameOver = true;
            Level.IsOutside = false;
            LevelScroller.layers[0].Enabled = false;
            LevelScroller.layers[1].Enabled = false;
            LevelScroller.layers[2].Enabled = false;
            LevelScroller.layers[3].Enabled = true;
            LevelScroller.layers[4].Enabled = true;
            LevelScroller.layers[5].Enabled = false;

            EmptyKeyBinds();
            GameOverKeyBinds();
        }

        public void GameWon() {
            IsGameOver = true;
            MediaPlayer.Stop();
            Level.IsOutside = false;
            LevelScroller.layers[0].Enabled = false;
            LevelScroller.layers[1].Enabled = false;
            LevelScroller.layers[2].Enabled = false;
            LevelScroller.layers[3].Enabled = true;
            LevelScroller.layers[4].Enabled = false;
            LevelScroller.layers[5].Enabled = true;

            EmptyKeyBinds();
            GameOverKeyBinds();
        }

        public void LoadGameLevel(string levelName) {
            Level = levelMap[levelName];
            CurrentLevelName = levelName;

            foreach (var obj in GameObjects) {
                obj.Init();
            }

            GameHUD = new HUD(this, Mario);
            LevelScroller = new LevelScroller(this, GameObjects, Level);

            controllers = new List<IController> {
                new KeyboardController(),
                new GamepadController()
            };

            GamePlayKeyBinds();
        }

        public void GamePlayKeyBinds() {
            foreach (Controller controller in controllers) {
                if (controller is KeyboardController) {
                    controller.CreateCommand((int)Keys.Q, new ExitCommand(this));
                    controller.CreateCommand((int)Keys.Up, new UpCommand(Mario));
                    controller.CreateCommand((int)Keys.W, new UpCommand(Mario));
                    controller.CreateCommand((int)Keys.Down, new CrouchCommand(Mario));
                    controller.CreateCommand((int)Keys.S, new CrouchCommand(Mario));
                    controller.CreateCommand((int)Keys.Left, new LeftCommand(Mario));
                    controller.CreateCommand((int)Keys.A, new LeftCommand(Mario));
                    controller.CreateCommand((int)Keys.Right, new RightCommand(Mario));
                    controller.CreateCommand((int)Keys.D, new RightCommand(Mario));
                    controller.CreateCommand((int)Keys.Y, new StandardMarioCommand(Mario));
                    controller.CreateCommand((int)Keys.U, new SuperMarioCommand(Mario));
                    controller.CreateCommand((int)Keys.I, new FireMarioCommand(Mario));
                    controller.CreateCommand((int)Keys.O, new DamageCommand(Mario));
                    controller.CreateCommand((int)Keys.C, new ToggleCollisionBoxCommand(GameObjects));
                    controller.CreateCommand((int)Keys.R, new ResetCommand(this));
                    controller.CreateCommand((int)Keys.Space, new SanitizerballCommand(Mario));
                    controller.CreateCommand((int)Keys.M, new MuteCommand(SoundLevel));
                    controller.CreateCommand((int)Keys.P, new PauseCommand(this));
                } else if (controller is GamepadController) {
                    controller.CreateCommand((int)Buttons.Start, new ExitCommand(this));
                    controller.CreateCommand((int)Buttons.A, new UpCommand(Mario));
                    controller.CreateCommand((int)Buttons.DPadDown, new CrouchCommand(Mario));
                    controller.CreateCommand((int)Buttons.DPadLeft, new LeftCommand(Mario));
                    controller.CreateCommand((int)Buttons.DPadRight, new RightCommand(Mario));
                    controller.CreateCommand((int)Buttons.B, new SanitizerballCommand(Mario));
                }
            }
        }

        public void PauseKeyBinds() {
            foreach (Controller controller in controllers) {
                if (controller is KeyboardController) {
                    controller.CreateCommand((int)Keys.Q, new ExitCommand(this));
                    controller.CreateCommand((int)Keys.P, new UnpauseCommand(this));
                }
            }
        }

        public void GameOverKeyBinds() {
            foreach (Controller controller in controllers) {
                if (controller is KeyboardController) {
                    controller.CreateCommand((int)Keys.Q, new ExitCommand(this));
                    controller.CreateCommand((int)Keys.R, new ResetCommand(this));
                }
            }
        }

        public void EmptyKeyBinds() {
            foreach (Controller controller in controllers) {
                controller.EmptyCommands();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextFont = Content.Load<SpriteFont>("Fonts/arial");

            backGroundMusic = Content.Load<Song>("Sounds/background");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backGroundMusic);

            HUDMap = new Dictionary<string, int>
            {
                { "Score", 0 },
                { "Lives", 3 },
                { "Coins", 0 },
                { "Time", 400 }
            };

            SoundMap = new Dictionary<string, SoundEffect> {
                { "NormalJump", Content.Load<SoundEffect>("Sounds/JumpSound") },
                { "SuperJump", Content.Load<SoundEffect>("Sounds/smb_jump-super") },
                { "Stomp", Content.Load<SoundEffect>("Sounds/smb_stomp") },
                { "Die", Content.Load<SoundEffect>("Sounds/smb_mariodie") },
                { "CoinCollect", Content.Load<SoundEffect>("Sounds/smb_coin") },
                { "PowerUpAppears", Content.Load<SoundEffect>("Sounds/smb_powerup_appears") },
                { "PUpShroom", Content.Load<SoundEffect>("Sounds/smb_powerup") },
                { "LifeShroom", Content.Load<SoundEffect>("Sounds/smb_1-up") },
                { "BrickBump", Content.Load<SoundEffect>("Sounds/smb_bump") },
                { "BrickBreak", Content.Load<SoundEffect>("Sounds/smb_breakblock") },
                { "GameOver", Content.Load<SoundEffect>("Sounds/smb_gameover") },
                { "TimeWarning", Content.Load<SoundEffect>("Sounds/smb_warning") },
                { "FlagPole", Content.Load<SoundEffect>("Sounds/smb_flagpole") },
                { "PipeTravel", Content.Load<SoundEffect>("Sounds/smb_pipe") },
                {"Cough", Content.Load<SoundEffect>("Sounds/cough")},
                {"BatScreech", Content.Load<SoundEffect>("Sounds/bat_screech")}
            };

            // load textures into textureMap
            TextureMap = new Dictionary<Type, Texture2D> {
                { typeof(Avatar), Content.Load<Texture2D>("Sprint 5 Sprites/diseasedmario") },
                { typeof(Block), Content.Load<Texture2D>("Sprint 5 Sprites/blocks") },
                { typeof(Castle), Content.Load<Texture2D>("Spritesheets/start_castle") },
                { typeof(Coin), Content.Load<Texture2D>("Spritesheets/Coin") },
                { typeof(CovidBoss), Content.Load<Texture2D>("Sprint 5 Sprites/covidboss")},
                { typeof(DoubleCloud), Content.Load<Texture2D>("Sprint 5 Sprites/Cloud - Double")},
                { typeof(Fireball), Content.Load<Texture2D>("Spritesheets/fireball") },
                { typeof(Flag), Content.Load<Texture2D>("Spritesheets/flag") },
                { typeof(Flagpole), Content.Load<Texture2D>("Spritesheets/flagpole") },
                { typeof(Flower), Content.Load<Texture2D>("Spritesheets/FlowerAndStar") },
                { typeof(Goomba), Content.Load<Texture2D>("Spritesheets/Goomba") },
                { typeof(Koopa), Content.Load<Texture2D>("Spritesheets/Koopas") },
                { typeof(LifeMushroom), Content.Load<Texture2D>("Spritesheets/Mushrooms") },
                { typeof(Mask), Content.Load<Texture2D>("Sprint 5 Sprites/masksmall") },
                { typeof(MiniCovid), Content.Load<Texture2D>("Sprint 5 Sprites/minicovid")},
                { typeof(Peach), Content.Load<Texture2D>("Sprint 5 Sprites/peachsmall")},
                { typeof(Piranah), Content.Load<Texture2D>("Spritesheets/PiranahPlant") },
                { typeof(Sanitizer), Content.Load<Texture2D>("Sprint 5 Sprites/handsanitizersmall") },
                { typeof(Sanitizerball), Content.Load<Texture2D>("Sprint 5 Sprites/drop") },
                { typeof(SingleCloud), Content.Load<Texture2D>("Sprint 5 Sprites/Cloud - Single")},
                { typeof(Starmans), Content.Load<Texture2D>("Spritesheets/FlowerAndStar") },
                { typeof(SuperMushroom), Content.Load<Texture2D>("Spritesheets/Mushrooms") },
                { typeof(TripleCloud), Content.Load<Texture2D>("Sprint 5 Sprites/Cloud - Triple")},
                { typeof(Vaccine), Content.Load<Texture2D>("Sprint 5 Sprites/syringesmall") },
                { typeof(WarpPipe), Content.Load<Texture2D>("Spritesheets/warppipe") },
                { typeof(Gate), Content.Load<Texture2D>("Sprint 5 Sprites/GoldGate") }
            };
            BlankTexture = new Texture2D(GraphicsDevice, 1, 1);
            BlankTexture.SetData(new Color[1] { Color.White });

            ColorMap = new Dictionary<Type, Color> {
                { typeof(Avatar), Color.Yellow },
                { typeof(Block), Color.Blue },
                { typeof(Castle), Color.Blue },
                { typeof(Coin), Color.Green },
                { typeof(CovidBoss), Color.Red },
                { typeof(Cloud), Color.Blue},
                { typeof(Fireball), Color.Red },
                { typeof(Flagpole), Color.Blue },
                { typeof(Goomba), Color.Red },
                { typeof(Koopa), Color.Red },
                { typeof(MiniCovid), Color.Red },
                { typeof(Peach), Color.Yellow },
                { typeof(Piranah), Color.Red },
                { typeof(PowerUp), Color.Green },
                { typeof(Sanitizerball), Color.Red },
                { typeof(WarpPipe), Color.Blue },
                { typeof(Gate), Color.Blue }
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // update inputs
            foreach (var controller in controllers) {
                controller.UpdateInput();
            }

            if (!IsGameOver) {
                LevelScroller.layers[2].Enabled = true;
                LevelScroller.layers[3].Enabled = true;
                LevelScroller.layers[4].Enabled = false;
                LevelScroller.layers[5].Enabled = false;

                if (Paused) {
                    MediaPlayer.Pause();
                } else {
                    MediaPlayer.Resume();
                }

                GameHUD.Update(gameTime);

                // add to-spawn items to the gameobjects list
                GameObjects.AddRange(Level.ToSpawn);
                Level.ToSpawn.Clear();

                if (Mario.IsDead) {
                    if (Mario.TimeDied.TotalMilliseconds == 0) {
                        Mario.TimeDied = gameTime.TotalGameTime;
                    } else if (gameTime.TotalGameTime > Mario.TimeDied + new TimeSpan(0, 0, 0, 3)) {
                        if (HUDMap["Lives"] <= 0) {
                            GameOver();
                        } else {
                            HUDMap["Lives"]--;
                            LoadGameLevel("mini_game");
                            MediaPlayer.Play(backGroundMusic);
                            Mario.Respawn();
                        }
                    }
                }

                if (Mario.ReachedGate) {
                    Mario.ReachedGate = false;
                    LoadGameLevel("hospital");
                    Mario.Respawn();
                }

                if (Mario.OnFlagpole) {
                    if (Mario.TimeWon.TotalMilliseconds == 0) {
                        Mario.TimeWon = gameTime.TotalGameTime;
                    } else if (gameTime.TotalGameTime > Mario.TimeWon + new TimeSpan(0, 0, 0, 5)) {
                        GameWon();
                    }
                }

                // update all game objects (their sprites are updated inside)
                foreach (var obj in GameObjects) {
                    if (!Paused) {
                        obj.Update(gameTime);
                    }
                }

                UpdateCollisions(); // update collisions

                CleanDeletedObjects(); // remove any deleted items from anywhere they might remain
            }

            base.Update(gameTime);

            LevelScroller.Update();
        }

        private void UpdateCollisions() {
            var movingObjects = new HashSet<CollidableObject>(GameObjects.Where(x => x is CollidableObject && (x as CollidableObject).Velocity.Length() > 0).Cast<CollidableObject>());
            HashSet<Tuple<CollidableObject, CollidableObject>> possibleCollisions = ColliderGrid.GetPossibleCollisionPairs(movingObjects);
            List<Collision> actualCollisions = new List<Collision>();

            foreach (var pair in possibleCollisions) {
                CollidableObject objA = pair.Item1;
                CollidableObject objB = pair.Item2;

                if (objA.CollisionBox.Intersects(objB.CollisionBox)) {
                    actualCollisions.Add(new Collision(objA, objB));
                }
            }

            foreach (var coll in actualCollisions) {
                coll.UpdatePostCollision();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            if (Level.IsOutside) {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            } else {
                GraphicsDevice.Clear(new Color(new Vector3(150 / 255.0f, 175 / 255.0f, 175 / 255.0f)));
            }

            spriteBatch.Begin(blendState: BlendState.AlphaBlend);

            LevelScroller.Draw(spriteBatch);

            GameHUD.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CleanDeletedObjects() {
            List<IGameObject> deletedObjects = GameObjects.Where(x => x.ToBeRemoved).ToList();

            foreach (var obj in deletedObjects) {
                GameObjects.Remove(obj);
                if (obj is CollidableObject) {
                    ColliderGrid.Remove(obj as CollidableObject);
                }
            }
        }
    }
}
