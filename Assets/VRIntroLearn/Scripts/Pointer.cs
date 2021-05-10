using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadAndButter.VR
{
    public class Pointer : MonoBehaviour
    {
        private const float TracerWidth = .025f;

        public Vector3 Endpoint { get; private set; } = Vector3.zero;
        public bool Active { get; private set; } = false;

        [SerializeField] private float cursorScaleFactor = .01f;
        public VRController controller;

        [SerializeField] private Color invalid = Color.red;
        [SerializeField] private Color valid = Color.green;

        private Transform cursor; //The sphere 
        private Transform traser; //The line

        private Renderer tracerRenderer;
        private Renderer cursorRenderer;

        void Start()
        {
            controller.Input.OnPointerPressed.AddListener(_args =>
            {
                Active = true;
                cursor.gameObject.SetActive(true);
                traser.gameObject.SetActive(true);
            });

            controller.Input.OnPointerReleased.AddListener(_args =>
            {
                Active = true;
                cursor.gameObject.SetActive(false);
                traser.gameObject.SetActive(false);
            });

            CreatePointer();
            cursor.gameObject.SetActive(false);
            traser.gameObject.SetActive(false);

            //SetValid(false);
        }

        void Update()
        {
            if (!Active)
                return;

            bool didHit = Physics.Raycast(controller.transform.position, controller.transform.forward, out RaycastHit hit);
            Endpoint = didHit ? hit.point : Vector3.zero;
            UpdateScalePos(hit, didHit);

            SetValid(didHit);
        }

        public void SetValid (bool _valid)
        {
            cursorRenderer.material.color = _valid ? valid : invalid;
            tracerRenderer.material.color = _valid ? valid : invalid;
        }

        private void UpdateScalePos(RaycastHit _hit, bool _didHit)
        {
            if (_didHit)
            {
                CalculateDirAndDst(controller.transform.position, _hit.point, out Vector3 dir, out float distance);

                //Set the tracer position to the midpoint of the parent pos and the endpoint.
                Vector3 midPoint = Vector3.Lerp(controller.transform.position, _hit.point, 0.5f);
                traser.position = midPoint;

                //Set the cursor to the endpoint and scale it.
                traser.localScale = new Vector3(TracerWidth, TracerWidth, distance);

                //Vector3 midPoint = Vector3.Lerp(controller.transform.position, controller.transform.position + dir * distance, 0.5f);
                //traser.localScale= new Vector3(TracerWidth, TracerWidth, 100f);
                cursor.position = _hit.point;
                cursor.localScale = Vector3.one * cursorScaleFactor;
            }
            else
            {
                //Set the cursor scale based on an arbitrary endpoint
                Vector3 end = controller.transform.position + controller.transform.forward * 100f;
                CalculateDirAndDst(controller.transform.position, end, out Vector3 dir, out float distance);

                Vector3 midPoint = Vector3.Lerp(controller.transform.position, end, 0.5f);
                //Vector3 midPoint = Vector3.Lerp(controller.transform.position, controller.transform.position + dir * distance, 0.5f);

                traser.position = midPoint;
                //traser.localScale= new Vector3(TracerWidth, TracerWidth, 100f);
                traser.localScale = new Vector3(TracerWidth, TracerWidth, distance);

                cursor.position = controller.transform.position + end;
                cursor.localScale = Vector3.one * cursorScaleFactor;
            }
        }

        private void CalculateDirAndDst (Vector3 _start, Vector3 _end, out Vector3 _dir, out float _distance)
        {
            Vector3 heading = _end - _start;
            _distance = heading.magnitude;
            _dir = heading / _distance;
        }

        private void CreatePointer()
        {
            GameObject traserObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cursorObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            //Cache
            cursor = cursorObj.transform;
            traser = traserObj.transform;
            tracerRenderer = traser.GetComponent<Renderer>();
            cursorRenderer = traser.GetComponent<Renderer>();

            //Parent
            traser.parent = controller.transform;
            cursor.parent = controller.transform;
        }
    }
}
