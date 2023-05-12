using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraCtrl : MonoBehaviour
{
    Transform playerTransform;
    Vector3 cameraPos;
    Vector2 center;
    Vector2 mapSize;
    float cameraSpeed;
    float cameraHeight;
    float cameraWidth;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameManager.Instance.playerCtrl.transform;
        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPos,cameraSpeed * Time.deltaTime);
        float mx = mapSize.x - cameraWidth;
        float clampX = Mathf.Clamp(transform.position.x, -mx + center.x, mx + center.x);
        float my = mapSize.y - cameraHeight;
        float clampY = Mathf.Clamp(transform.position.y, -my + center.y, my + center.y);
        transform.position = new Vector2(clampX, clampY);
    }
}
