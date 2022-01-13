using System;
using System.Security.Cryptography;
using UnityEngine;

namespace AntSimulation
{
    public class AntController : MonoBehaviour
    {
        [SerializeField] private AntSimulator antSimulator;
        private AntSpawner antSpawner;
        private EnemyGenerator enemyGenerator;
        void Awake()
        {
            antSimulator.OnRestart += OnRestart;
        }

        void OnRestart(GameObject g)
        {

            if(antSpawner) antSpawner.OnGenerateEvent -= antSimulator.Add;
            if(enemyGenerator) enemyGenerator.OnGenerateEvent -= antSimulator.Add;
            var spawner = g.GetComponentInChildren<AntSpawner>();
            var enemyGen = g.GetComponentInChildren<EnemyGenerator>();
            spawner.OnGenerateEvent += antSimulator.Add;
            enemyGen.OnGenerateEvent += antSimulator.Add;
            antSpawner = spawner;
            enemyGenerator = enemyGen;
        }
    }
}