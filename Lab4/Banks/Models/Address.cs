using Banks.Exceptions;

namespace Banks.Models;

public class Address
{
    public Address(string city, string street, int house, int flat)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw AddressException.InvalidCityName(city);
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            throw AddressException.InvalidStreetName(street);
        }

        if (house < 0 || flat < 0)
        {
            throw AddressException.NegativeNumber();
        }

        City = city;
        Street = street;
        House = house;
        Flat = flat;
    }

    public string City { get; }
    public string Street { get; }
    public int House { get; }
    public int Flat { get; }
}