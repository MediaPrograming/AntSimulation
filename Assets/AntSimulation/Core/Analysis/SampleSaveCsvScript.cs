using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AntSimulation;
using AntSimulation.Base;
using UnityEngine;

public class SampleSaveCsvScript : MonoBehaviour
{
    private StreamWriter sw;

    [SerializeField] private AntSimulator simulator;
    
    void Start()
    {
        sw = new StreamWriter(@"SaveData.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "Working", "Lazy", "time" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        StartCoroutine(Discharge());
    }

    public void SaveData(string txt1, string txt2, string txt3)
    {
        string[] s1 = { txt1, txt2, txt3 };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sw.Close();
        }
    }

    IEnumerator Discharge()
    {
        SaveData(simulator._ants.Where(ant => ant.CanWalk).Count().ToString(),
            simulator._ants.Where(ant => !ant.CanWalk).Count().ToString(),
            Time.time.ToString());

        yield return new WaitForSeconds(2);
        StartCoroutine(Discharge());
    }
}