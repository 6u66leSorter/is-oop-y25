using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities;

public class ClientBuilder
{
    private string _firstName = "non-nullable";
    private string _lastName = "non-nullable";
    private Address? _address;
    private Passport? _passport;

    public ClientBuilder SetFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
            throw ClientException.InvalidName(firstName);

        _firstName = firstName;
        return this;
    }

    public ClientBuilder SetLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
            throw ClientException.InvalidName(lastName);

        _lastName = lastName;
        return this;
    }

    public ClientBuilder SetAddress(Address? address)
    {
        _address = address;
        return this;
    }

    public ClientBuilder SetPassport(Passport? passport)
    {
        _passport = passport;
        return this;
    }

    public Client Build()
    {
        return new Client(_firstName, _lastName, _address, _passport);
    }
}