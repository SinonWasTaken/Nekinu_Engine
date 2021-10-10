using OpenTK.Graphics.OpenGL;
using System;

namespace Nekinu
{
    public class FrameBuffer
    {
        public struct FrameBufferSpecification
        {
            public int width, height;
            public int samples;

            public bool swapChainTarget;
        }

        private int id;
        public int colorBuffer { get; private set; }
        private int depthBuffer;
        public FrameBufferSpecification specification { get; private set; }

        public FrameBuffer(FrameBufferSpecification specification)
        {
            this.specification = specification;

            Create();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, id);
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void Create()
        {
            GL.CreateFramebuffers(1, out id);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
            int colorID;
            GL.CreateTextures(TextureTarget.Texture2D, 1, out colorID);
            colorBuffer = colorID;

            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, specification.width, specification.height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out depthBuffer);
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, specification.width, specification.height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthBuffer, 0);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Crash_Report.generate_crash_report(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
                Environment.Exit(-5);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Delete()
        {
            GL.DeleteFramebuffer(id);
        }
    }
}