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
            foreach (var ant in _ants)
            {
                if(ant.CanWalk == true) ant.stamina -= Time.deltaTime;
                if (ant.stamina <= 0.0)
                {
                    ant.stamina = 0.0;
                    ant.CanWalk = false;
                }
            }
        }

        IEnumerator Discharge()
        {
            List<Ant> removeList = new List<Ant>();
            foreach (var ant in _ants)
            {
                // フェロモンの排出
                _ = ant.DischargePheromones(pheromones);
                //スタミナ切れの時はHPを消費して回復。
                if (ant.stamina <= 0.0)
                {
                    ant.HP -= 1;
                    ant.stamina += 3.0;
                }
                ant.CanWalk = true;
                if(ant.HP <= 0) removeList.Add(ant);
            }
            foreach (var ant in removeList)
            {
                _ants.Remove(ant);
                Destroy(ant.gameObject);
            }

            yield return new WaitForSeconds(5);
            StartCoroutine(Discharge());
        }
    }
}