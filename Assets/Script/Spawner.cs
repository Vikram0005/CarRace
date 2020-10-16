using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] prefabs;

    Vector2 screenhalfSizeWorldUnits;

    private void Start()
    {
        screenhalfSizeWorldUnits = new Vector2((Camera.main.aspect*Camera.main.orthographicSize)-1,Camera.main.orthographicSize+2);
        InvokeRepeating("Spawn", 5, Random.Range(2f,3f));
    }

    private void Spawn()
    {
        if (!GameManager.manager.isGameStart) return;
        if (CarController.controller.carSpeed < 2) return;
        Here:
        int randomObject = Random.Range(0, prefabs.Length);
        if (randomObject == 1 && GameManager.manager.damageSlider.value == 1)//randomObject=1 means Health Kit
            goto Here;
        Vector2 randomPosition = new Vector2(Random.Range(-screenhalfSizeWorldUnits.x,screenhalfSizeWorldUnits.x),screenhalfSizeWorldUnits.y);
        Instantiate(prefabs[randomObject],randomPosition,Quaternion.identity);
    }

}
