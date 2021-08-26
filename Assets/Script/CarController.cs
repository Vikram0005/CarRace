using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static CarController controller;
    private void Awake()
    {
        controller = this;
    }

    public float carSpeed;

    private Vector3 currentPosition;
    private float direction;
    private float movementAmount;
    private bool speedUp;
    private bool speedDown;

    private int speedMultiply;
 
    void Start()
    {
        currentPosition = transform.position;
        movementAmount = currentPosition.x;
    }

    void Update()
    {
        if (!GameManager.manager.isGameStart) return;
        movementAmount += direction * carSpeed * Time.deltaTime;
        movementAmount = Mathf.Clamp(movementAmount, -2, 2);
        transform.position = new Vector2(movementAmount, currentPosition.y);

        if (speedUp)
        {
            if (carSpeed < 10)
                carSpeed += Time.deltaTime * speedMultiply;
            else
                carSpeed = 10;
        }

        if (speedDown)
        {
            if (carSpeed > 0)
                carSpeed -= Time.deltaTime * speedMultiply;
            else
                carSpeed = 0;
        }

        CarSpeedCalculate(carSpeed);
        ExpectedTimeCalculate(100, carSpeed);
        TimeUpdate();
    }

    public void CarSpeedCalculate(float speed)
    {
        GameManager.manager.carSpeedText.text = "Car Speed : " + (speed * 10).ToString("0.0");
    }

    public int ExpectedTimeCalculate(float distance, float speed)
    {
        if (speed == 0)
        {
            GameManager.manager.expectedTimeText.text = "ETA : infinity";
            return 0;
        }
        int totalTimeInMin = (int)(distance / (speed * 10) * 60);
        int hour = totalTimeInMin / 60;
        int min = totalTimeInMin - hour * 60;

        GameManager.manager.expectedTimeText.text = "ETA :" + hour.ToString() + " hour " + min.ToString() + " min";
        return totalTimeInMin;
    }

    public void TimeUpdate()
    {
        if (!GameManager.manager.isGameStart) return;
        GameManager.manager.timer +=Time.deltaTime;
        int min =(int)GameManager.manager.timer / 60;
        int sec =(int)GameManager.manager.timer - (min * 60);
        GameManager.manager.timeText.text = min.ToString("00") + " min : " + sec.ToString("00")+" sec";
        if(min==10)
            GameManager.manager.GameOver();
    }

   public IEnumerator CarSlowDownAftertriggerWithObstacle(float slowDownBy)
    {
        float t = 0;
        float actualSpeed = carSpeed;
        float slowBy = carSpeed / slowDownBy;
        while(t<=1)
        {
            t += Time.deltaTime;
            carSpeed = Mathf.Lerp(actualSpeed,slowBy,t);
            yield return null;
        }
    }

    #region Car Controller
    public void OnPointerDown(int dir)
    {
        direction = dir;
    }

    public void OnPointerExit()
    {
        direction = 0;
    }

    public void SpeedUp(int multiplier)
    {
        speedUp = true;
        speedMultiply = multiplier;
    }

    public void SpeedDown(int multiplier)
    {
        speedDown = true;
        speedMultiply = multiplier;
    }

    public void OnSpeedExit()
    {
        speedUp = false;
        speedDown = false;
    }
    #endregion
}
