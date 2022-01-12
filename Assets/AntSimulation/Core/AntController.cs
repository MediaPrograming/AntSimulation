using UnityEngine;

namespace AntSimulation
{
    public class AntController : MonoBehaviour
    {
        [SerializeField] private AntSimulator antSimulator;
        [SerializeField] private AntSpawner antSpawner;
        [SerializeField] private EnemyGenerator enemyGenerator;
        void Awake()
        {
            antSpawner.OnGenerateEvent += antSimulator.Add;
            enemyGenerator.OnGenerateEvent += antSimulator.Add;
        }
    }
}