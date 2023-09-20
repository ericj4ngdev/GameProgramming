using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public GameObject bullet;
    public Transform firePoint;
    public GameObject slowEffect;

    public Transform characterBody;
    public Transform cameraTransform;
    public float mouseSensitivity = 7f;

    [Header("Debug")]
    public float mouseXInput;
    public float mouseYInput;
    private float xRotation = 0f;

    public void Follow()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 3;
        // 마우스 커서를 화면안에 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        OnBulletTime();

        PlayerView();

        Move();
    }

    void OnBulletTime()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Time.timeScale = 0.2f;
            moveSpeed = 3 / 0.2f;
            slowEffect.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Time.timeScale = 1f;
            moveSpeed = 3;
            slowEffect.SetActive(false);
        }
    }

    void PlayerView()
    {
        mouseXInput = Input.GetAxisRaw("Mouse X");
        mouseYInput = Input.GetAxisRaw("Mouse Y");
        Vector2 mouseDelta = new Vector2(mouseXInput * mouseSensitivity, mouseYInput * mouseSensitivity);

        xRotation -= mouseDelta.y;
        // 위아래 내려보는 각도 범위 제한
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        characterBody.Rotate(Vector3.up * mouseDelta.x);
    }

    void Move()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        transform.position += transform.rotation * new Vector3(targetVelocity.x, 0, targetVelocity.y) * Time.deltaTime;
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint);
    }
}
