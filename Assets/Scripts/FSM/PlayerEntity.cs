using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

public enum PlayerStates { Idle = 0, Run, Push, EndPoint }

public class PlayerEntity : BaseGameEntity
{	
	private	Dictionary<PlayerStates, IState<PlayerEntity>> states;
	private	StateMachine<PlayerEntity> stateMachine;

    #region components
    private Rigidbody rb;
    public Animator myAnimator;
	#endregion

	public bool doInteraction;
	public KeyCode interactionKey = KeyCode.B;


    public override void Start()
    {
		base.Start();
        myAnimator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public override void Setup()
	{
		// states 할당
		// 새 Dictionary를 생성 후 Dictionary안에 Entity상태를 Key, State 클래스를 Value로 할당해주세요.
		states = new Dictionary<PlayerStates, IState<PlayerEntity>>();
        states[PlayerStates.Idle] = new PlayerOwnedStates.IdleState();
		states[PlayerStates.Run] = new PlayerOwnedStates.RunState();
		states[PlayerStates.Push] = new PlayerOwnedStates.PushState();

		// stateMachine 할당 및 초기화
		// stateMachine을 새로 entity 타입으로 할당한 후 초기값을 세팅해주세요
		stateMachine = new StateMachine<PlayerEntity>();
		stateMachine.Setup(this, states[PlayerStates.Idle]);
	}

	public override void Updated()
	{
		stateMachine.Execute();
	}

	public void ChangeState(PlayerStates newState)
	{
		stateMachine.ChangeState(states[newState]);
	}

	public void RevertToPreviousState()
	{
		stateMachine.RevertToPreviousState();
	}
}

