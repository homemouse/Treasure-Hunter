using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalCtrl : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Collider2D[] allstuff = Physics2D.OverlapCircleAll(this.transform.position, 0.1f);
            foreach (Collider2D stuff in allstuff)
            {
                portalTarget portaltarget = stuff.GetComponent<portalTarget>();
                if (portaltarget !=null)
                {
                    if (portaltarget.target != null)
                    {
                        this.transform.position = portaltarget.target.transform.position;
                    }
                }
            }
        }
    }
}
