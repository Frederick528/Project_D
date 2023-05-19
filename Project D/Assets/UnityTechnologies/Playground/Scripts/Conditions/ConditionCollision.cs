using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Playground/Conditions/Condition Collision")]
[RequireComponent(typeof(Collider2D))]
public class ConditionCollision : ConditionBase
{
    HealthSystemAttribute hS;
    private void Start()
    {
        hS = GameObject.FindWithTag("Player").GetComponent<HealthSystemAttribute>() as HealthSystemAttribute;
    }

    //This will create a dialog window asking for which dialog to add
    private void Reset()
    {
        Utils.Collider2DDialogWindow(this.gameObject, false);
    }

    // This function will be called when something touches the trigger collider
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag(filterTag)|| !filterByTag && hS.health == 1)
        {
            ExecuteAllActions(collision.gameObject);
        }

    }

    //private void Update()
    //{
    // if (hS.health == 0)
    //        ExecuteAllActions(this.gameObject);

    //}
}
