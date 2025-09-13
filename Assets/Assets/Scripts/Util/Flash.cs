using UnityEngine;

namespace TLS.Util
{
    public class Flash : MonoBehaviour
    {
        public float duration = 0.15f;
        public float startSize = 1f;
        public float endSize = 0.6f;
        public Color color = Color.white;

        private SpriteRenderer _sr;
        private float _t;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            if (_sr == null) _sr = gameObject.AddComponent<SpriteRenderer>();
            transform.localScale = Vector3.one * startSize;
        }

        private void Update()
        {
            _t += Time.deltaTime;
            float r = duration > 0f ? Mathf.Clamp01(_t / duration) : 1f;
            float size = Mathf.Lerp(startSize, endSize, r);
            transform.localScale = Vector3.one * size;
            var c = color; c.a = Mathf.Lerp(color.a, 0f, r);
            _sr.color = c;
            if (_t >= duration) Destroy(gameObject);
        }
    }
}

