using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    public class AntSimulator : MonoBehaviour
    {
        [SerializeField] private GameObject pheromones;
        private readonly List<Ant> _ants = new List<Ant>();

        private void Start()
        {
            StartCoroutine(Discharge());
        }

        /// <summary>
        /// アリ登録用関数
        /// </summary>
        public void Add(Ant ant)
        {
            if (!_ants.Contains(ant)) _ants.Add(ant);
        }

        private void Update()
        {
        }

        IEnumerator Discharge()
        {
            foreach (var ant in _ants)
            {
                // フェロモンの排出
                _ = ant.DischargePheromones(pheromones);
            }

            yield return new WaitForSeconds(5);
            StartCoroutine(Discharge());
        }
    }
}