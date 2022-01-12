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
    
        public void Start()
        {
            StartCoroutine(Spawn());
        }

        protected abstract void OnGenerate(T t);
            
        
        /// <summary>
        /// 生成用関数
        /// デフォルトでスポナー位置にスポーンする
        /// </summary>
        private IEnumerator Spawn()
        {
            if (canSpawn > 0)
            {
                var t = GameObject.Instantiate(prefab, this.transform.position, Quaternion.identity).GetComponent<T>();
                canSpawn -= 1;
                
                //イベントのコール
                OnGenerate(t);
            }

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(Spawn());
        }

        public static Vector3 CreateRandomXZ(float radius)
        {
            var x = UnityEngine.Random.Range(-radius, radius);
            var z = UnityEngine.Random.Range(-radius, radius);
            return new Vector3(x, 0, z);
        }
    }
}