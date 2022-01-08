using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class VimiiAnt : Ant
    {
        [SerializeField] private LayerMask pheromonesLayer;

        public override void OnFindPheromones(Transform[] transforms)
        {
        }

        public override void OnFindFeed(Transform[] feeds)
        {
            int index = 0;
            float distance = Vector3.Distance(this.transform.position, feeds[0].position);
            for (int i = 1; i < feeds.Length; i++)
            {
                if (distance > Vector3.Distance(this.transform.position, feeds[i].position))
                    index = i;
            }

            var feed = feeds[index].GetComponent<FeedContainer>();
            if (!feed) return;

        
            if (distance < 2f)
            {
                //餌を発見
                
            }
            else
            {
                // とりあえず方向セットに関しては適当
                // 後で修正
                SetDirection((feed.transform.position - transform.position).normalized);
            }
        }

        public override void Move()
        {
        }
    }
}