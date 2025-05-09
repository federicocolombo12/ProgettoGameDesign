using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyAction : Action
{
	protected Rigidbody2D rb;
	protected Enemy enemy;
	protected Collider2D col;
	protected Animator anim;
	protected Player player;
	public override void OnAwake()
	{
		rb = GetComponent<Rigidbody2D>();
		enemy = GetComponent<Enemy>();
		col = GetComponent<Collider2D>();
		anim = gameObject.GetComponentInChildren<Animator>();
		player = Player.Instance;
	}

	
}