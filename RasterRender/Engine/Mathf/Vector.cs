namespace RasterRender.Engine.Mathf
{
    public struct Vector2
    {
        public float x, y;

        public float this[int index]
        {
            get { return index == 0 ? x : y; }
        }

        public Vector2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct Vector3
    {
        public float x, y, z;

        public float this[int index]
        {
            get { return index == 0 ? x : index == 1 ? y : z; }
        }

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct Vector4
    {
        public float x, y, z, w;

        public float this[int index]
        {
            get { return index == 0 ? x : index == 1 ? y : index == 2 ? z : w; }
        }

        public Vector4(float x = 0, float y = 0, float z = 0, float w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator Vector4(Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z, 0);
        }
    }
}
