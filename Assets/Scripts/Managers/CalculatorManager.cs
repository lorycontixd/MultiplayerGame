using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatorManager : MonoBehaviour
{
    #region Singleton
    private static CalculatorManager _instance;
    public static CalculatorManager Instance { get { return _instance; } }

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

    public float minBaseForce;
    public float maxBaseForce;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseForce"></param>
    /// <param name="playerSpeed"></param>
    /// <param name="playerMass"></param>
    /// <returns></returns>
    public float CalculatePushForce(float baseForce, float playerSpeed, float playerMass)
    {
        if (baseForce < minBaseForce)
        {
            Debug.LogWarning($"Calculating force with too small base force: {baseForce}");
        }
        if (baseForce > maxBaseForce)
        {
            Debug.LogWarning($"Calculating force with too large base force: {baseForce}");
        }
        return baseForce + 0.1f * playerSpeed + playerMass;
    }
}
