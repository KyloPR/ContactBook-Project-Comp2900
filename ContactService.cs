namespace ContactBook;

public class ContactService
{
    private readonly InMemoryContactRepository _repository;

    public ContactService(InMemoryContactRepository repository)
    {
        _repository = repository;
    }

    public List<Contact> GetContacts()
    {
        return _repository.GetAll();
    }

    public void AddContact(Contact contact)
    {
        _repository.Add(contact);
    }

    public void OrderContacts(ContactOrderBy orderBy)
    {
        var list = _repository.GetAll();
        list.Sort(new ContactComparer(orderBy));
    }

    // Part 8: DSU-based duplicate grouping
    public List<List<Contact>> FindDuplicateGroups()
    {
        var list = _repository.GetAll();
        var result = new List<List<Contact>>();

        int n = list.Count;
        if (n == 0)
        {
            return result;
        }

        var dsu = new DisjointSetUnion(n);

        var indexByKey = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < n; i++)
        {
            var c = list[i];

            string email = (c.GetEmail() ?? string.Empty).Trim();
            string phone = (c.GetPhone() ?? string.Empty).Trim();
            string first = (c.GetFirstName() ?? string.Empty).Trim();
            string last = (c.GetLastName() ?? string.Empty).Trim();

            // Check for email duplicates
            if (!string.IsNullOrEmpty(email))
            {
                string key = "email:" + email;
                if (!indexByKey.TryGetValue(key, out var indicesForKey))
                {
                    indicesForKey = new List<int>();
                    indexByKey[key] = indicesForKey;
                }
                indicesForKey.Add(i);
            }

            // Check for phone duplicates
            if (!string.IsNullOrEmpty(phone))
            {
                string key = "phone:" + phone;
                if (!indexByKey.TryGetValue(key, out var indicesForKey))
                {
                    indicesForKey = new List<int>();
                    indexByKey[key] = indicesForKey;
                }
                indicesForKey.Add(i);
            }

            // Check for name pair duplicates (first + last both non-empty)
            if (!string.IsNullOrEmpty(first) && !string.IsNullOrEmpty(last))
            {
                string key = "namepair:" + first + "|" + last;
                if (!indexByKey.TryGetValue(key, out var indicesForKey))
                {
                    indicesForKey = new List<int>();
                    indexByKey[key] = indicesForKey;
                }
                indicesForKey.Add(i);
            }
        }

        foreach (var kvp in indexByKey)
        {
            var indices = kvp.Value;
            if (indices.Count <= 1)
            {
                continue;
            }

            int firstIndex = indices[0];
            for (int j = 1; j < indices.Count; j++)
            {
                dsu.Union(firstIndex, indices[j]);
            }
        }

        var groupsByRoot = new Dictionary<int, List<Contact>>();

        for (int i = 0; i < n; i++)
        {
            int root = dsu.Find(i);

            if (!groupsByRoot.TryGetValue(root, out var group))
            {
                group = new List<Contact>();
                groupsByRoot[root] = group;
            }

            group.Add(list[i]);
        }

        foreach (var kvp in groupsByRoot)
        {
            if (kvp.Value.Count > 1)
            {
                result.Add(kvp.Value);
            }
        }

        return result;
    }

    public void ApplyMergedContacts(List<Contact> mergedContacts)
    {
        var list = _repository.GetAll();
        list.Clear();
        list.AddRange(mergedContacts);
    }

    // Simple delete by zero-based index
    public void DeleteContactAt(int index)
    {
        var list = _repository.GetAll();
        if (index < 0 || index >= list.Count)
        {
            return;
        }

        list.RemoveAt(index);
    }

    // Update fields of contact at index
    public void UpdateContactAt(
        int index,
        string? firstName,
        string? lastName,
        string? phone,
        string? email
    )
    {
        var list = _repository.GetAll();
        if (index < 0 || index >= list.Count)
        {
            return;
        }

        var contact = list[index];

        contact.SetFirstName(firstName ?? string.Empty);
        contact.SetLastName(lastName ?? string.Empty);
        contact.SetPhone(phone ?? string.Empty);
        contact.SetEmail(email ?? string.Empty);
    }
}