using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using AntSimulation.Base;
using UnityEngine;

namespace AntSimulation.SimulatorTest
{
    public class SimulatorTest01 : MonoBehaviour
    {
        [SerializeField] private AntSimulator simulator;
        [SerializeField] private GameObject feedContainer;
        private AntSpawner _antSpawner;
        private StreamWriter sw;

        private FeedContainer container;

        private float time;
        private int Count;

        void Start()
        {
            simulator.OnRestart += (g) =>
            {
                if (container)
                    Destroy(container.gameObject);


                time = 0f;
                _antSpawner = g.GetComponentInChildren<AntSpawner>();
                container = Instantiate(feedContainer).GetComponent<FeedContainer>();
                container.OnFeedChangeEvent += count =>
                    SaveData(new[]
                        { simulator.PhaseCount.ToString(), time.ToString(), container.Count.ToString(), Count.ToString() });
            };

            _antSpawner.OnFeedChangeEvent += (count) =>
            {
                Count = count;
                SaveData(new[]
                    { simulator.PhaseCount.ToString(), time.ToString(), container.Count.ToString(), Count.ToString() });
            };

            string productName = Application.productName;
            if (string.IsNullOrEmpty(productName))
            {
                productName = "Unity";
            }

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                productName);
            DateTime now = DateTime.Now;
            string fileName = string.Format("{0}_{1}x{2}_{3}{4:D2}{5:D2}{6:D2}{7:D2}{8:D2}.csv", productName,
                Screen.width,
                Screen.height, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            string path = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            sw = new StreamWriter(path, true, Encoding.GetEncoding("Shift_JIS"));
            WriteHeader();

            StartCoroutine(Save());
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        public void WriteHeader()
        {
            string[] s1 = { "Phase", "Time", "FeedLeftCount", "SpawnerCount" };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
        }

        IEnumerator Save()
        {
            if (simulator.IsRecording)
                SaveData(new[]
                    { simulator.PhaseCount.ToString(), time.ToString(), container.Count.ToString(), Count.ToString() });

            //写真を撮るタイミングに合わせる
            yield return new WaitForSeconds(simulator.CaptureInterval);
            StartCoroutine(Save());
        }

        public void SaveData(string[] txt1)
        {
            //string[] s1 = { txt1, txt2, txt3 };
            string s2 = string.Join(",", txt1);
            sw.WriteLine(s2);
        }

        private void OnApplicationQuit()
        {
            sw.Close();
        }
    }
}