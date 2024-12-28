using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> objs = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            foreach (GameObject obj in objs)
            {
                obj.SetActive(true);
            }

            if (this.GetComponent<AudioSource>() != null)
                this.GetComponent<AudioSource>().enabled = true;
        }
    }
}
