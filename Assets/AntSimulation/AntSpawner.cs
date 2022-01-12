using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class AntSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject antPrefab;
        //蟻１がスポーンする割合
        [SerializeField] private float rate1=.3f;
        [SerializeField] private GameObject antPrefab2;
        //蟻２がスポーンする割合
        [SerializeField] private float rate2=.3f;
        [SerializeField] private GameObject antPrefab3;
        //蟻３がスポーンする割合
        [SerializeField] private float rate3=.3f;
        public event Action<Ant> OnGenerate;

        public int canSpawn = 100;
        private int feedcount=0; 
       
        
        [SerializeField] private float spawnerRadius = 0.5f;
        
        [SerializeField] private float spawnRate = 3;

        [SerializeField] private float viewRadius = 1f;
        [SerializeField] private LayerMask antLayerMask;
        [SerializeField] private Material _spawnerFeed;
        private void Start()
        {
            StartCoroutine(Spawn());
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
        
        
        /// <summary>
        /// ターゲットのリストの更新
        /// </summary>
        void FindVisibleTargets()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            var ants = Physics.OverlapSphere(transform.position, viewRadius, antLayerMask);


            foreach (var ant in ants.Select(x => x.GetComponent<Ant>())
                         .Where(x => x.HasFeed))
            {
                // 蟻の餌を回収
                var feed = ant.feed;
                feedcount++;
                feed.transform.parent = this.transform;
                feed.GetComponent<MeshRenderer>().material = _spawnerFeed;
                //運ばれた餌の数が10を超えたら餌を消すようにした。重いから
                if(feedcount>10){
                 GameObject.Destroy(this.transform.GetChild(this.transform.childCount-1).gameObject); 
                }
                ant.feed = null;
                ant.HP += 20;
                ant.transform.rotation = Quaternion.LookRotation(-ant.transform.forward, ant.transform.up);
               
                canSpawn++;
            }
        }

        
        
        /// <summary>
        /// アリ生成用関数　乱数で3種類の蟻を生成するように変更しましたayatya
        /// </summary>
        private IEnumerator Spawn()
        {
            if (canSpawn > 0)
            {   
                double n=Random.value;
                float r1=rate1/(rate1+rate2+rate3);
                float r2=rate2/(rate1+rate2+rate3);
                Ant newAnt; 
                if(n<r1){
                   newAnt  = GameObject.Instantiate(antPrefab).GetComponent<Ant>();
                }else if(n<r2+r1){
                    newAnt = GameObject.Instantiate(antPrefab2).GetComponent<Ant>();
                }else{
                    newAnt = GameObject.Instantiate(antPrefab3).GetComponent<Ant>();
                }
                newAnt.transform.position = this.transform.position +
                                            new Vector3(Random.Range(-spawnerRadius, spawnerRadius), 0, Random.Range(-spawnerRadius, spawnerRadius));
                newAnt.transform.Rotate(0, Random.Range(-180.0f, 180.0f), 0);
                OnGenerate?.Invoke(newAnt);
                canSpawn -= 1;
            }

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(Spawn());
        }
    }
}