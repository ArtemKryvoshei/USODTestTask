using Core.EventBus;
using UnityEngine;

namespace Content.Features.GameStateMachine.Scripts.States
{
    public class InitializeState : IGameState
    {
        private readonly IEventBus _eventBus;

        public InitializeState(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Enter()
        {
            Debug.Log("[GameState] Enter Initialize");
            GameStateChangedEvent stateChangedEvent = new GameStateChangedEvent();
            stateChangedEvent.newState = this;
            _eventBus.Publish(stateChangedEvent);
        }

        public void Exit()
        {
            Debug.Log("[GameState] Exit Initialize");
        }
    }
}