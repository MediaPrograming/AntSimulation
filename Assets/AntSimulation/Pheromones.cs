using UnityEngine;

namespace AntSimulation
{
    public class Pheromones : MonoBehaviour
    {
        // 最大寿命 (s)
        [SerializeField] private float lifeTimeMax;
        private float lifeTime;

        // 公開用プロパティ
        public float LifeTimeMax => lifeTimeMax;
        public float LifeTime => lifeTime;

        private void Start()
        {
            lifeTime = lifeTimeMax;
        }


        private void Update()
        {
            // 単純に経過時間を減算
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
            {
                // 寿命が0以下で削除
                Destroy(this.gameObject);
            }
        }
        
        /// <summary>
        /// 残りの寿命を0～1の割合で返す
        /// </summary>
        /// <returns></returns>
        public float CalculateLifeStrength()
        {
            var strength = Mathf.Clamp(lifeTime, 0, lifeTimeMax) / lifeTimeMax;
            return strength;
        }
    }
}