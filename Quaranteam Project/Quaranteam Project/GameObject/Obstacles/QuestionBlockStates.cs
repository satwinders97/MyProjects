using Microsoft.Xna.Framework;

namespace QuaranteamProject {
    public interface IQuestionBlockState {
        void Bump(QuestionBlock context);
        void Update(QuestionBlock context, GameTime gameTime);
    }

    public class IdleQuestionBlock : IQuestionBlockState {
        public void Bump(QuestionBlock context) {
            context.Velocity = new Vector2(context.Velocity.X, -1);
            context.State = new BumpedQuestionBlock();
        }
        public void Update(QuestionBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }

    public class BumpedQuestionBlock : IQuestionBlockState {
        public void Bump(QuestionBlock context) { }
        public void Update(QuestionBlock context, GameTime gameTime) {
            if (context.Position.Y + context.Velocity.Y <= context.minY) {
                context.Velocity = new Vector2(context.Velocity.X, -context.Velocity.Y);
                if (context.Items.Count > 0) {
                    context.SpawnItem(); // spawn item at peak, so collision isn't messy
                }
            } else if (context.Position.Y + context.Velocity.Y >= context.maxY) {
                if (context.Items.Count == 0) {
                    context.State = new UsedQuestionBlock();
                } else {
                    context.State = new IdleQuestionBlock();
                }
            }
        }
    }

    public class UsedQuestionBlock : IQuestionBlockState {
        public void Bump(QuestionBlock context) { }
        public void Update(QuestionBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }

    public class HiddenQuestionBlock : IQuestionBlockState {
        public void Bump(QuestionBlock context) {
            context.Velocity = new Vector2(context.Velocity.X, -1);
            context.State = new BumpedQuestionBlock();
        }
        public void Update(QuestionBlock context, GameTime gameTime) {
            context.Velocity = Vector2.Zero;
        }
    }
}
