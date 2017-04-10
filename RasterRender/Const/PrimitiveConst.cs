
using RasterRender.Engine;
using RasterRender.Engine.Mathf;
using System.Collections.Generic;
namespace RasterRender.Const
{
    class PrimitiveConst
    {
        public static List<Vertex> CubeVertexs = new List<Vertex>()
        {
            new Vertex() { pos=new Vector3(1,1,1),uv= new Vector2(1,1)},
            new Vertex() { pos=new Vector3(1,1,1),uv= new Vector2(1,0)},
            new Vertex() { pos=new Vector3(1,1,1),uv= new Vector2(0,1)},
            //3
            new Vertex() { pos=new Vector3(-1,1,1),uv= new Vector2(0,1)},
            new Vertex() { pos=new Vector3(-1,1,1),uv= new Vector2(0,0)},
            new Vertex() { pos=new Vector3(-1,1,1),uv= new Vector2(1,1)},
            //6
            new Vertex() { pos=new Vector3(1,-1,1),uv= new Vector2(1,0)},
            new Vertex() { pos=new Vector3(1,-1,1),uv= new Vector2(0,0)},
            new Vertex() { pos=new Vector3(1,-1,1),uv= new Vector2(1,1)},
            //9
            new Vertex() { pos=new Vector3(-1,-1,1),uv= new Vector2(0,0)},
            new Vertex() { pos=new Vector3(-1,-1,1),uv= new Vector2(1,0)},
            new Vertex() { pos=new Vector3(-1,-1,1),uv= new Vector2(0,1)},
            //12
            new Vertex() { pos=new Vector3(1,1,-1),uv= new Vector2(1,1)},
            new Vertex() { pos=new Vector3(1,1,-1),uv= new Vector2(1,1)},
            new Vertex() { pos=new Vector3(1,1,-1),uv= new Vector2(0,1)},
            //15
            new Vertex() { pos=new Vector3(-1,1,-1),uv= new Vector2(0,1)},
            new Vertex() { pos=new Vector3(-1,1,-1),uv= new Vector2(0,1)},
            new Vertex() { pos=new Vector3(-1,1,-1),uv= new Vector2(0,0)},
            //18
            new Vertex() { pos=new Vector3(1,-1,-1),uv= new Vector2(1,0)},
            new Vertex() { pos=new Vector3(1,-1,-1),uv= new Vector2(0,0)},
            new Vertex() { pos=new Vector3(1,-1,-1),uv= new Vector2(1,1)},
            //21
            new Vertex() { pos=new Vector3(-1,-1,-1),uv= new Vector2(0,0)},
            new Vertex() { pos=new Vector3(-1,-1,-1),uv= new Vector2(1,0)},
            new Vertex() { pos=new Vector3(-1,-1,-1),uv= new Vector2(0,1)},

        };

        public static List<int> CubeTriangles = new List<int>()
        {
            0,3,6, 3,6,9, 1,4,16, 1,13,16, 8,11,21, 8,18,21, 3,7,13, 18,7,13 ,//4,5,6, 5,6,7, 1,3,7, 1,5,7,
        };


    }
}
