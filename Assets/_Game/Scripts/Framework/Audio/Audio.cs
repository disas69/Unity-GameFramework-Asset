using System;
using Framework.Tools.Data;
using UnityEngine;

namespace Framework.Audio
{
    [Serializable]
    public class Audio : StorageItem
    {
        [HideInInspector]
        public AudioClip AudioClip;
    }
}