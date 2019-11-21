using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField]private float _sensitivity = 1.0f;


    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 newRotation = transform.localEulerAngles;
        newRotation.x -= mouseY * _sensitivity;
        transform.localEulerAngles = newRotation;
    }
}
