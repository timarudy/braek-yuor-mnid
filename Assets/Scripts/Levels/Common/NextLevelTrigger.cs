using System.Linq;
using Configs;
using Infrastructure.AssetManagement;
using Infrastructure.GameStates;
using Services.SceneServices;
using Services.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Levels.Common
{
    public class NextLevelTrigger : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private ILevelDataService _levelData;

        [Inject]
        private void Construct(IGameStateMachine stateMachine, ILevelDataService levelData)
        {
            _levelData = levelData;
            _stateMachine = stateMachine;
        }

        private void OnTriggerEnter(Collider other)
        {
            MoveToNextScene(_levelData.GetNextSceneName(currentSceneName: SceneManager.GetActiveScene().name));
        }

        private void MoveToNextScene(string nextScene)
        {
            if (nextScene == SceneManager.GetActiveScene().name)
            {
                _stateMachine.Enter<MainMenuState>();
            }
            else
            {
                _stateMachine.Enter<LoadLevelState, string>(nextScene);
            }
        }
    }
}