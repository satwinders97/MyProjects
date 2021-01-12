using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace QuaranteamProject {
    public interface IController {
        void CreateCommand(int key, ICommand value);
        void UpdateInput();
    }

    public abstract class Controller : IController {
        protected Dictionary<int, ICommand> commands;

        public Controller() {
            commands = new Dictionary<int, ICommand>();
        }
        public void CreateCommand(int key, ICommand value) {
            commands.Add(key, value);
        }

        public void EmptyCommands() {
            commands = new Dictionary<int, ICommand>();
        }

        public abstract void UpdateInput();
    }

    public class KeyboardController : Controller {
        private KeyboardState previousKeyboardState;
        public KeyboardController() : base() {
            previousKeyboardState = Keyboard.GetState();
        }

        public override void UpdateInput() {
            // Get the current Keyboard state.
            KeyboardState currentState = Keyboard.GetState();

            Keys[] keysPressed = currentState.GetPressedKeys();
            foreach (Keys key in keysPressed) {
                if (!previousKeyboardState.IsKeyDown(key)) {
                    if (commands.ContainsKey((int)key)) {
                        commands[(int)key].Execute();
                    }
                }
            }

            // Update previous Keyboard state.
            previousKeyboardState = currentState;
        }
    }

    public class GamepadController : Controller {
        private GamePadState[] previousGamePadState = new GamePadState[4];
        private GamePadState emptyInput;

        public GamepadController() : base() {
            previousGamePadState[0] = GamePad.GetState(PlayerIndex.One);
            previousGamePadState[1] = GamePad.GetState(PlayerIndex.Two);
            previousGamePadState[2] = GamePad.GetState(PlayerIndex.Three);
            previousGamePadState[3] = GamePad.GetState(PlayerIndex.Four);
            emptyInput = new GamePadState(Vector2.Zero, Vector2.Zero, 0, 0, new Buttons());
        }

        public override void UpdateInput() {
            // Get the current gamepad state.
            GamePadState[] currentState = new GamePadState[4];
            currentState[0] = GamePad.GetState(PlayerIndex.One);
            currentState[1] = GamePad.GetState(PlayerIndex.Two);
            currentState[2] = GamePad.GetState(PlayerIndex.Three);
            currentState[3] = GamePad.GetState(PlayerIndex.Four);

            for (int i = 0; i < 3; i++) {
                // Process input only if connected.
                if (currentState[i].IsConnected) {
                    if (currentState[i] != emptyInput) // Button Pressed
                    {
                        var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));

                        foreach (var button in buttonList) {
                            if (currentState[i].IsButtonDown(button) &&
                                !previousGamePadState[i].IsButtonDown(button)) {
                                if (commands.ContainsKey((int)button)) {
                                    commands[(int)button].Execute();
                                }
                            }
                        }
                    }

                    // Update previous gamepad state.
                    previousGamePadState[i] = currentState[i];
                }
            }
        }
    }
}