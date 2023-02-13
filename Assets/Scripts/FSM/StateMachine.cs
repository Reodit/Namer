using UnityEngine;

public class StateMachine<T> where T : class
{
	private	T			ownerEntity;	
	private	IState<T>	currentState;	
	private	IState<T>	previousState;	
	private	IState<T>	globalState;	

	public void Setup(T owner, IState<T> entryState)
	{
		ownerEntity		= owner;
		currentState	= null;
		previousState	= null;
		globalState		= null;

		ChangeState(entryState);
	}

	public void Execute()
	{
		if ( globalState != null )
		{
			globalState.Execute(ownerEntity);
		} ;
		
		if ( currentState != null )
		{
			currentState.Execute(ownerEntity);
		}
	}

	public void ChangeState(IState<T> newState)
	{
		if (newState == null)
		{
			return;
		}

		if ( currentState != null )
		{
			previousState = currentState;
			currentState.Exit(ownerEntity);	
		}

		currentState = newState;
		currentState.Enter(ownerEntity);
	}

	public void SetGlobalState(IState<T> newState)
	{
		globalState = newState;
	}

	public void RevertToPreviousState()
	{
		ChangeState(previousState);
	}
}

