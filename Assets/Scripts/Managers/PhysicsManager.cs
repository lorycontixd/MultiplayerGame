using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    #region Singleton
    private static PhysicsManager _instance;
    public static PhysicsManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    
}
