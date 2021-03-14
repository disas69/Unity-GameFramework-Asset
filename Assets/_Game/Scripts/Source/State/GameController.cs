using System;
using Framework.Tools.Gameplay;
using Framework.Tools.Singleton;
using Framework.UI;
using Source.Data;
using Source.UI.Pages;
using UnityEngine;

namespace Source.State
{
    [RequireComponent(typeof(GameSession))]
    public class GameController : MonoSingleton<GameController>
    {
        private StateMachine<GameState> _stateMachine;

        public GameState State => _stateMachine.CurrentState;
        public GameSession Session { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Session = GetComponent<GameSession>();
        }

        private void Start()
        {
            CreateStateMachine();
            ActivateStartState();
        }

        public void SetState(string stateString)
        {
            if (Enum.TryParse(stateString, out GameState state))
            {
                SetState(state);
            }
        }

        public void SetState(int state)
        {
            SetState((GameState) state);
        }

        public void SetState(GameState gameState)
        {
            _stateMachine.SetState(gameState);
        }

        public void ResetData()
        {
            GameData.Reset();
            GameData.Save();
        }

        private void CreateStateMachine()
        {
            _stateMachine = new StateMachine<GameState>(GameState.Start);
            _stateMachine.AddTransition(GameState.Start, GameState.Play, ActivatePlayState);
            _stateMachine.AddTransition(GameState.Play, GameState.Success, ActivateSuccessState);
            _stateMachine.AddTransition(GameState.Success, GameState.Start, ActivateStartState);
            _stateMachine.AddTransition(GameState.Play, GameState.Failure, ActivateFailureState);
            _stateMachine.AddTransition(GameState.Failure, GameState.Start, ActivateStartState);
        }

        private void ActivateStartState()
        {
            Session.Initialize(GameData.LevelIndex);
            NavigationManager.Instance.OpenScreen<StartPage>();
        }

        private void ActivatePlayState()
        {
            Session.Play();
            NavigationManager.Instance.OpenScreen<PlayPage>();
        }

        private void ActivateSuccessState()
        {
            Session.Stop(true);
            NavigationManager.Instance.OpenScreen<SuccessPage>();
        }

        private void ActivateFailureState()
        {
            Session.Stop(false);
            NavigationManager.Instance.OpenScreen<FailurePage>();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                GameData.Save();
            }
        }
    }
}