using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MyTestEditor : EditorWindow
{
    [Header("Set in inspector")]
    [SerializeField] VisualTreeAsset baseTree;
    [SerializeField] VisualTreeAsset UXMLTree;

    int clickCounter = 0;

    string buttonPrefix = "button";

    [MenuItem("Tools/UI Toolkit Tests/MyTestEditor")]
    public static void ShowExample()
    {
        MyTestEditor wnd = GetWindow<MyTestEditor>();
        wnd.titleContent = new GUIContent("MyTestEditor Title");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //c# created visual elements
        VisualElement label = new Label("This label got created through C#.");
        root.Add(label);

        //Instatiate the base ui from UI Builder
        root.Add(baseTree.Instantiate());

        //Instatiate the inspector added UI visual element.
        root.Add(UXMLTree.Instantiate());

        //Create the c# button
        Button button = new Button();
        button.name = "button3";
        button.text = "This is button 3";
        root.Add(button);

        //Create the c# toggle
        Toggle toggler = new Toggle();
        toggler.name = "toggle3";
        toggler.label = "Times pressed?";

        root.Add(toggler);

        SetupOnButtonPressed();
    }

    void SetupOnButtonPressed()
    {
        var buttons = rootVisualElement.Query<Button>();
        buttons.ForEach(RegisterHandler);
    }

    void RegisterHandler(Button button)
    {
        button.RegisterCallback<ClickEvent>(PrintClickMessage);
    }

    void PrintClickMessage(ClickEvent clickEvent)
    {
        ++clickCounter;

        Button button = clickEvent.currentTarget as Button;

        string buttonNum = button.name.Substring(buttonPrefix.Length);
        string toggleName = "toggle" + buttonNum;
        Toggle toggle = rootVisualElement.Query<Toggle>(toggleName).Build().First(); //shorthand exists as Q<>(...);

        Debug.Log("Button was clicked! " +
            (toggle.value ? "Count " + clickCounter : ""));
    }
}