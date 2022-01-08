using UnityEditor;
using UnityEngine;

namespace AntSimulation
{
    /// <summary>
    /// アリのプレハブにつける
    /// </summary>
    public abstract class Ant : MonoBehaviour
    {
        public GUID Id { get; } = new GUID();
        public Vector3 Direction { get; private set; }

        public int HP = 50;
        public Feed feed;
        public bool HasFeed => feed != null;
        [SerializeField] private TargetSearcher pheromonesSearcher;
        [SerializeField] private TargetSearcher feedSearcher;
        private void Start()
        {
            pheromonesSearcher.OnFindTargets += OnFindPheromones;
            feedSearcher.OnFindTargets += OnFindFeed;
        }



        /// <summary>
        /// フェロモンが視界に入った時
        /// </summary>
        /// <param name="ant"></param>
        /// <param name="transforms"></param>
        public abstract void OnFindPheromones(Transform[] transforms);
        public abstract void OnFindFeed(Transform[] feeds);


  

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
    }
}