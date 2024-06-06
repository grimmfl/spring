using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Persistence.Models;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class TraderSlot : VisualElement
{
    [CanBeNull] public XrefTraderItem Item;

    public Label Label;

    private Image _icon;

    public TraderSlot()
    {
        //Create a new Image element and add it to the root
        _icon = new Image();
        Label = new Label
        {
            style =
            {
                color = new StyleColor(new Color(255, 255, 255))
            }
        };
        Add(Label);

        //Add USS style properties to the elements
        _icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
    }

    #region UXML

    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits>
    {
    }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
    }

    #endregion
}