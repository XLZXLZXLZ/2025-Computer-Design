using Clipper2Lib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ClippablePolygon : MonoBehaviour
{
    private PolygonCollider2D coll;

    private Vector2[][] originPoints;

    public bool isOut;

    private void Awake()
    {
        //初始化记录的originPoints
        coll = GetComponent<PolygonCollider2D>();

        //如果物体身上挂的是BoxCollider2D，转成等价的PolygonCollider
        if(coll == null)
        {
            var box = GetComponent<BoxCollider2D>();
            if (box != null)
            {
                // 添加 PolygonCollider2D 组件
                coll = gameObject.AddComponent<PolygonCollider2D>();

                // 设置 PolygonCollider2D 的范围与 BoxCollider2D 相同
                coll.points = ConvertBoxColliderToPolygon(box);

                //记得把Trigger和其它配置也爬过去
                coll.isTrigger = box.isTrigger;
                coll.usedByComposite = box.usedByComposite;
                coll.usedByEffector = box.usedByEffector;

                // 删除 BoxCollider2D
                Destroy(box);
            }
            else
            {
                Debug.LogError(gameObject.name + " 物体初始化错误，可切割的物体至少具有 PolygonCollider2D 或 BoxCollider2D 之一");
            }
        }
        originPoints = new Vector2[coll.pathCount][];
        for (int i = 0; i < coll.pathCount; ++i)
        {
            var path = coll.GetPath(i);
            originPoints[i] = new Vector2[path.Length];
            for (int j = 0; j < path.Length; ++j)
            {
                originPoints[i][j] = path[j];
            }
        }

        //创建一个trigger子物体，这个子物体用于判断玩家是否在碰撞体内部，当玩家在碰撞体内部时不允许玩家对调世界状态以防卡住
        var triggerObj = new GameObject();
        triggerObj.layer = LayerMask.NameToLayer("GroundTrigger");

        triggerObj.transform.position = transform.position;
        triggerObj.transform.rotation = transform.rotation;
        triggerObj.transform.localScale = transform.localScale;
        triggerObj.transform.parent = transform;

        var trigger = triggerObj.AddComponent<PolygonCollider2D>();
        trigger.isTrigger = true;
        trigger.points = coll.points;
    }

    private void Update()
    {
        //将碰撞盒相对坐标系转换到世界坐标系
        var arr = new Vector2[originPoints.Length][];
        for(int i = 0; i < arr.Length; i++)
        {
            arr[i] = new Vector2[originPoints[i].Length];
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = originPoints[i][j];
                arr[i][j].x *= transform.localScale.x;
                arr[i][j].y *= transform.localScale.y;
                arr[i][j] += (Vector2)transform.position;
            }
        }

        //读取切割窗口
        var window = ColliderClipper.Instance.GetWindow();

        //根据自身属于表里世界状态选择切割结果
        if(isOut)
        {
            arr = ColliderCutterHelper.Difference(arr, window);
        }
        else
        {
            arr = ColliderCutterHelper.Intersect(arr, window);
        }

        //将切割结果转回相对坐标系
        for (int i = 0; i < arr.Length; i++)
        {
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] -= (Vector2)transform.position;
                arr[i][j].x /= transform.localScale.x;
                arr[i][j].y /= transform.localScale.y;
            }
        }

        //设置回polygon的path
        coll.pathCount = arr.Length;


        for (int i = 0; i < arr.Length; ++i) 
        {
            coll.SetPath(i, arr[i]);
        }
    }

    private Vector2[] ConvertBoxColliderToPolygon(BoxCollider2D box)
    {
        // 获取 BoxCollider2D 的范围
        Vector2 size = box.size;
        Vector2 center = box.offset;

        // 计算 PolygonCollider2D 的四个点
        return new Vector2[]
        {
            center + new Vector2(-size.x / 2, -size.y / 2),
            center + new Vector2(size.x / 2, -size.y / 2),
            center + new Vector2(size.x / 2, size.y / 2),
            center + new Vector2(-size.x / 2, size.y / 2)
        };
    }
}
