using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    public class FeedContainer : MonoBehaviour
    {
        [SerializeField] private int count;
        public Queue<Feed> feeds;

        private void Start()
        {
            feeds = new Queue<Feed>();
            for (int i = 0; i < count; i++)
            {
                feeds.Enqueue(new Feed());
            }
        }

        private void Update()
        {
            if (feeds.Count == 0)
            {
                Destroy(this.gameObject);
            }
        }

        public Feed Fetch()
        {
            return feeds.Dequeue();
        }
    }

    /// <summary>
    /// 餌
    /// </summary>
    public class Feed
    {
        
    }
}