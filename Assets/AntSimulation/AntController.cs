using UnityEngine;

namespace AntSimulation
{
    public class AntController : MonoBehaviour
    {
        [SerializeField] AntSimulator antSimulator;
        [SerializeField] AntSpawner antSpawner;
        void Start()
        {
            antSpawner.OnGenerate += antSimulator.Add;
        }
    }
}