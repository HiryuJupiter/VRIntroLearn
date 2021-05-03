using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 For checking when controller touchjes the cube
Create a joint between the VR controller and the cube it is touching
set the break force and torque to 20000
Take the velocity of the controller and direction and apply it to the thing you're holding, to give the effect that it is rotating and moving the same direction, gives a nice arc.
     */

namespace BreadAndButter.VR.Interaction
{
    [RequireComponent(typeof(VRControllerInput))]
    public class InteractGrab : MonoBehaviour
    {
        //We have interaction even on both the object and the controller

        public InteractionEvent grabbed = new InteractionEvent();
        public InteractionEvent released = new InteractionEvent();

        private VRControllerInput input;

        private InteractableObject collidingObject;
        private InteractableObject heldObject;

        //The held object's original parent before it got reparented to this controller
        private Transform heldOriginalParent;

        void Start()
        {
            input = GetComponent<VRControllerInput>();

            input.OnGrabPressed.AddListener(
                (_arg) => {
                    if (collidingObject != null)
                        GrabObject(); });

            input.OnGrabReleased.AddListener(
                (_arg) => {
                    if (heldObject != null)
                        ReleaseObject();
                });
        }

        #region Trigger
        private void OnTriggerEnter (Collider _other)
        {
            SetCollidingObject(_other);
        }

        private void SetCollidingObject(Collider _other)
        {
            InteractableObject interactable = _other.GetComponent<InteractableObject>();

            if (collidingObject != null || interactable == null) //We only want to handle the first thing we collide with
                return;

            collidingObject = interactable;
        }

        private void OnTriggerExit (Collider _other)
        {
            if (collidingObject == _other.GetComponent<InteractableObject>())
                collidingObject = null;
        }
        #endregion

        private void GrabObject ()
        {
            heldObject = collidingObject;
            collidingObject = null;

            heldOriginalParent = heldObject.transform.parent;

            heldObject.Rigidbody.isKinematic = true;
            SnapObject(heldObject.transform, heldObject.AttachPoint);

            heldObject.OnObjectGrabbed(input.Controller); //Object specific grab logic
            grabbed.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider)); //Genernic event
        }

        private void ReleaseObject()
        {
            heldObject.Rigidbody.isKinematic = false;
            heldObject.transform.SetParent(heldOriginalParent);

            heldObject.Rigidbody.velocity = input.Controller.velocity;
            heldObject.Rigidbody.angularVelocity = input.Controller.AngularVelocity;

            heldObject.OnObjectReleased(input.Controller);
            released.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
            heldObject = null;
        }

        private void SnapObject(Transform _object, Transform _snapHandle)
        {
            Rigidbody attachPoint = input.Controller.Rigidbody;
            _object.transform.SetParent(transform);

            if(_snapHandle == null)
            {
                //Reset it to the same as the controllers position + rotation
                _object.position = Vector3.zero;
                _object.localRotation = Quaternion.identity;
            }
            else
            {
                //Calculate the correct position and rotation based on the snap handle
                _object.rotation = attachPoint.transform.rotation * Quaternion.Euler(_snapHandle.localEulerAngles);
                _object.position = attachPoint.transform.position - (_snapHandle.position - _object.position);
            }
        }
    }
}


/*
 
        //private void GrabObject()
        //{
        //    //Safety measure to prevent connect to somethign that don't exist yet.
        //    if (collidingObject == null)
        //        return;

        //    Debug.Log("HELLO");
        //    heldObject = collidingObject;
        //    collidingObject = null;
        //    heldObject.Rigidbody.useGravity = false;
        //    FixedJoint joint = AddJoint(heldObject.Rigidbody);

        //    if (heldObject.AttachPoint != null)
        //    {
        //        heldObject.transform.position =
        //            transform.position - (heldObject.AttachPoint.position - heldObject.transform.position);

        //        heldObject.transform.rotation = transform.rotation * Quaternion.Euler(heldObject.AttachPoint.localEulerAngles);
        //    }
        //    else
        //    {
        //        heldObject.transform.position = transform.position;
        //        heldObject.transform.rotation = transform.rotation;
        //    }

        //    grabbed.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
        //    heldObject.OnObjectGrabbed(input.Controller);
        //}




        //private void ReleaseObject ()
        //{
        //    heldObject.Rigidbody.useGravity = true;
        //    RemoveJoint(gameObject.GetComponent<FixedJoint>());
        //    released.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
        //    heldObject.OnObjectReleased(input.Controller);
        //}

        //#region Joint
        //private FixedJoint AddJoint (Rigidbody _rigidbody)
        //{
        //    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        //    joint.breakForce = 20000;
        //    joint.breakTorque = 20000;
        //    joint.connectedBody = _rigidbody;
        //    return joint;
        //}

        //private void RemoveJoint(FixedJoint _joint)
        //{
        //    if(_joint != null)
        //    {
        //        _joint.connectedBody = null; //Disconnect first and then set rigidbody
        //        Destroy(_joint);
        //        heldObject.Rigidbody.velocity = input.Controller.velocity;
        //        heldObject.Rigidbody.angularVelocity = input.Controller.AngularVelocity;
        //    }
        //}
        //#endregion
 */