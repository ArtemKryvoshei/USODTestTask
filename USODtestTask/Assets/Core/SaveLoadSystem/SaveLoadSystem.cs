using System;
using System.Threading.Tasks;
using Content.Features.GameStateMachine.Scripts;
using Content.Features.GameStateMachine.Scripts.States;
using Core.EventBus;
using Core.Other;
using UnityEngine;
using Zenject;

namespace Core.SaveLoadSystem
{
    public class SaveLoadSystem : ISaveLoadSystem, IDisposable
    {
        public float CurrentLoadProgress { get; private set; }
        public float MaxLoadProgress { get; private set; } = 1f;
        
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus bus)
        {
            _eventBus = bus;
            if (_eventBus != null)
            {
                _eventBus.Subscribe<GameStateChangedEvent>(LoadOnInit);
            }
        }

        private async void LoadOnInit(GameStateChangedEvent obj)
        {
            if (obj.newState is InitializeState)
            {
                Debug.Log("[SaveLoadSystem] Load on init!");
                GameData savedData = await LoadGameData();
                Debug.Log("[SaveLoadSystem] Last session time: " + savedData?.sessionTime);
            }
        }

        public async Task<GameData> LoadGameData(float fakeDelaySeconds = 1f)
        {
            CurrentLoadProgress = 0f;

            // Задержка перед загрузкой
            float t = 0f;
            while (t < fakeDelaySeconds)
            {
                t += Time.deltaTime;
                CurrentLoadProgress = (t / (fakeDelaySeconds * 2f)) * MaxLoadProgress;
                _eventBus.Publish(new OnLoadProgressChangeEvent());
                await Task.Yield();
            }

            // Чтение PlayerPrefs
            var data = new GameData
            {
                sessionTime = PlayerPrefs.GetFloat(ConstantsManager.SESSION_TIME_KEY, 0f)
            };

            // Задержка после загрузки
            t = 0f;
            while (t < fakeDelaySeconds)
            {
                t += Time.deltaTime;
                CurrentLoadProgress = 0.5f + 0.5f * (t / fakeDelaySeconds);
                _eventBus.Publish(new OnLoadProgressChangeEvent());
                await Task.Yield();
            }

            CurrentLoadProgress = MaxLoadProgress;
            _eventBus.Publish(new OnLoadProgressChangeEvent());
            return data;
        }

        public async Task SaveData(GameData dataToSave, float fakeDelaySeconds = 1f)
        {
            CurrentLoadProgress = 0f;

            // Задержка перед сохранением
            float t = 0f;
            while (t < fakeDelaySeconds)
            {
                t += Time.deltaTime;
                CurrentLoadProgress = (t / (fakeDelaySeconds * 2f)) * MaxLoadProgress;
                _eventBus.Publish(new OnLoadProgressChangeEvent());
                await Task.Yield();
            }

            // Запись PlayerPrefs
            PlayerPrefs.SetFloat(ConstantsManager.SESSION_TIME_KEY, dataToSave.sessionTime);
            PlayerPrefs.Save();

            // Задержка после сохранения
            t = 0f;
            while (t < fakeDelaySeconds)
            {
                t += Time.deltaTime;
                CurrentLoadProgress = 0.5f + 0.5f * (t / fakeDelaySeconds);
                _eventBus.Publish(new OnLoadProgressChangeEvent());
                await Task.Yield();
            }

            CurrentLoadProgress = MaxLoadProgress;
        }

        public void Dispose()
        {
            _eventBus.Unsubscribe<GameStateChangedEvent>(LoadOnInit);
        }
    }
}