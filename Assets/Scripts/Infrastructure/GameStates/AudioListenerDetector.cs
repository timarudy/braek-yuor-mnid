using System;
using UnityEngine;

namespace Infrastructure.GameStates
{
    using UnityEngine;

    public class AudioListenerDetector : MonoBehaviour
    {
        void Start()
        {
            // Find all AudioListener components in the scene
            AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

            // Check if there is more than one AudioListener
            if (audioListeners.Length > 1)
            {
                Debug.LogWarning("More than one AudioListener found in the scene!");

                foreach (AudioListener listener in audioListeners)
                {
                    // Log the name and path of each GameObject with an AudioListener
                    Debug.Log($"AudioListener found on GameObject: {GetGameObjectPath(listener.gameObject)}");
                }
            }
            else
            {
                Debug.Log("Only one AudioListener found in the scene.");
            }
        }

        // Helper method to get the full path of a GameObject in the hierarchy
        private string GetGameObjectPath(GameObject obj)
        {
            string path = obj.name;
            Transform parent = obj.transform.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}