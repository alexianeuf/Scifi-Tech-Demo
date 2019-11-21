using UnityEngine;

public class LookX : MonoBehaviour
{
    [SerializeField]private float _sensitivity = 1.0f;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.y += mouseX * _sensitivity;
        transform.localEulerAngles = newRotation;
    }
}
