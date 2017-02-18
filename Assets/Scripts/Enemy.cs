using UnityEngine;
using System.Collections;
using System;

public class Enemy : MovingObject {

    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

	protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
	}
	
	

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove) {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy() {
        int xdir = 0;
        int ydir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) 
        {
            ydir = target.position.y > transform.position.y ? 1 : -1;
        }
        else {
            xdir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xdir, ydir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("enemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }
}
