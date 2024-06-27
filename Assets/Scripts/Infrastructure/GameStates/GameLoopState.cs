using Services.StateMachine;
using UnityEngine;

namespace Infrastructure.GameStates
{
    public class GameLoopState : IState
    {
        private readonly IGameStateMachine _stateMachine;

        public GameLoopState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            Debug.Log("GameLoopState");
        }

        public void Exit()
        {
        }
    }
}