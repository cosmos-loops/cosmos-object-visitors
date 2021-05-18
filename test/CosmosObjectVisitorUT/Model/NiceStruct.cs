using System;

namespace CosmosObjectVisitorUT.Model
{
    public struct NiceStruct
    {
        public NiceStruct(string name, int age, DateTime birthday, Country country, bool isValid)
        {
            Name = name;
            Age = age;
            Birthday = birthday;
            Country = country;
            IsValid = isValid;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime Birthday { get; set; }

        public Country Country { get; set; }

        public bool IsValid { get; set; }
    }
}