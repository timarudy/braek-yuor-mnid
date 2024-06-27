using System;
using System.Collections;
using Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace DailyReward
{
    public class DailyRewardComponent : MonoBehaviour
    {
        [Header("Properties")] public int ServerTime;
        public int LastReceivedBonusTime;

        [Space] public bool IsNetworkError;
        public bool IsHttpError;
        public bool IsLoaded;
        public bool IsCompletedLoaded;

        [Header("Settings")] [SerializeField] private string LocalLastReceiveBonusTimeKey;

        [Space] [SerializeField] private string ServerUri;

        public IEnumerator CheckDailyReward(Action<int> callback)
        {
            while (!IsLoaded)
            {
                yield return new WaitForSeconds(0.25f);
            }

            if (IsNetworkError)
            {
                callback(-1);
                yield break;
            }

            if (IsHttpError)
            {
                callback(-2);
                yield break;
            }

            if (IsCompletedLoaded)
            {
                callback(ServerTime - LastReceivedBonusTime);
            }
        }

        private IEnumerator SendRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get(ServerUri);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                IsNetworkError = true;
                IsLoaded = true;

                Debug.Log("Network Error");

                yield break;
            }

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                IsHttpError = true;
                IsLoaded = true;

                Debug.Log("Http Error");
            }

            string json = request.downloadHandler.text;

            ServerTimeResponse response = JsonUtility.FromJson<ServerTimeResponse>(json);

            ServerTime = response.unixtime;
            IsCompletedLoaded = true;
            IsLoaded = true;
        }

        public void LoadData()
        {
            LastReceivedBonusTime = ServerTime;
            PlayerPrefs.SetInt(LocalLastReceiveBonusTimeKey, LastReceivedBonusTime);
        }

        private void Awake()
        {
            LastReceivedBonusTime = PlayerPrefs.GetInt(LocalLastReceiveBonusTimeKey);
            StartCoroutine(SendRequest());
        }
    }
}