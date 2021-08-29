using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEngine.AddressableAssets;

[CustomEditor(typeof(DataSlot)), CanEditMultipleObjects]
public class DataSlotInspector : Editor
{
    VisualElement container;

    ObjectField groupField, idManagerField;
    TextField dataAddressField, contentIndexField, saveDateField;
    Foldout charList;
    Button modifyDataBtn;
    VisualElement modifierElement;
    Toggle isMainField;

    DataSlot slot;

    public override VisualElement CreateInspectorGUI()
    {
        container = new VisualElement();
        slot = target as DataSlot;
        InitFields(container);
        return container;
        //return base.CreateInspectorGUI();
    }

    private VisualElement CreateModifier ()
    {
        VisualElement modifierContainer = new VisualElement();
        VisualElement groupElement = new VisualElement();
        VisualElement contentIndexElement = new VisualElement();

        groupElement.style.flexDirection = FlexDirection.Row;
        contentIndexElement.style.flexDirection = FlexDirection.Row;

        Label groupLabel = new Label("Group");
        groupLabel.style.width = 120;
        ObjectField newGroupField = new ObjectField()
        {
            objectType = typeof(ContentGroup),
            allowSceneObjects = false,
        };
        newGroupField.style.width = 120;
        groupElement.Add(groupLabel);
        groupElement.Add(newGroupField);

        Label contentIndexLabel = new Label("Content Index");
        contentIndexLabel.style.width = 120;
        IntegerField newContentIndexField = new IntegerField();
        newContentIndexField.style.width = 30;
        contentIndexElement.Add(contentIndexLabel);
        contentIndexElement.Add(newContentIndexField);

        Button saveBtn = new Button();
        saveBtn.clicked += () =>
        {
            Debug.Log(slot);
            
            Data2 saveData = new Data2()
            {
                groupId = (idManagerField.value as ObjectIdManager).GetGroupIndex((newGroupField.value as ContentGroup)),
                contentIndex = newContentIndexField.value,
                saveDate = "",
                imageData = new List<Character> { null, null, null}
            };
            
            string json = JsonUtility.ToJson(saveData);
            slot.SaveJsonData(json);
            Debug.Log(saveData.groupId);
            Data2 loadData = JsonUtility.FromJson<Data2>(slot.GetJsonData());
            try { slot.GetData(loadData); } catch { };
           
        };

        modifierContainer.Add(groupElement);
        modifierContainer.Add(contentIndexElement);
        modifierContainer.Add(saveBtn);
        
        return modifierContainer;
    }
    void InitFields(VisualElement container)
    {
        //LoadFields();
        var rootAsset = Resources.Load<VisualTreeAsset>("DataSlotInspector");
        rootAsset.CloneTree(container);
        container.styleSheets.Add(Resources.Load<StyleSheet>("DataSlotStyleSheet"));

        VisualElement groupElement = container.Query<VisualElement>("group");
        VisualElement idManagerElement = container.Query<VisualElement>("idManager");
        VisualElement dataAddressElement = container.Query<VisualElement>("dataAddress");
        VisualElement contentIndexElement = container.Query<VisualElement>("contentIndex");
        VisualElement saveDateElement = container.Query<VisualElement>("saveDate");       

        groupField = groupElement.Q<ObjectField>();
        idManagerField = idManagerElement.Q<ObjectField>();
        saveDateField = saveDateElement.Q<TextField>();
        contentIndexField = contentIndexElement.Query<TextField>();
        dataAddressField = dataAddressElement.Q<TextField>();
        isMainField = container.Q<Toggle>("isMain");
        charList = container.Query<Foldout>();
        modifyDataBtn = container.Query<Button>();
        modifierElement = CreateModifier();

        foreach(var i in charList.Children().ToList())
        {
            ObjectField field = i as ObjectField;
            i.SetEnabled(false);
            field.objectType = typeof(Sprite);
            field.allowSceneObjects = false;
        }

        groupField.objectType = typeof(ContentGroup);
        groupField.SetEnabled(false);

        idManagerField.objectType = typeof(ObjectIdManager);
        idManagerField.bindingPath = "idManager";

        contentIndexField.SetEnabled(false);
        saveDateField.SetEnabled(false);

        //isMainField.value = slot.isMain;
        //isMainField.SetValueWithoutNotify(slot.isMain);
        //isMainField.RegisterValueChangedCallback(v =>
        //{
        //    slot.isMain = v.newValue;
        //});

        //modifyDataBtn.SetEnabled(false);

        Data2 loadData = JsonUtility.FromJson<Data2>(slot.GetJsonData());
        try { slot.GetData(loadData); } catch { };

        if (loadData == null) return;

        modifyDataBtn.SetEnabled(true);
        modifyDataBtn.clicked += () =>
        {
            try
            {
                container.Remove(modifierElement);
            }
            catch
            {
                container.Add(modifierElement);
            }
        };
    }
   
    
}
