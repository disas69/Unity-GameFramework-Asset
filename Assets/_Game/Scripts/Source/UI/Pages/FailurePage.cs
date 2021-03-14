using DG.Tweening;
using Framework.UI.Structure.Base.Model;
using Framework.UI.Structure.Base.View;
using JetBrains.Annotations;
using Source.Data;
using Source.State;
using UnityEngine;
using TMPro;

namespace Source.UI.Pages
{
    public class FailurePage : Page<PageModel>
    {
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private float _fadeInTime;
        [SerializeField] private CanvasGroup _overlay;

        public override void OnEnter()
        {
            base.OnEnter();

            if (_level != null)
            {
                _level.text = $"Level {GameData.LevelProgression}";
            }

            _overlay.alpha = 0f;
            _overlay.blocksRaycasts = false;

            DOTween.Sequence()
                .AppendInterval(1.5f)
                .AppendCallback(Retry)
                .Play();
        }

        [UsedImplicitly]
        public void Retry()
        {
            _overlay.blocksRaycasts = true;

            DOTween.Sequence()
                .Append(_overlay.DOFade(1f, _fadeInTime))
                .AppendCallback(() => GameController.Instance.Session.Dispose())
                .AppendCallback(() => GameController.Instance.Session.WaitUntilUnloaded(() =>
                {
                    GameController.Instance.SetState(GameState.Start);
                }))
                .Play();
        }
    }
}