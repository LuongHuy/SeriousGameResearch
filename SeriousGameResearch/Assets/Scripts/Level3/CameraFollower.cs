using UnityEngine;

public class CameraFollower : MonoBehaviour
{

    [SerializeField]
    private Transform targer;
    [SerializeField]
    private float factorY;

    private void LateUpdate()
    {
        if(targer == null)
        {
            return;
        }

        transform.position = new Vector3(0, targer.position.y + factorY, transform.position.z);
    }
}
