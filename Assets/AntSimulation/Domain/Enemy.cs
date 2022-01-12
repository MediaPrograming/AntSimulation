using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntSimulation
{


   public class Enemy : MonoBehaviour
   {
      [SerializeField] private int atk = 2;
      public int Atk => atk;
   }
}
