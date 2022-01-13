using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using AntSimulation.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace AntSimulation
{
    public class AntSimulator : MonoBehaviour
    {
        [SerializeField] private GameObject Spawner;
        [SerializeField] private GameObject pheromones;
        [SerializeField] private bool isRecord;
        private int phaseCount = 0;
    
        [SerializeField] private int interval = 600;
        [SerializeField] private int captureInterval = 30;
        [SerializeField] private Text _text;


        public bool IsRecording => isRecord;
        public int PhaseCount => phaseCount;
        public int Interval => interval;
        public int CaptureInterval => captureInterval;
        /// <summary>
        /// 適当にリスト管理
        /// </summary>

        public List<Ant> _ants = new List<Ant>();

        private List<Enemy> _enemies = new List<Enemy>();


        private GameObject _spawner;
        public event Action<GameObject> OnRestart;
        private void Start()
        {
           Restart();
            StartCoroutine(Capture());
        }

        public IEnumerator Capture()
        {
            if (isRecord)
            {
                CaptureScreenshot();
            }

            yield return new WaitForSeconds(captureInterval);

            StartCoroutine(Capture());
        }

        public void Restart()
        {
            StopCoroutine(Discharge());

            if (_spawner) Destroy(_spawner);
            _ants = new List<Ant>();
            _enemies = new List<Enemy>();
        

            _spawner = Instantiate(Spawner);
            OnRestart?.Invoke(_spawner);

            StartCoroutine(Discharge());

            _text.text = $"Phase {phaseCount.ToString()}";
            phaseCount++;
            
        }

        /// <summary>
        /// アリ登録用関数
        /// </summary>
        public void Add(Ant ant)
        {
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
            if(_ants.Count == 0) return;
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
            if (_ants.Count != 0)
            {
                List<Ant> removeList = new List<Ant>();
                // foreach (var ant1 in _ants.OrderBy(ant=> ant.responseThreshold))
                // {
                //     Debug.Log(ant1.responseThreshold);
                // }
                var max = _ants.Max(ant => ant.responseThreshold);
                var min = _ants.Min(ant => ant.responseThreshold);
                var th02 = (max - min) * 0.2 + min;
                var th08 = (max - min) * 0.8 + min;
                int count = 0;
                foreach (var ant in _ants.OrderBy(ant => ant.responseThreshold))
                {
                    // フェロモンの排出
                    var p = ant.DischargePheromones(pheromones);
                
            
                    //スタミナ切れの時はHPを消費して回復。
                    if (ant.stamina <= 0.0)
                    {
                        ant.HP -= 1;
                        ant.stamina += 3.0;
                    }


                    //働きアリの法則
                    if (count < _ants.Count / 5) ant.CanWalk = true;
                    else if (count < _ants.Count * 4 / 5) ant.CanWalk = (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f);
                    else ant.CanWalk = false;

                    if (ant.HasFeed)
                    {
                        ant.CanWalk = true;
                    }
                    
                    count++;
                    if (ant.HP <= 0) removeList.Add(ant);
                    

                }

                foreach (var ant in removeList)
                {
                    _ants.Remove(ant);
                    Destroy(ant.gameObject);
                }
            }

            yield return new WaitForSeconds(5);
            StartCoroutine(Discharge());
        }

        [MenuItem("Tools/Screenshot %F3", false)]
        public static void CaptureScreenshot()
        {
            string productName = Application.productName;
            if (string.IsNullOrEmpty(productName))
            {
                productName = "Unity";
            }

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                productName);
            DateTime now = DateTime.Now;
            string fileName = string.Format("{0}_{1}x{2}_{3}{4:D2}{5:D2}{6:D2}{7:D2}{8:D2}.png", productName,
                Screen.width,
                Screen.height, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            string path = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            ScreenCapture.CaptureScreenshot(path);
            Debug.LogFormat("Screenshot Save : {0}", path);
        }
    }
    //
    // [CustomEditor(typeof(AntSimulator))]
    // public class AntSimulatorEditor : Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         if (GUILayout.Button("Restart"))
    //         {
    //             var s = target as AntSimulator;
    //             if (s != null) s.Restart();
    //         }
    //     }
    // }
}