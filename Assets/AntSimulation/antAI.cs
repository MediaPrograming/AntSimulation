using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation{
    public class antAI : MonoBehaviour
    {
    // Start is called before the first frame update
    // Update is called once per frame
        //進みたい向きのスケール(使わなそう)
        [SerializeField] float speedscale=0.01f;
        //ランダムに動く範囲のスケール
        [SerializeField,Range(0.0f,0.5f)] float randomwidth=0.01f;
        //ランダムに動く大きさのスケール
        [SerializeField] float randomscale=0.01f;
        
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
                 //フェロモンの重心の相対位置tmp
                 tmp+=(i.position-self.position);
                 /*new Vector3(i.position.x-self.position.x,
                 i.position.y-self.position.y,
                 i.position.z-self.position.z);*/
             }
            //tmp+=new Vector3(Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale);
             float n=Random.Range(0.5f-randomwidth,randomwidth+0.5f)*Mathf.PI;
             tmp+=(Mathf.Cos((n))*self.right+Mathf.Sin(n)*self.forward)*randomscale;
             if (transforms.Length!=0){
                 tmp=(tmp/(transforms.Length+1)).normalized*speedscale;
                 print(transforms.Length);
            }
            //print(tmp);
            //緑軸方向成分を消去
            tmp=Vector3.Scale(tmp,(self.right+self.forward)).normalized;
            this.transform.rotation=Quaternion.LookRotation(tmp,self.up);
        }
    

 } 
}
