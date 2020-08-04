using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool isDead;
    [SerializeField] float hpMax = 5;
    public float hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value > hpMax)
            {
                _hp = hpMax;
                isDead = false;
            }
            else if (value <= 0)
            {
                _hp = 0;
                isDead = true;
            }
            else
            {
                _hp = value;
                isDead = false;
            }
        }
    }

    public float _hp = 5;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null)
        {
            animator.SetBool("isDead", isDead);
        }

        if (isDead)
            Destroy(this.gameObject, 0.5f);


    }
    

}
