using System.Collections.Generic;
using RasterRender.Engine.Mathf;

namespace RasterRender.Engine
{
    //public class Transform
    //{

    //	//世界坐标位置
    //    public Vector3 position;

    //    //局部坐标位置
    //    public Vector3 localePosition;

    //    //uvn旋转矩阵
    //    public Matrix4x4 localeRotation;

    //   	//缩放
    //   	public Matrix4x4 localeScale;

    //   	//所对应的物体
    //   	public GameObject gameObject;

    //   	//父节点
    //   	public Transform parent;

    //   	//子节点数量
    //   	public int childCount{
    //   		get{ return 0;}
    //   	}

    //   	//转换到本地坐标的矩阵
    //   	private Matrix4x4 mLocaleMatrix;

    //   	//子节点链表
    //   	private List<Transform> _children;

    //   	//创建游戏对象的时候调用
    //   	internal Transform(GameObject gameObject, Transform parent)
    //   	{
    //   		this.gameObject = gameObject;
    //   		this.parent = parent;

    //   		this.localePosition = Vector3.zero;
    //   		this.localeScale = Matrix4x4.identity;
    //   		this.localeRotation = Matrix4x4.identity;
    //   	}

    //   	public void SetParent(Transform parent)
    //   	{
    //   		parent._children.Add(this);
    //   		this.parent = parent;
    //   	}

    //   	public Transform GetChild(int index)
    //   	{
    //   		return _children[index];
    //   	}

    //    public Vector3 ConverseToLocale(Vector3 worldPosition)
    //    {
    //        Vector3 localePositionInParent;
    //        if (parent != null)
    //        {
    //            localePositionInParent = parent.ConverseToLocale(worldPosition);
    //        }
    //        else
    //        {
    //            localePositionInParent = worldPosition;
    //        }
    //        return localePositionInParent * mLocaleMatrix;
    //    }

    //}
}
