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
        public event Action<Ant> OnGenerate;

        public int canSpawn = 100;

        [SerializeField] private float spawnerRadius = 0.5f;
        
        [SerializeField] private float spawnRate = 3;

        [SerializeField] private float viewRadius = 1f;
        [SerializeField] private LayerMask antLayerMask;
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
                feed.transform.parent = this.transform;
                ant.feed = null;
                ant.HP = 50;
                canSpawn++;
            }
        }


        /// <summary>
        /// アリ生成用関数
        /// </summary>
        private IEnumerator Spawn()
        {
            if (canSpawn > 0)
            {
                var newAnt = GameObject.Instantiate(antPrefab).GetComponent<Ant>();
                newAnt.transform.position = this.transform.position +
                                            new Vector3(Random.Range(-spawnerRadius, spawnerRadius), 0, Random.Range(-spawnerRadius, spawnerRadius));
                newAnt.transform.Rotate(0, Random.Range(-10.0f, 10.0f), 0);
                OnGenerate?.Invoke(newAnt);
                canSpawn -= 1;
            }

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(Spawn());
        }
    }
}