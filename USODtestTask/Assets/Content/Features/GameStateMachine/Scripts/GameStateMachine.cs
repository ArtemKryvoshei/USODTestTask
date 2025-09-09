using System;
using Content.Features.GameStateMachine.Scripts.States;
using Core.EventBus;
using Core.SaveLoadSystem;
using UnityEngine;
using Zenject;

namespace Content.Features.GameStateMachine.Scripts
{
    public class GameStateMachine : MonoBehaviour, IGameStateMachine
    {
        private IGameState _currentState;
        public IGameState CurrentState => _currentState;

        private InitializeState _initializeState;
        private IEventBus _eventBus;
        private ISaveLoadSystem _saveLoadSystem;

        [Inject]
        public void Construct(IEventBus eventBus, ISaveLoadSystem saveLoadSystem)
        {
            _saveLoadSystem = saveLoadSystem;
            _eventBus = eventBus;
            _initializeState = new InitializeState(eventBus);
            _eventBus.Subscribe<OnLoadProgressChangeEvent>(ChangeStateWhenLoaded);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnLoadProgressChangeEvent>(ChangeStateWhenLoaded);
        }

        private void ChangeStateWhenLoaded(OnLoadProgressChangeEvent obj)
        {
            if (_saveLoadSystem != null && _saveLoadSystem.CurrentLoadProgress == _saveLoadSystem.MaxLoadProgress && !(_currentState is GameplayState))
            {
                ChangeState(new GameplayState(_eventBus));
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ChangeState(_initializeState);
        }
        
        private void ChangeState(IGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}