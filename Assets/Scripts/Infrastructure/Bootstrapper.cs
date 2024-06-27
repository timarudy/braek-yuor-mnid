using Infrastructure.GameStates;
using Services.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private StatesFactory _statesFactory;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, StatesFactory statesFactory)
        {
            _statesFactory = statesFactory;
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _gameStateMachine.RegisterState(_statesFactory.Create<BootstrapState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<MainMenuState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<LoadLevelState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<GameLoopState>());
            _gameStateMachine.RegisterState(_statesFactory.Create<DailyRewardState>());
            
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}