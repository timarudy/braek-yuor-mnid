using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HpBar : MonoBehaviour
    {
        public Image Filling;
        public TextMeshProUGUI HpAmount;

        public void SetHp(float currentHp, float maxHp)
        {
            HpAmount.text = ((int)currentHp).ToString();
            Filling.fillAmount = currentHp / maxHp;
        }
    }
}