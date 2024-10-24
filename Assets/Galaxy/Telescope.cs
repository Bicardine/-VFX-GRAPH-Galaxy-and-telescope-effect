using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;

namespace Galaxy
{
    public class Telescope : MonoBehaviour
    {
        [SerializeField] private Volume _volume;

        [SerializeField][Range(0, 10)] private float _damping = 0.75f;

        [SerializeField][Range(0, 10)] private float _minSecondsToNewTarget = 1f;
        [SerializeField][Range(0, 10)] private float _maxSecondsToNewTarget = 5f;

        [SerializeField][Range(0, 1)] private float _minXVignettePosition = 0.2f;
        [SerializeField][Range(0, 1)] private float _maxXVignettePosition = 0.8f;
        [SerializeField][Range(0, 1)] private float _minYVignettePosition = 0.35f;
        [SerializeField][Range(0, 1)] private float _maxYVignettePosition = 0.65f;

        private Vector2 _targetPosition;
        private Vector2 _velocity = Vector2.zero;

        private float _targetSeconds;
        private float _elapsedSeconds;

        private Vignette _vignette;

        private bool _itsTimeToNewTarget => _elapsedSeconds >= _targetSeconds;

        private Vector2 _vignetteCenterValue { get => _vignette.center.value; set => _vignette.center.value = value; }

        private void Awake()
        {
            if (!_volume.profile.TryGet(out _vignette))
                throw new NullReferenceException("Can't get Vignette component from Volume profile");
        }

        private void Update()
        {
            if (_itsTimeToNewTarget)
                ResetElapsedTimeAndGetTargetSecondsAndTargetPosition();

            _elapsedSeconds += Time.deltaTime;

            SmoothDampToNextPosition();
        }

        private void ResetElapsedTimeAndGetTargetSecondsAndTargetPosition()
        {
            _elapsedSeconds = 0f;

            _targetSeconds = Random.Range(_minSecondsToNewTarget, _maxSecondsToNewTarget);
            _targetPosition = new Vector2(
                Random.Range(_minXVignettePosition, _maxXVignettePosition),
                Random.Range(_minYVignettePosition, _maxYVignettePosition));
        }

        private void SmoothDampToNextPosition()
        {
            _vignetteCenterValue = Vector2.SmoothDamp(_vignetteCenterValue, _targetPosition, ref _velocity, _damping);
        }
    }
}