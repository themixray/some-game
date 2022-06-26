using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector;

public class CameraController : MonoBehaviour
{
    public Transform head;
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public Vector2 moveSpeed = new Vector2(1,1);
    public Vector2 moveStrong = new Vector2(5,5);
    public Vector2 angle = new Vector2(0,0);
    public bool back = true;
    bool pressed;

    public int indexList, indexLookPoint;
    public float offSetPlayerPivot;
    public string currentStateName;
    public Transform currentTarget;
    public Vector2 movementSpeed;
    Transform targetLookAt;
    Vector3 currentTargetPos;
    Vector3 current_cPos;
    Vector3 desired_cPos;
    Camera _camera;
    float distance = 5f;
    float mouseY = 0f;
    float mouseX = 0f;
    float currentHeight;
    float cullingDistance;
    float checkHeightRadius = 0.4f;
    float clipPlaneMargin = 0f;
    float forward = -1f;
    float xMinLimit = -360f;
    float xMaxLimit = 360f;
    float cullingHeight = 0.2f;
    float cullingMinDist = 0.1f;
    public float smoothCameraRotation = 12f;
    public LayerMask cullingLayer = 1 << 0;
    public bool lockCamera;
    public float rightOffset = 0f;
    public float defaultDistance = 2.5f;
    public float height = 1.4f;
    public float smoothFollow = 10f;
    public float xMouseSensitivity = 3f;
    public float yMouseSensitivity = 3f;
    public float yMinLimit = -40f;
    public float yMaxLimit = 80f;
    float X, Y;

    bool CullingRayCast(Vector3 from, ClipPlanePoints _to, out RaycastHit hitInfo, float distance, LayerMask cullingLayer, Color color)
    {
        bool value = false;

        if (Physics.Raycast(from, _to.LowerLeft - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.LowerRight - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.UpperLeft - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        if (Physics.Raycast(from, _to.UpperRight - from, out hitInfo, distance, cullingLayer))
        {
            value = true;
            if (cullingDistance > hitInfo.distance) cullingDistance = hitInfo.distance;
        }

        return hitInfo.collider && value;
    }


    public void RotateCamera(float x, float y)
    {
        // free rotation 
        mouseX += x * xMouseSensitivity;
        mouseY -= y * yMouseSensitivity;

        movementSpeed.x = x;
        movementSpeed.y = -y;
        if (!lockCamera)
        {
            mouseY = vExtensions.ClampAngle(mouseY, yMinLimit, yMaxLimit);
            mouseX = vExtensions.ClampAngle(mouseX, xMinLimit, xMaxLimit);
        }
        else
        {
            mouseY = currentTarget.root.localEulerAngles.x;
            mouseX = currentTarget.root.localEulerAngles.y;
        }
    }

    void CameraMovement()
    {
        if (currentTarget == null)
            return;

        distance = Mathf.Lerp(distance, defaultDistance, smoothFollow * Time.deltaTime);
        cullingDistance = Mathf.Lerp(cullingDistance, distance, Time.deltaTime);
        var camDir = (forward * targetLookAt.forward) + (rightOffset * targetLookAt.right);

        camDir = camDir.normalized;

        var targetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);
        currentTargetPos = targetPos;
        desired_cPos = targetPos + new Vector3(0, height, 0);
        current_cPos = currentTargetPos + new Vector3(0, currentHeight, 0);
        RaycastHit hitInfo;

        ClipPlanePoints planePoints = _camera.NearClipPlanePoints(current_cPos + (camDir * (distance)), clipPlaneMargin);
        ClipPlanePoints oldPoints = _camera.NearClipPlanePoints(desired_cPos + (camDir * distance), clipPlaneMargin);

        //Check if Height is not blocked 
        if (Physics.SphereCast(targetPos, checkHeightRadius, Vector3.up, out hitInfo, cullingHeight + 0.2f, cullingLayer))
        {
            var t = hitInfo.distance - 0.2f;
            t -= height;
            t /= (cullingHeight - height);
            cullingHeight = Mathf.Lerp(height, cullingHeight, Mathf.Clamp(t, 0.0f, 1.0f));
        }

        //Check if desired target position is not blocked       
        if (CullingRayCast(desired_cPos, oldPoints, out hitInfo, distance + 0.2f, cullingLayer, Color.blue))
        {
            distance = hitInfo.distance - 0.2f;
            if (distance < defaultDistance)
            {
                var t = hitInfo.distance;
                t -= cullingMinDist;
                t /= cullingMinDist;
                currentHeight = Mathf.Lerp(cullingHeight, height, Mathf.Clamp(t, 0.0f, 1.0f));
                current_cPos = currentTargetPos + new Vector3(0, currentHeight, 0);
            }
        }
        else
        {
            currentHeight = height;
        }
        //Check if target position with culling height applied is not blocked
        if (CullingRayCast(current_cPos, planePoints, out hitInfo, distance, cullingLayer, Color.cyan)) distance = Mathf.Clamp(cullingDistance, 0.0f, defaultDistance);
        var lookPoint = current_cPos + targetLookAt.forward * 2f;
        lookPoint += (targetLookAt.right * Vector3.Dot(camDir * (distance), targetLookAt.right));
        targetLookAt.position = current_cPos;

        Quaternion newRot = Quaternion.Euler(mouseY, mouseX, 0);
        targetLookAt.rotation = Quaternion.Slerp(targetLookAt.rotation, newRot, smoothCameraRotation * Time.deltaTime);
        transform.position = current_cPos + (camDir * (distance));
        var rotation = Quaternion.LookRotation((lookPoint) - transform.position);

        transform.rotation = rotation;
        movementSpeed = Vector2.zero;
    }

    public void Init()
    {
        if (target == null)
            return;

        _camera = GetComponent<Camera>();
        currentTarget = target;
        currentTargetPos = new Vector3(currentTarget.position.x, currentTarget.position.y + offSetPlayerPivot, currentTarget.position.z);

        targetLookAt = new GameObject("targetLookAt").transform;
        targetLookAt.position = currentTarget.position;
        targetLookAt.hideFlags = HideFlags.HideInHierarchy;
        targetLookAt.rotation = currentTarget.rotation;

        mouseY = currentTarget.eulerAngles.x;
        mouseX = currentTarget.eulerAngles.y;

        distance = defaultDistance;
        currentHeight = height;
    }

     void Start()
    {
        Init();
    }

     void Update()
    {
        X = Input.GetAxis("Mouse X");
        Y = Input.GetAxis("Mouse Y");
        RotateCamera(X,Y);

        CameraMovement();
    }


    /* public Transform Obstruction;

     void CameraRotate()
     {
         angle.x += Input.GetAxis("Mouse X") * 5;
         angle.y -= Input.GetAxis("Mouse Y") * 5;
         //angle.y = Mathf.Clamp();


         target.rotation = Quaternion.Euler(0, angle.x, angle.y);
     }

     void ViewObstructed()
     {
         RaycastHit hit;

         if(Physics.Raycast(transform.position,target.position - transform.position,out hit, 4.5f))
         {
             if (hit.collider.gameObject.tag != "Player")
             {
                 Obstruction = hit.transform;
                 Obstruction.gameObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                 if (Vector3.Distance(Obstruction.position, transform.position) >= 3 && Vector3.Distance(transform.position, target.position) >= 1.5f) transform.Translate(Vector3.forward * Time.deltaTime * 5);
             }
             else
             {
                 Obstruction.gameObject.GetComponentInChildren<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                 if (Vector3.Distance(transform.position, target.position) < 4.5f) transform.Translate(Vector3.forward * Time.deltaTime * 5);
             }
         }
     }

     void Start()
     {
         Obstruction = target;
         angle.x = 0;
         angle.y = 0;
     }


     void FixedUpdate()
     {
         ViewObstructed();
         if (back)
         {
             target.rotation = Quaternion.Euler(0, 0, 0);
             transform.position = Vector3.Lerp(transform.position, 
               head.position + new Vector3(head.right.x * moveStrong.x, offset.y, head.right.z * moveStrong.x), smoothSpeed);

         }
         if(!back && pressed)
         {

             CameraRotate();
         }
         transform.LookAt(target);

         if (Input.GetMouseButton(1))
         {

             pressed = true;
             back = false;
         }
         else pressed = false;
     }*/
}
