using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extensions
{
    public static class SceneExtensions
    {
        public static void MoveToActiveScene<TComponent>(TComponent component) where TComponent : Object
        {
            if (component is Component unityComponent && unityComponent.gameObject.scene.name == "DontDestroyOnLoad")
            {
                SceneManager.MoveGameObjectToScene(unityComponent.gameObject, SceneManager.GetActiveScene());
            }
        }
    }
}