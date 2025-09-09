using System;
using Core.EventBus;
using Core.SaveLoadSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace Content.Features.LoadScreenView.Scripts
{
    public class LoadScreenUI : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;
        [SerializeField] private TMP_Text progressText;
        [SerializeField] private GameObject loadingScreenHolder;

        private ISaveLoadSystem _saveLoadSystem;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(ISaveLoadSystem saveLoadSystem, IEventBus eventBus)
        {
            _saveLoadSystem = saveLoadSystem;
            _eventBus = eventBus;
            if (_eventBus != null)
            {
                _eventBus.Subscribe<OnLoadProgressChangeEvent>(ReactToProgressChange);
            }
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OnLoadProgressChangeEvent>(ReactToProgressChange);
        }

        private void ReactToProgressChange(OnLoadProgressChangeEvent obj)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (_saveLoadSystem == null) return;

            if (_saveLoadSystem.MaxLoadProgress != _saveLoadSystem.CurrentLoadProgress)
            {
                loadingScreenHolder.SetActive(true);
            }
            else
            {
                loadingScreenHolder.SetActive(false);
            }

            float progress = 0f;
            if (_saveLoadSystem.MaxLoadProgress > 0f)
                progress = _saveLoadSystem.CurrentLoadProgress / _saveLoadSystem.MaxLoadProgress;

            progressBar.value = progress;
            progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
        }
    }
}