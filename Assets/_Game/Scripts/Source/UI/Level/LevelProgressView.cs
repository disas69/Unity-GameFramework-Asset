using TMPro;
using System;
using DG.Tweening;
using Source.Data;
using Source.Level;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Level
{
    public class LevelProgressView : MonoBehaviour, IDisposable
    {
        private LevelController _levelController;

        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _nextLevel;
        [SerializeField] private Transform _progressContainer;
        [SerializeField] private Image _progress;

        public void Initialize(LevelController levelController)
        {
            _levelController = levelController;
            _levelController.Progress.Changed += UpdateProgress;

            if (_level != null)
            {
                _level.text = GameData.LevelProgression.ToString();
            }

            if (_nextLevel != null)
            {
                _nextLevel.text = (GameData.LevelProgression + 1).ToString();
            }

            _progressContainer.localScale = Vector3.one;
            UpdateProgress(levelController.Progress.Value);
        }

        private void UpdateProgress(float progress)
        {
            _progress.fillAmount = progress;
        }

        public void Dispose()
        {
            _progressContainer.DOKill();
            _levelController.Progress.Changed -= UpdateProgress;
            _levelController = null;
        }
    }
}