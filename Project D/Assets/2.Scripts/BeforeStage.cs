using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStage : MonoBehaviour
{
    Vector2 start;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(start.x - 2, start.y),Time.deltaTime);
    }
}
