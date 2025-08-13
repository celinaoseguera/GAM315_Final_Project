using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject directionals;
    private float movementSpeed = 3.0f;
    private bool isMoving;
    private Rigidbody2D rigidBody;
    private Vector2 input;
    public LayerMask solidObjectsLayer;
    private Animator animator;
    private Vector2 prevInput;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
           

            //avoiding diagonal movement and lag
            if (input.x != 0) input.y = 0;

            // if user it pressing left or right or up or down (-1,0 ... 1,0 ... 0,1 ... 1,0)
            if (input.x != 0)
            {

                animator.SetFloat("moveX", input.x);
                //store previous direction when we move a different direction
                prevInput.x = animator.GetFloat("moveX");
                var targetPos = transform.position;
                targetPos.x += input.x;
               
                // walakble area? Only move if that is the case
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }
            if (input.y!= 0)
            {
                animator.SetFloat("moveX", prevInput.x);
                var targetPos = transform.position;
                targetPos.y += input.y;


                // walakble area? Only move if that is the case
                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        directionals.SetActive(false);
        // if there was any movement (bigger than zero)... means the user moved and we need to proces that.
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {

            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        // if there is overlap... meaning NOT null
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solidObjectsLayer) != null)
        {
            return false;
        }

        return true;
    }

}
