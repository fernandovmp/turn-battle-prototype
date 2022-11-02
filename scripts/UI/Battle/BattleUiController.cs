using Godot;
using Rpg2d.Battle;

namespace Rpg2d.UI.Battle
{
    public class BattleUiController : Node
    {
        private BattleSystem _battleSystem;
        private ResultUI _resultUi;
        private BattleUi _battleUi;

        public override void _Ready()
        {
            base._Ready();
            _resultUi = GetNode<ResultUI>("ResultUI");
            _battleUi = GetNode<BattleUi>("BattleUI");
        }

        public void SetBattleSystem(BattleSystem battleSystem)
        {
            _battleSystem = battleSystem;
            _battleSystem.PhaseChanged += BattlePhaseChanged;
        }

        private void BattlePhaseChanged(BattlePhaseEnum phase)
        {
            switch (phase)
            {
                case BattlePhaseEnum.Undefined:
                    break;
                case BattlePhaseEnum.Initialization:
                    _battleUi.Init(_battleSystem);
                    _resultUi.HideResult();
                    break;
                case BattlePhaseEnum.PartyTurn:
                    break;
                case BattlePhaseEnum.EnemyTurn:
                    break;
                case BattlePhaseEnum.PartyVictory:
                    ShowResult("Victory");
                    break;
                case BattlePhaseEnum.EnemyVictory:
                    ShowResult("Defeated");
                    break;
            }
        }

        private void ShowResult(string title)
        {
            GetNode<Control>("BattleUI").Visible = false;
            _resultUi.ShowResult(title);
        }
    }
}
