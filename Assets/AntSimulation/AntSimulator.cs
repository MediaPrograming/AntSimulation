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

        /// <summary>
        /// アリ登録用関数
        /// </summary>
        public void Add(Ant ant)
        {
            if(!_ants.Contains(ant)) _ants.Add(ant);
            StartCoroutine(Discharge());
        }
        
        private void Update()
        {
          
        }

        IEnumerator Discharge()
        {
            yield return new WaitForSeconds(5);

            foreach (var ant in _ants)
            {
                // フェロモンの排出
                _ = ant.DischargePheromones(pheromones);
            }

            StartCoroutine(Discharge());
        }
    }
}