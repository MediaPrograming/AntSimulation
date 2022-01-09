using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class FeedContainer : MonoBehaviour
    {
        private Queue<Feed> feeds;

        private void Start()
        {
            // 初期化
            feeds = new Queue<Feed>();

            // 子オブジェクト中のFeedを取得する
            var children = this.GetComponentsInChildren<Feed>();
            foreach (var child in children) feeds.Enqueue(child);
        }

        private void Update()
        {
            if (feeds.Count == 0)
                Destroy(this.gameObject);
        }

        public Feed Fetch()
        {
            return feeds.Dequeue();
        }
    }

}