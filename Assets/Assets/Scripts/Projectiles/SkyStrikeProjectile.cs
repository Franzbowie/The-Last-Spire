using UnityEngine;
using TLS.Inputs;

namespace TLS.Projectiles
{
    // Falls from above to target point; on arrival applies AoE damage.
    public class SkyStrikeProjectile : MonoBehaviour
    {
        public Vector3 target;
        public float radius = 1f;
        public float damage = 10f;
        public float fallSpeed = 20f;
        public float spawnHeight = 10f;
        public System.Action<Vector3> onImpact;
        public bool trackTarget = false; // legacy chase
        public bool useAnchor = false;    // follow cursor with local vertical fall
        public AimProvider aim;           // optional live aim provider
        private Transform _anchor;
        private GameObject _trailGO;

        private bool _impacted;

        private void Awake()
        {
            // simple visual
            var sr = gameObject.GetComponent<SpriteRenderer>();
            if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();
            sr.color = new Color(1f, 0.9f, 0.3f, 1f);
            sr.sprite = GetFallbackSprite();
        }

        private void Start()
        {
            if (useAnchor && aim != null)
            {
                var go = new GameObject("SkyStrikeAnchor");
                _anchor = go.transform;
                _anchor.position = aim.AimPoint;
                transform.SetParent(_anchor);
                transform.localPosition = new Vector3(0f, spawnHeight, 0f);

                // Local trail relative to cursor/anchor
                _trailGO = new GameObject("SkyStrikeLocalTrail");
                _trailGO.transform.SetParent(_anchor, false);
                var localTrail = _trailGO.AddComponent<TLS.Util.LocalTrail>();
                localTrail.anchor = _anchor;
                localTrail.target = this.transform;
                localTrail.width = 0.12f;
                localTrail.lifeTime = 0.04f; // 2x shorter tail
                localTrail.minDistance = 0.02f;
                var grad = new Gradient();
                grad.SetKeys(
                    new[] { new GradientColorKey(new Color(1f, 0.9f, 0.3f), 0f), new GradientColorKey(new Color(1f, 0.6f, 0.2f), 1f) },
                    new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
                );
                localTrail.color = grad;
            }
            else
            {
                // World-space trail for non-anchor mode
                var tr = gameObject.AddComponent<TrailRenderer>();
                tr.time = 0.03f; // 2x shorter
                tr.startWidth = 0.12f; tr.endWidth = 0.0f; // taper to zero
                tr.numCapVertices = 8; tr.numCornerVertices = 2;
                var shader = Shader.Find("Sprites/Default");
                tr.material = new Material(shader) { color = new Color(1f, 0.8f, 0.3f, 1f) };
            }
        }

        private void LateUpdate()
        {
            if (_impacted) return;

            if (useAnchor && aim != null && _anchor != null)
            {
                // move anchor to live aim, fall locally straight down
                _anchor.position = aim.AimPoint;
                var lp = transform.localPosition;
                lp.y -= fallSpeed * Time.deltaTime;
                if (lp.y <= 0f)
                {
                    lp.y = 0f;
                    transform.localPosition = lp;
                    Impact();
                }
                else
                {
                    transform.localPosition = lp;
                }
            }
            else
            {
                // legacy chase toward a world target
                Vector3 currentTarget = (trackTarget && aim != null) ? aim.AimPoint : target;
                Vector3 dir = (currentTarget - transform.position);
                float dist = dir.magnitude;
                float step = fallSpeed * Time.deltaTime;
                if (step >= dist)
                {
                    transform.position = currentTarget;
                    Impact();
                }
                else
                {
                    transform.position += dir.normalized * step;
                }
            }
        }

        private void Impact()
        {
            if (_impacted) return;
            _impacted = true;
            Vector3 pos = useAnchor && _anchor != null ? _anchor.position : target;
            TLS.Util.VFX.SpawnFlash(pos, radius * 0.9f, new Color(1f, 0.95f, 0.6f, 0.9f), 0.12f);
            TLS.Util.VFX.Explosion(pos, radius);
            foreach (var hit in Physics2D.OverlapCircleAll(pos, radius))
            {
                if (hit.TryGetComponent(out TLS.Combat.EnemyController e))
                {
                    float dmg = TLS.Util.Damage.WithCrit(damage);
                    e.hp -= dmg;
                    if (e.hp <= 0f) e.Die(true);
                }
            }
            onImpact?.Invoke(pos);
            if (_trailGO != null) Destroy(_trailGO);
            if (_anchor != null) Destroy(_anchor.gameObject);
            Destroy(gameObject);
        }

        private static Sprite _fallback;
        private static Sprite GetFallbackSprite()
        {
            if (_fallback != null) return _fallback;
            var tex = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            var colors = new Color32[64];
            for (int i = 0; i < colors.Length; i++) colors[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(colors); tex.Apply();
            _fallback = Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 32f);
            _fallback.name = "SkyStrikeProjFallback";
            return _fallback;
        }
    }
}
