using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeStage : MonoBehaviour
{
    public enum BeforeType
    {
        Up,
        Down,
        Right,
        Left
    }
    public BeforeType beforeType = BeforeType.Left;
    Vector2 start;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (beforeType == BeforeType.Up)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(start.x, start.y + 2), Time.deltaTime);
        else if (beforeType == BeforeType.Down)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(start.x, start.y - 2), Time.deltaTime);
        else if (beforeType == BeforeType.Right)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(start.x + 2, start.y), Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(start.x - 2, start.y),Time.deltaTime);
    }
}
