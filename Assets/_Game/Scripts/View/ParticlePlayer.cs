using System.Collections.Generic;
using ColorBlocks.Core;
using UnityEngine;

namespace ColorBlocks.View
{
    public class ParticlePlayer : MonoBehaviour
    {
        private Pooler _pooler;

        private readonly List<ParticleSystem> _playing = new List<ParticleSystem>();

        private void Awake()
        {
            _pooler = new Pooler(transform);
        }

        public void Play(ParticleSystem prefab, Vector3 position, Quaternion rotation, Color color, float duration)
        {
            ParticleSystem particle = _pooler.Get(prefab);
            particle.transform.SetPositionAndRotation(position, rotation);

            var main = particle.main;
            main.startColor = color;
            main.duration = duration;

            particle.Play();
            _playing.Add(particle);
        }

        private void Update()
        {
            // Finished particles go back to the pool.
            for (int i = _playing.Count - 1; i >= 0; i--)
            {
                if (_playing[i].IsAlive())
                    continue;

                _pooler.Release(_playing[i]);
                _playing.RemoveAt(i);
            }
        }
    }
}