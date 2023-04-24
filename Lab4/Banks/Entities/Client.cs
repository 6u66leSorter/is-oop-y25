using Banks.Models;

namespace Banks.Entities;

public class Client
{
    public Client(string firstName, string lastName, Address? address, Passport? passport)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Passport = passport;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public Address? Address { get; private set; }
    public Passport? Passport { get; private set; }

    public void UpdateAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        Address = address;
    }

    public void UpdatePassport(Passport passport)
    {
        ArgumentNullException.ThrowIfNull(passport);

        Passport = passport;
    }
}