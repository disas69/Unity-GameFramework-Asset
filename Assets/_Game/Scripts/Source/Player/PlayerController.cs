using System;
using Framework.Input;
using Source.Tools;
using UnityEngine;

namespace Source.Player
{
    [RequireComponent(typeof(InputController))]
    public class PlayerController : ActivatableMonoBehaviour, IDisposable
    {
        private InputController _inputController;

        protected void Awake()
        {
            _inputController = GetComponent<InputController>();
        }

        public void Initialize()
        {
        }

        public void PlaySuccess()
        {
        }

        public void PlayFailure()
        {
        }

        public void Dispose()
        {
        }
    }
}