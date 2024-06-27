using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DailyReward
{
    public class DailyReward : MonoBehaviour
    {
        [SerializeField] private Button TakeRewardButton;
        [SerializeField] private Image UpdateMarker;
        [SerializeField] private TextMeshProUGUI HoursLeft;
        [SerializeField] private DailyRewardComponent DailyRewardComponent;
        [SerializeField] private int WaitForRewardHours;

        [Header("Colors")] [SerializeField] private Color RewardEnabled;
        [SerializeField] private Color RewardDisabled;

        private void Awake()
        {
            TakeRewardButton.enabled = false;
            RewardEnabled.a = 1;
            RewardDisabled.a = 1;
            UpdateMarker.gameObject.SetActive(false);
            HoursLeft.gameObject.SetActive(false);
            UpdateMarker.color = RewardDisabled;
        }

        private void Start()
        {
            TakeRewardButton.onClick.AddListener(LoadData);

            StartCoroutine(DailyRewardComponent.CheckDailyReward(timeFromLastReceivedBonus =>
                    {
                        if (timeFromLastReceivedBonus == -1)
                        {
                            Debug.Log("Network Error");
                            UpdateMarker.gameObject.SetActive(true);
                            TakeRewardButton.enabled = false;
                        }
                        else if (timeFromLastReceivedBonus == -2)
                        {
                            Debug.Log("Http Error");
                            UpdateMarker.gameObject.SetActive(true);
                            TakeRewardButton.enabled = false;
                        }
                        else if (timeFromLastReceivedBonus < WaitForRewardHours * 3600 && timeFromLastReceivedBonus != 0)
                        {
                            string leftHours = (24 - timeFromLastReceivedBonus / 3600).ToString();
                            HoursLeft.text = $"{leftHours}h";
                            HoursLeft.gameObject.SetActive(true);
                            UpdateMarker.gameObject.SetActive(true);
                            TakeRewardButton.enabled = false;
                        }
                        else
                        {
                            UpdateMarker.gameObject.SetActive(true);
                            UpdateMarker.color = RewardEnabled;
                            TakeRewardButton.enabled = true;
                        }
                    }
                )
            );
        }

        private void LoadData() =>
            DailyRewardComponent.LoadData();
    }
}