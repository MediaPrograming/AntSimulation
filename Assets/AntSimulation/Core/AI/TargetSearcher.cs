using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class TargetSearcher : MonoBehaviour
    {
        public event Action<Transform[]> OnFindTargets;
        public float viewRadius;
        [Range(0, 360)] public float viewAngle;

        /// <summary>
        /// ターゲットマスク
        /// </summary>
        public LayerMask targetMask;

        /// <summary>
        /// 障害物マスク
        /// </summary>
        public LayerMask obstacleMask;

        public MeshFilter meshFilter;
        private Mesh _viewMesh;


        void Start()
        {
#if UNITY_EDITOR
            _viewMesh = new Mesh { name = "View Mesh" };
            meshFilter.mesh = _viewMesh;
#endif
            StartCoroutine(nameof(FindTargetsWithDelay), .2f);
        }


        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

#if UNITY_EDITOR
        void LateUpdate() => DrawFieldOfView();
#endif

        /// <summary>
        /// ターゲットのリストの更新
        /// </summary>
        void FindVisibleTargets()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

            var inTargets = targets
                .Where(x => IsInSightTarget(this.transform, x.transform, viewAngle, obstacleMask))
                .Select(x => x.transform)
                .ToArray();

            // if (inTargets.Length == 0)
            //     return;
            
            OnFindTargets?.Invoke(inTargets);
        }

        /// <summary>
        /// ターゲットとの角度差を返す
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static Vector3 GetAngleFromTarget(Transform self, Transform target) =>
            (target.position - self.position).normalized;

        /// <summary>
        /// ターゲットが視界内にいるかどうかを返す
        /// </summary>
        /// <param name="self">自分</param>
        /// <param name="target">目標物</param>
        /// <param name="angle">視野角</param>
        /// <param name="layerMask">障害物レイヤー</param>
        /// <returns></returns>
        private static bool IsInSightTarget(Transform self, Transform target, float angle, LayerMask layerMask)
        {
            var dir = GetAngleFromTarget(self, target);
            if (!(Vector3.Angle(self.forward, dir) < angle / 2)) return false;

            var position = self.position;
            float dstToTarget = Vector3.Distance(position, target.position);
            // ターゲット以外のオブジェクトとぶつかったとき
            return !Physics.Raycast(position, dir, dstToTarget, layerMask);
        }


        /// <summary>
        /// プレビュー用メッシュデータの描画
        /// </summary>
        private void DrawFieldOfView()
        {
            //　角度分
            int angleQuantify = Mathf.RoundToInt(viewAngle);
            List<Vector3> points = new List<Vector3>();
            for (int i = 0; i <= angleQuantify; i++)
            {
                //　角度
                float angle = transform.eulerAngles.y - viewAngle / 2 + i;
                Vector3 dir = DirFromAngle(angle, true);
                // 障害物がある場合はそこまで
                var point = Physics.Raycast(transform.position, dir, out var hit, viewRadius, obstacleMask)
                    ? hit.point
                    : transform.position + dir * viewRadius;

                points.Add(point);
            }

            // ポイントの描画
            CreateViewMesh(points);
        }

        /// <summary>
        /// プレビュー用メッシュデータの作成
        /// </summary>
        /// <param name="viewPoints"></param>
        public void CreateViewMesh(List<Vector3> viewPoints)
        {
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

            _viewMesh.Clear();

            _viewMesh.vertices = vertices;
            _viewMesh.triangles = triangles;
            _viewMesh.RecalculateNormals();
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
         
    }
}