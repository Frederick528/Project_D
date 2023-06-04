using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Playground/Conditions/Condition Collision")]
[RequireComponent(typeof(Collider2D))]
public class ConditionCollision : ConditionBase
{
    private void Start()
    {
    }

    //This will create a dialog window asking for which dialog to add
    private void Reset()
    {
        Utils.Collider2DDialogWindow(this.gameObject, false);
    }


    private void Update()
    {
        if (PlayerCtrl.health <= 0)
        {
            ExecuteAllActions(gameObject);
        }
    }
    // This function will be called when something touches the trigger collider
    //void OnCollisionEnter2D(Collision2D collision)
    //{

    //    if (/*collision.collider.CompareTag(filterTag)|| !filterByTag && */PlayerCtrl.health <= 0)
    //    {
    //        ExecuteAllActions(collision.gameObject);
    //    }

    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (PlayerCtrl.health <= 0)
    //    {
    //        ExecuteAllActions(collision.gameObject);
    //    }
    //}

}
