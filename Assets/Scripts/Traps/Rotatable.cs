using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Rotatable : MonoBehaviour
{
    [HideInInspector] public int listIdx = 0;
    [HideInInspector] public List<string> rotAxis = new List<string> { "X", "Y", "Z" };

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _waitSecs = 3f; // -1 is no waitting time
    [SerializeField] private float _minVal;
    [SerializeField] private float _maxVal;
    [SerializeField] private bool isCircle = false;
    [SerializeField] private bool _isReverted = false;

    private float _value;
    private float _sec = 0;

    private void Start()
    {
        _value = _isReverted ? _maxVal : _minVal;
        _sec = _waitSecs;
    }

    private void Update()
    {
        if (_sec <= 0)
        {
            if (!_isReverted) _value += _speed * Time.deltaTime;
            else _value -= _speed * Time.deltaTime;

            if (isCircle && _value >= 360) _value -= 360; 

            switch (listIdx)
            {
                case 0: // X
                    transform.localRotation = Quaternion.Euler(_value, transform.localRotation.y, transform.localRotation.z);
                    break;
                case 1: // Y
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, _value, transform.localRotation.z);
                    break;
                case 2: // Z
                    transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, _value);
                    break;
            }

            if (!_isReverted && _value >= _maxVal)
            {
                if (!isCircle) _isReverted = true;
                _sec = _waitSecs;
            }
            else if (_isReverted && _value <= _minVal)
            {
                if (!isCircle) _isReverted = false;
                _sec = _waitSecs;
            }
        }
        else
        {
            _sec -= Time.deltaTime;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Rotatable))]
public class RotDropDownEdtior : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Rotatable script = (Rotatable)target;

        GUIContent arrayList = new GUIContent("rotationAxis");
        script.listIdx = EditorGUILayout.Popup(arrayList, script.listIdx, script.rotAxis.ToArray());
    }
}
#endif