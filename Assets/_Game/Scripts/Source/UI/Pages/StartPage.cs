using DG.Tweening;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using Source.Data;
using Source.State;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Source.UI.Pages
{
    public class StartPage : Page<PageModel>
    {
        private Sequence _sequence;
        private bool _isLevelUpgraded;
        private int _levelProgression = -1;

        [SerializeField] private Button _button;
        [SerializeField] private Transform _levelTransform;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private CanvasGroup _overlay;
        [SerializeField] private float _fadeOutTime;

        public override void OnEnter()
        {
            base.OnEnter();

            _overlay.alpha = 1f;
            _overlay.blocksRaycasts = true;
            _button.enabled = false;

            if (_level != null && _levelTransform != null)
            {
                var level = GameData.LevelProgression;

                _levelTransform.localScale = Vector3.one;

                if (_levelProgression == -1)
                {
                    _levelProgression = level;
                }

                if (_levelProgression == level)
                {
                    _isLevelUpgraded = false;
                    _level.text = $"Level {level}";
                }
                else
                {
                    _isLevelUpgraded = true;
                    _levelProgression = level;
                }
            }

            GameController.Instance.Session.WaitUntilLoaded(() =>
            {
                DOTween.Sequence()
                    .AppendInterval(0.1f)
                    .AppendCallback(() =>
                    {
                        _button.enabled = true;
                        _overlay.blocksRaycasts = false;
                    })
                    .Append(_overlay.DOFade(0f, _fadeOutTime)
                        .OnComplete(() =>
                        {
                            _overlay.alpha = 0f;
                        }))
                    .Play();

                if (_isLevelUpgraded)
                {
                    _sequence = DOTween.Sequence()
                        .AppendInterval(0.5f)
                        .Append(_levelTransform.DOScale(0, 0.2f).SetEase(Ease.Linear))
                        .AppendCallback(() => _level.text = $"Level {GameData.LevelProgression}")
                        .AppendInterval(0.2f)
                        .AppendCallback(() => _levelTransform.DOScale(1.5f, 0.4f).SetEase(Ease.OutBounce))
                        .AppendInterval(0.4f)
                        .Append(_levelTransform.DOScale(1f, 0.5f))
                        .Play();
                }
            });
        }

        [UsedImplicitly]
        public void Play()
        {
            _button.enabled = false;
            _overlay.blocksRaycasts = true;
            GameController.Instance.SetState(GameState.Play);
        }

        public override void OnExit()
        {
            base.OnExit();

            if (_sequence != null)
            {
                _sequence.Kill();
            }
        }
    }
}