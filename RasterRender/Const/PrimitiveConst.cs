
using RasterRender.Engine;
using RasterRender.Engine.Mathf;
using System.Collections.Generic;
namespace RasterRender.Const
{
    class PrimitiveConst
    {
        public static List<Vector3> CubeVertexs=new List<Vector3>()
        {
            new Vector3(1,1,1),new Vector3(-1,1,1),new Vector3(1,-1,1),new Vector3(-1,-1,1),
            new Vector3(1,1,-1),new Vector3(-1,1,-1),new Vector3(1,-1,-1),new Vector3(-1,-1,-1),
        };

        public static List<int> CubeTriangles=new List<int>()
        {
            0,1,2, 1,2,3, 0,1,5, 0,4,5, 0,2,4, 2,4,6, 2,3,7, 2,6,7, 4,5,6, 5,6,7, 1,3,7, 1,5,7,
        };

    }
}
