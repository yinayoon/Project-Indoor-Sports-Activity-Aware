using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 10.0f;
    public float zoomSpeed = 10.0f;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Rotate();
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            mainCamera.fieldOfView += distance;
        }
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 rot = transform.rotation.eulerAngles; // 현재 카메라의 각도를 Vector3로 반환
            rot.y += Input.GetAxis("Mouse X") * rotateSpeed; // 마우스 X 위치 * 회전 스피드
            rot.x += -1 * Input.GetAxis("Mouse Y") * rotateSpeed; // 마우스 Y 위치 * 회전 스피드
            Quaternion q = Quaternion.Euler(rot); // Quaternion으로 변환
            q.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f); // 자연스럽게 회전
        }
    }

    public void OriginPosRot()
    {
        transform.position = new Vector3(-107, 0, 89.5f);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        mainCamera.fieldOfView = 60;
    }
}
