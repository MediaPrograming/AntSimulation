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

        public bool hasFeed;
        [SerializeField] private TargetSearcher targetSearcher;

        private void Start()
        {
            targetSearcher.OnFindTargets += Find;
        }

        /// <summary>
        /// フェロモンが視界に入った時
        /// </summary>
        /// <param name="ant"></param>
        /// <param name="transforms"></param>
        public abstract void Find(Transform[] transforms);

        public abstract void Move();


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