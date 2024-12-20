using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    //public SoldierStats stats;//utilización flyweight

    private Enemy _enemy;
    private HideBody _enemyDead;
    private float currentAngle;
    private float currentViewRadius;
    public float viewRadius; // Este
    [Range(0, 360)] public float viewAngle; // Este
    public float _verticalOffsetValue; // Este

    public LayerMask targetMask; // Este
    public LayerMask obstacleMask; // Este

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution; // Este
    public int edgeResolveIterations; // Este
    public float edgeDstThreshold; // Este

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    void Start()
    {
        StartCoroutine(SuscribeCoroutine(1f));
        currentAngle = viewAngle;
        currentViewRadius = viewRadius;
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        _enemy = GetComponent<Enemy>();
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator SuscribeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.FullActivity += FullRadio;
        GameManager.NormalActivity += NormalRadio;
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void LateUpdate()
    {
        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            
            Enemy detectedEnemy = target.GetComponentInParent<Enemy>();
            if (detectedEnemy != null && detectedEnemy == this.GetComponent<Enemy>())
            {
                continue;
            }
            
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                Vector3 _verticalOffset = new Vector3(0, _verticalOffsetValue, 0);
                if (!Physics.Raycast(transform.position + _verticalOffset, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    if (target.gameObject.layer == 6)
                    {
                        _enemy.GetPlayer(target);
                        print("veo al player");
                    }

                    if (target.gameObject.layer == 10)
                    {
                        Enemy enemyScript = target.GetComponentInParent<Enemy>();
                        if (enemyScript && enemyScript.Dead && enemyScript.SeenDead == false)
                        {
                            if (_enemy.Dead) return;
                            print("detecté enemigo muerto");
                            _enemy.SetBehavior(new InvestigateDeadBodyBehavior(target.transform.position - Vector3.back * 2));
                            enemyScript.SeenDead = true;
                        }
                    }
                }
            }
        }
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit ||
                    (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    public void Destroy()
    {
        viewMeshFilter.mesh = null;
        Destroy(this);
    }

    private void OnDestroy()
    {
        GameManager.FullActivity -= FullRadio;
        GameManager.NormalActivity -= NormalRadio;
    }

    private void FullRadio()
    {
        viewRadius = currentViewRadius * 1.5f;
    }

    private void NormalRadio()
    {
        viewRadius = currentViewRadius;
    }

    private void OnDisable()
    {
        viewMeshFilter.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        viewMeshFilter.gameObject.SetActive(true);
    }
}