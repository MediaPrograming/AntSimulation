using System;
using System.Security.Cryptography;
using UnityEngine;

namespace AntSimulation.Base
{
    /// <summary>
    /// アリのプレハブにつける
    /// </summary>
    public abstract class Ant : MonoBehaviour, IFreeAgentItem
    {
        public bool CanWalk { get; set; } = true;
        public int HP = 20;
        public double stamina = 100.0;
        public Feed feed { get; set; }
        public bool HasFeed => feed != null;
        [SerializeField] private TargetSearcher pheromonesSearcher;
        [SerializeField] private TargetSearcher feedSearcher;
        [SerializeField] private TargetSearcher enemySearcher;
        
        private void Start()
        {
            pheromonesSearcher.OnFindTargets += OnFindPheromones;
            feedSearcher.OnFindTargets += OnFindFeed;
            enemySearcher.OnFindTargets += OnFindEnemy;
        }


        /// <summary>
        /// フェロモンが視界に入った時
        /// </summary>
        /// <param name="ant"></param>
        /// <param name="transforms"></param>
        public abstract void OnFindPheromones(Transform[] transforms);

        public abstract void OnFindFeed(Transform[] feeds);
        public abstract void OnFindEnemy(Transform[] enemies);


        /// <summary>
        /// フェロモンの放出
        /// </summary>
        public GameObject DischargePheromones(GameObject pheromones)
        {
            var pos = this.transform.position;
            var go = Instantiate(pheromones);
            go.transform.position = pos;
            return go;
        }


        private void OnDestroy()
        {
            if (feed)
            {
                Destroy(feed);
            }
        }
    }
}