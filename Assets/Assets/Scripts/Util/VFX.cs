using UnityEngine;

namespace TLS.Util
{
    public static class VFX
    {
        private static Material _lineMat;
        private static Sprite _discSprite;

        private static Material LineMat
        {
            get
            {
                if (_lineMat == null)
                {
                    var shader = Shader.Find("Sprites/Default");
                    _lineMat = new Material(shader) { color = Color.white };
                }
                return _lineMat;
            }
        }

        public static void Explosion(Vector3 pos, float radius, float duration = 0.2f)
        {
            // Simple debug circle to visualize explosion without VFX assets
            int steps = 24;
            Vector3 prev = pos + new Vector3(radius, 0f, 0f);
            for (int i = 1; i <= steps; i++)
            {
                float ang = (i / (float)steps) * Mathf.PI * 2f;
                Vector3 cur = pos + new Vector3(Mathf.Cos(ang) * radius, Mathf.Sin(ang) * radius, 0f);
                Debug.DrawLine(prev, cur, new Color(1f, 0.8f, 0.2f, 1f), duration);
                prev = cur;
            }
        }

        public static void SpawnRing(Vector3 center, float radius, Color color, float life = 0.25f, int steps = 32)
        {
            var go = new GameObject("VFX_Ring");
            go.transform.position = center;
            var lr = go.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.loop = true;
            lr.positionCount = steps;
            lr.widthMultiplier = 0.05f;
            lr.material = LineMat;
            lr.startColor = lr.endColor = color;
            for (int i = 0; i < steps; i++)
            {
                float t = (i / (float)steps) * Mathf.PI * 2f;
                lr.SetPosition(i, center + new Vector3(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius, 0f));
            }
            Object.Destroy(go, life);
        }

        public static void SpawnFlash(Vector3 pos, float size, Color color, float duration = 0.15f)
        {
            var go = new GameObject("VFX_Flash");
            go.transform.position = pos;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = GetDiscSprite();
            var flash = go.AddComponent<Flash>();
            flash.duration = duration;
            flash.startSize = size;
            flash.endSize = size * 0.6f;
            flash.color = color;
        }

        private static Sprite GetDiscSprite()
        {
            if (_discSprite != null) return _discSprite;
            int s = 32;
            var tex = new Texture2D(s, s, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Bilinear;
            var px = new Color32[s * s];
            float r = (s - 2) * 0.5f;
            Vector2 c = new Vector2((s - 1) * 0.5f, (s - 1) * 0.5f);
            for (int y = 0; y < s; y++)
            {
                for (int x = 0; x < s; x++)
                {
                    int i = y * s + x;
                    float d = Vector2.Distance(new Vector2(x, y), c);
                    float a = Mathf.Clamp01(1f - (d - r + 1f));
                    px[i] = new Color(1f, 1f, 1f, a);
                }
            }
            tex.SetPixels32(px);
            tex.Apply();
            _discSprite = Sprite.Create(tex, new Rect(0, 0, s, s), new Vector2(0.5f, 0.5f), s);
            _discSprite.name = "VFX_DiscSprite";
            return _discSprite;
        }
    }
}
