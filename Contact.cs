namespace ContactBook;

public class Contact : IEquatable<Contact>
{
    private string _firstName = string.Empty;
    private string _lastName  = string.Empty;
    private string _phone     = string.Empty;
    private string _email     = string.Empty;

    public Contact(
        string firstName = "",
        string lastName  = "",
        string phone     = "",
        string email     = ""
    )
    {
        _firstName = firstName;
        _lastName  = lastName;
        _phone     = phone;
        _email     = email;
    }

    public string GetFirstName() => _firstName;
    public string GetLastName()  => _lastName;
    public string GetPhone()     => _phone;
    public string GetEmail()     => _email;

    public void SetFirstName(string firstName) => _firstName = firstName;
    public void SetLastName(string lastName)   => _lastName  = lastName;
    public void SetPhone(string phone)         => _phone     = phone;
    public void SetEmail(string email)         => _email     = email;

    public override string ToString()
    {
        return $"Contact(FirstName='{_firstName}', LastName='{_lastName}', Phone='{_phone}', Email='{_email}')";
    }

    public bool Equals(Contact? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return _firstName == other._firstName
            && _lastName  == other._lastName
            && _phone     == other._phone
            && _email     == other._email;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Contact);
    }

    public static bool operator ==(Contact? x, Contact? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x!.Equals(y);
    }

    public static bool operator !=(Contact? x, Contact? y) => !(x == y);

    public override int GetHashCode()
    {
        return HashCode.Combine(_firstName, _lastName, _phone, _email);
    }
}