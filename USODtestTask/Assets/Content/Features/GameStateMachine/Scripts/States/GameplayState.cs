using Core.EventBus;
using UnityEngine;

namespace Content.Features.GameStateMachine.Scripts.States
{
    public class GameplayState : IGameState
    {
        private readonly IEventBus _eventBus;

        public GameplayState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Enter()
        {
            Debug.Log("[GameState] Enter Gameplay");
            GameStateChangedEvent stateChangedEvent = new GameStateChangedEvent();
            stateChangedEvent.newState = this;
            _eventBus.Publish(stateChangedEvent);
        }

        public void Exit()
        {
            Debug.Log("[GameState] Exit Gameplay");
        }
    }
}