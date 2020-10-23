using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera _inst;
    public CinemachineVirtualCamera menuCam;
    public CinemachineFreeLook gameplayCamera;

    float baseGameplayCameraXSpeed;
    bool moveTowardPlatform = false;

    // Start is called before the first frame update
    private void Awake()
    {
        _inst = this;
    }
    void Start()
    {
        baseGameplayCameraXSpeed = gameplayCamera.m_XAxis.m_MaxSpeed;
        InvokeRepeating("CheckAllowGuider", .05f, .05f);
    }

    bool isAllowedToMoveTheCamera = false;
    bool isPosSetted = false;
    Vector2 lastPlayerPosXZ;
    float lastAngle, angle;

    
    void CheckAllowGuider()
    {
        if (moveTowardPlatform)
        {
          
            Vector3 pos = MainPlayer._inst.transform.position;
            if (
                isPosSetted && 
                (Mathf.Abs(lastAngle - angle) <= .1f) && 
                Vector2.Distance(lastPlayerPosXZ, new Vector2(pos.x, pos.z)) < .1f
                )
            {
                isAllowedToMoveTheCamera = true;
            }

            lastAngle = angle;
     
            lastPlayerPosXZ = new Vector2(pos.x, pos.z);
            
            isPosSetted = true;

          
        }
    }
    private void FixedUpdate()
    {
        if (GameManager._inst.isAboutToFinish) return;
        if (moveTowardPlatform)
        {

     
            if (Input.GetMouseButton(0))
            {
                moveTowardPlatform = false;
                isAllowedToMoveTheCamera = false;
                isPosSetted = false;
            }

            Vector3 targetDir = LevelManager._inst.getNextPlatformPos() - gameplayCamera.transform.position;
            var dirTeller = AngleDir(transform.forward, targetDir, transform.up);
            angle = Vector3.Angle(targetDir, transform.forward);

            if (!isAllowedToMoveTheCamera)
            {
                return;
            }

            Vector3 playerPos = MainPlayer._inst.transform.position;
            if (Vector2.Distance(lastPlayerPosXZ, new Vector2(playerPos.x, playerPos.z)) > .001f)
            {
                moveTowardPlatform = false;
                isAllowedToMoveTheCamera = false;
                isPosSetted = false;
       
            }


            if (angle > 15f)
            {
                if (dirTeller == -1)
                {
                    gameplayCamera.m_XAxis.Value = Time.fixedDeltaTime * 7f * angle  * -1;
                }
                else if (dirTeller == 1)
                {
                    gameplayCamera.m_XAxis.Value = Time.fixedDeltaTime * 7f * angle;
                }
                else
                {
                    gameplayCamera.m_XAxis.Value = Time.fixedDeltaTime * 7f * angle;
                }

            }
            else
            {
                moveTowardPlatform = false;
                isAllowedToMoveTheCamera = false;
                isPosSetted = false;
            }
        }

        /*
        if (targetXValueTime > 0f)
        {
            gameplayCamera.m_XAxis.Value = targetXValue * Time.fixedDeltaTime;
        }
        else if (isWaitingForReset)
        {
            resetSpeed();
        }
        targetXValueTime -= Time.fixedDeltaTime;
        */
    }

    public void onGameStarted()
    {
        menuCam.Priority = 9;
    }

    void resetSpeed()
    {
        gameplayCamera.m_XAxis.m_MaxSpeed = baseGameplayCameraXSpeed;
    }
    public void onGameFinished()
    {
        menuCam.Priority = 11;

    }

    public void lookAtNextPlatform()
    {
        moveTowardPlatform = true;
    }




    int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1;
        }
        else if (dir < 0f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}
