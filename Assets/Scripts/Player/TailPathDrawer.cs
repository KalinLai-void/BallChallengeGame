using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPathDrawer : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _scale = 0.1f;

    // Update is called once per frame
    void Update()
    {
        LeaveTrail(transform.position, _scale, new Color(255, 0, 0, 0.5f));
    }

    private void LeaveTrail(Vector3 point, float scale, Color color)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * scale;
        sphere.transform.position = point;
        sphere.transform.parent = transform.parent;
        sphere.GetComponent<Collider>().enabled = false;
        sphere.GetComponent<Renderer>().material.color = color;
        Destroy(sphere, 3f);
    }
}
