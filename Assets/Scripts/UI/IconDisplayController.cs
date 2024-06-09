using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IconDisplayController : MonoBehaviour
{
    private static VisualElement _root;
    private static VisualElement _imageContainerTemplate;

    private static readonly IDictionary<Guid, IconImageContainer> _iconDict = new Dictionary<Guid, IconImageContainer>();

    private void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _imageContainerTemplate = _root.Q<VisualElement>("ImageContainer");
    }

    public static Guid AddIcon(Vector3 mousePosition)
    {
        var pos = RuntimePanelUtils.ScreenToPanel(_root.panel, mousePosition);
        
        var iconContainer = new IconImageContainer();
        var guid = Guid.NewGuid();

        iconContainer.style.left = pos.x;
        iconContainer.style.bottom = pos.y;
        
        _root.Add(iconContainer);
        _iconDict.Add(guid, iconContainer);

        return guid;
    }

    public static void RemoveIcon(Guid id)
    {
        var icon = _iconDict[id];
        _root.Remove(icon);
    }
}