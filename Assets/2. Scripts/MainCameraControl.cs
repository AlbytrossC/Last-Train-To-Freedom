using System;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    private GameObject _pl;
    private PlayerControl _pcs;
    private Vector3 _plPos;
    private Vector3 pos;
    public Camera _cam;
    public float maxFOV = 90;
    public float minFOV = 70;

    public enum CameraPosition { lo, mid, hi }
    public CameraPosition camPos =  CameraPosition.lo;
    private bool higherCam = false;
    private bool highestCam = false;

    private Vector3 highestPos;
    private Vector3 highPos;
    private Vector3 lowPos;

    private void Start()
    {
        _pl = GameObject.Find("Player");
        _pcs = _pl.GetComponent<PlayerControl>();
        _plPos = _pl.transform.position;
        pos = transform.position;
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = new Vector3(_plPos.x, pos.y, pos.z);

        _plPos = _pl.transform.position;

        highestPos = new Vector3(pos.x, 123, pos.z);
        highPos = new Vector3(pos.x, 65, pos.z);
        lowPos = new Vector3(pos.x, 38.5f, pos.z);
        
        pos = transform.position;
        // _cam.fieldOfView = higherCam ? maxFOV : minFOV;
        // _cam.transform.position = higherCam ? highPos : lowPos;
        if (_plPos.y <= 39)
            camPos = CameraPosition.lo;
        else if (_plPos.y > 40 && _plPos.y < 110)
            camPos = CameraPosition.mid;
        else if (_plPos.y >= 111)
            camPos = CameraPosition.hi;

        switch (camPos)
        {
            case CameraPosition.lo:
                _cam.fieldOfView = minFOV;
                SetPosition(lowPos);
                break;
            case CameraPosition.mid:
                _cam.fieldOfView = maxFOV;
                SetPosition(highPos);
                break;
            case CameraPosition.hi:
                _cam.fieldOfView = maxFOV;
                SetPosition(highestPos);
                break;
        }
    }

    private void SetPosition(Vector3 _pos)
    {
        transform.position = new Vector3(_plPos.x, _pos.y, pos.z);
    }
}