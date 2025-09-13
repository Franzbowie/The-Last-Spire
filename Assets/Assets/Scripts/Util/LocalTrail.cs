using System.Collections.Generic;
using UnityEngine;

namespace TLS.Util
{
    // Renders a trail in local space of an anchor so it moves with anchor (e.g., cursor).
    public class LocalTrail : MonoBehaviour
    {
        public Transform anchor;   // space in which to render the trail (parent)
        public Transform target;   // object to track (world transform)
        public float lifeTime = 0.25f;
        public float minDistance = 0.02f;
        public float width = 0.1f;
        public Gradient color;
        public AnimationCurve widthCurve; // 0..1 along trail

        private struct Point { public Vector3 local; public float time; }
        private readonly List<Point> _points = new List<Point>(64);
        private LineRenderer _lr;
        private float _lastAddTime;
        private Vector3 _lastLocalPos;

        private void Awake()
        {
            _lr = gameObject.AddComponent<LineRenderer>();
            _lr.useWorldSpace = false; // draw in local coordinates of this transform
            _lr.widthMultiplier = width;
            _lr.positionCount = 0;
            _lr.loop = false;
            _lr.numCapVertices = 8;    // rounded caps
            _lr.numCornerVertices = 2; // soften corners
            var shader = Shader.Find("Sprites/Default");
            if (shader != null)
                _lr.material = new Material(shader);

            if (color == null || color.colorKeys == null || color.colorKeys.Length == 0)
            {
                var grad = new Gradient();
                grad.SetKeys(
                    new[] { new GradientColorKey(new Color(1f, 0.9f, 0.3f), 0f), new GradientColorKey(new Color(1f, 0.6f, 0.2f), 1f) },
                    new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
                );
                color = grad;
            }
            _lr.colorGradient = color;

            if (widthCurve == null || widthCurve.length == 0)
            {
                widthCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); // tapers to zero
            }
            _lr.widthCurve = widthCurve;
        }

        private void LateUpdate()
        {
            if (anchor == null || target == null) { Destroy(gameObject); return; }
            // Ensure this renderer is under anchor so local space matches
            if (transform.parent != anchor) transform.SetParent(anchor, false);

            // Compute target local position in anchor space
            Vector3 local = anchor.InverseTransformPoint(target.position);
            float now = Time.time;

            // Add new point if moved enough or after small time slice
            if (_points.Count == 0 || (now - _lastAddTime) > 0.02f || (local - _lastLocalPos).sqrMagnitude > (minDistance * minDistance))
            {
                _points.Add(new Point { local = local, time = now });
                _lastLocalPos = local;
                _lastAddTime = now;
            }

            // Remove expired
            float cutoff = now - lifeTime;
            int start = 0;
            while (start < _points.Count && _points[start].time < cutoff) start++;
            if (start > 0) _points.RemoveRange(0, start);

            // Update LR positions (in local space)
            int n = _points.Count;
            _lr.positionCount = n;
            for (int i = 0; i < n; i++)
                _lr.SetPosition(i, _points[i].local);
        }
    }
}
