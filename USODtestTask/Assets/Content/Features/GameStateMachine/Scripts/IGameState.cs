namespace Content.Features.GameStateMachine.Scripts
{
    public interface IGameState
    {
        void Enter();
        void Exit();
    }
}