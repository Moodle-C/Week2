using Microsoft.AspNetCore.Mvc;
using VetLinq.Models;

namespace VetLinq.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var patients = ClinicData.GetPatients();
        var allPets  = patients.SelectMany(p => p.Pets).ToList();

        var dictionary = patients.ToDictionary(p => p.Id);

        var queryResults  = BuildQueryResults(patients, allPets);
        var chainedResults = BuildChainedResults(patients);
        var practicalResults = BuildPracticalResults(patients, allPets);
        var groupResults = BuildGroupResults(patients, allPets);

        var model = new ClinicViewModel
        {
            Patients           = patients,
            PatientDictionary  = dictionary,
            QueryResults       = queryResults,
            ChainedResults     = chainedResults,
            PracticalResults   = practicalResults,
            GroupResults       = groupResults
        };

        return View(model);
    }

    private List<LinqResult> BuildQueryResults(List<Patient> patients, List<Pet> allPets)
    {
        var results = new List<LinqResult>();

        // Where
        var dogOwners = patients
            .Where(p => p.Pets.Any(pet => pet.Species == "Dog"))
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients who own at least one Dog",
            Method      = "Where",
            Description = "patients.Where(p => p.Pets.Any(pet => pet.Species == \"Dog\"))",
            Rows        = dogOwners.Select(p => $"{p.Name} — {p.Phone}").ToList()
        });

        // Where
        var over30 = (from p in patients
                      where p.Age > 30
                      select p).ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients older than 30 (query syntax)",
            Method      = "Where (query)",
            Description = "from p in patients where p.Age > 30 select p",
            Rows        = over30.Select(p => $"{p.Name}, {p.Age} yrs").ToList()
        });

        // Select
        var names = patients.Select(p => p.Name.ToUpper()).ToList();

        results.Add(new LinqResult
        {
            Title       = "All patient names in uppercase",
            Method      = "Select",
            Description = "patients.Select(p => p.Name.ToUpper())",
            Rows        = names
        });

        // OrderBy
        var byAge = patients.OrderBy(p => p.Age).ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients ordered by age (ascending)",
            Method      = "OrderBy",
            Description = "patients.OrderBy(p => p.Age)",
            Rows        = byAge.Select(p => $"{p.Name} — {p.Age} yrs").ToList()
        });

        // OrderByDescending
        var byAgeDesc = patients.OrderByDescending(p => p.Age).ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients ordered by age (descending)",
            Method      = "OrderByDescending",
            Description = "patients.OrderByDescending(p => p.Age)",
            Rows        = byAgeDesc.Select(p => $"{p.Name} — {p.Age} yrs").ToList()
        });

        // F-FirstOrDefault
        var firstCatOwner = patients.FirstOrDefault(p => p.Pets.Any(pet => pet.Species == "Cat"));

        results.Add(new LinqResult
        {
            Title       = "First patient with a Cat",
            Method      = "FirstOrDefault",
            Description = "patients.FirstOrDefault(p => p.Pets.Any(pet => pet.Species == \"Cat\"))",
            ScalarValue = firstCatOwner?.Name ?? "None found"
        });

        // Any
        bool hasBirdOwner = patients.Any(p => p.Pets.Any(pet => pet.Species == "Bird"));

        results.Add(new LinqResult
        {
            Title       = "Does any patient own a Bird?",
            Method      = "Any",
            Description = "patients.Any(p => p.Pets.Any(pet => pet.Species == \"Bird\"))",
            ScalarValue = hasBirdOwner.ToString()
        });

        // All
        bool allHavePets = patients.All(p => p.Pets.Count > 0);

        results.Add(new LinqResult
        {
            Title       = "Do all patients have at least one pet?",
            Method      = "All",
            Description = "patients.All(p => p.Pets.Count > 0)",
            ScalarValue = allHavePets.ToString()
        });

        // Count
        int totalPets = allPets.Count();
        int dogCount  = allPets.Count(pet => pet.Species == "Dog");

        results.Add(new LinqResult
        {
            Title       = "Pet counts",
            Method      = "Count",
            Description = "allPets.Count() / allPets.Count(pet => pet.Species == \"Dog\")",
            ScalarValue = $"Total pets: {totalPets} | Dogs: {dogCount}"
        });

        return results;
    }

    private List<LinqResult> BuildChainedResults(List<Patient> patients)
    {
        var results = new List<LinqResult>();

        // Chain 1: dog owners, sorted by age, project name + phone
        var dogOwnersSorted = patients
            .Where(p => p.Pets.Any(pet => pet.Species == "Dog"))
            .OrderBy(p => p.Age)
            .Select(p => $"{p.Name} ({p.Age} yrs) — {p.Phone}")
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Dog owners sorted by age → name + phone",
            Method      = "Where → OrderBy → Select",
            Description = "patients.Where(...Dog...).OrderBy(p => p.Age).Select(p => $\"{p.Name} ({p.Age}) — {p.Phone}\")",
            Rows        = dogOwnersSorted
        });

        // Chain 2: all pets from patients under 35, order by pet name
        var youngOwnerPets = patients
            .Where(p => p.Age < 35)
            .SelectMany(p => p.Pets)
            .OrderBy(pet => pet.Name)
            .Select(pet => $"{pet.Name} ({pet.Species}, {pet.Breed})")
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Pets belonging to patients under 35, sorted by pet name",
            Method      = "Where → SelectMany → OrderBy → Select",
            Description = "patients.Where(p => p.Age < 35).SelectMany(p => p.Pets).OrderBy(pet => pet.Name).Select(...)",
            Rows        = youngOwnerPets
        });

        // Chain 3: patients with multi-pet households, sorted by pet count desc
        var multiPet = patients
            .Where(p => p.Pets.Count > 1)
            .OrderByDescending(p => p.Pets.Count)
            .Select(p => $"{p.Name} — {p.Pets.Count} pets: {string.Join(", ", p.Pets.Select(pet => pet.Name))}")
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients with more than one pet, sorted by pet count",
            Method      = "Where → OrderByDescending → Select",
            Description = "patients.Where(p => p.Pets.Count > 1).OrderByDescending(p => p.Pets.Count).Select(...)",
            Rows        = multiPet
        });

        return results;
    }

    private List<LinqResult> BuildPracticalResults(List<Patient> patients, List<Pet> allPets)
    {
        var results = new List<LinqResult>();

        // Youngest patient
        var youngest = patients.MinBy(p => p.Age);

        results.Add(new LinqResult
        {
            Title       = "Youngest patient",
            Method      = "MinBy",
            Description = "patients.MinBy(p => p.Age)",
            ScalarValue = $"{youngest?.Name}, {youngest?.Age} yrs"
        });

        // Oldest patient
        var oldest = patients.MaxBy(p => p.Age);

        results.Add(new LinqResult
        {
            Title       = "Oldest patient",
            Method      = "MaxBy",
            Description = "patients.MaxBy(p => p.Age)",
            ScalarValue = $"{oldest?.Name}, {oldest?.Age} yrs"
        });

        // Pet with no breed defined
        bool hasNoBread = allPets.Any(pet => string.IsNullOrWhiteSpace(pet.Breed));

        results.Add(new LinqResult
        {
            Title       = "Any pet with no breed defined?",
            Method      = "Any",
            Description = "allPets.Any(pet => string.IsNullOrWhiteSpace(pet.Breed))",
            ScalarValue = hasNoBread ? "Yes — at least one pet has no breed registered" : "No — all pets have a breed"
        });

        // Names uppercase sorted
        var upperSorted = patients
            .Select(p => p.Name.ToUpper())
            .OrderBy(n => n)
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Patient names in uppercase, alphabetically sorted",
            Method      = "Select → OrderBy",
            Description = "patients.Select(p => p.Name.ToUpper()).OrderBy(n => n)",
            Rows        = upperSorted
        });

        // Average patient age
        double avgAge = patients.Average(p => p.Age);

        results.Add(new LinqResult
        {
            Title       = "Average patient age",
            Method      = "Average",
            Description = "patients.Average(p => p.Age)",
            ScalarValue = $"{avgAge:F1} yrs"
        });

        // Dictionary lookup demo
        results.Add(new LinqResult
        {
            Title       = "Dictionary lookup by patient ID",
            Method      = "ToDictionary",
            Description = "patients.ToDictionary(p => p.Id) — then dictionary[2]",
            ScalarValue = $"dictionary[2] → {patients.FirstOrDefault(p => p.Id == 2)?.Name}"
        });

        return results;
    }

    private List<LinqResult> BuildGroupResults(List<Patient> patients, List<Pet> allPets)
    {
        var results = new List<LinqResult>();

        // GroupBy species
        var bySpecies = allPets
            .GroupBy(pet => pet.Species)
            .OrderByDescending(g => g.Count())
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Pets grouped by species",
            Method      = "GroupBy",
            Description = "allPets.GroupBy(pet => pet.Species).OrderByDescending(g => g.Count())",
            Rows        = bySpecies
                .Select(g => $"{g.Key}: {g.Count()} pet(s) — {string.Join(", ", g.Select(p => p.Name))}")
                .ToList()
        });

        // GroupBy patients by age range
        var byAgeRange = patients
            .GroupBy(p => p.Age < 30 ? "Under 30" : p.Age < 45 ? "30–44" : "45+")
            .OrderBy(g => g.Key)
            .ToList();

        results.Add(new LinqResult
        {
            Title       = "Patients grouped by age range",
            Method      = "GroupBy",
            Description = "patients.GroupBy(p => p.Age < 30 ? \"Under 30\" : p.Age < 45 ? \"30–44\" : \"45+\")",
            Rows        = byAgeRange
                .Select(g => $"{g.Key}: {string.Join(", ", g.Select(p => p.Name))}")
                .ToList()
        });

        return results;
    }
}
