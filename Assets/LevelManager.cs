using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _inst;
    [SerializeField]
    Platform platformPrefab, platformFinalPrefab;

    float startHeight = 200f;
    [SerializeField]
    float stepHeight = 5f;

    [SerializeField]
    int minDistance = 10, maxDistance = 15;

    int platformerCount = 5;
    [SerializeField]
    int minPlatformsInDirection = 3, maxPlatformsInDirection = 7;


    [SerializeField]
    float maxDirectionAngle = 200f;
    [SerializeField]
    [Range(0, 10)]
    int sameHeightChacne = 3;
    [SerializeField]
    Material[] targetSurfacesMaterials;
    List<Platform> platforms = new List<Platform>();
   
    [SerializeField]
    PlatformsLine PlatLineRenderer;

    [SerializeField]
    bool isStairUp = false;

    //Env Extender 
    float lastExtendTime = 0f;
    [SerializeField]
    GameObject envContainerObject;
    //level logic 
 
    public int platformPerLevel = 5, maxPlatforms = 100;

    private void Awake()
    {
        _inst = this;
    }
    void Start()
    {
        //Calc PlatCount 
        platformerCount = (SharedData._inst.currentLevel * platformPerLevel);
        if (platformerCount > maxPlatforms)
        {
            platformerCount = maxPlatforms;
        }

        //Calc Start Height
        startHeight = platformerCount * (stepHeight * .8f);
        if (isStairUp)
            startHeight = 10f;
        MainPlayer._inst.setPlayerHeightPos(startHeight + 5f);
        //spawn first piece 
        Platform plat;
        int matIndex = 0;
        Vector3 position = new Vector3(0f, startHeight, 0f);
        plat = Instantiate(platformPrefab, position, Quaternion.identity);
        plat.setTargetMaterial(targetSurfacesMaterials[matIndex]);
        plat.setNumber(0);
        platforms.Add(plat);
        matIndex++;

        //Direction Calc
        float lastAngle = Random.Range(0f, maxDirectionAngle);
        Vector2 randOnCircle = GetUnitOnCircle(lastAngle, Random.Range(minDistance, maxDistance));
        int allowedPlatformInDirection = Random.Range(minPlatformsInDirection, maxPlatformsInDirection);
        int currentPlatformInDirectionCount = 0;
        float virtualMin = lastAngle - (maxDirectionAngle/2);
        float virtualMax = lastAngle + (maxDirectionAngle/2);

       
        for (int x = 1; x < platformerCount; x++)
        {
      
            //Direction Changer 
            if (currentPlatformInDirectionCount >= allowedPlatformInDirection)
            {
               
                currentPlatformInDirectionCount = 0;
                allowedPlatformInDirection = Random.Range(minPlatformsInDirection, maxPlatformsInDirection);
                lastAngle = Random.Range(virtualMin, virtualMax);
                float transAngle = Mathf.Repeat(lastAngle, 360f);
                randOnCircle = GetUnitOnCircle(transAngle, Random.Range(minDistance, maxDistance));
                virtualMin = lastAngle - (maxDirectionAngle/2);
                virtualMax = lastAngle + (maxDirectionAngle/2);
            }
            
            Vector3 newPos = platforms[x - 1].transform.position;
            float yValue = -stepHeight;
            if (isStairUp)
                yValue = stepHeight;
            if (Random.Range(0, 11) > (10 - sameHeightChacne))
                yValue = 0;

            newPos += new Vector3(randOnCircle.x, yValue, randOnCircle.y);
            Platform newPlat;
            if (x + 1 == platformerCount)
            {
                newPlat = Instantiate(platformFinalPrefab, newPos, Quaternion.identity);
            }
            else
            {
                newPlat = Instantiate(platformPrefab, newPos, Quaternion.identity);
                newPlat.setNumber(x);
                newPlat.setAngle(Mathf.Repeat(lastAngle, 360f));
                newPlat.informLastPlatformPosition(platforms[x - 1].transform.position);
            }
            if (matIndex >= targetSurfacesMaterials.Length)
                matIndex = 0;
            newPlat.applyFloatingAnimtion();
            newPlat.setTargetMaterial(targetSurfacesMaterials[matIndex]);
            
            platforms.Add(newPlat);
           
            matIndex++;
            currentPlatformInDirectionCount++;
        }
        redrawLine();

        InvokeRepeating("redrawLine", .1f, .1f);

    }

    void redrawLine()
    {
        List<Vector3> platVectorList = new List<Vector3>();
        foreach (var plat in platforms)
        {
            platVectorList.Add(plat.transform.position);
        }
        PlatLineRenderer.drawLineBetweenPlatform(platVectorList.ToArray());
    }

    Vector2 GetUnitOnCircle(float angleDegrees, float radius)
    {

        // initialize calculation variables
        float _x = 0;
        float _y = 0;
        float angleRadians = 0;
        Vector2 _returnVector;

        // convert degrees to radians
        angleRadians = angleDegrees * Mathf.PI / 180.0f;

        // get the 2D dimensional coordinates
        _x = radius * Mathf.Cos(angleRadians);
        _y = radius * Mathf.Sin(angleRadians);

        // derive the 2D vector
        _returnVector = new Vector2(_x, _y);

        // return the vector info
        return _returnVector;
    }

    
    public void extendEnv()
    {
        if (lastExtendTime < Time.time)
        {
            lastExtendTime = Time.time + 1f;
            envContainerObject.transform.localScale *= 1.5f;
        }
    }

    public float getAngleForTheNextPlatform()
    {
        var comingPlat = platforms[MainPlayer._inst.lastJumpedPlatform + 1];
        return comingPlat.angleToPrevPlat;
    }

    public Vector3 getNextPlatformPos()
    {
        var comingPlat = platforms[MainPlayer._inst.lastJumpedPlatform + 1];
        return comingPlat.transform.position;
    }

}
