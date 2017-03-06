namespace RasterRender.Engine.Mathf
{
    public struct Plane3D
    {
        /// <summary>
        /// 平面上的点
        /// </summary>
        public Vector3 p0;

        /// <summary>
        /// 法向量
        /// </summary>
        public Vector3 n;

        public void Init(Vector3 p0, Vector3 n, bool normalize = true)
        {
            this.p0 = p0;
            this.n = normalize ? n : n.Normalize();
        }
    }
}
