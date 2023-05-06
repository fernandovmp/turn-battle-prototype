using Godot;
using TurnBattle.Battle.Actors;

namespace TurnBattle.UI.Battle
{
    public class UnitHud : Node
    {
        private Label _nameLabel;
        private TextureProgress _hpBar;
        private IBattlerSlot _unit;

        private const int BAR_MAGNITUDE = 100;

        public void Initialize()
        {
            _nameLabel = GetNode<Label>("NameLabel");
            _hpBar = GetNode<TextureProgress>("HpBar");
        }

        public void SetUnit(IBattlerSlot unit)
        {
            if (_unit != null)
            {
                RemoveUnit();
            }
            _unit = unit;
            _unit.Died += FullUpdate;
            _unit.Battler.Update += UpdateHud;
            FullUpdate();
        }

        private void RemoveUnit()
        {
            _unit.Died -= FullUpdate;
            _unit.Battler.Update -= UpdateHud;
        }

        private void UpdateHud(string property)
        {
            var battler = _unit.Battler;
            switch(property)
            {
                case nameof(battler.Hp):
                    _hpBar.Value = _unit.Battler.Hp.Ratio() * BAR_MAGNITUDE;
                    break;
                default:
                    break;
            }
        }

        public void FullUpdate()
        {
            _nameLabel.Text = _unit.Battler.Name;
            _hpBar.Value = _unit.Battler.Hp.Ratio() * BAR_MAGNITUDE;
        }
    }
}
