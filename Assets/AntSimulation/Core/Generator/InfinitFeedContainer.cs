using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class InfinitFeedContainer : MonoBehaviour
    {
        private Queue<Feed> feeds;
        [SerializeField] private Feed food;
        private void Start()
        {
            // 初期化
            feeds = new Queue<Feed>();

            // 子オブジェクト中を取得する
        }

        private void Update()
        {
            if (feeds.Count == 0)
                Destroy(this.gameObject);
        }
        public bool IsEmpty => !(feeds != null && feeds.Count > 0);

        public Feed Fetch()
        {
            var obj=(Feed)Instantiate(food,this.transform.position,Quaternion.identity);
            obj.transform.parent=this.transform;
            feeds.Enqueue(obj);
            return feeds.Dequeue();
        }
    }

}