using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsLine : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public LineRenderer _lineRenderer;
    int verixCount = 3;

   
    private void Update()
    {
  
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(point2.position, point3.position);

        //float ratio = .25f;
        Gizmos.color = Color.red;
        for (float ratio = .5f / verixCount; ratio < 1f; ratio += 1.0f / verixCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio), Vector3.Lerp(point2.position, point3.position, ratio));
        }
        
       
    }

    public void drawLineBetweenPlatform(Vector3[] arrayOfPlatformPos)
    {
        List<Vector3> points = new List<Vector3>();
        for (var x = 0; x < arrayOfPlatformPos.Length; x++)
        {
            Vector3 point = arrayOfPlatformPos[x];
            point.y -= 4f;
            points.Add(point);
            if (x % 2 == 0)
            {
                point.y -= 5f;
          
                points.Add(point);
            }
        }

        var bazierPointList = new List<Vector3>();
        for (var i = 0; i < points.Count; i += 3)
        {

            if (i + 2 >= points.Count) break;

            for (float ratio = 0f / verixCount; ratio <= 1f; ratio += 1.0f / verixCount)
            {
                var tang1 = Vector3.Lerp(points[i], points[i + 1], ratio);
                var tang2 = Vector3.Lerp(points[i + 1], points[i + 2], ratio);
                var bazirPoint = Vector3.Lerp(tang1, tang2, ratio);
                bazierPointList.Add(bazirPoint);
            }
        }

     
        _lineRenderer.positionCount = bazierPointList.Count;
        _lineRenderer.SetPositions(bazierPointList.ToArray());
    }

}
