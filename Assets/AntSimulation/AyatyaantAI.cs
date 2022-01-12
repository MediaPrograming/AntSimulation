using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class AyatyaantAI : Ant
    {
        // Start is called before the first frame update
        // Update is called once per frame
        //進みたい向きのスケール(使わなそう)
        [SerializeField] float speedscale = 0.01f;

        //ランダムに動く範囲のスケール
        [SerializeField, Range(0.0f, 0.5f)] float randomwidth = 0.01f;
        
        //ランダムに動く大きさのスケール
        [SerializeField] float randomscale = 0.01f;
        //餌を発見したときのランダムさ
       
        //餌を発見したときの寿命長めのフェロモン
        [SerializeField] private GameObject feedpheromones;
        private Vector3 rotated =new Vector3(0,0,0);

        
        private void start(){
            this.DischargePheromones(feedpheromones);
        }
        private void Update()
        {
            if (feed)
            {
                this.feed.transform.position = this.transform.position + new Vector3(0, 0.02f, 0);
            }
           
        }

        /// <summary>
        /// フェロモンを見つけたとき行きたい方向を決定する。
        /// </summary>
        public override void OnFindPheromones(Transform[] transforms)
        {
            //transform フェロモンの位置の配列
            Transform self = this.transform; //蟻の位置

            Vector3 tmp = new Vector3(0, 0, 0);
            if(feed&&Random.value<0.5){
                this.DischargePheromones(feedpheromones);
            }
            if(Random.value<0.25){
             this.DischargePheromones(feedpheromones);
            
            } 
            foreach (var i in transforms)
            {
                //フェロモンの重心の相対位置tmp
                tmp += (i.position - self.position);
                /*new Vector3(i.position.x-self.position.x,
                i.position.y-self.position.y,
                i.position.z-self.position.z);*/
            }

            //tmp+=new Vector3(Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale);
            float n;
             //改良餌を持っているとき
             if(feed){
                n = Random.Range(0.5f - randomwidth*.1f, randomwidth*.1f + 0.5f) * Mathf.PI;
                tmp += (Mathf.Cos(n) * self.right + Mathf.Sin(n) * self.forward) * randomscale*.1f;
            
             }else{
                n = Random.Range(0.5f - randomwidth, randomwidth + 0.5f) * Mathf.PI;      
                tmp += (Mathf.Cos(n) * self.right + Mathf.Sin(n) * self.forward) * randomscale;
             }
                //
            if (transforms.Length != 0)
            {
                tmp = (tmp / (transforms.Length + 1)).normalized * speedscale;
                //print(transforms.Length);
            }
            rotated+=tmp;
            //print(tmp);
            //緑軸方向成分を消去
            //tmp = Vector3.Scale(tmp, (self.right + self.forward));
            this.transform.rotation = Quaternion.LookRotation(tmp, Vector3.up);
            //this.transform.rotation *= Quaternion.Euler(tmp);
        }

        public override void OnFindFeed(Transform[] feeds)
        {
            //餌を持っている場合
            if (feed) return;
            if (feeds.Length == 0) return;
            int index = 0;
            float distance = Vector3.Distance(this.transform.position, feeds[0].position);
            for (int i = 1; i < feeds.Length; i++)
            {
                if (distance > Vector3.Distance(this.transform.position, feeds[i].position))
                    index = i;
                distance = Vector3.Distance(this.transform.position, feeds[i].position);
            }

            var feedContainer = feeds[index].GetComponent<InfinitFeedContainer>();
            if (!feedContainer) return;
        
            Transform self = this.transform; //蟻の位置git 


            if (distance < 1f)
            {
                //餌を発見
                var newFeed = feedContainer.Fetch();
                this.feed = newFeed;
                feed.transform.parent = this.transform; 
                //デフォルト
                this.transform.rotation = Quaternion.LookRotation(-self.forward, self.up);
                //
               
                //砂漠蟻
                //this.transform.rotation = Quaternion.LookRotation(-rotated, self.up);
                //
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
            }
            else
            {
                //一番近い餌の方向を向く
                var direction = (feedContainer.transform.position - self.position).normalized;
                transform.LookAt( this.transform.position + direction);
                
            }
        }

        public override void OnFindEnemy(Transform[] enemies)
        {
            if(enemies.Length == 0 ) return;
          
            
            Vector3 direction = new Vector3(0f, 0f,0f);
            foreach (var enemy in enemies)
                direction += this.transform.position - enemy.transform.position;
       
            transform.LookAt(this.transform.position + direction.normalized);
            // ここにSetRotation
        }
  
    }
}