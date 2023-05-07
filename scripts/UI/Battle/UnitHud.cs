using Godot;
using TurnBattle.Battle.Actors;

namespace TurnBattle.UI.Battle
{
    public class UnitHud : Node
    {
        private Label _nameLabel;
        private ProgressBar _hpBar;
        private ProgressBar _mpBar;
        private IBattlerSlot _unit;
        [Export]
        public bool ShowMpBar { get; set; }

        private const int BAR_MAGNITUDE = 100;

        public void Initialize()
        {
            _nameLabel = GetNode<Label>("NameLabel");
            _hpBar = GetNode<ProgressBar>("HpBar");
            _mpBar = GetNode<ProgressBar>("MpBar");
            _mpBar.Visible = ShowMpBar;
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
                    _hpBar.GetNode<Label>("Label").Text = $"{_unit.Battler.Hp.Current}/{_unit.Battler.Hp.Max}";
                    break;
                case nameof(battler.Mp):
                    _mpBar.Value = _unit.Battler.Mp.Ratio() * BAR_MAGNITUDE;
                    _mpBar.GetNode<Label>("Label").Text = $"{_unit.Battler.Mp.Current}/{_unit.Battler.Mp.Max}";
                    break;
                default:
                    break;
            }
        }

        public void FullUpdate()
        {
            _nameLabel.Text = _unit.Battler.Name;
            _hpBar.Value = _unit.Battler.Hp.Ratio() * BAR_MAGNITUDE;
            _hpBar.GetNode<Label>("Label").Text = $"{_unit.Battler.Hp.Current}/{_unit.Battler.Hp.Max}";
            _mpBar.Value = _unit.Battler.Mp.Ratio() * BAR_MAGNITUDE;
            _mpBar.GetNode<Label>("Label").Text = $"{_unit.Battler.Mp.Current}/{_unit.Battler.Mp.Max}";
        }
    }
}
