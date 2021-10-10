using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nekinu
{
    public class Input
    {
        private static Window window;

        public static Keys KP_0 = Keys.KeyPad0;
        public static Keys KP_1 = Keys.KeyPad1;
        public static Keys KP_2 = Keys.KeyPad2;
        public static Keys KP_3 = Keys.KeyPad3;
        public static Keys KP_4 = Keys.KeyPad4;
        public static Keys KP_5 = Keys.KeyPad5;
        public static Keys KP_6 = Keys.KeyPad6;
        public static Keys KP_7 = Keys.KeyPad7;
        public static Keys KP_8 = Keys.KeyPad8;
        public static Keys KP_9 = Keys.KeyPad9;
        public static Keys KP_Divide = Keys.KeyPadDivide;
        public static Keys KP_Multiply = Keys.KeyPadMultiply;
        public static Keys KP_Add = Keys.KeyPadAdd;
        public static Keys KP_Subtract = Keys.KeyPadSubtract;
        public static Keys KP_Decimal = Keys.KeyPadDecimal;
        public static Keys KP_Enter = Keys.KeyPadEnter;

        public static Keys UP_ARROW = Keys.Up;
        public static Keys DOWN_ARROW = Keys.Down;
        public static Keys LEFT_ARROW = Keys.Left;
        public static Keys RIGHT_ARROW = Keys.Right;

        public static Keys LeftShift = Keys.LeftShift;
        public static Keys RightShift = Keys.RightShift;
        public static Keys Enter = Keys.Enter;
        public static Keys Escape = Keys.Escape;
        public static Keys CapsLock = Keys.CapsLock;
        public static Keys LeftAlt = Keys.LeftAlt;
        public static Keys RightAlt = Keys.RightAlt;
        public static Keys BackSpace = Keys.Backspace;
        public static Keys Space = Keys.Space;
        public static Keys LeftControl = Keys.LeftControl;
        public static Keys RightControl = Keys.RightControl;
        public static Keys TAB = Keys.Tab;

        public static Keys A = Keys.A;
        public static Keys B = Keys.B;
        public static Keys C = Keys.C;
        public static Keys D = Keys.D;
        public static Keys E = Keys.E;
        public static Keys F = Keys.F;
        public static Keys G = Keys.G;
        public static Keys H = Keys.H;
        public static Keys I = Keys.I;
        public static Keys J = Keys.J;
        public static Keys K = Keys.K;
        public static Keys L = Keys.L;
        public static Keys M = Keys.M;
        public static Keys N = Keys.N;
        public static Keys O = Keys.O;
        public static Keys P = Keys.P;
        public static Keys Q = Keys.Q;
        public static Keys R = Keys.R;
        public static Keys S = Keys.S;
        public static Keys T = Keys.T;
        public static Keys U = Keys.U;
        public static Keys V = Keys.V;
        public static Keys W = Keys.W;
        public static Keys X = Keys.X;
        public static Keys Y = Keys.Y;
        public static Keys Z = Keys.Z;

        public static Keys LeftBracket = Keys.LeftBracket;
        public static Keys RightBracket = Keys.RightBracket;
        public static Keys BackSlash = Keys.Backslash;
        public static Keys ForwardSlash = Keys.Slash;
        public static Keys Comma = Keys.Comma;
        public static Keys Period = Keys.Period;

        public static Keys Keyboard_0 = Keys.D0;
        public static Keys Keyboard_1 = Keys.D1;
        public static Keys Keyboard_2 = Keys.D2;
        public static Keys Keyboard_3 = Keys.D3;
        public static Keys Keyboard_4 = Keys.D4;
        public static Keys Keyboard_5 = Keys.D5;
        public static Keys Keyboard_6 = Keys.D6;
        public static Keys Keyboard_7 = Keys.D7;
        public static Keys Keyboard_8 = Keys.D8;
        public static Keys Keyboard_9 = Keys.D9;

        public static Keys Keyboard_Minus = Keys.Minus;
        public static Keys Keyboard_Equals = Keys.Equal;

        public static MouseButton Mouse0 = MouseButton.Left;
        public static MouseButton Mouse1 = MouseButton.Middle;
        public static MouseButton Mouse2 = MouseButton.Right;

        private static int mouse_X, mouse_Y;

        private static float mouse_x_delta, mouse_y_delta;
        private static bool[] keysPressed;

        public Input(Window inputWindow)
        {
            window = inputWindow;

            keysPressed = new bool[400];

            window.MouseMove += updateMousePosition;
            window.Closing += Window_Closing;
        }

        private void Window_Closing(System.ComponentModel.CancelEventArgs obj)
        {
            window.MouseMove -= updateMousePosition;
            window.Closing -= Window_Closing;
        }

        public static bool isKeyDown(Keys key)
        {
            return window.IsKeyDown(key);
        }

        public static bool isKeyPressed(Keys key)
        {
            //check if the key is currently down
            bool pressed = window.IsKeyDown(key);

            //if it is, then
            if (pressed)
            {
                //if the value at (int)key == false, then
                if (!keysPressed[(int) key])
                {
                    //set the value at (int)key to true, then return true
                    keysPressed[(int) key] = true;
                    return true;
                }
                //However, if the value at (int)key == true, then it means the button was pushed last frame, thus we return false;
                else
                {
                    return false;
                }
            }
            //if it isnt true, then
            else
            {
                //we set the value at (int)key to false and return false;
                keysPressed[(int) key] = false;
                return false;
            }
        }

        public static bool isButtonDown(MouseButton button)
        {
            return window.IsMouseButtonPressed(button);
        }

        public static bool isButtonUp(MouseButton button)
        {
            return window.IsMouseButtonReleased(button);
        }

        public static bool isButtonHeld(MouseButton button)
        {
            return window.IsMouseButtonDown(button);
        }

        private void updateMousePosition(OpenTK.Windowing.Common.MouseMoveEventArgs obj)
        {
            OpenTK.Mathematics.Vector2 point = obj.Position;
            mouse_X = (int)point.X;
            mouse_Y = (int)point.Y;

            mouse_x_delta = obj.DeltaX;
            mouse_y_delta = obj.DeltaY;
        }

        public static int Mouse_X => mouse_X;
        public static int Mouse_Y => mouse_Y;

        public static float Mouse_X_Delta
        {
            get
            {
                float i = mouse_x_delta;
                mouse_x_delta = 0;
                return i;
            }
        }
        public static float Mouse_Y_Delta
        {
            get
            {
                float i = mouse_y_delta;
                mouse_y_delta = 0;
                return i;
            }
        }
    }
}