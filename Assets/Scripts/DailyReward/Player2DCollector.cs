using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Infrastructure.GameStates;
using Services.Progress;
using Services.Progress.SaveLoadService;
using Services.StateMachine;
using UI.Windows;
using UnityEngine;
using Zenject;

namespace DailyReward
{
    public class Player2DCollector : MonoBehaviour, ISavedProgress
    {
        private int _balance;
        private ISaveLoadService _saveLoadService;
        private IGameStateMachine _stateMachine;

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IGameStateMachine stateMachine)
        {
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
        }

        private void Awake()
        {
            _saveLoadService.LoadReaderProgress(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                Coin2D coin = other.GetComponent<Coin2D>();
                coin.Collect(this);
            }

            if (other.gameObject.CompareTag("RewardExit"))
            {
                _stateMachine.Enter<MainMenuState>();
            }
        }

        public void AddMoney(int money)
        {
            _balance += money;
            _saveLoadService.SaveProgress(this);
        }

        public void LoadProgress(PlayerProgress progress) => 
            _balance = progress.PlayerProgressData.Balance;

        public void UpdateProgress(PlayerProgress progress) => 
            progress.PlayerProgressData.Balance = _balance;
    }
}