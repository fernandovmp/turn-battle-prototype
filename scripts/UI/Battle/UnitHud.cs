using Godot;
using Rpg2d.Battle.Actors;

namespace Rpg2d.UI.Battle
{
    public class UnitHud : Node
    {
        private Label _nameLabel;
        private TextureProgress _hpBar;
        private IBattlerSlot _unit;

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
            _unit.Died += Update;
            _unit.DamageRecived += DamageRecived;
            Update();
        }

        private void DamageRecived(SlotDamageRecivedArgs obj)
        {
            Update();
        }

        private void RemoveUnit()
        {
            _unit.Died -= Update;
            _unit.DamageRecived -= DamageRecived;
        }

        public void Update()
        {
            _nameLabel.Text = _unit.Battler.Name;
            _hpBar.Value = _unit.Battler.Hp / (double)_unit.Battler.MaxHp * 100;
        }
    }
}
