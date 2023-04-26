using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(WaifuBehaviour))]
class WaifuBehaviorEditor : Editor
{
    List<string> labels = new List<string>();

    void OnEnable()
    {
        //SerializeField] [Tooltip("Element 0 - evade, element 1 - splitter star, element 2 - basic attack, element 3 - rush attack")] List<int> delays;
        labels.Add("Evade");
        labels.Add("Splitter Star");
        labels.Add("Basic Attack");
        labels.Add("Rush Attack");
    }

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();

        InspectorElement.FillDefaultInspector(inspector, serializedObject, this);

        VisualElement delays = null;

        foreach (var child in inspector.Children())
        {
            if (child is PropertyField childPF && childPF.bindingPath == "delays")
            {
                delays = child;
                break;
            }
        }
        if (delays != null)
        {
            inspector.Remove(delays);
        }

        Foldout foldout = new Foldout();
        foldout.text = "Ability Delays";

        var delaysProperty = serializedObject.FindProperty("delays");

        delaysProperty.Next(true); // type
        delaysProperty.Next(true); // size

        var size = delaysProperty.intValue;

        delaysProperty.Next(true);

        for (int i = 0; i < size; ++i)
        {
            var sp = new PropertyField(delaysProperty);

            if (labels.Count > i)
            {
                sp.label = labels[i];
            }

            foldout.Add(sp);
            if (i != size - 1)
            {
                delaysProperty.Next(false);
            }
        }

        inspector.Add(foldout);

        return inspector;
    }
}