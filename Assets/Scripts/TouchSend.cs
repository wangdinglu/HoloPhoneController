using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MixOne
{
    public class TouchSend : MonoBehaviour
    {

        public static bool testMode = false;
        private Vector2 dualTouchDistance,dualMoveDistance ;
        private Vector2 moveDelta;
        private float tapTime = 0.3f;
        private float holdTime = 0.8f;

        private PhoneClient pc;
        public Text t;
        public Toggle sendToggle;

        private float dualDeltaDistance;
        private bool tap, swipeLeft, swipeRight, swipeTop, swipeDown;
        private Vector2 startTouch,moveTouch;
        private Vector2 startDualTouch, moveDualTouch;

        private float deltaTime,startTime;

        private enum status
        {
            Free,
            SingleTap,
            SingleMove,
            SingleHold,
            DualTap,
            DualMove,
            DualHold,
            DualZoomOut,
            DualZoomIn,

        }
        private status touchStatus;

        // Start is called before the first frame update
        void Start()
        {
            touchStatus = status.Free;
            pc = GameObject.Find("ClientManager").GetComponent<PhoneClient>();
        }

        private void Reset()
        {
            startTouch = moveDelta = moveTouch = dualTouchDistance = Vector2.zero;
            touchStatus = status.Free;
        }

        // Update is called once per frame
        void Update()
        {
            //#region Standalone Inputs
            //if (Input.GetMouseButton(0))
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        startTouch = Input.mousePosition;
            //        deltaTime = 0;
            //        startTime = Time.time;
            //        moveTouch = Input.mousePosition;

            //    }
            //    else
            //    {
            //        deltaTime = Time.time - startTime;
            //        moveDelta = (Vector2)Input.mousePosition - startTouch;

            //        if (moveDelta.magnitude < 20 && deltaTime > holdTime)
            //        {
            //            if (touchStatus != status.SingleHold && touchStatus != status.SingleMove)
            //            {
            //                pc.Send("Touch:Hold");
            //                Debug.Log("Hold");
            //                touchStatus = status.SingleHold;
            //            }

            //        }
            //        //else if (deltaTime > tapTime && moveDelta.magnitude > 20)
            //        //{
            //        //    pointer.transform.position = Input.mousePosition;

            //        //}
            //    }
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    deltaTime = Time.time - startTime;
            //    moveDelta = (Vector2)Input.mousePosition - startTouch;
            //    //t.text ="Touch end in " + deltaTime.ToString());
            //    if (deltaTime < tapTime)
            //    {
            //        if (moveDelta.magnitude < 20)
            //        {
            //            pc.Send("Touch:Tap");
            //            Debug.Log("Tap");
            //            touchStatus = status.SingleTap;

            //        }
            //        else if (moveDelta.magnitude > 50)
            //        {
            //            float x = moveDelta.x;
            //            float y = moveDelta.y;
            //            if (Mathf.Abs(x) > Mathf.Abs(y))
            //            {
            //                if (x < 0)
            //                {
            //                    pc.Send("Touch:Left");
            //                    Debug.Log("Left");
            //                }
            //                else
            //                {
            //                    pc.Send("Touch:Right");
            //                    Debug.Log("Right");
            //                }
            //            }
            //            else
            //            {
            //                if (y < 0)
            //                {
            //                    pc.Send("Touch:Down");
            //                    Debug.Log("Down");
            //                }
            //                else
            //                {
            //                    pc.Send("Touch:Up");
            //                    Debug.Log("Up");
            //                }
            //            }
            //        }
            //    }
            //    Reset();
            //}
            //else if (Input.GetMouseButton(1))
            //{
            //    if (Input.GetMouseButtonDown(1))
            //    {
            //        tap = true;
            //        dualTouchDistance = (Vector2)Input.mousePosition;
            //        deltaTime = 0;
            //        startTime = Time.time;
            //        moveTouch = Input.mousePosition;
            //    }
            //    else
            //    {
            //        deltaTime = Time.time - startTime;
            //        dualMoveDistance = (Vector2)Input.mousePosition - dualTouchDistance;
            //        dualDeltaDistance = dualMoveDistance.magnitude;
            //        if (deltaTime > holdTime && dualDeltaDistance < 20)
            //        {
            //            if (touchStatus != status.DualHold && touchStatus != status.DualMove)
            //            {
            //                t.text ="Dual Hold";
            //                touchStatus = status.DualHold;
            //            }
            //        }
            //        else if (deltaTime > tapTime && dualDeltaDistance > 20)
            //        {
            //            moveDelta = (Vector2)Input.mousePosition - moveTouch;

            //            t.text ="Right Moveing x: " + moveDelta.x.ToString() + " y: " + moveDelta.y.ToString();
            //            touchStatus = status.DualMove;
            //            moveTouch = Input.mousePosition;
            //        }
            //    }
            //}
            //else if (Input.GetMouseButtonUp(1))
            //{
            //    deltaTime = Time.time - startTime;
            //    dualMoveDistance = (Vector2)Input.mousePosition - dualTouchDistance;
            //    dualDeltaDistance = dualMoveDistance.magnitude;
            //    if (deltaTime < tapTime)
            //    {
            //        if (dualDeltaDistance < 20)
            //        {
            //            t.text ="Dual Tap";
            //        }
            //    }
            //    Reset();
            //}
            //#endregion
            //#region Mobile Inputs
            if (Input.touchCount == 1)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    tap = true;
                    startTouch = Input.touches[0].position;
                    deltaTime = 0;
                    startTime = Time.time;
                    moveTouch = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
                {
                    deltaTime = Time.time - startTime;
                    moveDelta = Input.touches[0].position - startTouch;

                    if (deltaTime < tapTime)
                    {
                        if (moveDelta.magnitude < 50)
                        {
                            t.text ="Tap";
                            if(sendToggle.isOn)
                                pc.Send("Touch:Tap");
                        }
                        else if (moveDelta.magnitude > 100 && deltaTime > 0.1)
                        {
                            float x = moveDelta.x;
                            float y = moveDelta.y;
                            if (Mathf.Abs(x) > Mathf.Abs(y))
                            {
                                if (x < 0)
                                {
                                    t.text ="Left";
                                    if (sendToggle.isOn)
                                        pc.Send("Touch:Left");

                                }
                                else
                                {
                                    t.text ="Right";
                                    if (sendToggle.isOn)
                                        pc.Send("Touch:Right");

                                }
                            }
                            else if((Mathf.Abs(x) < Mathf.Abs(y)))
                            {
                                if (y < 0)
                                {
                                    t.text ="Down";
                                    if (sendToggle.isOn)
                                        pc.Send("Touch:Down");

                                }
                                else
                                {
                                    t.text ="Up";
                                    if (sendToggle.isOn)
                                        pc.Send("Touch:Up");

                                }
                            }
                        }
                    }

                    Reset();
                }
                else if (Input.touches[0].phase == TouchPhase.Stationary)
                {
                    deltaTime = Time.time - startTime;
                    moveDelta = Input.touches[0].position - startTouch;
                    if (moveDelta.magnitude < 50 && deltaTime > holdTime)
                    {
                        if(touchStatus!= status.SingleHold)
                        {
                            t.text ="Hold";
                            if (sendToggle.isOn)
                                pc.Send("Touch:Hold");

                            touchStatus = status.SingleHold;
                        }
                        
                    }
                    if (deltaTime > tapTime && moveDelta.magnitude > 100)
                    {
                        t.text ="Stop in Moving ";
                        touchStatus = status.SingleMove;
                    }
                }
                else if (Input.touches[0].phase == TouchPhase.Moved)
                {
                    deltaTime = Time.time - startTime;
                    if (deltaTime > tapTime && touchStatus != status.SingleHold)
                    {
                        moveDelta = Input.touches[0].position - moveTouch;
                        moveTouch = Input.touches[0].position;
                        t.text ="Moving x: " + moveDelta.x.ToString() + " y: " + moveDelta.y.ToString();
                        if (sendToggle.isOn)
                            pc.Send("Touch:x/" + moveDelta.x.ToString() + "/y/" + moveDelta.y.ToString());
                    }
                }
            }
            else if (Input.touchCount == 2)
            {
                if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
                {
                    tap = true;
                    dualTouchDistance = Input.touches[0].position - Input.touches[1].position;
                    deltaTime = 0;
                    startTime = Time.time;
                    startDualTouch = Input.touches[0].position + Input.touches[1].position;

                }
                else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled || Input.touches[1].phase == TouchPhase.Canceled)
                {
                    deltaTime = Time.time - startTime;
                    dualMoveDistance = Input.touches[0].position - Input.touches[1].position;
                    dualDeltaDistance = dualMoveDistance.magnitude - dualTouchDistance.magnitude;
                    moveDualTouch = Input.touches[0].position + Input.touches[1].position - startDualTouch;
                    if (deltaTime < tapTime)
                    {
                        if (dualDeltaDistance < 20 && moveDualTouch.magnitude < 20)
                        {
                            t.text ="DualTap";
                            if (sendToggle.isOn)
                                pc.Send("Touch:DualTap");

                        }
                    }
                    Reset();
                }
                else if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary || Input.touches[1].phase == TouchPhase.Stationary)
                {
                    deltaTime = Time.time - startTime;
                    dualMoveDistance = Input.touches[0].position - Input.touches[1].position;
                    dualDeltaDistance = dualMoveDistance.magnitude - dualTouchDistance.magnitude;
                    moveDualTouch = Input.touches[0].position + Input.touches[1].position - startDualTouch;

                    if (deltaTime > tapTime)
                    {
                        if (dualDeltaDistance > 5 )
                        {
                            if (touchStatus != status.DualZoomOut && touchStatus != status.DualHold)
                            {
                                t.text ="Dual Zoom Out";
                                if (sendToggle.isOn)
                                    pc.Send("Touch:ZoomOut");

                                touchStatus = status.DualZoomOut;
                            }
                            
                        }
                        else if (dualDeltaDistance < -5)
                        {
                            if (touchStatus != status.DualZoomIn && touchStatus != status.DualHold)
                            {
                                t.text ="Dual Zoom In";
                                if (sendToggle.isOn)
                                    pc.Send("Touch:ZoomIn");

                                touchStatus = status.DualZoomIn;
                            }
                        }
                        else if (moveDualTouch.magnitude<20 && Mathf.Abs(dualDeltaDistance)<5)
                        {
                            if (touchStatus != status.DualHold)
                            {
                                t.text ="Dual Hold";
                                if (sendToggle.isOn)
                                    pc.Send("Touch:DualHold");
                                touchStatus = status.DualHold;
                            }
                           
                        }

                    }
                }
            }


            //#endregion
        }


    }

}
