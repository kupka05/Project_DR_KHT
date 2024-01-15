
namespace Js.Boss
{
    public interface IState
    {
        // 상태 진입시 수행할 작업
        void EnterState();

        // 상태에서 수행할 업데이트 작업
        void UpdateState();

        // 상태 종료 시 수행할 작업
        void ExitState();
    }
}
