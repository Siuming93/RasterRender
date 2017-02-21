namespace RasterRender.Engine.Mathf
{
    public struct ParmLine
    {
        /// <summary>
        /// 起点
        /// </summary>
        public Vector2 p0;

        /// <summary>
        /// 终点
        /// </summary>
        public Vector2 p1;

        /// <summary>
        /// 线段的方向 |v|=|p0->p1|
        /// </summary>
        public Vector2 v;
    }

    public struct ParLine3D
    {
        /// <summary>
        /// 起点
        /// </summary>
        public Vector3 p0;

        /// <summary>
        /// 终点
        /// </summary>
        public Vector3 p1;

        /// <summary>
        /// 线段的方向 |v|=|p0->p1|
        /// </summary>
        public Vector3 v;
    }
}
