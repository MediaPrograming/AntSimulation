using System;
using System.Collections;
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
        [SerializeField] private FeedContainer feedContainer;
        private StreamWriter sw;

        void Start()
        {
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

            feedContainer.OnFeedChangeEvent += count =>
                SaveData(simulator.PhaseCount.ToString(), Time.time.ToString(), count.ToString());

            StartCoroutine(Save());
        }

        private void Update()
        {
        }

        public void WriteHeader()
        {
            string[] s1 = { "Phase", "Time", "Count" };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
        }

        IEnumerator Save()
        {
            if (simulator.IsRecording)
                SaveData(simulator.PhaseCount.ToString(), Time.time.ToString(), feedContainer.Count.ToString());

            //写真を撮るタイミングに合わせる
            yield return new WaitForSeconds(simulator.CaptureInterval);
            StartCoroutine(Save());
        }

        public void SaveData(string txt1, string txt2, string txt3)
        {
            string[] s1 = { txt1, txt2, txt3 };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
        }

        private void OnApplicationQuit()
        {
            sw.Close();
        }
    }
}