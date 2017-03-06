using RasterRender.Engine.Mathf;

namespace RasterRender.Engine
{
    /// <summary>
    /// 场景
    /// </summary>
    public class Scene
    {
        public static Scene instance = new Scene();

        private List<GameObject> mObjectList = new List<GameObject>();

        /// <summary>
        /// 在场景中增加一个物体
        /// </summary>
        /// <returns>返回物体的唯一id</returns>
        public string AddGameObject(GameObject gameObject)
        {

        }
    }
}
