using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System;

public class ButtonFunctionality : MonoBehaviour
{
    // Start is called before the first frame update
    public static ButtonFunctionality BFM;
    public GameObject mainBtnLeft, submitBtn, inputSection, section;
    private Text mainBtnText;
    // 0 = starting
    private int page = 0;
    private List<string> subs = new List<string>();
    private Button btn, submitListener;
    private HashSet<int> listenerCount = new HashSet<int>();
    private List<SectionControls> Section_1_Controls = new List<SectionControls>() {
        new SectionControls("AMR 0", 1),
        new SectionControls("AMR 1", 2),
        new SectionControls("AMR 5", 3),
        new SectionControls("Robot 0", 4)
    };
    private List<SectionControls> Section_2_Controls = new List<SectionControls>() {
        new SectionControls("AMR 2", 1),
        new SectionControls("AMR 3", 2),
        new SectionControls("AMR 4", 3),
        new SectionControls("Robot 1", 4),
        new SectionControls("Robot 2", 5),
        new SectionControls("Robot 3", 6)
    };
    private string BOT = null;
    public List<SectionControls> RobotControls = new List<SectionControls>() {
        new SectionControls("Rotate", 1),
        new SectionControls("Drop", 2),
        new SectionControls("Lift", 3),
        new SectionControls("Release", 4)
    };
    public List<SectionControls> AMRControls = new List<SectionControls>() {
        new SectionControls("Forward", 1),
        new SectionControls("Rotate", 2),
    };
    void Awake(){
        if(BFM == null)BFM = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        mainBtnText = mainBtnLeft.transform.GetChild(0).gameObject.GetComponent<Text>();
        btn = mainBtnLeft.GetComponent<Button>();
        submitListener = submitBtn.GetComponent<Button>();
        for (int i = 1; i <= 8; i++)
        {
            hideButtons(i);
        }
        btn.onClick.AddListener(mainBtnClicked);
        submitListener.onClick.AddListener(submitClicked);

    }

    private void setSectionTitle(string text)
    {
        section.GetComponent<Text>().text = text;
    }

    private void hideButtons(int i)
    {
        transform.GetChild(i).gameObject.SetActive(false);
    }

    private void submitClicked()
    {
        if (subs.Count <= 2)
        {
            Debug.Log($"NOT ENOUGH {subs[0]} {subs.Count}");
            return;
        }
        RobotInstance.RIM.SendCommand($"Section: {subs[0]}\nPart: {subs[1]}\nOperation: {subs[2]}\nDistance: {inputSection.GetComponent<InputField>().text}", "listener");
        Debug.Log("TRANSMIT DATA: " + subs[0] + " " + subs[1] + " " + subs[2] + inputSection.GetComponent<InputField>().text);
        if (subs[0] == "Section 1")
        {
            string[] part = subs[1].Split(' ');
            if (part[0] == "AMR")
            {
                if (subs[2] == "forward")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    AMRManager.AMRIM.GetAMR((int)id).MoveForward((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
                else if (subs[2] == "rotate")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    AMRManager.AMRIM.GetAMR((int)id).Rotate((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
            }
            else if (part[0] == "Robot")
            {
                // Check if Body has operation is holding or is having an object at docker.
                // Check if at docker there is an object to pick
                float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                Data od = RobotInstance.RIM.GetBody((int) id).gameObject.GetComponent<Data>();
                if(od != null){
                    Debug.Log($"ULTRA NEW SHIT: {RobotInstance.RIM.GetSphere((int)id).gameObject.transform.childCount}");
                    if(AMRManager.AMRIM.GetAMR(od.RobotID).gameObject.transform.GetChild(0).childCount == 0 && RobotInstance.RIM.GetSphere((int)id).gameObject.transform.childCount == 0) return;
                }
                if (subs[2] == "rotate")
                {
                    RobotInstance.RIM.GetBody((int)id).handleMoveBody((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
                else if (subs[2] == "drop")
                {
                    RobotInstance.RIM.GetExtender((int)id).DropExtender(float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat), (int)id);
                }
                else if (subs[2] == "lift")
                {
                    RobotInstance.RIM.GetExtender((int)id).LiftExtender(float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat), (int)id);
                }
                else if (subs[2] == "release")
                {
                    RobotInstance.RIM.GetSphere((int)id).Release((int)id);
                }
            }
        }
        else if (subs[0] == "Section 2")
        {
            string[] part = subs[1].Split(' ');
            if (part[0] == "AMR")
            {
                if (subs[2] == "forward")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    AMRManager.AMRIM.GetAMR((int)id).MoveForward((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
                else if (subs[2] == "rotate")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    AMRManager.AMRIM.GetAMR((int)id).Rotate((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
            }
            else if (part[0] == "Robot")
            {
                if (subs[2] == "rotate")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    RobotInstance.RIM.GetBody((int)id).handleMoveBody((int)id, float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat));
                }
                else if (subs[2] == "drop")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    RobotInstance.RIM.GetExtender((int)id).DropExtender(float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat), (int)id);
                }
                else if (subs[2] == "lift")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    RobotInstance.RIM.GetExtender((int)id).LiftExtender(float.Parse(inputSection.GetComponent<InputField>().text, CultureInfo.InvariantCulture.NumberFormat), (int)id);
                }
                else if (subs[2] == "release")
                {
                    float id = float.Parse(part[1], CultureInfo.InvariantCulture.NumberFormat);
                    RobotInstance.RIM.GetSphere((int)id).Release((int)id);
                }
            }
        }
    }

    private void showButtons(int i, string text)
    {
        GameObject sideBtn = transform.GetChild(i).gameObject;
        GameObject sideBtnChild = sideBtn.transform.GetChild(0).gameObject;
        sideBtn.SetActive(true);
        sideBtnChild.GetComponent<Text>().text = text;
        // Debug.Log($"ListenerCount: {listenerCount.Count} && i: {i} && text: {text}");
        if (listenerCount.Count < i)
        {
            // if(page >= 2)return;
            listenerCount.Add(i);
            sideBtn.GetComponent<Button>().onClick.AddListener(() => sideButtonClicked(sideBtnChild.GetComponent<Text>()));
        }
    }

    private void showInputSection(string s)
    {
        // s = "S" - string
        //   = "D" - decimal
        inputSection.SetActive(true);
        submitBtn.SetActive(true);
        InputField _temp = inputSection.GetComponent<InputField>();
        // if(s == "D"){
        // _temp.contentType = InputField.ContentType.DecimalNumber;
        // inputSection.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enter the distance/angle.";
        // }
        // _temp.ForceLabelUpdate();
    }

    private void hideInputSection()
    {
        inputSection.SetActive(false);
        submitBtn.SetActive(false);
    }

    private void updateColor(int i)
    {
        Color newColor;
        ColorUtility.TryParseHtmlString("#00BE14", out newColor);
        transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = newColor;
    }

    public string GetBot(){
        return BOT;
    }

    private void resetColors(int i)
    {
        transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
    }

    private void ShowSection<T>(T SecControls, string name) where T : IEnumerable<SectionControls>
    {
        for (int i = 1; i <= 8; i++) hideButtons(i);
        foreach (SectionControls section in SecControls) showButtons(section.position, section.name);
        setSectionTitle(name);
    }

    private void sideButtonClicked(Text btnText)
    {
        Debug.Log("LISTENER COUNT: " + listenerCount.Count + " BTNCLICKED: " + btnText.text + " Page: " + page + " Subs: " + subs.Count);
        if (page >= 4)
        {
            Debug.Log($"SubsCount: {subs.Count}\n Page: {page}");
            // subs.RemoveAt(1);
            subs.RemoveAt(2);
            page = 2;
            for (int i = 1; i <= 6; i++)
                resetColors(i);
            inputSection.GetComponent<InputField>().text = "";
        }

        page += 1;

        if (subs.Count == 0)
        {
            // Body section done
            // if(btnText.text == "Body"){
            //     subs.Add("body");
            //     ShowSection(bodyControls, "Body");
            //     showInputSection("D");
            // }
            // else if(btnText.text == "Extender"){
            //     subs.Add("extender");
            //     ShowSection(extenderControls, "Extender");
            //     showInputSection("D");
            // }
            if (btnText.text == "Section 1")
            {
                subs.Add("Section 1");
                ShowSection(Section_1_Controls, "Section 1");
                // showInputSection("D");
            }
            else if (btnText.text == "Section 2")
            {
                subs.Add("Section 2");
                ShowSection(Section_2_Controls, "Section 2");
                // showInputSection("D");
            }
        }
        else if (subs.Count == 1)
        {
            string[] part = btnText.text.Split(' ');
            subs.Add(btnText.text);
            showInputSection("D");
            BOT = btnText.text;
            if (part[0] == "AMR")
            {
                ShowSection(AMRControls,$"{subs[0]} - {btnText.text}");
            }
            if (part[0] == "Robot")
            {
                ShowSection(RobotControls, $"{subs[0]} - {btnText.text}");
            }
        }
        else
        {
            // Rotate - Body
            // Drop - Extender
            string[] part = subs[1].Split(' ');
            Debug.Log($"CHOSE OP {part[0]} && {part[1]} && {btnText.text}\nSubsCount: {subs.Count}");
            showInputSection("D");
            if (part[0] == "Robot")
            {
                if (btnText.text == "Rotate")
                {
                    if (subs.Count > 2) subs.RemoveAt(2);
                    subs.Add("rotate");
                    updateColor(1);
                }
                else if (btnText.text == "Drop")
                {
                    if (subs.Count > 2) subs.RemoveAt(2);
                    subs.Add("drop");
                    updateColor(2);
                }
                // Lift, Release - Extender
                else if (btnText.text == "Lift")
                {
                    if (subs.Count > 2) subs.RemoveAt(2);
                    subs.Add("lift");
                    updateColor(3);
                }
                else if (btnText.text == "Release")
                {
                    inputSection.GetComponent<InputField>().text = "0";
                    if (subs.Count > 2) subs.RemoveAt(2);
                    subs.Add("release");
                    updateColor(4);
                }
            }
            else
            {
                if (btnText.text == "Forward")
                {
                    if (subs.Count > 2) subs.RemoveAt(2);
                    for (int i = 1; i <= AMRControls.Count; i++)
                    {
                        resetColors(i);
                    }
                    subs.Add("forward");
                    updateColor(1);
                }
                else if (btnText.text == "Rotate")
                {
                    Debug.Log(subs.Count);
                    if (subs.Count > 2) subs.RemoveAt(2);
                    for (int i = 1; i <= AMRControls.Count; i++)
                    {
                        resetColors(i);
                    }
                    subs.Add("rotate");
                    updateColor(2);
                }
            }
        }
    }

    private void loadPage(int i)
    {
        if (i == 1)
        {
            mainBtnText.text = "Back";
            for (int j = 0; j < 2; j++) showButtons(j + 1, (j == 0) ? "Section 1" : "Section 2");
        }
    }

    private void mainBtnClicked()
    {
        BOT = null;
        try
        {
            // Starting page
            Debug.Log($"{page} {subs.Count}");
            if (page == 0)
            {
                page = 1;
                // Go forward
                loadPage(1);
            }
            // Going back from section 1
            else if (page == 1)
            {
                page = 0;
                mainBtnText.text = "Start";
                for (int i = 1; i <= 6; i++) hideButtons(i);
                subs.Clear();
            }
            // Going back from MainSectionControls
            else if (page == 2)
            {
                Debug.Log("GOING BACK FROM 2 MOSTLY");
                subs.RemoveAt(subs.Count - 1);
                page = 1;
                setSectionTitle("");
                for (int i = 1; i <= 8; i++) hideButtons(i);
                loadPage(1);
            }
            else if (page >= 3)
            {
                Debug.Log($"GOING BACK FROM 3 {subs.Count} {subs[0]} {subs[1]}");
                page = 2;
                if(subs.Count == 3) subs.RemoveAt(subs.Count - 1);
                subs.RemoveAt(subs.Count - 1);
                hideInputSection();
                for (int i = 1; i <= 6; i++) { resetColors(i); hideButtons(i); }
                for (int i = 7; i <= 8; i++) hideButtons(i);
                inputSection.GetComponent<InputField>().text = "";
                ShowSection(subs[0] == "Section 1" ? Section_1_Controls : Section_2_Controls, subs[0]);
            }
            // else if (page == 4)
            // {
            //     for (int i = 1; i <= 6; i++)
            //     {
            //         resetColors(i);
            //     }
            //     page = 2;
            //     subs.RemoveAt(subs.Count - 1);
            //     subs.RemoveAt(subs.Count - 1);
            //     hideInputSection();
            //     Debug.Log($"{subs.Count} {subs[0]}");
            //     inputSection.GetComponent<InputField>().text = "";
            //     ShowSection(MainSectionControls, subs[0]);
            // }
            // else if (page >= 5)
            // {
            //     page = 1;
            //     subs.Clear();
            //     hideInputSection();
            //     inputSection.GetComponent<InputField>().text = "";
            //     loadPage(1);
            //     setSectionTitle("");
            //     for (int i = 1; i <= 6; i++)
            //     {
            //         resetColors(i);
            //         if (i >= 3) hideButtons(i);
            //     }
            // }
        }
        catch (Exception e)
        {
            subs.Clear();
            page = 1;
            for (int i = 1; i <= 6; i++) resetColors(i);

            hideInputSection();
            inputSection.GetComponent<InputField>().text = "";
            loadPage(1);
        }
    }
}
