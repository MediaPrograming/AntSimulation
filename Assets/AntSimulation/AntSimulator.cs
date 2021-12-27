using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{
    public class AntSimulator : MonoBehaviour
    {
        private readonly List<Ant> _ants = new List<Ant>();

        /// <summary>
        /// アリ登録用関数
        /// </summary>
        public void Add(Ant ant)
        {
            if(!_ants.Contains(ant)) _ants.Add(ant);
        }
        
        private void Update()
        {
            foreach (var ant in _ants) ant.Move();
        }
    }
}