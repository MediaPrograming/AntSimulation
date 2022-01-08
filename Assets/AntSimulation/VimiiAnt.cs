using System.Linq;
using UnityEngine;

namespace AntSimulation
{
    public class VimiiAnt : Ant
    {
        [SerializeField] private LayerMask pheromonesLayer;

        public override void Find(Transform[] transforms)
        {
            var pheromonesObj = transforms.Where(x => ((1 << x.gameObject.layer) & pheromonesLayer) != 0);
            // pheromonesObj.Select(x => x.gameObject.transform.)
        }

        public override void Move()
        {
        }
    }
}