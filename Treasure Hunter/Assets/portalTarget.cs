using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalTarget : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        this.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }
}
