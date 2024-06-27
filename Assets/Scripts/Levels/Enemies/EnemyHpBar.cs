using UnityEngine;
using UnityEngine.UI;

namespace Levels.Enemies
{
    public class EnemyHpBar : MonoBehaviour
    {
        public Image Filling;

        public void SetHp(float currentHp, float maxHp) => 
            Filling.fillAmount = currentHp / maxHp;
    }
}