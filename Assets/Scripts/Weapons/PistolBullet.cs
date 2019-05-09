using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : Bullet {

	void Start () {
		if (duration == 0)
		{
			duration = 3;
		}
		Destroy(gameObject, duration);
	}
	

	public override void dealDamage()
	{
		enemy.dealDamage(damage);
		destroy();
	}
}
