using System.Collections;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingCurtain : MonoBehaviour, IVisibility
    {
        public Image LoadingProgress;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Open()
        {
            LoadingProgress.fillAmount = 0;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
            LoadingProgress.fillAmount = 1;
        }

        // private IEnumerator FadeIn()
        // {
        //     while (LoadingProgress.fillAmount < 1)
        //     {
        //         LoadingProgress.fillAmount += 0.03f;
        //         yield return new WaitForSeconds(0.03f);
        //     }
        //
        //     gameObject.SetActive(false);
        // }
    }
}