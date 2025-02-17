[System.Serializable] // Allows Unity to serialize this class in the Inspector
public class SectionControls{
    public string name;
    public int position;
    public SectionControls(string name, int position){
        this.name = name;
        this.position = position;
    }
}