using Godot;
using Rpg2d.UI.Battle;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Battle
{
    public class BattleSystem : Node
    {

        private UnitSlot _leftUnit;
        private UnitSlot _upUnit;
        private Node _enemiesRoot;
        [Export]
        private UnitResource _partyLeftUnit;
        [Export]
        private UnitResource _partyUpUnit;
        [Export]
        private EnemyTroopResource _troop;
        [Export]
        private PackedScene _enemyModel;
        private List<EnemySlot> _enemies = new List<EnemySlot>();
        private TargetSelector _targetSelector;
        private BattleUi _battleUi;
        private IActionDispatcher _actionDispatcher = new ActionDispatcher();
        public override void _Ready()
        {
            var partyNode = GetNode("../Party");
            _enemiesRoot = GetNode("../Enemies");
            _leftUnit = partyNode.GetNode<UnitSlot>("LeftUnit");
            _upUnit = partyNode.GetNode<UnitSlot>("UpUnit");
            _leftUnit.SetUnit(_partyLeftUnit);
            _leftUnit.ActionDispatcher = _actionDispatcher;
            _upUnit.SetUnit(_partyUpUnit);
            _upUnit.ActionDispatcher = _actionDispatcher;
            _targetSelector = GetNode<TargetSelector>("../TargetSelector");
            _battleUi = GetNode<BattleUi>("../CanvasLayer");
            _targetSelector.SelectedTargetChanged += _battleUi.UpdateTargetHud;
            _targetSelector.EnableChanged += _battleUi.ShowTargetHud;
            SetupTroop(_troop);
            _battleUi.InitUnitHuds(EnumerateUnits());
            _actionDispatcher.AllActionsFinished += AllActionFinished;
            StartPartyTurn();
        }

        private void SetupTroop(EnemyTroopResource troop)
        {
            for (int i = 0; i < troop.Enemies.Length; i++)
            {
                var enemyNode = _enemyModel.Instance();
                _enemiesRoot.AddChild(enemyNode);
                var node2D = enemyNode.GetNode<Node2D>("AnimatedSprite");
                var enemySlot = enemyNode.GetNode<EnemySlot>(".");
                node2D.Position = troop.Positions[i];
                enemySlot.SetEnemy(troop.Enemies[i]);
                enemySlot.ActionFinished = EnemyActionFinished;
                _enemies.Add(enemySlot);
            }
            _targetSelector.Init(_enemies);
            _targetSelector.Select(0);
        }

        private void EnemyActionFinished(BattleAction obj)
        {
            if (!_enemies.Any(unit => unit.CanAct))
            {
                StartPartyTurn();
            }
        }

        private void StartPartyTurn()
        {
            _leftUnit.CanAct = true;
            _upUnit.CanAct = true;
            _targetSelector.Enabled = true;
        }

        public override void _Input(InputEvent inputEvent)
        {
        }

        public void AllActionFinished()
        {
            if (!EnumerateUnits().Any(unit => unit.CanAct || unit.IsActing))
            {
                StartEnemyTurn();
            }
        }

        private async void StartEnemyTurn()
        {
            _targetSelector.Enabled = false;
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            foreach (var enemy in _enemies)
            {
                enemy.CanAct = true;
            }
            var ai = _troop.AI;
            var actions = ai.GetActions(new BattleContext
            {
                Enemies = _enemies,
                Party = EnumerateUnits()
            });
            while (actions.MoveNext())
            {
                var action = actions.Current;
                if (action is WaitAction waitAction)
                {
                    await ToSignal(GetTree().CreateTimer(waitAction.Seconds), "timeout");
                }
                else
                {
                    action.Owner.PerformAction(action);
                    foreach (var target in action.TargetGroup.GetTargets())
                    {
                        action.Skill.Cast(new Skills.CastContext
                        {
                            Caster = action.Owner.Battler,
                            Skill = action.Skill,
                            Target = target
                        });
                    }
                }
            }
        }

        public IEnumerable<UnitSlot> EnumerateUnits()
        {
            yield return _leftUnit;
            yield return _upUnit;
        }
    }
}
