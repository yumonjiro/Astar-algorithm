using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public Transform followTransform;
    public float lookPower;
    public Vector2 _look;
    public bool isLookActive = false;
    public void OnLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }
    public void OnMousePress(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            Debug.Log("called 1");

            isLookActive = true;
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("called 2");

            isLookActive = false;
        }
    }
    
    public void ChangeTarget(Transform newtarget)
    {
        followTransform = newtarget.transform;
        virtualCamera.Follow = followTransform;
        virtualCamera.LookAt = followTransform;
    }

    void Update()
    {
        #region Camera controll
        if(isLookActive)
        {
                //Horizontal camera rotation
            followTransform.rotation *= Quaternion.AngleAxis(_look.x * lookPower * Time.deltaTime, Vector3.up);
            //Vertical camera rotation
            followTransform.rotation *= Quaternion.AngleAxis(-_look.y * lookPower * Time.deltaTime, Vector3.right);
            // 回転の妥当性確認、z軸の固定。
            var angles = followTransform.localEulerAngles; //Euler angles compared to Player object
            if(angles.x > 40 && angles.x <180)
            {
                Debug.Log(angles);
                angles.x = 40;
            }
            else if(angles.x < 340 && angles.x >180)
            {
                Debug.Log(angles);
                angles.x = 340;
            }
            angles.z = 0;
            followTransform.localEulerAngles = angles;
        }
        #endregion
    }
}
