namespace Content.Features.GameStateMachine.Scripts
{
    public interface IGameStateMachine
    {
        public IGameState CurrentState { get; }
    }
}