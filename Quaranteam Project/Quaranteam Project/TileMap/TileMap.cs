using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace QuaranteamProject {
    public class TileMap {
        public Game1 Game { get; set; }
        public readonly Vector2 Gravity = new Vector2(0, 0.058f);
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public Avatar Avatar { get; set; }
        public Vector2 CheckpointPosition { get; set; }
        public Flagpole Flagpole { get; set; }
        public Peach Peach { get; set; }
        public Gate Gate { get; set; }
        public List<IGameObject> GameObjects { get; set; }
        public ColliderGrid ColliderGrid { get; set; }
        public List<IGameObject> ToSpawn { get; set; }
        public bool IsOutside { get; set; }

        public TileMap(string filename, Game1 game) {
            Game = game;

            string json = File.ReadAllText(filename);
            RawTileMap raw = JsonConvert.DeserializeObject<RawTileMap>(json);

            MapWidth = raw.MapWidth * 16;
            MapHeight = raw.MapHeight * 16;

            IsOutside = raw.IsOutside;

            ColliderGrid = new ColliderGrid(MapWidth, MapHeight, Utils.GCD(MapWidth, MapHeight));

            ToSpawn = new List<IGameObject>();

            GameObjects = new List<IGameObject>();

            GameObjects.Add(Avatar = new Avatar(this, new Vector2(raw.Avatar.X * 16, raw.Avatar.Y * 16)));
            CheckpointPosition = new Vector2(raw.Avatar.X * 16, raw.Avatar.Y * 16);

            if (raw.Enemies != null) {
                if (raw.Enemies.Goombas != null) {
                    foreach (var enemy in raw.Enemies.Goombas) {
                        GameObjects.Add(new Goomba(this, new Vector2(enemy.X * 16, enemy.Y * 16)));
                    }
                }

                if (raw.Enemies.Piranahs != null) {
                    foreach (var enemy in raw.Enemies.Piranahs) {
                        GameObjects.Add(new Piranah(this, new Vector2(enemy.X * 16, enemy.Y * 16)));
                    }
                }

                if (raw.Enemies.Koopas != null) {
                    foreach (var enemy in raw.Enemies.Koopas) {
                        if (enemy.Color.ToLower() == "red") {
                            GameObjects.Add(new Koopa(this, new Vector2(enemy.X * 16, enemy.Y * 16), new RedKoopa()));
                        } else if (enemy.Color.ToLower() == "green") {
                            GameObjects.Add(new Koopa(this, new Vector2(enemy.X * 16, enemy.Y * 16), new GreenKoopa()));
                        }
                    }
                }

                if (raw.Enemies.MiniCovids != null) {
                    foreach (var enemy in raw.Enemies.MiniCovids) {
                        GameObjects.Add(new MiniCovid(this, new Vector2(enemy.X * 16, enemy.Y * 16)));
                    }
                }

                if (raw.Enemies.CovidBosses != null) {
                    foreach (var enemy in raw.Enemies.CovidBosses) {
                        GameObjects.Add(new CovidBoss(this, new Vector2(enemy.X * 16, enemy.Y * 16)));
                    }
                }
            }

            if (raw.Obstacles != null) {

                if (raw.Obstacles.QuestionBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.QuestionBlocks) {

                        List<IGameObject> hiddenItems = new List<IGameObject>();
                        if (obstacle.HiddenItems != null) {
                            for (int i = 0; i < obstacle.HiddenItems.Coins; i++) {
                                hiddenItems.Add(new Item(this, new Vector2(), new Coin()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.FireFlowers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Flower()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.SuperMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new SuperMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Starmans; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Starmans()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.LifeMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new LifeMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Sanitizers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Sanitizer()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Masks; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Mask()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Vaccines; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Vaccine()));
                            }
                        }

                        GameObjects.Add(new QuestionBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new IdleQuestionBlock(), hiddenItems));
                    }
                }

                if (raw.Obstacles.UsedBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.UsedBlocks) {
                        GameObjects.Add(new QuestionBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new UsedQuestionBlock(), new List<IGameObject>()));
                    }
                }

                if (raw.Obstacles.BrickBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.BrickBlocks) {
                        List<IGameObject> hiddenItems = new List<IGameObject>();
                        if (obstacle.HiddenItems != null) {
                            for (int i = 0; i < obstacle.HiddenItems.Coins; i++) {
                                hiddenItems.Add(new Item(this, new Vector2(), new Coin()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.FireFlowers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Flower()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.SuperMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new SuperMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Starmans; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Starmans()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.LifeMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new LifeMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Sanitizers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Sanitizer()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Masks; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Mask()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Vaccines; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Vaccine()));
                            }
                        }

                        bool isBlue = obstacle.Color != null && obstacle.Color.ToLower() == "blue";
                        GameObjects.Add(new BrickBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new IdleBrickBlock(), hiddenItems, isBlue));
                    }
                }

                if (raw.Obstacles.FloorBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.FloorBlocks) {
                        bool isBlue = obstacle.Color != null && obstacle.Color.ToLower() == "blue";
                        GameObjects.Add(new FloorBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), isBlue));
                    }
                }

                if (raw.Obstacles.Clouds != null) {
                    foreach (var obstacle in raw.Obstacles.Clouds) {
                        if (obstacle.Type.Equals("Single")) {
                            GameObjects.Add(new SingleCloud(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new Vector2(obstacle.XSpeed, obstacle.YSpeed), new Vector2(0, 0)));
                        } else if (obstacle.Type.Equals("Double")) {
                            GameObjects.Add(new DoubleCloud(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new Vector2(obstacle.XSpeed, obstacle.YSpeed), new Vector2(0, 0)));
                        } else if (obstacle.Type.Equals("Triple")) {
                            GameObjects.Add(new TripleCloud(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new Vector2(obstacle.XSpeed, obstacle.YSpeed), new Vector2(0, 0)));
                        }
                    }
                }

                if (raw.Obstacles.StairBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.StairBlocks) {
                        GameObjects.Add(new StairBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16)) { IsDoor = obstacle.IsDoor });
                    }
                }

                if (raw.Obstacles.HiddenBlocks != null) {
                    foreach (var obstacle in raw.Obstacles.HiddenBlocks) {
                        List<IGameObject> hiddenItems = new List<IGameObject>();
                        if (obstacle.HiddenItems != null) {
                            for (int i = 0; i < obstacle.HiddenItems.Coins; i++) {
                                hiddenItems.Add(new Item(this, new Vector2(), new Coin()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.FireFlowers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Flower()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.SuperMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new SuperMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Starmans; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Starmans()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.LifeMushrooms; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new LifeMushroom()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Sanitizers; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Sanitizer()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Masks; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Mask()));
                            }

                            for (int i = 0; i < obstacle.HiddenItems.Vaccines; i++) {
                                hiddenItems.Add(new PowerUp(this, new Vector2(), new Vaccine()));
                            }
                        }

                        bool isBlue = obstacle.Color != null && obstacle.Color.ToLower() == "blue";
                        GameObjects.Add(new BrickBlock(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new HiddenBrickBlock(), hiddenItems, isBlue));
                    }
                }

                if (raw.Obstacles.Castle != null) {
                    foreach (var obstacle in raw.Obstacles.Castle) {
                        GameObjects.Add(new Castle(this, new Vector2(obstacle.X * 16, obstacle.Y * 16)));
                    }
                }

                if (raw.Obstacles.Flagpole != null) {
                    foreach (var obstacle in raw.Obstacles.Flagpole) {
                        GameObjects.Add(Flagpole = new Flagpole(this, new Vector2(obstacle.X * 16, obstacle.Y * 16)));
                        GameObjects.Add(new Flag(this, Flagpole));
                    }
                }

                if (raw.Obstacles.Peach != null) {
                    GameObjects.Add(Peach = new Peach(this, new Vector2(raw.Obstacles.Peach.X * 16, raw.Obstacles.Peach.Y * 16)));
                }

                if (raw.Obstacles.Gate != null) {
                    GameObjects.Add(Gate = new Gate(this, new Vector2(raw.Obstacles.Gate.X * 16, raw.Obstacles.Gate.Y * 16)));
                }

                if (raw.Obstacles.WarpPipes != null) {
                    foreach (var obstacle in raw.Obstacles.WarpPipes) {
                        if (obstacle.Part.ToLower() == "end") {
                            GameObjects.Add(new WarpPipe(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new End(), obstacle.WarpTo));
                        } else if (obstacle.Part.ToLower() == "middle") {
                            GameObjects.Add(new WarpPipe(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new Middle(), null));
                        } else if (obstacle.Part.ToLower() == "side") {
                            GameObjects.Add(new WarpPipe(this, new Vector2(obstacle.X * 16, obstacle.Y * 16), new Side(), obstacle.WarpTo));
                        }
                    }
                }
            }

            if (raw.PowerUps != null) {
                if (raw.PowerUps.SuperMushrooms != null) {
                    foreach (var powerUp in raw.PowerUps.SuperMushrooms) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new SuperMushroom()));
                    }
                }

                if (raw.PowerUps.LifeMushrooms != null) {
                    foreach (var powerUp in raw.PowerUps.LifeMushrooms) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new LifeMushroom()));
                    }
                }

                if (raw.PowerUps.FireFlowers != null) {
                    foreach (var powerUp in raw.PowerUps.FireFlowers) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new Flower()));
                    }
                }

                if (raw.PowerUps.Starmans != null) {
                    foreach (var powerUp in raw.PowerUps.Starmans) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new Starmans()));
                    }
                }

                if (raw.PowerUps.Sanitizers != null) {
                    foreach (var powerUp in raw.PowerUps.Sanitizers) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new Sanitizer()));
                    }
                }

                if (raw.PowerUps.Masks != null) {
                    foreach (var powerUp in raw.PowerUps.Masks) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new Mask()));
                    }
                }

                if (raw.PowerUps.Vaccines != null) {
                    foreach (var powerUp in raw.PowerUps.Vaccines) {
                        GameObjects.Add(new PowerUp(this, new Vector2(powerUp.X * 16, powerUp.Y * 16), new Vaccine()));
                    }
                }
            }

            if (raw.Items != null && raw.Items.Coins != null) {
                foreach (var item in raw.Items.Coins) {
                    GameObjects.Add(new Item(this, new Vector2(item.X * 16, item.Y * 16), new Coin()));
                }
            }
        }

        // returns a deep copy of the Gravity vector
        public Vector2 GetGravity() {
            return new Vector2(Gravity.X, Gravity.Y);
        }

        public void OpenPeach() {
            foreach (var obj in GameObjects) {
                if (obj is StairBlock && (obj as StairBlock).IsDoor) {
                    (obj as StairBlock).CanCollide = false;
                    obj.Velocity = new Vector2(0, 1);
                } else if (obj is MiniCovid) {
                    (obj as MiniCovid).Die();
                }
            }
        }
    }
}
