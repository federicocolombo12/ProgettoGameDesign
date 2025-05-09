using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro.Examples;

public class Shoot : EnemyAction
{
    public bool shakeCamera;
    public List<Weapon> weapons;
	
	public override void OnStart()
	{
		
	}

	public override TaskStatus OnUpdate()
	{
		foreach (var weapon in weapons)
        {
            var projectile = Object.Instantiate(weapon.projectilePrefab, weapon.weaponTransform.position, weapon.weaponTransform.rotation);
            projectile.shooter = gameObject;
			var force = new Vector2(weapon.horizontalForce * transform.localScale.x, weapon.verticalForce);
			projectile.SetForce(force);
			if (shakeCamera)
			{ 
				CameraManager.Instance.ShakeCamera(0.2f);
			}
        }
		return TaskStatus.Success;
	}
}