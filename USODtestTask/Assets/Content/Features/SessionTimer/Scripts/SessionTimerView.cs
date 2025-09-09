using TMPro;
using UnityEngine;
using Zenject;

namespace Content.Features.SessionTimer.Scripts
{
    public class SessionTimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionTimeText;

        private SessionTimer _sessionTimer;

        [Inject]
        public void Construct(SessionTimer sessionTimer)
        {
            _sessionTimer = sessionTimer;
        }

        private void Update()
        {
            if (_sessionTimer == null) return;
            sessionTimeText.text = $"Time: {_sessionTimer.SessionTime:F1} s";
        }
    }
}