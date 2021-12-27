using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation{
    public class antdeciding : MonoBehaviour
    {
    // Start is called before the first frame update
    // Update is called once per frame

        [SerializeField] private TargetSearcher targetSearcher;

        void Start()
        {//OnFindTargets イベント
            targetSearcher.OnFindTargets += OnFind;

        }

        /// <summary>
        /// フェロモンを見つけたとき行きたい方向を決定する。
        /// </summary>
        void OnFind(Transform[] transforms)
        {   //transform フェロモンの位置の配列
             Transform self=this.transform;//蟻の位置
             Vector3 tmp =new Vector3(0,0,0);
             foreach(var i in transforms){
                 tmp+=new Vector3(i.position.x-self.position.x,
                 i.position.y-self.position.y,
                 i.position.z-self.position.z);
             }
            tmp+=new Vector3(Random.value,Random.value,Random.value);
             if (transforms.Length!=0){
                 tmp=(tmp/transforms.Length).normalized;
            }
            print(tmp);
        }
 } 
}
