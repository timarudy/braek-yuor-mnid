using UnityEngine;

namespace UI.PopUps
{
    public class ToolTipVisibilityHandler : MonoBehaviour
    {
        public ToolTip ToolTip;

        public void OpenToolTip() => 
            ToolTip.Open();

        public void CloseToolTip()
        {
            if (ToolTip != null) 
                ToolTip.Close();
        } 
    }
}