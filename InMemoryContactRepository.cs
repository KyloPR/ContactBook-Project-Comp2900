namespace ContactBook;

public class InMemoryContactRepository
{
    private readonly List<Contact> _contacts;

    public InMemoryContactRepository(IEnumerable<Contact> initialContacts)
    {
        _contacts = new List<Contact>(initialContacts);
    }

    public List<Contact> GetAll()
    {
        return _contacts;
    }

    public void Add(Contact contact)
    {
        _contacts.Add(contact);
    }

    // Later: Update, Delete, Find, etc.
}