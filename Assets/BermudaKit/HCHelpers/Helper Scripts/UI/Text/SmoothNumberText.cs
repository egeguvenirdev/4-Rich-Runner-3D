using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ali.Helper.UI
{
    public class SmoothNumberText : MonoBehaviour
    {
        [SerializeField] private bool _isSpeedBased = false;
        [SerializeField] private float _speed = 1f;

        private int _targetPoints = 0;
        private int _points = 0;
        private Tweener _smoothTween;
        private Text _textComponent;

        public void SetPoints(int points)
        {
            if (_textComponent == null)
            {
                _textComponent = GetComponent<Text>();
            }
            _targetPoints = points;
            _smoothTween?.Kill();
            _smoothTween = DOTween.To(() => _points, x => _points = x, _targetPoints, _speed).SetSpeedBased(_isSpeedBased).OnUpdate(UpdateText);
        }

        public void SetPointsInstantly(int points)
        {
            if(_textComponent == null)
            {
                _textComponent = GetComponent<Text>();
            }
            _smoothTween?.Kill();
            _targetPoints = points;
            _points = points;
            UpdateText();
        }

        void UpdateText()
        {
            if (_textComponent)
            {
                _textComponent.text = GameUtility.FormatFloatToReadableString((float)_points);
            }

        }
    }
}