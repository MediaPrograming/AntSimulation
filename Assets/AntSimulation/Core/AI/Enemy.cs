using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using AntSimulation.Base;
using UnityEngine;

namespace AntSimulation
{
    public class Enemy : MonoBehaviour, IFreeAgentItem
    {
        public double stamina = 100.0;

        public float atk = 5f;
        public bool CanWalk { get; set; } = true;

        [SerializeField] private TargetSearcher antSearcher;

        private void Start()
        {
            antSearcher.OnFindTargets += Find;
            StartCoroutine(ChangeDir());
        }

        void Find(Transform[] ants)
        {
            if (ants == null) return;


            // 方向決定
            Vector3 tmp = new Vector3();
            foreach (var ant in ants)
            {
                if(!ant) continue;
                var vec = ant.position - transform.position;

                var distance = Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
                if (distance < 1f)
                {
                    var a = ant.GetComponent<Ant>();
                    if (a) { a.HP -= (int)atk; }
                }

                tmp += vec;
            }

            this.transform.LookAt(this.transform.position + tmp.normalized);
        }

        IEnumerator ChangeDir()
        {
            var y = UnityEngine.Random.Range(0f, 360f);
            this.transform.rotation = Quaternion.Euler(0f, y, 0f);
            yield return new WaitForSeconds(5);
            //StartCoroutine(ChangeDir());
        }
        void OnDestory(){        
            
            for( int i=0; i < this.transform.childCount; ++i ){
            GameObject.Destroy( this.transform.GetChild( i ).gameObject );
            }
            StopCoroutine(ChangeDir());
        }
    }
}