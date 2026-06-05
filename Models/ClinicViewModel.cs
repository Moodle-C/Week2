namespace VetLinq.Models;

public class LinqResult
{
    public string Title { get; set; } = "";
    public string Method { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Rows { get; set; } = new();
    public string? ScalarValue { get; set; }
}

public class ClinicViewModel
{
    public List<Patient> Patients { get; set; } = new();
    public Dictionary<int, Patient> PatientDictionary { get; set; } = new();
    public List<LinqResult> QueryResults { get; set; } = new();
    public List<LinqResult> ChainedResults { get; set; } = new();
    public List<LinqResult> PracticalResults { get; set; } = new();
    public List<LinqResult> GroupResults { get; set; } = new();
}
