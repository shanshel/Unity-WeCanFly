using Suriyun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.EnumData;

public class MainPlayer : MonoBehaviour
{


    Rigidbody _rigid;

    public Animator[] animators;

    public Transform mainCam; 
    //Jump speed;
    public float jumpSpeed = 10f;
    public float speed = 10f;

    //Jump duration variables;
    public float jumpDuration = 0.5f;
    float currentJumpTime = 0f;
    //animations 
    [SerializeField]
    float fromJumpToInAirTime = .3f;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
     
    }

    Vector3 lastPos = Vector3.zero;
    private void FixedUpdate()
    {

        var camEluer = mainCam.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, camEluer.y, 0f);
 
        if (currentJumpTime > 0f)
        {
            
            _rigid.AddForce(Vector3.up * Time.deltaTime * jumpSpeed);
           

        }
        else
        {
            if (lastPos.y > transform.position.y)
            {
                animSetBool("isTouchThePlatform", false);
                animSetBool("isReachedMaxHeight", true);
            }
         
         
            
        }
        
        if (Input.GetMouseButton(1))
        {
            _rigid.position = _rigid.position + transform.forward * Time.fixedDeltaTime * speed;
        }


        currentJumpTime -= Time.fixedDeltaTime;
        lastPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            animSetBool("isCloseEnoughToPlatform", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
       
        if (collision.gameObject.layer == 8)
        {
           
            Platform collidedPlatform = collision.gameObject.GetComponent<Platform>();
            if (collidedPlatform != null)
            {
                animSetBool("isReachedMaxHeight", false);
                animSetBool("isTouchThePlatform", true);
                animTrigger("Jump");
                if (collidedPlatform.type == PlatformerType.Ground)
                {
                
                    currentJumpTime = jumpDuration * 3f;
                }
                else
                {
                    currentJumpTime = jumpDuration;
                }
            }
          
   
        }
     
    }


    void animSetBool(string key, bool val)
    {
        
        foreach (var anim in animators)
        {
            anim.SetBool(key, val);
        }

    }

    void animTrigger(string key)
    {
        foreach (var anim in animators)
        {
            anim.SetTrigger(key);
        }
    }

  
}
