
public interface IState
{

    void OnAwake();
    void OnExecute();
    void OnSleep();

    void SetFSM(FSM manager);
    void SetAgent(Entity entity);
}
