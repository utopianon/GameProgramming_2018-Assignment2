using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.AI
{

    public class ShootState : AIStateBase
    {
        public float SqrShootingDistance
        {
            get { return Owner.ShootingDistance * Owner.ShootingDistance; }
        }

        public ShootState(EnemyUnit owner)
            : base(owner, AIStateType.Shoot)
        {
            AddTransition(AIStateType.Patrol);
            AddTransition(AIStateType.FollowTarget);
        }
        public override void Update()
        {
            //if the player is alive and within shooting range
            //follows the player and shoots
            if (!ChangeState())
            {
                Owner.Mover.Move(Owner.transform.forward);
                Owner.Mover.Turn(Owner.Target.transform.position);
                Owner.Weapon.Shoot();
            }
        }

        private bool ChangeState()
        {
            //Is the player active?
            //if yes, go to patrol state.
            if (Owner.Target.isActiveAndEnabled == false)
            {
                return Owner.PerformTransition(AIStateType.Patrol);
            }


           //Is the player no longer at shooting range?
           // If yes, go to follow target state.
            Vector3 toPlayerVector =
            Owner.transform.position - Owner.Target.transform.position;
            float sqrDistanceToPlayer = toPlayerVector.sqrMagnitude;
            if (sqrDistanceToPlayer > SqrShootingDistance)
            {
                return Owner.PerformTransition(AIStateType.FollowTarget);
            }

            return false;

        }
    }
}
