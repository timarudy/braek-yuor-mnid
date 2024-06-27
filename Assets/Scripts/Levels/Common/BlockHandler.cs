using UI;
using UI.Windows;
using UnityEngine;

namespace Levels.Common
{
    public class BlockHandler : MonoBehaviour, IVisibility
    {
        public void Open() => 
            gameObject.SetActive(true);

        public void Close() => 
            gameObject.SetActive(false);
    }
}