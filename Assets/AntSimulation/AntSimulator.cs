using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<Ant> removeList = new List<Ant>();
            foreach (var ant in _ants)
            {
                // フェロモンの排出
                _ = ant.DischargePheromones(pheromones);
                ant.HP -= 1;
                if(ant.HP <= 0) removeList.Add(ant);
            }
            foreach (var ant in removeList)
            {
                _ants.Remove(ant);
                Destroy(ant.gameObject);
            }

            yield return new WaitForSeconds(10);
            StartCoroutine(Discharge());
        }
    }
}