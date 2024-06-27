using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PopUps
{
    public class ToolTip : MonoBehaviour, IVisibility
    {
        public Image Fill;
        
        [SerializeField] private TextMeshProUGUI KeyName;
        
        private bool _workable = true;

        public void Open()
        {
            if (_workable) 
                gameObject.SetActive(true);
        }

        public void Close()
        {
            if (_workable) 
                gameObject.SetActive(false);
        }

        public void SetKey(string key) =>
            KeyName.text = key;

        public void SwitchOff() =>
            _workable = false;
        
        public void SwitchOn() =>
            _workable = true;
    }
}