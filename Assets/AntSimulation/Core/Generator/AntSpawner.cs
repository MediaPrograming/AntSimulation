using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntSimulation.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class AntSpawner : Generator<Ant>
    {
        [SerializeField] private float spawnerRadius = 0.5f;
        [SerializeField] private float viewRadius = 1f;
        [SerializeField] private LayerMask antLayerMask;
        [SerializeField] private Material _spawnerFeed;

        private readonly List<Ant> RestAnts = new List<Ant>();

        public event Action<Ant> OnGenerateEvent;

        private new void Start()
        {
            base.Start();
            StartCoroutine(nameof(FindTargetsWithDelay), .2f);
        }

        protected override void OnGenerate(Ant t)
        {
            t.transform.position = this.transform.position + CreateRandomXZ(spawnerRadius);
            t.transform.Rotate(0, Random.Range(-10.0f, 10.0f), 0);
            
            OnGenerateEvent?.Invoke(t);
        }

     

        private void Update()
        {
            List<Ant> removeList = new List<Ant>();
            foreach (var ant in RestAnts)
            {
                ant.stamina += Time.deltaTime;
                if (ant.stamina >= 100.0)
                {
                    ant.HP = 20;
                    ant.CanWalk = true;
                    removeList.Add(ant);
                }
            }

            foreach (var fullAnt in removeList)
                RestAnts.Remove(fullAnt);
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

            foreach (var ant in ants)
            {
                var item = ant.GetComponent<Ant>();
                if (!item) continue;
                RestAnts.Add(item);
                item.CanWalk = false;
                if (item.HasFeed)
                {
                    // 蟻の餌を回収
                    var feed = item.feed;
                    feed.transform.parent = this.transform;
                    feed.GetComponent<MeshRenderer>().material = _spawnerFeed;
                    item.feed = null;
                    canSpawn ++;
                }
            }
        }
    }
}