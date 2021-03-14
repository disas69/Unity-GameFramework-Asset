using System;
using UnityEngine;

namespace Framework.Spawn
{
    public class SpawnableObject : MonoBehaviour
    {
        public event Action<SpawnableObject> Deactivated;

        public virtual void Deactivate()
        {
            if (Deactivated != null)
            {
                Deactivated.Invoke(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}