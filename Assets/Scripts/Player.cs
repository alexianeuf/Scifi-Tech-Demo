using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _characterController;
    [SerializeField] private float _speed = 3.5f;
    private float _gravity = 9.81f;

    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private GameObject _hitMarkerPrefab;

    [SerializeField] private AudioSource _weaponAudioSource;

    [SerializeField] private int currentAmmo;
    private int maxAmmo = 50;

    private bool isReloading = false;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.Log("Can't find a UIManger in the Canvas.");
        }

        if (_characterController == null)
        {
            Debug.Log("The CharacterController is NULL.");
        }

        if (_weaponAudioSource == null)
        {
            Debug.Log("The Player Audio Source is NULL.");
        }

        // hide mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        // if left click
        // cast ray from center point of main camera

        if (Input.GetMouseButton(0) && currentAmmo > 0)
        {
            Shoot();
        }
        else
        {
            _muzzleFlash.SetActive(false);
            _weaponAudioSource.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // show cursor again if asked
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }

        CalculateMovement();
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(1.5f);
        currentAmmo = maxAmmo;
        isReloading = false;
        _uiManager.UpdateAmmo(currentAmmo);
    }

    void Shoot()
    {
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        currentAmmo--;
        _uiManager.UpdateAmmo(currentAmmo);

        _muzzleFlash.SetActive(true);

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            GameObject hitMarker =
                Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(hitMarker, 1.0f);
        }

        if (!_weaponAudioSource.isPlaying)
        {
            _weaponAudioSource.Play();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;

        velocity.y -= _gravity;
        velocity = transform.transform.TransformDirection(velocity);
        _characterController.Move(velocity * Time.deltaTime);
    }
}