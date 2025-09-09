using Content.Features.GameStateMachine.Scripts;

namespace Core.EventBus
{
    public struct OnLoadProgressChangeEvent { }

    public struct GameStateChangedEvent
    {
        public IGameState newState;
    }
}