public class Address
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Zip { get; private set; }
    public string Country { get; private set; } 
    public string State { get; private set; }  

    public Address(string street, string city, string zip, string country, string state)
    {
        Street = street;
        City = city;
        Zip = zip;
        Country = country;
        State = state;
    }

    public override bool Equals(object? obj) => obj is Address other &&
        Street == other.Street && City == other.City && Zip == other.Zip &&
        Country == other.Country && State == other.State;

    public override int GetHashCode() => HashCode.Combine(Street, City, Zip, Country, State);
}
