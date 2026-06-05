namespace VetLinq.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Phone { get; set; } = "";
    public string Symptom { get; set; } = "";
    public List<Pet> Pets { get; set; } = new();
}
