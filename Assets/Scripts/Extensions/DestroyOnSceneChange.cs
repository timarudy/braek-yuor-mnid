using UnityEngine;
using UnityEngine.SceneManagement;

namespace Extensions
{
    public class DestroyOnSceneChange : MonoBehaviour
    {
        private void Start() => 
            SceneManager.sceneUnloaded += DestroyObject;

        private void OnDestroy() => 
            SceneManager.sceneUnloaded -= DestroyObject;

        private void DestroyObject(Scene scene) => 
            Destroy(gameObject);
    }
}