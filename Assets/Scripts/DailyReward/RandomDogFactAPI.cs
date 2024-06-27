using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace DailyReward
{
    public class RandomDogFactAPI : MonoBehaviour
    {
        public bool IsNetworkError;
        public bool IsHttpError;
        public bool IsLoaded;
        public bool IsCompletedLoaded;

        [Space] 
        [SerializeField] private string ServerUri;
        [SerializeField] private TextMeshProUGUI RandomDogFactText;


        private IEnumerator SendRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(ServerUri);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                IsNetworkError = true;
                IsLoaded = true;

                RandomDogFactText.text = " ";

                yield break;
            }

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                IsHttpError = true;
                IsLoaded = true;

                RandomDogFactText.text = " ";

                yield break;
            }

            string json = request.downloadHandler.text;

            RandomDogFact response = JsonUtility.FromJson<RandomDogFact>(json);

            RandomDogFactText.text = response.fact;
            IsCompletedLoaded = true;
            IsLoaded = true;
        }

        private void Awake()
        {
            StartCoroutine(SendRequest());
        }
    }
}