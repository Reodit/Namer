/**
* @interface IState<T>
* @brief [상태 인터페이스]
*/
public interface IState<T> where T : class
{
	/**
    * @details entity의 State에 진입했을 때 한 번 불립니다.
	* @param[in] entity Class
    * @return void
    */
	public void Enter(T entity);
	
	/**
    * @details entity가 State에 있을 때 update로 불립니다.
	* @param[in] entity Class
    * @return void
    */
	public void Execute(T entity);
	
	/**
    * @details entity가 State에서 빠져나올 때 한 번 불립니다.
	* @param[in] entity Class
    * @return void
    */
	public void Exit(T entity);
}

