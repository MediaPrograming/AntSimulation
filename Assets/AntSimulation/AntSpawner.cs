using System;
using System.Threading.Tasks;
using UnityEngine;

namespace AntSimulation
{
    public class AntSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject antPrefab;
        public event Action<Ant> OnGenerate;
        
        async void Update()
        {
            // 2秒に一回生成
            Spawn();
            await Task.Delay(2000);
        }
        
        /// <summary>
        /// アリ生成用関数
        /// </summary>
        void Spawn()
        {
            var newAnt = GameObject.Instantiate(antPrefab, this.transform).GetComponent<Ant>();
            OnGenerate?.Invoke(newAnt);
        }
    }
}