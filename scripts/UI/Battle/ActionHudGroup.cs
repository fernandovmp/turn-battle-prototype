using System.Linq;
using Godot;

namespace Rpg2d.UI.Battle
{
    public class ActionHudGroup : Container
    {
        [Export]
        public float HorizontalGap { get; set; }
        [Export]
        public float VerticalGap { get; set; }

        public override void _Notification(int what)
        {
            switch(what)
            {
                case NotificationSortChildren:
                    HandleResize();
                    break;
                case NotificationResized:
                    HandleResize();
                    break;
                default:
                    break;
            }
        }

        private void HandleResize()
        {
            var hudSize = new Vector2((RectSize.x - HorizontalGap) / 2, (RectSize.y - VerticalGap) / 3);
            var huds = GetChildren()
                .Cast<ActionHudContainer>();
            int i = 0;
            foreach(var hud in huds)
            {
                hud.RectSize = hudSize;
                hud.SetAnchorsPreset(LayoutPreset.CenterTop);
                Vector2 position = GetHudPosition(i, hudSize);
                hud.RectPosition = position;
                i++;
            }
        }

        private Vector2 GetHudPosition(int position, Vector2 hudSize)
        {
            switch(position)
            {
                case 0:
                    return new Vector2(hudSize.x / HorizontalGap + HorizontalGap, 0);
                case 1:
                    return new Vector2(hudSize.x + HorizontalGap, hudSize.y + VerticalGap);
                case 2:
                    return new Vector2(0, hudSize.y + VerticalGap);
                default:
                    return new Vector2(hudSize.x - hudSize.x / HorizontalGap, hudSize.y * 2 + VerticalGap);
            }
        }
    }
}
