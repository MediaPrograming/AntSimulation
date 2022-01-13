using System;
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
    [SerializeField] private AntSpawner spawner;
    
    void Start()
    {
        var path = @"Analysis/Analysis" + "_" +
                   DateTime.Now.Hour.ToString() + "_" +
                   DateTime.Now.Minute.ToString() +
                   ".csv";
        sw = new StreamWriter(path, true, Encoding.GetEncoding("Shift_JIS"));
        string[] s1 = { "Working", "Lazy","GatheredFeeds", "time" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        StartCoroutine(Discharge());
    }

    public void SaveData(string txt1, string txt2, string txt3,string txt4)
    {
        string[] s1 = { txt1, txt2, txt3 ,txt4};
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
            spawner.GatheredFeeds.ToString(),
            Time.time.ToString());

        yield return new WaitForSeconds(2);
        StartCoroutine(Discharge());
    }
}