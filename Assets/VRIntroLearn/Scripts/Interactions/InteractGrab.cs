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
        private interactableObject collidingObject;
        private interactableObject heldObject;

        void Start()
        {
            input = GetComponent<VRControllerInput>();

            input.OnGrabPressed.AddListener(
                (_arg) => {
                    if (collidingObject != null)
                        GrabObject(); });

            input.OnGrabReleased.AddListener(
                (_arg) => {
                    if (collidingObject != null)
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
            interactableObject interactable = _other.GetComponent<interactableObject>();

            if (collidingObject != null || interactable == null) //We only want to handle the first thing we collide with
                return;

            collidingObject = interactable;
        }

        private void OnTriggerExit (Collider _other)
        {
            if (collidingObject == _other.GetComponent<interactableObject>())
                collidingObject = null;
        }
        #endregion

        private void GrabObject ()
        {
            //Safety measure to prevent connect to somethign that don't exist yet.
            if (collidingObject == null)
                return;

            heldObject = collidingObject;
            collidingObject = null;
            FixedJoint joint = AddJoint(heldObject.Rigidbody);

            if (heldObject.AttachPoint != null)
            {
                heldObject.transform.position =
                    transform.position - (heldObject.AttachPoint.position - heldObject.transform.position);

                heldObject.transform.rotation = transform.rotation * Quaternion.Euler(heldObject.AttachPoint.localEulerAngles);
            }
            else
            {
                heldObject.transform.position = transform.position;
                heldObject.transform.rotation = transform.rotation;
            }

            grabbed.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
            heldObject.OnObjectGrabbed(input.Controller);
        }

        private void ReleaseObject ()
        {
            RemoveJoint(gameObject.GetComponent<FixedJoint>());
            released.Invoke(new InteractEventArgs(input.Controller, heldObject.Rigidbody, heldObject.Collider));
            heldObject.OnObjectReleased(input.Controller);
        }

        #region Joint
        private FixedJoint AddJoint (Rigidbody _rigidbody)
        {
            FixedJoint joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = _rigidbody;
            joint.breakForce = 20000;
            joint.breakTorque = 20000;
            return joint;
        }

        private void RemoveJoint(FixedJoint _joint)
        {
            if(_joint != null)
            {
                _joint.connectedBody = null; //Disconnect first and then set rigidbody
                Destroy(_joint);
                heldObject.Rigidbody.velocity = input.Controller.velocity;
                heldObject.Rigidbody.angularVelocity = input.Controller.AngularVelocity;
            }
        }
        #endregion
    }
}