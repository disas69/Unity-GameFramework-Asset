using System.Collections.Generic;
using Framework.Tools.Gameplay;
using UnityEngine;

namespace Framework.Tools.Path
{
    [ExecuteInEditMode]
    public class PathSpline : MonoBehaviour
    {
        private Spline _spline;

        public List<Transform> Points = new List<Transform>();

        public void Build()
        {
            _spline = BuildSpline();
        }
        
        public float Build(float speed)
        {
            Build();
            return ComputePathLength(_spline.Points) / speed;
        }

        public Vector3 Interpolate(float value)
        {
            return _spline.Interpolate(value);
        }

        public Spline BuildSpline()
        {
            var path = new Vector3[Points.Count];

            for (var i = 0; i < Points.Count; i++)
            {
                path[i] = Points[i].position;
            }

            return new Spline(path, true);
        }

        private static float ComputePathLength(Vector3[] path)
        {
            var sum = 0f;
            for (var i = 0; i < path.Length - 1; i++)
            {
                sum += Vector3.Distance(path[i], path[i + 1]);
            }

            return sum;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_spline == null)
            {
                return;
            }

            for (var i = 0; i < 20; i++)
            {
                Gizmos.DrawSphere(_spline.Interpolate(i * 0.05f), 0.2f);
            }
        }
#endif
    }
}