using System;
using System.Collections;
using UnityEngine;

namespace AntSimulation.Base
{
    public abstract class Generator<T> : MonoBehaviour
    {
        [SerializeField] protected GameObject prefab;
        [SerializeField] protected int canSpawn;
        [SerializeField] protected float spawnRate;
        public event Action<T> OnGenerate;
        public void Start()
        {
            StartCoroutine(Spawn());
        }

        /// <summary>
        /// 生成用関数
        /// </summary>
        private IEnumerator Spawn()
        {
            if (canSpawn > 0)
            {
                var t = GameObject.Instantiate(prefab).GetComponent<T>();
                canSpawn -= 1;
                
                //イベントのコール
                OnGenerate?.Invoke(t);
            }

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(Spawn());
        }

    }
}