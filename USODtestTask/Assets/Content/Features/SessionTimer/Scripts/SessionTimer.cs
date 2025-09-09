using System;
using System.Threading;
using System.Threading.Tasks;
using Content.Features.GameStateMachine.Scripts.States;
using Core.EventBus;
using Core.Other;
using Core.SaveLoadSystem;
using UnityEngine;
using Zenject;

namespace Content.Features.SessionTimer.Scripts
{
    public class SessionTimer : ITickable, IInitializable, IDisposable
    {
        private ISaveLoadSystem _saveLoadSystem;

        private float _sessionTime;
        private float _timeSinceLastSave;

        private CancellationTokenSource _cts;
        private IEventBus _eventBus;

        public float SessionTime => _sessionTime;

        private bool canWork = false;

        [Inject]
        public SessionTimer(ISaveLoadSystem saveLoadSystem, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _saveLoadSystem = saveLoadSystem;
            
            _eventBus.Subscribe<GameStateChangedEvent>(CheckGameState);
        }

        private void CheckGameState(GameStateChangedEvent obj)
        {
            if (obj.newState is GameplayState)
            {
                canWork = true;
            }
            else
            {
                canWork = false;
            }
        }

        public void Initialize()
        {
            Debug.Log("[SessionTimer] Initialized");
            _cts = new CancellationTokenSource();
        }

        public void Tick()
        {
            if (!canWork)
            {
                return;
            }

            _sessionTime += Time.deltaTime;
            _timeSinceLastSave += Time.deltaTime;

            if (_timeSinceLastSave >= ConstantsManager.SESSION_SAVE_INTERVAL)
            {
                _timeSinceLastSave = 0f;
                _ = SaveProgressAsync(_cts.Token);
            }
        }

        private async Task SaveProgressAsync(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            var data = new GameData
            {
                sessionTime = _sessionTime
            };

            Debug.Log($"[SessionTimer] AutoSaved at {_sessionTime:F2} sec");
            await _saveLoadSystem.SaveData(data);
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _eventBus.Unsubscribe<GameStateChangedEvent>(CheckGameState);
        }
    }
}