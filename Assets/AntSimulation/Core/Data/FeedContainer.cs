using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class FeedContainer : MonoBehaviour
    {
        private Queue<Feed> feeds;

        public event Action OnDestroyEvent;

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
            {
                Destroy(this.gameObject);
            }
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }

        public float Count => feeds.Count;

        public bool IsEmpty => !(feeds != null && feeds.Count > 0);

        public Feed Fetch()
        {
            return feeds.Dequeue();
        }
    }
}