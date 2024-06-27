using System;
using System.Collections;
using Infrastructure;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SceneServices
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly LoadingCurtain _curtain;

        public SceneLoader(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            _coroutineRunner = coroutineRunner;
            _curtain = curtain;
        }

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        private IEnumerator LoadScene(string nextScene, Action onLoaded)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitSceneLoad = SceneManager.LoadSceneAsync(nextScene);

            while (!waitSceneLoad.isDone)
            {
                _curtain.Open();
                _curtain.LoadingProgress.fillAmount = waitSceneLoad.progress;
                yield return new WaitForEndOfFrame();
            }

            _curtain.Close();

            onLoaded?.Invoke();
        }
    }
}