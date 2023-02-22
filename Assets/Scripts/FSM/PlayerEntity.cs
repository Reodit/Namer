using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 플레이어의 상태 enum값
/// </summary>
public enum PlayerStates { Idle = 0, Move, Push, Victory, Teeter, Obtain, Climb, AddCard, EndPoint }

/**
	* @class PlayerEntity
	* @brief [BaseEntity를 상속받은 Player의 Entity입니다.]
*/
public class PlayerEntity : BaseGameEntity
{	
	private	Dictionary<PlayerStates, IState<PlayerEntity>> states;
	private	StateMachine<PlayerEntity> stateMachine;
	/// <summary>
	/// 플레이어 Entity의 현태 상태값
	/// </summary>
	public PlayerStates currentStates;
	#region components
    /// <summary>
    /// 플레이어 Entity의 Animator
    /// </summary>
    public Animator pAnimator;
    /// <summary>
    /// 플레이어 Entity의 EffectClass
    /// </summary>
    public PlayerEffetct pEffetct;
	#endregion

	public override void Start()
    {
		base.Start();
        pAnimator = GetComponentInChildren<Animator>();
        FindObjectOfType<StateMachineRunner>().entitys.Add(this);
        GameManager.GetInstance.localPlayerEntity = this;
        pEffetct = GetComponent<PlayerEffetct>();
    }

	/// <summary>
	/// Entity 초기화
	/// 1. 새 Dictionary를 생성 후 Dictionary안에 Entity상태를 Key, State 클래스를 Value로 할당해주세요.
	/// 2. stateMachine을 새로 entity 타입으로 할당한 후 초기값을 세팅해주세요
	/// </summary>
    public override void Setup()
	{
		// states 할당
		states = new Dictionary<PlayerStates, IState<PlayerEntity>>();
		states[PlayerStates.Idle] = new PlayerOwnedStates.IdleState();
		states[PlayerStates.Move] = new PlayerOwnedStates.MoveState();
		states[PlayerStates.Push] = new PlayerOwnedStates.PushState();
		states[PlayerStates.Victory] = new PlayerOwnedStates.WinState();
		states[PlayerStates.Obtain] = new PlayerOwnedStates.ObtainState();
		states[PlayerStates.Climb] = new PlayerOwnedStates.ClimbState();
		states[PlayerStates.AddCard] = new PlayerOwnedStates.AddCardState();
		states[PlayerStates.Teeter] = new PlayerOwnedStates.TeeterState();
		// stateMachine 할당 및 초기화
		stateMachine = new StateMachine<PlayerEntity>();
		stateMachine.Setup(this, states[PlayerStates.Idle]);
	}

	public override void Updated()
	{
		stateMachine.Execute();
	}

	/// <summary>
	/// entity의 상태를 parameter의 상태로 변경합니다
	/// </summary>
	/// <param name="newState"></param>
	public void ChangeState(PlayerStates newState)
	{
		stateMachine.ChangeState(states[newState]);
	}
	
	/// <summary>
	/// entity에 저장된 바로 이전 상태로 돌아갑니다.
	/// </summary>
	public void RevertToPreviousState()
	{
		stateMachine.RevertToPreviousState();
	}
}

