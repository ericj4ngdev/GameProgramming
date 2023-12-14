using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Rigidbody rb;

    public float hp;
    public GameObject bullet;
    public Transform firePoint;
    public GameObject slowEffect;
    public ParticleSystem muzzleEffect;
    public AudioSource shootSound;

    [Header("Pause")]
    public GameObject pauseUI;
    private bool isPaused = false;

    [Header("Mouse Controll view")]
    public Transform characterBody;
    public Transform cameraTransform;
    public float mouseSensitivity = 7f;

    [Header("Debug")]
    public float mouseXInput;
    public float mouseYInput;
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 3;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;       // ���콺 Ŀ���� ȭ��ȿ� ���
    }

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape)) { Pause(); }
    }

    private void GetControl()
    {
        if (Input.GetMouseButtonDown(0)) { Shoot(); }
        OnBulletTime();
        PlayerView();
    }

    private void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            StopCoroutine(Control());
            moveSpeed = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0; // ���� �ð��� �����մϴ�.
            pauseUI.SetActive(true); // �Ͻ����� UI�� Ȱ��ȭ�մϴ�.
        }
        else
        {
            StartCoroutine(Control());
            moveSpeed = 5;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1; // ���� �ð��� �ٽ� �����մϴ�.
            pauseUI.SetActive(false); // �Ͻ����� UI�� ��Ȱ��ȭ�մϴ�.
        }
    }
    
    IEnumerator Control()
    {
        while (!isPaused)
        {
            GetControl();
            yield return null;
        }
    }

    private void FixedUpdate()
    {
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
        // ���Ʒ� �������� ���� ���� ����
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        characterBody.Rotate(Vector3.up * mouseDelta.x);
    }

    void Move()
    {
        // xz ���󿡼� ������ �Է�
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed);
        rb.velocity = transform.rotation * new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.y);
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint);
        muzzleEffect.Play();
        shootSound.Play();
    }



    public void TakeDamage()
    {
        Debug.Log("Player Damaged");
        if (hp > 0) hp--;
        else hp = 0;
    }
}
