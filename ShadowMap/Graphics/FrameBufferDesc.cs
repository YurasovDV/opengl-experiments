using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowMap
{
    public class FrameBufferDesc
    {
        public int TextureId { get; set; }
        public int FramBufferObject { get; set; }
        public int RenderBufferObject { get; set; }
    }
}
