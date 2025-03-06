using MyExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LightLine : MonoBehaviour
{
    private LineRenderer line;
    private Vector2 origin;
    private Vector2 dir;
    [SerializeField] private bool isOn;
    private float lineWidth;
    private HashSet<LightPhysics> currentHits = new HashSet<LightPhysics>();

    public Vector2 Origin => origin;
    public Vector2 Dir => dir;
    public LineRenderer Line => line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        lineWidth = line.startWidth;
        line.startWidth = isOn ? lineWidth : 0;
    }

    private void Update()
    {
        Effect();
    }

    private void Effect()
    {
        if (!isOn)
            return;

        List<Vector3> positions = new List<Vector3>();
        positions.Add(origin);

        Vector2 currentOrigin = origin;
        Vector2 currentDir = dir.normalized;
        float remainingDistance = 1000f;
        int reflections = 0;
        const int maxReflections = 5;

        HashSet<LightPhysics> newHits = new HashSet<LightPhysics>();

        bool continueTracing = true;

        while (continueTracing && reflections <= maxReflections)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(currentOrigin, currentDir, remainingDistance,~LayerMask.GetMask("BackGround"))
                .Where(h => !CheckParentCollider(h.collider))
                .OrderBy(h => h.distance)
                .ToArray();

            bool hitProcessed = false;
            LightPhysics hitLightPhys = null;

            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;

                Vector2 hitPoint = hit.point;
                float hitDistance = hit.distance;

                LightPhysics lightPhys = hit.collider.GetComponent<LightPhysics>();
                if (lightPhys == null)
                {
                    positions.Add(hitPoint);
                    hitProcessed = true;
                    break;
                }

                if(!currentHits.Contains(lightPhys))
                    lightPhys.LightHit(this);

                newHits.Add(lightPhys);
                hitLightPhys = lightPhys;

                switch (lightPhys.interaction)
                {
                    case LightInterAction.Stop:
                        positions.Add(hitPoint);
                        hitProcessed = true;
                        break;
                    case LightInterAction.Pencentrate:
                        break; // 穿透，继续检测后续碰撞
                    case LightInterAction.Reflect:
                        positions.Add(hitPoint);
                        Vector2 normal = hit.collider.transform.up;
                        currentDir = Vector2.Reflect(currentDir, normal).normalized;
                        currentOrigin = hitPoint + currentDir * 0.001f;
                        remainingDistance -= hitDistance;
                        reflections++;
                        hitProcessed = true;
                        break;
                }

                if (hitProcessed) break;
            }

            if (!hitProcessed)
            {
                positions.Add(currentOrigin + currentDir * remainingDistance);
                continueTracing = false;
            }
            else if (hitLightPhys?.interaction == LightInterAction.Reflect)
            {
                continueTracing = true;
            }
            else
            {
                continueTracing = false;
            }
        }

        // 触发LightLeave
        foreach (var oldHit in currentHits.Except(newHits))
        {
            oldHit.LightLeave(this);
        }
        currentHits = newHits;

        // 更新LineRenderer
        line.positionCount = positions.Count;
        for (int i = 0; i < positions.Count; i++)
        {
            line.SetPosition(i, positions[i]);
        }
    }

    private bool CheckParentCollider(Collider2D input)
    {
        var trans = transform;
        while(trans != null)
        {
            if(input == trans.GetComponent<Collider2D>())
                return true;
            trans = trans.parent;
        }

        return false;
    }

    public void SetActivate(bool activate)
    {
        if (isOn == activate) return;
        isOn = activate;
        line.DOStartWidth(isOn ? lineWidth : 0, 0.3f);

        foreach (var hit in currentHits)
        {
            hit.LightLeave(this);
        }

        currentHits.Clear();
    }

    // 设置光线起点和方向
    public void SetParameters(Vector2 origin, Vector2 direction)
    {
        this.origin = origin;
        this.dir = direction;
    }
}