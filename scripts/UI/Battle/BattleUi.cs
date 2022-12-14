using Godot;
using TurnBattle.Battle.Actors;
using TurnBattle.Godot.Battle;
using TurnBattle.Godot.Battle.Actors;
using System.Collections.Generic;

namespace TurnBattle.UI.Battle
{
    public class BattleUi : Control
    {
        [Export]
        private PackedScene _unitHudModel;
        [Export]
        private PackedScene _actionHudModel;
        private Control _targetHudRoot;
        private Control _damageTextRoot;
        private Dictionary<IBattlerSlot, HitLabel> _hitLabelDictionary = new Dictionary<IBattlerSlot, HitLabel>();
        private BattleSystem _battleSystem;
        private DynamicFontData _fontData;
        private DynamicFont _font;

        public void Init(BattleSystem battleSystem)
        {
            _battleSystem = battleSystem;
            if (_damageTextRoot == null)
            {
                _damageTextRoot = GetNode<Control>("DamageTextContainer");
            }
            if (_targetHudRoot == null)
            {
                _targetHudRoot = GetNode<Control>("TargetHudContainer");
            }
            _fontData = ResourceLoader.Load<DynamicFontData>("res://fonts/Roboto-Regular.ttf");
            _font = new DynamicFont();
            _font.FontData = _fontData;
            _font.Size = 26;
            _font.OutlineSize = 2;
            _font.OutlineColor = Colors.Black;
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
            var actionHudRoot = GetNode<Node>("ActionHudContainer");
            int i = 0;
            foreach (var unit in units)
            {
                var hudNode = _unitHudModel.Instance();
                var hud = hudNode as UnitHud;
                hud.Initialize();
                hud.SetUnit(unit);
                hudRoot.AddChild(hud);
                unit.DamageRecived += DisplayDamageText;
                var actionHud = _actionHudModel.Instance<ActionHudContainer>();
                actionHud.Initialize();
                actionHud.Direction = i % 2 == 0 ? ActionHudContainer.DirectionEnum.Left : ActionHudContainer.DirectionEnum.Right;
                actionHud.SetUnit(unit);
                actionHudRoot.AddChild(actionHud);
                i++;
            }
        }

        public void DisplayDamageText(SlotDamageRecivedArgs args)
        {
            Vector2 position;
            if (args.Slot is EnemySlot enemySlot)
            {
                var slotNode = enemySlot.GetNode<AnimatedSprite>("AnimatedSprite");
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
            damageLabel.AddFontOverride("font", _font);
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
                hitLabel.AddFontOverride("font", _font);
                hitLabel.RectGlobalPosition = position + new Vector2(0, -50);
                AddChild(hitLabel);
                _hitLabelDictionary.Add(slot, hitLabel);
            }
            hitLabel.ShowHitCount(hitCount);
        }

        public void UpdateTargetHud(IBattlerSlot target)
        {
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
            _targetHudRoot.Visible = show;
        }
    }
}
