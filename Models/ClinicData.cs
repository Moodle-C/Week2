namespace VetLinq.Models;

public static class ClinicData
{
    public static List<Patient> GetPatients()
    {
        return new List<Patient>
        {
            new Patient
            {
                Id = 1, Name = "Carlos Mendez", Age = 34, Phone = "3001234567",
                Symptom = "Limping",
                Pets = new List<Pet>
                {
                    new Pet { Id = 1, Name = "Rex",  Species = "Dog", Breed = "German Shepherd", Age = 3 },
                    new Pet { Id = 2, Name = "Luna", Species = "Cat", Breed = "Siamese",          Age = 5 }
                }
            },
            new Patient
            {
                Id = 2, Name = "Sofia Torres", Age = 28, Phone = "3119876543",
                Symptom = "Loss of appetite",
                Pets = new List<Pet>
                {
                    new Pet { Id = 3, Name = "Kiwi",  Species = "Bird", Breed = "Canary",  Age = 2 },
                    new Pet { Id = 4, Name = "Mochi", Species = "Cat",  Breed = "Persian", Age = 4 }
                }
            },
            new Patient
            {
                Id = 3, Name = "Andres Ruiz", Age = 41, Phone = "3204567890",
                Symptom = "Skin rash",
                Pets = new List<Pet>
                {
                    new Pet { Id = 5, Name = "Bolt",  Species = "Dog", Breed = "Labrador",   Age = 1 },
                    new Pet { Id = 6, Name = "Sasha", Species = "Dog", Breed = "Pomeranian", Age = 6 }
                }
            },
            new Patient
            {
                Id = 4, Name = "Lucia Vargas", Age = 22, Phone = "3055551234",
                Symptom = "Vomiting",
                Pets = new List<Pet>
                {
                    new Pet { Id = 7, Name = "Nemo", Species = "Fish", Breed = "", Age = 1 }
                }
            },
            new Patient
            {
                Id = 5, Name = "Miguel Castro", Age = 55, Phone = "3187654321",
                Symptom = "Lethargy",
                Pets = new List<Pet>
                {
                    new Pet { Id = 8, Name = "Rocky", Species = "Dog", Breed = "Bulldog",        Age = 7 },
                    new Pet { Id = 9, Name = "Cleo",  Species = "Cat", Breed = "British Shorthair", Age = 3 }
                }
            }
        };
    }
}
