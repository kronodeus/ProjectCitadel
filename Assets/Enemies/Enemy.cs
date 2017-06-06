﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{
	//[SerializeField] float meleeDamage = 10f;
	[SerializeField] float rangedDamage = 10f;
	[SerializeField] float chaseRadius = 8f;
	[SerializeField] float attackRadius = 3f;
	[SerializeField] Projectile projectile;
	[SerializeField] GameObject projectileSocket;
	[SerializeField] Vector3 aimOffset = Vector3.up;

	AICharacterControl ai;
	GameObject player;
	bool isAttacking = false;

	private void Awake()
	{
		ai = GetComponent<AICharacterControl>();
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		float distance = Vector3.Distance(player.transform.position, transform.position);
		if (distance <= chaseRadius)
		{
			BeginChase();
		}
		else
		{
			EndChase();
		}

		if (distance <= attackRadius)
		{
			if (!isAttacking)
			{
				BeginAttack();
			}
		}
		else
		{
			if (isAttacking)
			{
				EndAttack();
			}
		}
	}

	void BeginChase()
	{
		ai.SetTarget(player.transform);
	}

	void EndChase()
	{
		ai.SetTarget(null);aaa
	}

	void BeginAttack()
	{
		isAttacking = true;
		InvokeRepeating("FireProjectile", 0f, 1f);
	}

	void EndAttack()
	{
		CancelInvoke();
		isAttacking = false;
	}

	void FireProjectile()
	{
		var launchedProjectile = Instantiate(projectile, projectileSocket.transform.position, Quaternion.identity);
		var speed = launchedProjectile.speed;
		var direction = ((player.transform.position + aimOffset) - projectileSocket.transform.position).normalized;
		launchedProjectile.SetDamage(rangedDamage);
		launchedProjectile.GetComponent<Rigidbody>().velocity = direction * speed;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, chaseRadius);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);
	}
}
