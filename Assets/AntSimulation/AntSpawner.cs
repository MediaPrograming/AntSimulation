using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AntSimulation
{
    public class AntSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject antPrefab;
        public event Action<Ant> OnGenerate;


        [SerializeField] private float spawnRate = 3;
        private void Start()
        {
            StartCoroutine(Spawn());
        }

        /// <summary>
        /// アリ生成用関数
        /// </summary>
        private IEnumerator Spawn()
        {
            var newAnt = GameObject.Instantiate(antPrefab).GetComponent<Ant>();
            newAnt.transform.position = this.transform.position + new Vector3(Random.Range(-1.0f,1.0f),0,Random.Range(-1.0f,1.0f));
            newAnt.transform.Rotate(0,Random.Range(-10.0f,10.0f),0);
            OnGenerate?.Invoke(newAnt);

            yield return new WaitForSeconds(spawnRate);
            StartCoroutine(Spawn());
        }
    }
}