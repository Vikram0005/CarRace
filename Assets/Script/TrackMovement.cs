using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    public Transform endPoint;
    public Transform startPoint;

    void Update()
    {
        if (!GameManager.manager.isGameStart) return;
        transform.Translate(Vector3.down*CarController.controller.carSpeed*Time.deltaTime);
        if (transform.position.y <= endPoint.position.y)
            transform.position = startPoint.position;
    }
}
                                                                