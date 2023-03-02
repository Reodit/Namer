using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WeatherEffect : MonoBehaviour
{
    [SerializeField] private GameObject starFallWeatherEffect;
    [SerializeField] private GameObject snowFallWeatherEffect;
    [SerializeField] private GameObject haloEffect;
    [SerializeField] private GameObject defualtLight;
    [SerializeField] private GameObject curLight;
    [SerializeField] private GameObject cameras;
    private GameObject InstanceLight;
    void Start()
    {
        Init();
    }

    void Init()
    {
        defualtLight = GameObject.Find("Directional Light").gameObject;

        if (GameManager.GetInstance.Level == 1)
        {
            GameObject curCam = GameManager.GetInstance.cameraController.curCam.gameObject;
            if (curCam)
            {
                GameObject weatherParticles = Instantiate(starFallWeatherEffect, Vector3.zero, Quaternion.identity, curCam.transform);
                weatherParticles.GetComponent<ParticleSystem>().Play();
                StartCoroutine(CameraChase(weatherParticles));
            }
        }
        
        else if (GameManager.GetInstance.Level == 9)
        {
            GameObject curCam = GameManager.GetInstance.cameraController.curCam.gameObject;
            if (curCam)
            {
                GameObject weatherParticles = Instantiate(snowFallWeatherEffect, curCam.transform);
                weatherParticles.GetComponent<ParticleSystem>().Play();
                weatherParticles.transform.localPosition = Vector3.zero;
                weatherParticles.transform.localRotation = Quaternion.identity;
                StartCoroutine(CameraChase(weatherParticles));
            }
        }
        
        else if (GameManager.GetInstance.Level == 10)
        {
            defualtLight.SetActive(false);
            InstanceLight = Instantiate(curLight);

            cameras = GameObject.Find("Cameras");
            if (cameras)
            {
                Instantiate(haloEffect, cameras.transform, true);
            }
        }

        if (GameManager.GetInstance.Level != 10)
        {
            defualtLight.SetActive(true);
            if (InstanceLight)
            {
                Destroy(InstanceLight);
            }
            
        }
    }

    IEnumerator CameraChase(GameObject weatherParticle)
    {
        cameras = GameObject.Find("Cameras");

        while (true)
        {
            if (cameras)
            {
                CinemachineBrain cb;
                if (cameras.TryGetComponent<CinemachineBrain>(out cb))
                {
                    weatherParticle.transform.SetParent(
                        cb.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().
                            gameObject.transform);
                }
                yield return new WaitForSeconds(1f);
            }

            else
            {
                yield break;
            }
        }
    }

}
