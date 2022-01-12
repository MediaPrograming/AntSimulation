using UnityEngine;

namespace AntSimulation
{
    public class AntController : MonoBehaviour
    {
        [SerializeField] AntSimulator antSimulator;
        [SerializeField] AntSpawner antSpawner;
        void Awake()
        {
            antSpawner.OnGeneateEvent += antSimulator.Add;
        }
    }
}