using UnityEngine;


/**
	* @class BaseGameEntity
	* @brief [상태 머신을 사용하기 위한 abstract 클래스 Entity Script 입니다.]
*/
public abstract class BaseGameEntity : MonoBehaviour
{
    public virtual void Start()
    {
        Setup();
    } 
    
    /**
    * @details entity Init을 위한 함수입니다. entity가 생성될 때 한 번 불립니다.
    * @return void
    * @todo 리플렉션을 이용하여 클래스 동적 할당 생각해보기
    */
    public virtual void Setup()
	{
        // TODO 리플렉션을 이용하여 클래스 동적 할당 생각해보기
    }

    
    /**
    * @details StateMachineRunner에서 지속적으로 update해주는 함수입니다.
    * @return void
    */
    public abstract void Updated();
}

