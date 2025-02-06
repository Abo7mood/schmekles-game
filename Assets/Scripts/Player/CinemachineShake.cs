using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineShake: MonoBehaviour
{


    public static CinemachineShake instance { get; private set; }
    public static CinemachineVirtualCamera virtualCamera { get; private set; }

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startinginstatntiy;

    private void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    private void Start()
    {

        CinemachineBasicMultiChannelPerlin cvirtual =
      virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cvirtual.m_AmplitudeGain = 0;




    }
    public void Shaker(float instantity, float time)
    {
        CinemachineBasicMultiChannelPerlin cvirtual =
          virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cvirtual.m_AmplitudeGain = instantity;
        startinginstatntiy = instantity;
        shakeTimer = time;
        shakeTimerTotal = time;

    }

    private void Update()
    {

        CinemachineBasicMultiChannelPerlin c =
   virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            c.m_AmplitudeGain = Mathf.Lerp(startinginstatntiy, 0f, 1 - (shakeTimer / shakeTimerTotal));

        }
        if (c.m_AmplitudeGain > 0 && c.m_AmplitudeGain < 1)
            c.m_AmplitudeGain = 0;
    }


}
