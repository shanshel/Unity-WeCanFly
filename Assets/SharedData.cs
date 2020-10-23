using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedData : MonoBehaviour
{
    public static SharedData _inst = null;
    public int currentLevel = 1;
    
    // Start is called before the first frame update

    private void Awake()
    {
        if (_inst != null)
        {
            Destroy(gameObject);
            return;
        }
        _inst = this;
        DontDestroyOnLoad(gameObject);
    }

  
    

}
