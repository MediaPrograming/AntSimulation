using System;
using AntSimulation.Base;
using UnityEngine;
using Random = UnityEngine.Random;


namespace AntSimulation
{
    public class FeedContainerGenerator : Generator<FeedContainer>
    {
        [SerializeField] private float spawnerRadius;
        
        protected override void OnGenerate(FeedContainer t)
        {
            //AntSpawnerと同様
            t.transform.position = this.transform.position + CreateRandomXZ(spawnerRadius);
            t.OnDestroyEvent += () => this.canSpawn++;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(255, 255f, 255f, 0.1f);
            Gizmos.DrawSphere(this.transform.position, spawnerRadius);
        }
    }
}