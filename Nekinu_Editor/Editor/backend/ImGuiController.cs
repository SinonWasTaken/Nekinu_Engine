using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Nekinu;
using Window = Nekinu.Window;

namespace Nekinu.Editor
{
    /// <summary>
    /// A modified version of Veldrid.ImGui's ImGuiRenderer.
    /// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
    /// </summary>
    internal class ImGuiController : IDisposable
    {
        private bool _frameBegun;

        private int _vertexArray;
        private int _vertexBuffer;
        private int _vertexBufferSize;
        private int _indexBuffer;
        private int _indexBufferSize;

        private GuiTexture _fontTexture;
        private GuiShader _shader;

        private int _windowWidth;
        private int _windowHeight;

        private System.Numerics.Vector2 _scaleFactor = System.Numerics.Vector2.One;

        private List<Keys> ignoreKeys;
        private List<Special_Keys> _specialKeysList;

        private ImGuiControllerInputClass input;
        
        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(int width, int height, Window window)
        {
            input = new ImGuiControllerInputClass(window);
            
            setIgnoredKeys();
            
            _windowWidth = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            var io = ImGui.GetIO();
            io.Fonts.AddFontDefault();

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;

            ImGuiDockNodeFlags dockNodeFlags = 0;
            dockNodeFlags |= ImGuiDockNodeFlags.PassthruCentralNode;

            CreateDeviceResources();
            SetKeyMappings();

            SetPerFrameImGuiData(1f / 60f);

            ImGui.NewFrame();
            _frameBegun = true;
        }

        private void setIgnoredKeys()
        {
            ignoreKeys = new List<Keys>()
            {
                Keys.Enter,
                Keys.LeftAlt,
                Keys.RightAlt,
                Keys.LeftControl,
                Keys.RightControl,
                Keys.LeftShift,
                Keys.RightShift,
                Keys.LeftSuper,
                Keys.RightSuper,
                Keys.Delete,
                Keys.Backspace,
                Keys.Tab,
                Keys.Left,
                Keys.Right,
                Keys.Up,
                Keys.Down,
                Keys.PageUp,
                Keys.PageDown,
                Keys.Home,
                Keys.End,
                Keys.Delete,
                Keys.Enter,
                Keys.Escape,
                Keys.PrintScreen,
                Keys.Insert,
                Keys.Home,
                Keys.NumLock,
                Keys.KeyPadEnter
            };
            
            setSpecialKeys();
        }

        private void setSpecialKeys()
        {
            _specialKeysList = new List<Special_Keys>()
            {
                new Special_Keys(Keys.Space, ' ', ' '),
                new Special_Keys(Keys.RightBracket, '}', ']'),
                new Special_Keys(Keys.LeftBracket, '{', '['),
                new Special_Keys(Keys.Slash, '?', '/'),
                new Special_Keys(Keys.Backslash, '|', '\\'),
                new Special_Keys(Keys.Period, '>', '.'),
                new Special_Keys(Keys.Comma, '<', ','),
                new Special_Keys(Keys.Semicolon, ':', ':'),
                new Special_Keys(Keys.Apostrophe, '"', '\''),
                new Special_Keys(Keys.GraveAccent, '~', '`'),
                new Special_Keys(Keys.Minus, '_', '-'),
                new Special_Keys(Keys.Equal, '+', '='),
                new Special_Keys(Keys.D0, ')', '0'),
                new Special_Keys(Keys.D1, '!', '1'),
                new Special_Keys(Keys.D2, '@', '2'),
                new Special_Keys(Keys.D3, '#', '3'),
                new Special_Keys(Keys.D4, '$', '4'),
                new Special_Keys(Keys.D5, '%', '5'),
                new Special_Keys(Keys.D6, '^', '6'),
                new Special_Keys(Keys.D7, '&', '7'),
                new Special_Keys(Keys.D8, '*', '8'),
                new Special_Keys(Keys.D9, '(', '9'),
                new Special_Keys(Keys.KeyPad0, '0', '0'),
                new Special_Keys(Keys.KeyPad1, '1', '1'),
                new Special_Keys(Keys.KeyPad2, '2', '2'),
                new Special_Keys(Keys.KeyPad3, '3', '3'),
                new Special_Keys(Keys.KeyPad4, '4', '4'),
                new Special_Keys(Keys.KeyPad5, '5', '5'),
                new Special_Keys(Keys.KeyPad6, '6', '6'),
                new Special_Keys(Keys.KeyPad7, '7', '7'),
                new Special_Keys(Keys.KeyPad8, '8', '8'),
                new Special_Keys(Keys.KeyPad9, '9', '9'),
                new Special_Keys(Keys.KeyPadDecimal, '.', '.'),
                new Special_Keys(Keys.KeyPadAdd, '+', '+'),
                new Special_Keys(Keys.KeyPadDivide, '/', '/'),
                new Special_Keys(Keys.KeyPadMultiply, '*', '*'),
                new Special_Keys(Keys.KeyPadSubtract, '-', '-'),
            };
        }
        
        public void WindowResized(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        public void CreateDeviceResources()
        {
            Util.CreateVertexArray("ImGui", out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize = 2000;

            Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
            Util.CreateElementBuffer("ImGui", out _indexBuffer);
            GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            RecreateFontDeviceTexture();

            string VertexSource = @"#version 330 core
uniform mat4 projection_matrix;
layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;
out vec4 color;
out vec2 texCoord;
void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
            string FragmentSource = @"#version 330 core
uniform sampler2D in_fontTexture;
in vec4 color;
in vec2 texCoord;
out vec4 outputColor;
void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";
            _shader = new GuiShader("ImGui", VertexSource, FragmentSource);

            GL.VertexArrayVertexBuffer(_vertexArray, 0, _vertexBuffer, IntPtr.Zero, Unsafe.SizeOf<ImDrawVert>());
            GL.VertexArrayElementBuffer(_vertexArray, _indexBuffer);

            GL.EnableVertexArrayAttrib(_vertexArray, 0);
            GL.VertexArrayAttribBinding(_vertexArray, 0, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 0, 2, VertexAttribType.Float, false, 0);

            GL.EnableVertexArrayAttrib(_vertexArray, 1);
            GL.VertexArrayAttribBinding(_vertexArray, 1, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 1, 2, VertexAttribType.Float, false, 8);

            GL.EnableVertexArrayAttrib(_vertexArray, 2);
            GL.VertexArrayAttribBinding(_vertexArray, 2, 0);
            GL.VertexArrayAttribFormat(_vertexArray, 2, 4, VertexAttribType.UnsignedByte, true, 16);

            Util.CheckGLError("End of ImGui setup");
        }

        /// <summary>
        /// Recreates the device texture used to render text.
        /// </summary>
        public void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            _fontTexture = new GuiTexture("ImGui Text Atlas", width, height, pixels);
            _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            io.Fonts.SetTexID((IntPtr)_fontTexture.GLTexture);

            io.Fonts.ClearTexData();
        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                ImGui.Render();
                RenderImDrawData(ImGui.GetDrawData());
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(GameWindow wnd, float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput(wnd);

            _frameBegun = true;
            ImGui.NewFrame();
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(
                _windowWidth / _scaleFactor.X,
                _windowHeight / _scaleFactor.Y);
            io.DisplayFramebufferScale = _scaleFactor;
            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        readonly List<char> PressedChars = new List<char>();

        private void UpdateImGuiInput(GameWindow wnd)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            MouseState MouseState = wnd.MouseState;
            KeyboardState KeyboardState = wnd.KeyboardState;

            MouseScroll(new System.Numerics.Vector2(MouseState.Scroll.X, MouseState.Scroll.Y));
            
            io.MouseDown[0] = MouseState[MouseButton.Left];
            io.MouseDown[1] = MouseState[MouseButton.Right];
            io.MouseDown[2] = MouseState[MouseButton.Middle];

            var screenPoint = new Vector2i((int)MouseState.X, (int)MouseState.Y);
            var point = screenPoint;//wnd.PointToClient(screenPoint);
            io.MousePos = new System.Numerics.Vector2(point.X, point.Y);
            
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key == Keys.Unknown)
                {
                    continue;
                }
                io.KeysDown[(int)key] = KeyboardState.IsKeyDown(key);

                if (!isKeyIgnored(key))
                {
                    if (input.isKeyPressed(key))
                    {
                        Special_Keys special = isKeySpecial(key);
                        if (special != null)
                        {
                            if (input.isKeyDown(input.LeftShift) || input.isKeyDown(input.RightShift))
                            {
                                PressChar(special.special_upper_case);
                            }
                            else
                            {
                                PressChar(special.special_lower_case);
                            }
                        }
                        else
                        {
                            if (input.isKeyDown(input.LeftShift) || input.isKeyDown(input.RightShift))
                            {
                                PressChar(key.ToString().ToUpper().ToCharArray()[0]);
                            }
                            else
                            {
                                PressChar(key.ToString().ToLower().ToCharArray()[0]);
                            }
                        }
                    }
                }
            }

            foreach (var c in PressedChars)
            {
                io.AddInputCharacter(c);
            }
            PressedChars.Clear();

            io.KeyCtrl = KeyboardState.IsKeyDown(Keys.LeftControl) || KeyboardState.IsKeyDown(Keys.RightControl);
            io.KeyAlt = KeyboardState.IsKeyDown(Keys.LeftAlt) || KeyboardState.IsKeyDown(Keys.RightAlt);
            io.KeyShift = KeyboardState.IsKeyDown(Keys.LeftShift) || KeyboardState.IsKeyDown(Keys.RightShift);
            io.KeySuper = KeyboardState.IsKeyDown(Keys.LeftSuper) || KeyboardState.IsKeyDown(Keys.RightSuper);
        }

        internal bool isKeyIgnored(Keys key)
        {
            for (int i = 0; i < ignoreKeys.Count; i++)
            {
                if (key == ignoreKeys[i])
                    return true;
            }

            return false;
        }

        internal Special_Keys isKeySpecial(Keys key)
        {
            for (int i = 0; i < _specialKeysList.Count; i++)
            {
                if (key == _specialKeysList[i].special_key)
                {
                    return _specialKeysList[i];
                }
            }

            return null;
        }
        
        internal void PressChar(char keyChar)
        {
            PressedChars.Add(keyChar);
        }

        internal void MouseScroll(System.Numerics.Vector2 offset)
        {
            ImGuiIOPtr io = ImGui.GetIO();

            io.MouseWheel = offset.Y;
            io.MouseWheelH = offset.X;
        }

        private static void SetKeyMappings()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
            io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;
        }

        private void RenderImDrawData(ImDrawDataPtr draw_data)
        {
            if (draw_data.CmdListsCount == 0)
            {
                return;
            }

            for (int i = 0; i < draw_data.CmdListsCount; i++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                int vertexSize = cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>();
                if (vertexSize > _vertexBufferSize)
                {
                    int newSize = (int)System.Math.Max(_vertexBufferSize * 1.5f, vertexSize);
                    GL.NamedBufferData(_vertexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _vertexBufferSize = newSize;
                }

                int indexSize = cmd_list.IdxBuffer.Size * sizeof(ushort);
                if (indexSize > _indexBufferSize)
                {
                    int newSize = (int)System.Math.Max(_indexBufferSize * 1.5f, indexSize);
                    GL.NamedBufferData(_indexBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                    _indexBufferSize = newSize;
                }
            }

            // Setup orthographic projection matrix into our constant buffer
            ImGuiIOPtr io = ImGui.GetIO();
            Matrix4 mvp = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _shader.UseShader();
            GL.UniformMatrix4(_shader.GetUniformLocation("projection_matrix"), false, ref mvp);
            GL.Uniform1(_shader.GetUniformLocation("in_fontTexture"), 0);
            Util.CheckGLError("Projection");

            GL.BindVertexArray(_vertexArray);
            Util.CheckGLError("VAO");

            draw_data.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];

                GL.NamedBufferSubData(_vertexBuffer, IntPtr.Zero, cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);
                Util.CheckGLError($"Data Vert {n}");

                GL.NamedBufferSubData(_indexBuffer, IntPtr.Zero, cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);
                Util.CheckGLError($"Data Idx {n}");

                int vtx_offset = 0;
                int idx_offset = 0;

                for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);
                        Util.CheckGLError("Texture");

                        // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                        var clip = pcmd.ClipRect;
                        GL.Scissor((int)clip.X, _windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
                        Util.CheckGLError("Scissor");

                        if ((io.BackendFlags & ImGuiBackendFlags.RendererHasVtxOffset) != 0)
                        {
                            GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), vtx_offset);
                        }
                        else
                        {
                            GL.DrawElements(BeginMode.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (int)pcmd.IdxOffset * sizeof(ushort));
                        }
                        Util.CheckGLError("Draw");
                    }

                    idx_offset += (int)pcmd.ElemCount;
                }
                vtx_offset += cmd_list.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            _fontTexture.Dispose();
            _shader.Dispose();
        }
    }
}

internal class Special_Keys
{
    public Keys special_key { get; private set; }

    public char special_upper_case { get; private set; }
    public char special_lower_case { get; private set; }

    public Special_Keys(Keys key, char upper, char lower)
    {
        special_key = key;
        special_upper_case = upper;
        special_lower_case = lower;
    }
}

internal class ImGuiControllerInputClass
{
    private Window window;

    public Keys KP_0 = Keys.KeyPad0;
    public Keys KP_1 = Keys.KeyPad1;
    public Keys KP_2 = Keys.KeyPad2;
    public Keys KP_3 = Keys.KeyPad3;
    public Keys KP_4 = Keys.KeyPad4;
    public Keys KP_5 = Keys.KeyPad5;
    public  Keys KP_6 = Keys.KeyPad6;
    public  Keys KP_7 = Keys.KeyPad7;
    public  Keys KP_8 = Keys.KeyPad8;
    public  Keys KP_9 = Keys.KeyPad9;
    public  Keys KP_Divide = Keys.KeyPadDivide;
    public  Keys KP_Multiply = Keys.KeyPadMultiply;
    public  Keys KP_Add = Keys.KeyPadAdd;
    public  Keys KP_Subtract = Keys.KeyPadSubtract;
    public  Keys KP_Decimal = Keys.KeyPadDecimal;
    public  Keys KP_Enter = Keys.KeyPadEnter;

    public  Keys UP_ARROW = Keys.Up;
    public  Keys DOWN_ARROW = Keys.Down;
    public  Keys LEFT_ARROW = Keys.Left;
    public  Keys RIGHT_ARROW = Keys.Right;

    public  Keys LeftShift = Keys.LeftShift;
    public  Keys RightShift = Keys.RightShift;
    public  Keys Enter = Keys.Enter;
    public  Keys Escape = Keys.Escape;
    public  Keys CapsLock = Keys.CapsLock;
    public  Keys LeftAlt = Keys.LeftAlt;
    public  Keys RightAlt = Keys.RightAlt;
    public  Keys BackSpace = Keys.Backspace;
    public  Keys Space = Keys.Space;
    public  Keys LeftControl = Keys.LeftControl;
    public  Keys RightControl = Keys.RightControl;
    public  Keys TAB = Keys.Tab;

    public  Keys A = Keys.A;
    public  Keys B = Keys.B;
    public  Keys C = Keys.C;
    public  Keys D = Keys.D;
    public  Keys E = Keys.E; 
    public  Keys F = Keys.F;
    public  Keys G = Keys.G;
    public  Keys H = Keys.H;
    public  Keys I = Keys.I;
    public  Keys J = Keys.J;
    public  Keys K = Keys.K;
    public  Keys L = Keys.L;
    public  Keys M = Keys.M;
    public  Keys N = Keys.N;
    public  Keys O = Keys.O; 
    public  Keys P = Keys.P;
    public  Keys Q = Keys.Q;
    public  Keys R = Keys.R;
    public  Keys S = Keys.S;
    public  Keys T = Keys.T; 
    public  Keys U = Keys.U;
    public  Keys V = Keys.V;
    public  Keys W = Keys.W;
    public  Keys X = Keys.X;
    public  Keys Y = Keys.Y;
    public  Keys Z = Keys.Z;

    public  Keys LeftBracket = Keys.LeftBracket;
    public  Keys RightBracket = Keys.RightBracket;
    public  Keys BackSlash = Keys.Backslash;
    public  Keys ForwardSlash = Keys.Slash;
    public  Keys Comma = Keys.Comma;
    public  Keys Period = Keys.Period;

    public  Keys Keyboard_0 = Keys.D0;
    public  Keys Keyboard_1 = Keys.D1;
    public  Keys Keyboard_2 = Keys.D2;
    public  Keys Keyboard_3 = Keys.D3;
    public  Keys Keyboard_4 = Keys.D4;
    public  Keys Keyboard_5 = Keys.D5;
    public  Keys Keyboard_6 = Keys.D6;
    public  Keys Keyboard_7 = Keys.D7;
    public Keys Keyboard_8 = Keys.D8;
    public Keys Keyboard_9 = Keys.D9;

    public Keys Keyboard_Minus = Keys.Minus;
    public Keys Keyboard_Equals = Keys.Equal;

    public MouseButton Mouse0 = MouseButton.Left;
    public MouseButton Mouse1 = MouseButton.Middle;
    public MouseButton Mouse2 = MouseButton.Right;

    private int mouse_X, mouse_Y;

    private float mouse_x_delta, mouse_y_delta;
    private bool[] keysPressed;

    public ImGuiControllerInputClass(Window inputWindow)
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

    public bool isKeyDown(Keys key)
    {
        return window.IsKeyDown(key);
    }

    public bool isKeyPressed(Keys key)
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

    public bool isButtonDown(MouseButton button)
    {
        return window.IsMouseButtonPressed(button);
    }

    public bool isButtonUp(MouseButton button) 
    {
        return window.IsMouseButtonReleased(button);
    }

    public bool isButtonHeld(MouseButton button)
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
}