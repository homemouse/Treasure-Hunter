using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] Rigidbody2D rigidbody2D;
    [SerializeField] Animator animator;

    [Header("Animator Property")]
    [SerializeField] bool isCrouch = false;
    [SerializeField] bool isSliding = false;
    [SerializeField] bool isAttack = false;
    [SerializeField] bool isFloor = false;
    [SerializeField] bool isWall = false;

    [Header("Rigidbody2D Property")]
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float airMoveSpeed = 10;
    [SerializeField] float airDownSpeed = -30f;
    [SerializeField] float wallDownSpeed = -5f;
    [SerializeField] float moveX = 0;
    [SerializeField] float moveY = 0;
    [SerializeField] float jump;
    [SerializeField] float jumpForce = 10;
    [SerializeField] float SlidingTimer = 0;
    [SerializeField] float SlidingDelays = 0.2f;
    [SerializeField] float floorTimer = 0;
    [SerializeField] float floorDelays = 0.5f;
    [SerializeField] float wallTimer = 0;
    [SerializeField] float wallDelays = 0.5f;
    [SerializeField] bool canAirRoll = false;

    [Header("Attack Property")]
    [SerializeField] float demage = 1;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackRebound = 0.2f;
    [SerializeField] float attackDelays = 0.5f;
    [SerializeField] float attackTimer = 0f;
    [SerializeField] LayerMask enemyLayers;

    [Header("Platform Property")]
    [SerializeField] int playerLayerMask = 0;
    [SerializeField] float platformDownDelay = 0.3f;

    [Header("Point")]
    [SerializeField] GameObject floorChecker;
    [SerializeField] GameObject wallChecker;
    [SerializeField] GameObject attackPoint;
    [SerializeField] LayerMask floorLayers;
    [SerializeField] LayerMask wallLayers;


    


    // Start is called before the first frame update
    void Start()
    {
        playerLayerMask = 1 << this.gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        //按鍵偵測
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        jump = Input.GetAxis("Jump");
        jump = Input.GetAxis("Jump");


        if (attackTimer <= attackDelays)
        {
            attackTimer += Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            isAttack = true;
            attackTimer = 0f;
        }


        //是否為慢走
        if (Input.GetKey(KeyCode.LeftShift))
            moveX = (float)(moveX * 0.5);
        if (isSliding && (SlidingTimer < SlidingDelays))
        {
            SlidingTimer += Time.deltaTime;
            moveX = (float)(moveX * 1.5);

        }
        else if (isCrouch)
        {
            moveX = (float)(moveX * 0.5);
            isSliding = false;
        }
        else
        {
            SlidingTimer = 0f;

        }

        //動畫
        animator.SetFloat("x", Mathf.Abs(moveX));
        animator.SetBool("isCrouch", isCrouch);
        animator.SetBool("isFloor", isFloor);
        animator.SetBool("isWall", isWall);
        animator.SetBool("isAttack", isAttack);
        animator.SetFloat("velX", rigidbody2D.velocity.x);
        animator.SetFloat("velY", rigidbody2D.velocity.y);
        
        
    }

    void FixedUpdate()
    {

        //走路
        playerMove(rigidbody2D, isFloor, moveX, moveSpeed, airMoveSpeed);
        
        
        //轉向
        if (moveX > 0.1f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (moveX < -0.1f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        if (moveY < -0.1f && Mathf.Abs(moveX) > 0.7)
        {
            isSliding = true;
            isCrouch = true;
        }
        else if(moveY < -0.1f)
        {
            isSliding = false;
            isCrouch = true;
        }
        else
        {
            isSliding = false;
            isCrouch = false;
        }

        //跳躍
        if (isFloor && jump >= 1f)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isFloor = false;
            isWall = false;
            floorTimer = 0f;
            wallTimer = 0f;
        }

        //牆壁跳躍
        if (isWall && jump >= 1f)
        {
            if (this.transform.localScale.x > 0f)
                rigidbody2D.AddForce(new Vector2(-jumpForce / 4, (float)(jumpForce * 1.3)), ForceMode2D.Impulse);
            else
                rigidbody2D.AddForce(new Vector2(jumpForce / 4, (float)(jumpForce * 1.3)), ForceMode2D.Impulse);
            isFloor = false;
            isWall = false;
            floorTimer = 0f;
            wallTimer = 0f;
        }

        //攻擊
        playerAttack(isAttack, attackPoint, attackRange, enemyLayers);

        //平台下跳
        if (Input.GetKey(KeyCode.S))
        {
            Collider2D[] allstuff = Physics2D.OverlapCircleAll(floorChecker.transform.position, 0.1f, floorLayers);
            foreach (Collider2D stuff in allstuff)
            {
                PlatformEffector2D PE2D = stuff.GetComponent<PlatformEffector2D>();
                if (PE2D == null)
                    continue;
                int rioMask = PE2D.colliderMask;
                PE2D.colliderMask &= ~(playerLayerMask);

                int resMask = PE2D.colliderMask;
                Debug.Log("rioMask: " + Convert.ToString(rioMask, 2) + " resMask: " + Convert.ToString(resMask, 2) + " playerLayer:" + Convert.ToString(playerLayerMask, 2));
                StartCoroutine(platformNoDown(PE2D, platformDownDelay));
            }
        }

        //限制下降速度
        if (rigidbody2D.velocity.y < airDownSpeed)
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, airDownSpeed);
        if (isWall && (Mathf.Abs(moveX) >= 1f) && rigidbody2D.velocity.y < wallDownSpeed)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, wallDownSpeed);

        }

        //跳躍偵測器判定
        if (isFloor == false && floorTimer <= floorDelays)
            floorTimer += Time.deltaTime;
            
        if (floorChecker != null && floorTimer >= floorDelays)
        {
            isFloor = false;
            Collider2D[] allstuff = Physics2D.OverlapCircleAll(floorChecker.transform.position, 0.1f, floorLayers);
            foreach (Collider2D stuff in allstuff)
            {
                isFloor = true;
                canAirRoll = true;
            }
        }

        if (isFloor == false && wallTimer <= wallDelays)
            wallTimer += Time.deltaTime;

        if (wallChecker != null && wallTimer >= wallDelays)
        {
            isWall = false;
            Collider2D[] allstuff = Physics2D.OverlapCircleAll(wallChecker.transform.position, 0.2f, wallLayers);
            foreach (Collider2D stuff in allstuff)
            {
                isWall = true;
            }
        }
    }

    private bool playerMove(Rigidbody2D rigidbody2D, bool isFloor, float moveX, float moveSpeed, float airMoveSpeed)
    {
        if (rigidbody2D == null)
            return false;

        if (isFloor)
        {
            rigidbody2D.velocity = (new Vector2((moveX * moveSpeed), rigidbody2D.velocity.y));
        }
        else
        {
            if (moveX < -0.1f)
            {
                //rigidbody2D.velocity = (new Vector2((moveX * airMoveSpeed), rigidbody2D.velocity.y));
                if(rigidbody2D.velocity.x > -airMoveSpeed) 
                    rigidbody2D.AddForce(new Vector2(-airMoveSpeed * 0.1f, 0));
            }
            if (moveX > 0.1f)
            {
                //rigidbody2D.velocity = (new Vector2((moveX * airMoveSpeed), rigidbody2D.velocity.y));
                if (rigidbody2D.velocity.x < airMoveSpeed)
                    rigidbody2D.AddForce(new Vector2(airMoveSpeed * 0.1f, 0));
            }
        }

        return true;
    }
    private void playerAttack(bool isAttack, GameObject attackPoint, float attackRange, LayerMask enemyLayers)
    {
        if (isAttack == false)
            return;
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            //rigidbody2D.AddForce(transform.TransformVector(new Vector2(-15f, 0f)), ForceMode2D.Impulse);
            //rigidbody2D.velocity = transform.TransformVector(new Vector2(-35f, 0f));
            if (this.transform.localScale.x > 0f)
                rigidbody2D.MovePosition(new Vector2(rigidbody2D.position.x - (float)(attackRebound * 0.1f), rigidbody2D.position.y));
            else
                rigidbody2D.MovePosition(new Vector2(rigidbody2D.position.x + (float)(attackRebound * 0.1f), rigidbody2D.position.y));

            if (enemyHealth != null)
            {
                enemyHealth.hp -= 1;
            }
        }

        this.isAttack = false;
    }

    //IEnumerator JumpOff()
    //{
    //    jumpOffCoroutineIsRunning = true;
    //    Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
    //    yield return new WaitForSeconds(0.5f);
    //    Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
    //    jumpOffCoroutineIsRunning = false;
    //}

    private void OnDrawGizmos()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    IEnumerator platformNoDown(PlatformEffector2D PE2D, float delay)
    {
        yield return new WaitForSeconds(delay);
        PE2D.colliderMask |= playerLayerMask;
        Debug.Log("OK");
    }
}
