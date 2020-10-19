using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGround : MonoBehaviour
{
    [SerializeField]
    bool isScriptAlreadyBaked = true;
    [SerializeField]
    GameObject platform;

    [SerializeField]
    int areaSize;

    [SerializeField]
    int minDistance, maxDistance;

    List<Vector3> platPoses = new List<Vector3>();
    private void Start()
    {

        if (isScriptAlreadyBaked) return;
        int rowCount = Mathf.RoundToInt( areaSize / maxDistance );

        for (var i =0; i < rowCount; i++)
        {

            int zPos = -maxDistance;
            int xPos = (i * maxDistance) - maxDistance;


            while (zPos < areaSize)
            {
                var pos = new Vector3(xPos, 0f, zPos);
                Instantiate(platform, pos, Quaternion.identity, transform);
                zPos += Random.Range(minDistance, maxDistance);
            }
         
        }
     
    
    }
}
