namespace RasterRender.Engine.Mathf
{
    public struct Vertex
    {
        public Vector3 pos;
        public Vector2 uv;

        public static Vertex Lerp(Vertex v1, Vertex v2, float t)
        {
            return new Vertex()
            {
                pos = Vector3.Lerp(v1.pos, v2.pos, t),
                uv = Vector2.Lerp(v1.uv, v2.uv, t)
            };
        }

        public static Vertex Division(Vertex v1, Vertex v2, float t)
        {
            return new Vertex()
            {
                pos = 1 / t * (v2.pos - v1.pos),
                uv = 1 / t * (v2.uv - v1.uv),
            };
        }
    }
}

