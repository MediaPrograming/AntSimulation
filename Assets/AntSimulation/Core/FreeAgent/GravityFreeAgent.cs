using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class GravityFreeAgent : MonoBehaviour
    {
        public float movableDist = 1.0f;
        // public float rayOffset = 0.02f;

        [SerializeField] Transform CenterOfBalance; // 重心
        [SerializeField] private MonoBehaviour freeAgent;
        [SerializeField] private LayerMask groundLayerMask;

        private IFreeAgentItem _freeAgent;

        private void Start()
        {
            var freeAgentItem = freeAgent.GetComponent<IFreeAgentItem>();
            if (freeAgentItem == null)
            {
                Debug.LogError("オブジェクトをアタッチするか、インタフェースを実装しているか確認してください");
            }
            _freeAgent = freeAgentItem;
        }

        void FixedUpdate()
        {
            // null check
            if(_freeAgent == null || !_freeAgent.CanWalk) return;
            
            if (this.transform.position.y < 0) this.transform.position += this.transform.up * 0.01f;
            Ray ray = new Ray(CenterOfBalance.position + transform.up * 0.05f - transform.forward * 0.01f,
                -transform.up + transform.forward);

            RaycastHit hit;

            // Transformの少し前方の地形を調べる
            if (Physics.Raycast(
                    ray,
                    out hit,
                    movableDist, groundLayerMask))
            {
                //傾斜があったら行かない。
                if (Vector3.Dot(hit.normal, Vector3.up) < 0.99)
                {
                    this.transform.position += hit.normal * 0.1f;
                    this.transform.rotation = Quaternion.LookRotation(new Vector3(0, Random.Range(-10.0f, 10.0f), 0),
                        new Vector3(0, 1, 0));
                    return;
                }

                // 傾きの差を求める
                // Quaternion q = Quaternion.FromToRotation(
                //     transform.up,
                //     hit.normal);
                // transform.rotation *= q;


                transform.position = hit.point + (transform.position - CenterOfBalance.position);

                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1, true);

                return;
            }
            //
            // int rayNum = 4;
            // Ray[] rays = new Ray[rayNum];
            // rays[0] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward);
            // rays[1] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.forward - transform.up);
            // rays[2] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.up);
            // rays[3] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, -transform.forward-transform.up);
            // // rays[4] = new Ray(CenterOfBalance.position+transform.forward*rayOffset, transform.up);
            //
            // RaycastHit[] hitArray = new RaycastHit[rayNum];
            //
            // float minDist = float.PositiveInfinity;
            // int minIndex = -1;
            // for (int i = 0; i < rayNum; i++)
            // {
            //     Physics.Raycast(rays[i], out hitArray[i], float.PositiveInfinity);
            //     if (hitArray[i].distance > 0 && minDist > hitArray[i].distance)
            //     {
            //         minDist = hitArray[i].distance;
            //         minIndex = i;
            //     }
            //     Debug.DrawRay(rays[i].origin,rays[i].direction * 100,Color.blue,1,true);
            // }
            //
            // //近くに地形があったら
            // if (minIndex != -1 && minDist < movableDist)
            // {
            //     Quaternion q = Quaternion.FromToRotation(
            //         transform.up,
            //         hitArray[minIndex].normal);
            //
            //     transform.rotation *= q;
            //     
            //     Debug.Log("minDist="+minDist);
            //
            //     transform.position = hitArray[minIndex].point + (transform.position - CenterOfBalance.position);
            //     
            //     Debug.DrawRay(rays[minIndex].origin,rays[minIndex].direction * 100, Color.yellow, 1,true);
            // }
            // else
            // {
            //     transform.Rotate(10,10,10);
            //     transform.position = transform.position +new Vector3(0,0.01f,0);
            // }
        }
    }
}