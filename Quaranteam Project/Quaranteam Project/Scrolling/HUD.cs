using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace QuaranteamProject {
    public class HUD {
        public Game1 game;
        public float currentTime = 0f;
        public float countDuration = 1f;
        public Vector2 TextPos;
        public Avatar avatar;
        public bool coinBoo, timeBoo, pitBoo, resetBoo;
        private bool warning = false;
        public Texture2D healthBar;
        public Texture2D emptyHealthBar;
        public Texture2D healthTexture;

        public HUD(Game1 game, Avatar avi) {
            this.game = game;
            avatar = avi;
            timeBoo = true;
            pitBoo = true;
            resetBoo = true;
            healthBar = game.Content.Load<Texture2D>("Sprint 5 Sprites/health");
            emptyHealthBar = game.Content.Load<Texture2D>("Sprint 5 Sprites/emptyhealth");
        }

        public void Update(GameTime gameTime) {
            TextPos = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);

            if (game.Paused == false) {
                currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentTime >= countDuration) {

                    if (avatar.Toxicity >= 0 && !avatar.Sprite.IsMasked) {
                        avatar.Toxicity++;
                        currentTime -= countDuration;
                    }
                }
            }

            if (timeBoo) {
                if (Game1.HUDMap["Time"] == 0) {
                    avatar.Die();
                    timeBoo = false;
                }
            }
            if (resetBoo) {
                //if (avatar.Position.Y >= 209) {
                //    if (Game1.HUDMap["Lives"] > 1) {
                //        avatar.Die();
                //        resetBoo = false;
                //   } else {
                //       avatar.Die();
                //        game.GameOver();
                //     }
                //}
            }

            if (Game1.HUDMap["Time"] <= 3 && warning == false) {
                MediaPlayer.Stop();
                Game1.SoundMap["TimeWarning"].Play();

                warning = true;
            }


        }

        public void Draw(SpriteBatch spriteBatch) {
            // spriteBatch.DrawString(game.TextFont, "x " + Game1.HUDMap["Coins"], TextPos, Color.White, 0, new Vector2(-100, 130), 0.9f, SpriteEffects.None, 0.5f);
            // spriteBatch.DrawString(game.TextFont, Game1.HUDMap["Score"].ToString("000000"), TextPos, Color.White, 0, new Vector2(222, 130), 0.9f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(game.TextFont, "x " + Game1.HUDMap["Lives"], TextPos, Color.Black, 0, new Vector2(100, 130), 0.9f, SpriteEffects.None, 0.5f);
            //spriteBatch.DrawString(game.TextFont, "" + Game1.HUDMap["Time"], TextPos, Color.White, 0, new Vector2(-185, 110), 0.9f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(game.TextFont, "x " + avatar.ballCounter, TextPos, Color.Black, 0, new Vector2(100, 110), 0.9f, SpriteEffects.None, 0.5f); ;

            int width = healthBar.Width / 1;
            int height = healthBar.Height / 6;
            int row = 0;
            int column = 1 % 1;



            if (avatar.Toxicity < 20) {
                healthTexture = emptyHealthBar;
                height = healthBar.Height / 1;
                row = 0;
            } else if (avatar.Toxicity >= 20 && avatar.Toxicity < 40) {
                healthTexture = healthBar;
                height = healthBar.Height / 6;
                row = 0;
            } else if (avatar.Toxicity >= 40 && avatar.Toxicity < 60) {
                healthTexture = healthBar;
                row = 1;
            } else if (avatar.Toxicity >= 60 && avatar.Toxicity < 80) {
                healthTexture = healthBar;
                row = 2;
            } else if (avatar.Toxicity >= 80 && avatar.Toxicity < 100) {
                healthTexture = healthBar;
                row = 3;
                if (avatar.Toxicity == 91) {
                    Game1.SoundMap["Cough"].Play();
                }
            } else if (avatar.Toxicity >= 100 && avatar.Toxicity < 120) {
                healthTexture = healthBar;
                row = 4;

            } else if (avatar.Toxicity >= 120) {
                healthTexture = healthBar;
                row = 5;
            }
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(325, 20, width, height);
            spriteBatch.Draw(healthTexture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}


