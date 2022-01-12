using System;
using AntSimulation.Base;
using UnityEngine;

namespace AntSimulation
{
    public class EnemyGenerator : Generator<Enemy>
    {
        public event Action<Enemy> OnGenerateEvent;
        [SerializeField] private float spawnRadius;
        [SerializeField] private float y;

        protected override void OnGenerate(Enemy t)
        {
            t.transform.position = this.transform.position + CreateRandomXZ(spawnRadius) + new Vector3(0, y, 0);
            OnGenerateEvent?.Invoke(t);
        }
    }
}