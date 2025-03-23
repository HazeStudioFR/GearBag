using GearBag.Engine.UI;

namespace GearBag.Engine.Common.UI;

internal static class UIMenu
{
    private static readonly string MenuTitle = LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(Global.PlayerPed).FullName;

    internal static RAGENativeUI.UIMenu CreateMenu(string MenuSubtitle)
    {
        var menu = new RAGENativeUI.UIMenu(MenuTitle, MenuSubtitle.ToUpper()) { MouseControlsEnabled = false, AllowCameraMovement = true, DescriptionSeparatorColor = Color.RoyalBlue };
        menu.SetBannerType(new Sprite("commonmenu", "interaction_bgd", Point.Empty, Size.Empty));
        InterfaceHandler.s_menuPool.Add(menu);
        return menu;
    }

    internal static UIMenuItem CreateItem(string title, string description, RAGENativeUI.UIMenu menuToAssignTo, bool sideMarkers = false)
    {
        var item = new UIMenuItem(title, description);
        if (sideMarkers) item.RightLabel = ">>";
        menuToAssignTo.AddItem(item);
        return item;
    }


    internal static UIMenuListScrollerItem<string> CreateStringScroller(string title, string desc, RAGENativeUI.UIMenu menu, params string[] options)
    {
        var item = new UIMenuListScrollerItem<string>(title, desc, options);
        menu.AddItem(item);
        return item;
    }


    internal static UIMenuNumericScrollerItem<int> CreateScrollerInt(string title, string desc, int min, int max, int step, int value, RAGENativeUI.UIMenu menu)
    {
        var item = new UIMenuNumericScrollerItem<int>(title, desc, min, max, step) { Value = 0 };
        menu.AddItem(item);
        return item;
    }

    internal static UIMenuCheckboxItem CreateCheckBox(string title, string desc, RAGENativeUI.UIMenu menu)
    {
        var item = new UIMenuCheckboxItem(title, false, desc);
        menu.AddItem(item);
        return item;
    }

    internal static UIMenuNumericScrollerItem<float> CreateScrollerFloat(string title, string desc, float min, float max, float step, RAGENativeUI.UIMenu menu)
    {
        var item = new UIMenuNumericScrollerItem<float>(title, desc, min, max, step) { Value = 1 };
        menu.AddItem(item);
        return item;
    }

    internal static RAGENativeUI.UIMenu WithFastScrollingOn(this RAGENativeUI.UIMenu menu, IEnumerable<UIMenuItem> items)
    {
        var itemsSet = new HashSet<UIMenuItem>(items);

        void UpdateAcceleration(RAGENativeUI.UIMenu menu, UIMenuItem selectedItem)
        {
            var accel = itemsSet.Contains(selectedItem) ? new[] { new RAGENativeUI.UIMenu.AccelerationStep(0, 300), new RAGENativeUI.UIMenu.AccelerationStep(600, 10) } : null;

            menu.SetKeyAcceleration(RAGENativeUI.Common.MenuControls.Left, accel);
            menu.SetKeyAcceleration(RAGENativeUI.Common.MenuControls.Right, accel);
        }

        menu.OnIndexChange += (m, i) => UpdateAcceleration(m, m.MenuItems[i]);

        if (menu.MenuItems.Count > 0) UpdateAcceleration(menu, menu.MenuItems[menu.CurrentSelection]);

        return menu;
    }

    internal static void AddItems(this UIMenuListScrollerItem<string> scrollerItem, List<string> list)
    {
        foreach (var a in list) scrollerItem.Items.Add(a);
    }

    internal static void ItemToggleSkip(this RAGENativeUI.UIMenu menu, bool skipItems, params UIMenuItem[] items)
    {
        foreach (var uiItem in items)
            if (skipItems)
            {
                uiItem.Skipped = true;
                uiItem.ForeColor = Color.DimGray;
            }
            else
            {
                uiItem.Skipped = false;
                uiItem.ForeColor = Color.White;
            }
    }

    internal static void IsMenuOpenedWithKeybind(this RAGENativeUI.UIMenu sender, bool MenuOpenedViaKeybind)
    {
        if (!MenuOpenedViaKeybind) return;
        if (sender.ParentMenu != null) sender.ParentMenu.Visible = false;
    }
}