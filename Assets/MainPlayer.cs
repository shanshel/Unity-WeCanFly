using Suriyun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Assets.Scripts.EnumData;

public class MainPlayer : MonoBehaviour
{

    public static MainPlayer _inst;
    Rigidbody _rigid;

    [HideInInspector]
    public currentPlayerMoveStatus currentMove = currentPlayerMoveStatus.Normal;
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

    [SerializeField]
    ParticleSystem hitPlatformParticle, hitPlatformUndergroundParticlePrefab;

    [SerializeField]
    ParticleSystem fallInWaterVFXPrefab;

    [SerializeField]
    Transform onCameraPoint, offCameraPoint;


    public PlayerGroundDetector groundDetector;

    Vector3 lastPos = Vector3.zero;

    //Character Selection Animation
    float lastPlayingAnimationTime = 0f;
    int animToPlay = 1;
    int currentSelectedCharacter = 0;
    bool isTransiningBetweenCharacters = false;


    //Jump Score 
    int lastJumpedPlatform = 0;
    public ScorePoint scorePrefab;
    [SerializeField]
    ParticleSystem perfectScoreParticleSystem;
    private void Awake()
    {
        _inst = this;
        _rigid = GetComponent<Rigidbody>();
     
    }


    private void Start()
    {
        characterRenderUpdate();
    }


    private void FixedUpdate()
    {
     
        if (GameManager._inst.gameStatus == GameStatus.GameStarted)
        {
            //Rotate The Character to Face The Camera Direction 
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
                    currentMove = currentPlayerMoveStatus.BackJump;
                }
            }


            if (Input.GetMouseButton(1))
            {
                _rigid.position = _rigid.position + transform.forward * Time.fixedDeltaTime * speed;
            }

            currentJumpTime -= Time.fixedDeltaTime;
            lastPos = transform.position;
        }
        
        
 
     
        
   

        if (GameManager._inst.gameStatus == GameStatus.MainMenu)
        {
            if (GameManager._inst.gameTime > lastPlayingAnimationTime + 2.5f)
            {
                lastPlayingAnimationTime = GameManager._inst.gameTime;
                if (!isTransiningBetweenCharacters)
                {
                    playAnimationInt(animToPlay);

                    if (animToPlay == 1)
                    {
                        Invoke("resetAnimToPlayValue", 2f);
                    }
                    else
                    {
                        Invoke("resetAnimToPlayValue", 1f);
                    }

                    animToPlay++;
                    if (animToPlay > 3) animToPlay = 1;
                }
            }
        }

     
    }

    void resetAnimToPlayValue()
    {
        if (!isTransiningBetweenCharacters)
            playAnimationInt(0);
    }

    void characterRenderUpdate()
    {
        int i = 0;
        foreach (var anim in animators)
        {
            if (currentSelectedCharacter != i)
                anim.gameObject.SetActive(false);
            i++;
        }
    }
   

    public void nextCharacter()
    {
        if (isTransiningBetweenCharacters) return;
        playAnimationInt(0);
        isTransiningBetweenCharacters = true;

        Vector3 targetMove = new Vector3(-1 * offCameraPoint.position.x, offCameraPoint.position.y, offCameraPoint.position.z);
        animators[currentSelectedCharacter].transform.DOMove(targetMove, 1.5f).Play();
        playAnimationInt(4, currentSelectedCharacter);
        int lastSelectedCharacter = currentSelectedCharacter;
        currentSelectedCharacter++;
        if (currentSelectedCharacter >= animators.Length) currentSelectedCharacter = 0;

        animators[currentSelectedCharacter].gameObject.SetActive(true);
        targetMove = new Vector3(onCameraPoint.position.x, onCameraPoint.position.y, onCameraPoint.position.z);
        playAnimationInt(4, currentSelectedCharacter);
        animators[currentSelectedCharacter].transform.DOMove(targetMove, 1.5f).Play().OnComplete(() => {
            isTransiningBetweenCharacters = false;
            playAnimationInt(0);
            animators[currentSelectedCharacter].transform.DORotate(onCameraPoint.rotation.eulerAngles, .5f).Play();
            animators[lastSelectedCharacter].transform.position = offCameraPoint.position;
            animators[lastSelectedCharacter].transform.rotation = offCameraPoint.rotation;
        });
        

    }

    public void onGameStarted()
    {
        characterRenderUpdate();
        animators[currentSelectedCharacter].transform.DOLocalRotate(offCameraPoint.rotation.eulerAngles, .3f).Play().OnComplete(() => {
            hitPlatform(PlatformerType.Normal);
        });
        
        
        
    }

  

    private void OnCollisionEnter(Collision collision)
    {

        if (GameManager._inst.gameStatus != GameStatus.GameStarted) return; 

        if (collision.gameObject.layer == 8)
        {
           
            Platform collidedPlatform = collision.gameObject.GetComponent<Platform>();
     
            if (!collidedPlatform.isJumped && collidedPlatform.type == PlatformerType.Normal)
            {
                //Score 
                Vector3 detectorPos = groundDetector.transform.position;
                Vector3 platCenterPos = collidedPlatform._centerPointHover.transform.position;

                float distance = Vector2.Distance(new Vector2(detectorPos.x, detectorPos.z), new Vector2(platCenterPos.x, platCenterPos.z));

                if (distance > .8f)
                {
                    
                    if (collidedPlatform.platNumber - lastJumpedPlatform  > 1)
                    {
                        GameManager._inst.addScore(2, "Long Jump");
                    }
                    else
                    {
                        if (distance > 1.2f)
                            GameManager._inst.addScore(1, "Good", true);
                        else
                            GameManager._inst.addScore(1, "Good");
                    }
                }
                else
                {
                    GameManager._inst.applyTimeScaleEffect(.6f, .65f);
                    if (collidedPlatform.platNumber - lastJumpedPlatform > 1)
                    {
                        GameManager._inst.addScore(4, "Wooow!");
                        perfectScoreParticleSystem.Play();
                    }
                    else
                    {
                        GameManager._inst.addScore(3, "Perfect Jump");
                        perfectScoreParticleSystem.Play();
                    }
                }
            }
            

            if (collidedPlatform != null)
            {
                hitPlatform(collidedPlatform.type);
                collidedPlatform.onHitPlayer();
                Instantiate(hitPlatformUndergroundParticlePrefab, collidedPlatform.transform.position, Quaternion.identity);
            }

            lastJumpedPlatform = collidedPlatform.platNumber;
        }
        else if (collision.gameObject.layer == 10)
        {
            //Player Die

            Instantiate(fallInWaterVFXPrefab, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

     
    }

    void hitPlatform(PlatformerType platformType)
    {
        currentMove = currentPlayerMoveStatus.Normal;
        animSetBool("isReachedMaxHeight", false);
        animSetBool("isTouchThePlatform", true);
        animTrigger("Jump");
      
        hitPlatformParticle.Play();
        if (platformType == PlatformerType.Ground)
        {

            currentJumpTime = jumpDuration * 3f;
        }
        else if (platformType == PlatformerType.Final)
        {
            currentJumpTime = jumpDuration * 1000f;
        }
        else
        {
            currentJumpTime = jumpDuration;
        }
       
        Invoke("setPlayerMoveToInAir", .2f);


    }
    void setPlayerMoveToInAir()
    {
        currentMove = currentPlayerMoveStatus.InAir;
    }


    void animSetBool(string key, bool val)
    {
        
        foreach (var anim in animators)
        {
            anim.SetBool(key, val);
        }

    }
     
    void playAnimationInt(int val, int characterIndex = -1)
    {

        if (characterIndex == -1)
        {
            foreach (var anim in animators)
            {

                anim.SetInteger("AnimToPlay", val);
            }
        } 
        else
        {
            animators[characterIndex].SetInteger("AnimToPlay", val);
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
