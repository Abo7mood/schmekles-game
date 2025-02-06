using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSaver : MonoBehaviour
{
    public static WebSaver instance;
    #region DATA
   public string id { get; set; }
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
