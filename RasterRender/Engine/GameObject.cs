using RasterRender.Engine.Mathf;

namespace RasterRender.Engine
{
    public class GameObject
    {

        /// <summary>
        ///  物体的层级和变换对象
        /// </summary>
        public Transform transform { get; private set; }

        /// <summary>
        /// 物体的名称
        /// </summary>
        public string name;

        /// <summary>
        ///物体的状态
        /// </summary>
        public bool activeSelf;

        /// <summary>
        /// 平均半径
        /// </summary>
        float avg_radius;
        /// <summary>
        /// 最大半径
        /// </summary>
        float max_radius;

        /// <summary>
        /// 顶点数量
        /// </summary>
        int num_verteices;

        /// <summary>
        /// 存储顶点局部坐标的数组
        /// </summary>
        Vector4[] vlist_locale;
        /// <summary>
        /// 存储顶点变换后坐标的数组
        /// </summary>
        Vector4[] vlist_trans;

        /// <summary>
        /// 物体的多边形数
        /// </summary>
        int num_polys;      



        public GameObject()
        {
            transform = new Transform();
            name = "New GameObject";
        }


    }
}
