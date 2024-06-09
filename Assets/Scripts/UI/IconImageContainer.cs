using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class IconImageContainer : VisualElement
{
    private Image _icon;

    public IconImageContainer()
    {
        //Create a new Image element and add it to the root
        style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Images/door.png");
        //Add USS style properties to the elements
        //_icon.AddToClassList("slotIcon");
        AddToClassList("icon-image-container");
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