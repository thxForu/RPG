using System;
using UnityEngine;

namespace Core
{
    public class CameraFacing : MonoBehaviour
    {
        private Camera cam;
        private void Start()
        {
            cam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.forward = cam.transform.forward;
        }
    }
}
