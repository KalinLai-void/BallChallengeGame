using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Moveable : MonoBehaviour
{
    [HideInInspector] public int listIdx = 0;
    [HideInInspector] public List<string> moveAxis = new List<string> { "X", "Y", "Z" };

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _waitSecs = 3f; // -1 is no waitting time
    [SerializeField] private float _minVal;
    [SerializeField] private float _maxVal;
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

            switch (listIdx)
            {
                case 0: // X
                    transform.position = new Vector3(_value, transform.position.y, transform.position.z);
                    break;
                case 1: // Y
                    transform.position = new Vector3(transform.position.x, _value, transform.position.z);
                    break;
                case 2: // Z
                    transform.position = new Vector3(transform.position.x, transform.position.y, _value);
                    break;
            }

            if (!_isReverted && _value >= _maxVal)
            {
                _isReverted = true;
                _sec = _waitSecs;
            }
            else if (_isReverted && _value <= _minVal)
            {
                _isReverted = false;
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
[CustomEditor(typeof(Moveable))]
public class MovDropDownEdtior : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Moveable script = (Moveable)target;

        GUIContent arrayList = new GUIContent("moveAxis");
        script.listIdx = EditorGUILayout.Popup(arrayList, script.listIdx, script.moveAxis.ToArray());
    }
}
#endif