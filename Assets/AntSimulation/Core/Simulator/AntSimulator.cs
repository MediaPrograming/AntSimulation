using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AntSimulation.Base;
using UnityEngine;
using Random = System.Random;

namespace AntSimulation
{
    public class AntSimulator : MonoBehaviour
    {
        [SerializeField] private GameObject pheromones;
        
        /// <summary>
        /// 適当にリスト管理
        /// </summary>
        public List<Ant> _ants = new List<Ant>();
        private readonly List<Enemy> _enemies = new List<Enemy>();
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
        /// <summary>
        /// 敵登録用関数
        /// </summary>
        public void Add(Enemy enemy)
        {
            if (!_enemies.Contains(enemy)) _enemies.Add(enemy);
        }
        
        private void Update()
        {
            
            ////////////////////////////////////
            // 蟻
            /////////////////////////////////////
            foreach (var ant in _ants)
            {
                //スタミナ減少
                if(ant.CanWalk == true) ant.stamina -= Time.deltaTime;
                //尽きたら動けない
                if (ant.stamina <= 0.0)
                {
                    ant.stamina = 0.0;
                    ant.CanWalk = false;
                }
            }

            ////////////////////////////////////
            // 敵
            // 敵性オブジェクトも蟻と同じ実装でいっかな
            /////////////////////////////////////
         
            List<Enemy> removableList = new List<Enemy>(); 
            foreach (var enemy in _enemies)
            {
                if(enemy.CanWalk) enemy.stamina -= Time.deltaTime;
                
                if (enemy.stamina <= 0)
                {
                    enemy.stamina = 0;
                    enemy.CanWalk = false;
                    removableList.Add(enemy);
                }
            }
            
            foreach (var enemy in removableList)
            {
                _enemies.Remove(enemy);
                //削除
                Destroy(enemy);
            }
        }

        IEnumerator Discharge()
        {
            List<Ant> removeList = new List<Ant>();
            int count = 0;
            foreach (var ant in _ants.OrderBy(ant=> ant.responseThreshold))
            {
                // フェロモンの排出
                _ = ant.DischargePheromones(pheromones);
                //スタミナ切れの時はHPを消費して回復。
                if (ant.stamina <= 0.0)
                {
                    ant.HP -= 1;
                    ant.stamina += 3.0;
                }
                
                //働きアリの法則
                if(count < _ants.Count / 5) ant.CanWalk = true;
                else if (count < _ants.Count * 4 / 5) ant.CanWalk = (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f);
                else ant.CanWalk = false;
                count++;

                //社畜モード
                // ant.CanWalk = true;
                
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