using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antdeciding : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame

        [SerializeField] private Ant ant;

        void Start()
        {
            ant.OnFindTargets += OnFind;
        }

        /// <summary>
        /// フェロモンを見つけたとき
        /// </summary>
        void OnFind(Transform[] transforms)
        {
            
        }
   
}
