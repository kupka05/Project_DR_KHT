
namespace BossMonster
{
    public interface IState
    {
        // 상태 진입시 수행할 작업
        void EnterState(Boss boss);

        // 상태에서 수행할 업데이트 작업
        void UpdateState(Boss boss);

        // 상태 종료 시 수행할 작업
        void ExitState(Boss boss);
    }
}
