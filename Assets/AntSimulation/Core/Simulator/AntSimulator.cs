using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AntSimulation.Base;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace AntSimulation
{
    public class AntSimulator : MonoBehaviour
    {
        [SerializeField] private GameObject Spawner;
        [SerializeField] private GameObject pheromones;

        /// <summary>
        /// 適当にリスト管理
        /// </summary>
        private List<Ant> _ants = new List<Ant>();

        private List<Enemy> _enemies = new List<Enemy>();
        private List<Pheromones> _pheromones = new List<Pheromones>();

        private GameObject _spawner;

        public event Action<GameObject> OnRestart;

        private void Start()
        {
            Restart();
        }


        public void Restart()
        {
            StopCoroutine(Discharge());
            foreach (var p in _pheromones) if (p) Destroy(p.gameObject);
            if (_spawner) Destroy(_spawner);
            _ants = new List<Ant>();
            _enemies = new List<Enemy>();
            _pheromones = new List<Pheromones>();

            _spawner = Instantiate(Spawner);
            OnRestart?.Invoke(_spawner);

            StartCoroutine(Discharge());
        }

        /// <summary>
        /// アリ登録用関数
        /// </summary>
        public void Add(Ant ant)
        {
            Debug.Log(ant.name);
            if (!_ants.Contains(ant))
                _ants.Add(ant);
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
                if (ant.CanWalk == true) ant.stamina -= Time.deltaTime;
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
                if (enemy.CanWalk) enemy.stamina -= Time.deltaTime;

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
            foreach (var ant in _ants)
            {
                // フェロモンの排出
                var p = ant.DischargePheromones(pheromones);
                _pheromones.Add(p.GetComponent<Pheromones>());
                //スタミナ切れの時はHPを消費して回復。
                if (ant.stamina <= 0.0)
                {
                    ant.HP -= 1;
                    ant.stamina += 3.0;
                }

                ant.CanWalk = true;
                if (ant.HP <= 0) removeList.Add(ant);
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

    [CustomEditor(typeof(AntSimulator))]
    public class AntSimulatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Restart"))
            {
                var s = target as AntSimulator;
                if (s != null) s.Restart();
            }
        }
    }
}