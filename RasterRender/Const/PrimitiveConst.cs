﻿
using RasterRender.Engine;
using RasterRender.Engine.Mathf;
using System.Collections.Generic;
using RasterRender.Engine.Simple;

namespace RasterRender.Const
{
    internal class PrimitiveConst
    {
        public static List<Vertex> CubeVertexs = new List<Vertex>()
        {
            new Vertex() {pos = new Vector3(1, 1, 1), uv = new TexCoord(1, 1)},
            new Vertex() {pos = new Vector3(1, 1, 1), uv = new TexCoord(1, 0)},
            new Vertex() {pos = new Vector3(1, 1, 1), uv = new TexCoord(0, 1)},
            //3
            new Vertex() {pos = new Vector3(-1, 1, 1), uv = new TexCoord(0, 1)},
            new Vertex() {pos = new Vector3(-1, 1, 1), uv = new TexCoord(0, 0)},
            new Vertex() {pos = new Vector3(-1, 1, 1), uv = new TexCoord(1, 1)},
            //6
            new Vertex() {pos = new Vector3(1, -1, 1), uv = new TexCoord(1, 0)},
            new Vertex() {pos = new Vector3(1, -1, 1), uv = new TexCoord(0, 0)},
            new Vertex() {pos = new Vector3(1, -1, 1), uv = new TexCoord(1, 1)},
            //9
            new Vertex() {pos = new Vector3(-1, -1, 1), uv = new TexCoord(0, 0)},
            new Vertex() {pos = new Vector3(-1, -1, 1), uv = new TexCoord(1, 0)},
            new Vertex() {pos = new Vector3(-1, -1, 1), uv = new TexCoord(0, 1)},
            //12
            new Vertex() {pos = new Vector3(1, 1, -1), uv = new TexCoord(1, 1)},
            new Vertex() {pos = new Vector3(1, 1, -1), uv = new TexCoord(1, 1)},
            new Vertex() {pos = new Vector3(1, 1, -1), uv = new TexCoord(0, 1)},
            //15
            new Vertex() {pos = new Vector3(-1, 1, -1), uv = new TexCoord(0, 1)},
            new Vertex() {pos = new Vector3(-1, 1, -1), uv = new TexCoord(0, 1)},
            new Vertex() {pos = new Vector3(-1, 1, -1), uv = new TexCoord(0, 0)},
            //18
            new Vertex() {pos = new Vector3(1, -1, -1), uv = new TexCoord(1, 0)},
            new Vertex() {pos = new Vector3(1, -1, -1), uv = new TexCoord(0, 0)},
            new Vertex() {pos = new Vector3(1, -1, -1), uv = new TexCoord(1, 1)},
            //21
            new Vertex() {pos = new Vector3(-1, -1, -1), uv = new TexCoord(0, 0)},
            new Vertex() {pos = new Vector3(-1, -1, -1), uv = new TexCoord(1, 0)},
            new Vertex() {pos = new Vector3(-1, -1, -1), uv = new TexCoord(0, 1)},

        };

        public static Vertex[] mesh = new Vertex[]
        {
            new Vertex() {pos = new Vector4(1, -1, 1),   uv = new TexCoord(1, 1), color = new Color(1.0f, 0.2f, 0.2f), rhw = 1},
            new Vertex() {pos = new Vector4(-1, -1, 1),  uv = new TexCoord(0, 1), color = new Color(0.2f, 1.0f, 0.2f), rhw = 1},
            new Vertex() {pos = new Vector4(-1, 1, 1),   uv = new TexCoord(1, 0), color = new Color(0.2f, 0.2f, 1.0f), rhw = 1},
            new Vertex() {pos = new Vector4(1, 1, 1),    uv = new TexCoord(0, 0), color = new Color(1.0f, 0.2f, 1.0f), rhw = 1},
            new Vertex() {pos = new Vector4(1, -1, -1),  uv = new TexCoord(1, 1), color = new Color(1.0f, 0.2f, 0.2f), rhw = 1},
            new Vertex() {pos = new Vector4(-1, -1, -1), uv = new TexCoord(0, 1), color = new Color(0.2f, 1.0f, 1.0f), rhw = 1},
            new Vertex() {pos = new Vector4(-1, 1, -1),  uv = new TexCoord(1, 0), color = new Color(1.0f, 0.3f, 0.3f), rhw = 1},
            new Vertex() {pos = new Vector4(1, 1, -1),   uv = new TexCoord(0, 0), color = new Color(0.2f, 1.0f, 0.3f), rhw = 1},
        };

}
}
