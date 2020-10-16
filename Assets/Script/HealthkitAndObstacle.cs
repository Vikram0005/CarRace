using UnityEngine;

public class HealthkitAndObstacle : MonoBehaviour
{
    public Type objectType;
    private Transform car;
    private Transform destroyPosition;
    private void Start()
    {     
        car = GameObject.Find("Car").transform;
        destroyPosition = GameObject.Find("DestroyPosition").transform;
    }

    private void Update()
    {
        if (!GameManager.manager.isGameStart) return;
        transform.Translate(Vector3.down * CarController.controller.carSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, car.position);
        if (distance <= 1f)
        {
            if (objectType == Type.Coin)
                GameManager.manager.OnTriggerCoin();
            else if (objectType == Type.Healthkit)
                GameManager.manager.OnTriggerHealthkit(20);
            else if (objectType == Type.Obstacle)
                GameManager.manager.OnTriggerObstacle(20);
            Destroy(this.gameObject);
        }

        if (gameObject!=null && transform.position.y <= destroyPosition.position.y)
            Destroy(this.gameObject);
    }
   public enum Type { Healthkit,Obstacle,Coin}
}
