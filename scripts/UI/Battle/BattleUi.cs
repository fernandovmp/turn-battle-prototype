using Godot;
using Rpg2d.Battle;
using System;
using System.Collections.Generic;

namespace Rpg2d.UI.Battle
{
    public class BattleUi : Node
    {
        [Export]
        private PackedScene _unitHudModel;
        private Control _targetHudRoot;
        private Control _damageTextRoot;
        private Dictionary<IBattlerSlot, HitLabel> _hitLabelDictionary = new Dictionary<IBattlerSlot, HitLabel>();
        private BattleSystem _battleSystem;

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
                    InitUnitHuds(_battleSystem.EnumerateUnits());
                    _battleSystem.TargetSelector.SelectedTargetChanged += UpdateTargetHud;
                    _battleSystem.TargetSelector.EnableChanged += ShowTargetHud;
                    foreach (var enemySlot in _battleSystem.Enemies)
                    {
                        enemySlot.DamageRecived += DisplayDamageText;
                    }
                    GetNode<Control>("BattleUI").Visible = true;
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
            var resultUi = GetNode<Control>("ResultUI");
            resultUi.Visible = true;
            resultUi.GetNode<Label>("TitleLabel").Text = title;
        }

        public void InitUnitHuds(IEnumerable<UnitSlot> units)
        {
            var hudRoot = GetNode<Node>("BattleUI/UnitHudContainer");
            foreach (var unit in units)
            {
                var hudNode = _unitHudModel.Instance();
                var hud = hudNode as UnitHud;
                hud.Initialize();
                hud.SetUnit(unit);
                hudRoot.AddChild(hud);
                unit.DamageRecived += DisplayDamageText;
            }
        }

        public void DisplayDamageText(SlotDamageRecivedArgs args)
        {
            if (_damageTextRoot == null)
            {
                _damageTextRoot = GetNode<Control>("BattleUI/DamageTextContainer");
            }
            Vector2 position;
            if (args.Slot is Node2D slotNode)
            {
                position = slotNode.GlobalPosition + new Vector2(100, 0);
            }
            else if (args.Slot is UnitSlot unitSlot)
            {
                var sprite = unitSlot.GetNode<AnimatedSprite>("AnimatedSprite");
                position = sprite.GlobalPosition + new Vector2(-100, 0);
            }
            else
            {
                position = Vector2.Zero;
            }
            var damageLabel = new DamageLabel();
            damageLabel.Text = args.Damage.ToString();
            damageLabel.SetStartPosition(position);
            var font = new DynamicFont();
            font.FontData = ResourceLoader.Load<DynamicFontData>("res://fonts/Roboto-Regular.ttf");
            font.Size = 26;
            damageLabel.AddFontOverride("font", font);
            _damageTextRoot.AddChild(damageLabel);
            damageLabel.DestroyAfter(1f);
            if (args.HitCount > 1)
            {
                DisplayHitCount(args.Slot, args.HitCount, position);
            }
        }

        private void DisplayHitCount(IBattlerSlot slot, int hitCount, Vector2 position)
        {
            var hasValue = _hitLabelDictionary.TryGetValue(slot, out HitLabel hitLabel);
            if (!hasValue)
            {
                hitLabel = new HitLabel();
                var font = new DynamicFont();
                font.FontData = ResourceLoader.Load<DynamicFontData>("res://fonts/Roboto-Regular.ttf");
                font.Size = 26;
                hitLabel.AddFontOverride("font", font);
                hitLabel.RectGlobalPosition = position + new Vector2(0, -50);
                AddChild(hitLabel);
                _hitLabelDictionary.Add(slot, hitLabel);
            }
            hitLabel.ShowHitCount(hitCount);
        }

        public void UpdateTargetHud(IBattlerSlot target)
        {
            if (_targetHudRoot == null)
            {
                _targetHudRoot = GetNode<Control>("BattleUI/TargetHudContainer");
            }
            var hud = _targetHudRoot.GetNodeOrNull<UnitHud>("TargetHud");
            if (hud == null)
            {
                var hudNode = _unitHudModel.Instance();
                hud = hudNode as UnitHud;
                hud.Initialize();
                hud.Name = "TargetHud";
                _targetHudRoot.AddChild(hud);
            }
            _targetHudRoot.Visible = true;
            hud.SetUnit(target);
        }

        public void ShowTargetHud(bool show)
        {
            if (_targetHudRoot == null)
            {
                _targetHudRoot = GetNode<Control>("BattleUI/TargetHudContainer");
            }
            _targetHudRoot.Visible = show;
        }
    }
}
