using UnityEngine;
using System.Collections;

[AddComponentMenu("Playground/Movement/Follow Target")]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Physics2DObject
{
    GameObject enemyBullet;
    bool isSearch = false;
	// This is the target the object is going to move towards
	public Transform target;

	[Header("Movement")]
	// Speed used to move towards the target
	public float speed;

	// Used to decide if the object will look at the target while pursuing it
	public bool lookAtTarget = false;

	// The direction that will face the target
	public Enums.Directions useSide = Enums.Directions.Up;
    void Start()
    {
		StartCoroutine(EnemyBulletCooldown());
    }

    void Update()
    {
		if (target == null)
			return;
		Search();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
	{
		//do nothing if the target hasn't been assigned or it was detroyed for some reason
		if (target == null)
			return;

		if (isSearch == true)
		{
            //look towards the target
            if (lookAtTarget)
			{
				Utils.SetAxisTowards(useSide, transform, target.position - transform.position);
			}
			//Move towards the target
			rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * speed));
		}

	}

	void Search()
	{

		float distance = Vector3.Distance(target.transform.position, transform.position);

		if (distance <= 13)
			isSearch = true;
		else
			isSearch = false;
	}
    IEnumerator EnemyBulletCooldown()
    {
		if (isSearch == true)
		{
			enemyBullet = GameManager.Instance.poolManager.Get(2);
			enemyBullet.transform.position = transform.position;
			enemyBullet.transform.rotation = transform.rotation;
		}
        yield return new WaitForSeconds(2f);
        StartCoroutine(EnemyBulletCooldown());
    }
}
