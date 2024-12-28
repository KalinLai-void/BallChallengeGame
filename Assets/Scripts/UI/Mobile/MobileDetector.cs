using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MobileDetector : MonoBehaviour
{
    [DllImport ("__Internal")]
    private static extern bool IsMobile ();

    [SerializeField] private GameObject _mobileUI;

    private void Start()
    {
        if (IsRunningOnMobile()) _mobileUI.SetActive(true);
        else _mobileUI.SetActive(false);
    }

    bool IsRunningOnMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobile ();
#else
        return false;
#endif
    }
}
