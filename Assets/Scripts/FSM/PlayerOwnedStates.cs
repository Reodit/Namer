using System.Collections;
using UnityEngine;

namespace PlayerOwnedStates
{
	public class IdleState : IState<PlayerEntity>
	{
		public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Idle;
            if (entity.pAnimator)
            {
                entity.pAnimator.SetBool("isMove", false);
            }
        }

		public void Execute(PlayerEntity entity)
		{
        }

        public void Exit(PlayerEntity entity)
		{
		}
	}

    public class MoveState : IState<PlayerEntity>
	{
		public void Enter(PlayerEntity entity)
		{
            entity.currentStates = PlayerStates.Move;
            entity.pAnimator.SetBool("isMove", true);
        }

        public void Execute(PlayerEntity entity)
		{
        }

        public void Exit(PlayerEntity entity)
		{
            if (GameManager.GetInstance.isPlayerDoAction != true)
            {
                entity.pAnimator.SetBool("isMove", false);
            }
        }
    }
    
    public class TeeterState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Teeter;
            entity.pAnimator.SetBool("isTeeter", true);
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Teeter") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.RevertToPreviousState();
            }   
        }

        public void Exit(PlayerEntity entity)
        {
            if (GameManager.GetInstance.isPlayerDoAction != true)
            {
                entity.pAnimator.SetBool("isTeeter", false);
            }
        }
    }

    public class ObtainState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Obtain;
            entity.pAnimator.SetBool("isObtain", true);
            //GameManager.GetInstance.isPlayerDoAction = true;
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Obtain") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(PlayerStates.Idle);
            }        
        }

        public void Exit(PlayerEntity entity)
        {
            entity.pAnimator.SetBool("isObtain", false);
        }
    }
    
    public class ClimbState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Climb;
            entity.pAnimator.SetBool("isClimb", true);
            //GameManager.GetInstance.isPlayerDoAction = true;        
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Climb") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                entity.ChangeState(PlayerStates.Idle);
            }                
        }

        public void Exit(PlayerEntity entity)
        {
            entity.pAnimator.SetBool("isClimb", false);
        }
    }
    
    public class PushState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Push;
            entity.pAnimator.SetBool("isPush", true);
            GameManager.GetInstance.isPlayerDoAction = true;
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Push") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5)
            {
                entity.ChangeState(PlayerStates.Idle);
            }
        }

        public void Exit(PlayerEntity entity)
        {
            entity.pAnimator.SetBool("isPush", false);
        }
    }

    public class AddCardState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.AddCard;
            entity.pAnimator.SetBool("isAddCard", true);
            GameManager.GetInstance.isPlayerDoAction = true;
            InteractionSequencer.GetInstance.PlayerActionQueue.Enqueue(GameManager.GetInstance.localPlayerMovement.AddcardRootmotion());
            CardManager.GetInstance.ableCardCtr = false;
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("AddCard") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(PlayerStates.Idle);
            }
        }

        public void Exit(PlayerEntity entity)
        {
            entity.pAnimator.SetBool("isAddCard", false);
            CardManager.GetInstance.ableCardCtr = true;
        }
    }

    public class WinState : IState<PlayerEntity>
    {
        public void Enter(PlayerEntity entity)
        {
            entity.currentStates = PlayerStates.Victory;
            entity.pAnimator.SetBool("isVictory", true);
            GameManager.GetInstance.isPlayerDoAction = true;
        }

        public void Execute(PlayerEntity entity)
        {
            if (entity.pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Victory") &&
                entity.pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                entity.ChangeState(PlayerStates.Idle);
            }
        }

        public void Exit(PlayerEntity entity)
        {
            entity.pAnimator.SetBool("isVictory", false);
            GameManager.GetInstance.isPlayerDoAction = false;
        }
    }

}

