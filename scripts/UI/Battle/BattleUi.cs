using Godot;
using Rpg2d.Battle;
using System.Collections.Generic;

namespace Rpg2d.UI.Battle
{
    public class BattleUi : Control
    {
        [Export]
        private PackedScene _unitHudModel;
        private Control _targetHudRoot;
        private Control _damageTextRoot;
        private Dictionary<IBattlerSlot, HitLabel> _hitLabelDictionary = new Dictionary<IBattlerSlot, HitLabel>();
        private BattleSystem _battleSystem;

        public void Init(BattleSystem battleSystem)
        {
            _battleSystem = battleSystem;
            InitUnitHuds(_battleSystem.EnumerateUnits());
            _battleSystem.TargetSelector.SelectedTargetChanged += UpdateTargetHud;
            _battleSystem.TargetSelector.EnableChanged += ShowTargetHud;
            foreach (var enemySlot in _battleSystem.Enemies)
            {
                enemySlot.DamageRecived += DisplayDamageText;
            }
            Visible = true;
        }

        public void InitUnitHuds(IEnumerable<UnitSlot> units)
        {
            var hudRoot = GetNode<Node>("UnitHudContainer");
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
                _damageTextRoot = GetNode<Control>("DamageTextContainer");
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
                _targetHudRoot = GetNode<Control>("TargetHudContainer");
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
                _targetHudRoot = GetNode<Control>("TargetHudContainer");
            }
            _targetHudRoot.Visible = show;
        }
    }
}
