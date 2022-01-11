using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class VimiiAntAI : Ant
    {
        // Start is called before the first frame update
        // Update is called once per frame
        //進みたい向きのスケール(使わなそう)
        [SerializeField] float speedscale = 0.01f;

        //ランダムに動く範囲のスケール
        [SerializeField, Range(0.0f, 0.5f)] float randomwidth = 0.01f;

        //ランダムに動く大きさのスケール
        [SerializeField] float randomscale = 0.01f;

        //餌を発見したときの寿命長めのフェロモン
        [SerializeField] private GameObject feedpheromones;
        
        //フェロモン大放出期間
        private int PheromonesTime = 0;

        private bool b_onFindFeed = false;
        
        private void Update()
        {
            if (feed)
            {
                this.feed.transform.position = this.transform.position + new Vector3(0, 0.02f, 0);
            }

            if (PheromonesTime > 0)
            {
                PheromonesTime -= 1;
                if(HasFeed && PheromonesTime % 10 == 0) this.DischargePheromones(feedpheromones);
            }
        }

        /// <summary>
        /// フェロモンを見つけたとき行きたい方向を決定する。
        /// </summary>
        public override void OnFindPheromones(Transform[] transforms)
        {
            if(b_onFindFeed) return;
            //transform フェロモンの位置の配列
            Transform self = this.transform; //蟻の位置
         
            Vector3 tmp = new Vector3(0, 0, 0);
            
            //tmp+=new Vector3(Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale,Random.Range(-1.0f,1.0f)*randomspeedscale);

            if (transforms.Length != 0)
            {
                foreach (var i in transforms)
                {
                    //フェロモンの重心の相対位置tmp
                    tmp += (i.position - self.position);
                }
                // tmp = (tmp / (transforms.Length + 1)).normalized * speedscale;
                // print(transforms.Length);
                if (!HasFeed)
                {
                    float n = Random.Range(0.5f - randomwidth, randomwidth + 0.5f) * Mathf.PI;
                    tmp += (Mathf.Cos((n)) * self.right + Mathf.Sin(n) * self.forward) * randomscale;
                }
            }
            else
            {
                float k = Random.Range(0.5f - randomwidth, randomwidth + 0.5f) * Mathf.PI;
                tmp += (Mathf.Cos((k)) * self.right + Mathf.Sin(k) * self.forward) * randomscale;
            }

            //print(tmp);
            //緑軸方向成分を消去
            // tmp = Vector3.Scale(tmp, (self.right + self.forward)).normalized;
            this.transform.rotation = Quaternion.LookRotation(tmp, new Vector3(0,1,0));
        }

        public override void OnFindFeed(Transform[] feeds)
        {
            b_onFindFeed = false;
            //餌を持っている場合
            if (feed) return;
            if (feeds.Length == 0) return;
            b_onFindFeed = true;
            int index = 0;
            float distance = Vector3.Distance(this.transform.position, feeds[0].position);
            for (int i = 1; i < feeds.Length; i++)
            {
                if (distance > Vector3.Distance(this.transform.position, feeds[i].position))
                    index = i;
                distance = Vector3.Distance(this.transform.position, feeds[i].position);
            }

            var feedContainer = feeds[index].GetComponent<FeedContainer>();
            if (!feedContainer) return;
        
            Transform self = this.transform; //蟻の位置git 

            if (distance < 1f)
            {
                //餌を発見
                var newFeed = feedContainer.Fetch();
                this.feed = newFeed;
                feed.transform.parent = this.transform;
                
                this.transform.rotation = Quaternion.LookRotation(-self.forward, self.up);
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
                this.DischargePheromones(feedpheromones);
                PheromonesTime += 500;
            }
            else
            {
                //一番近い餌の方向を向く
                this.transform.rotation =
                    Quaternion.LookRotation((feedContainer.transform.position - self.position).normalized, self.up);
            }
        }


        public override void OnFindEnemy(Transform[] enemies)
        {
            if (enemies.Length == 0) return;

            Vector3 direction = new Vector3(0f, 0f,0f);
            foreach (var enemy in enemies)
                direction += this.transform.position - enemy.transform.position;
       
            transform.LookAt(this.transform.position + direction.normalized);
            // ここにSetRotation
        }        
    }
}