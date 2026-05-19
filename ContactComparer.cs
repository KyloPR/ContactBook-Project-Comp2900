namespace ContactBook;

public enum ContactOrderBy
{
    FirstName,
    LastName,
    Phone,
    Email
}

public class ContactComparer : IComparer<Contact>
{
    private readonly ContactOrderBy _orderBy;

    public ContactComparer(ContactOrderBy orderBy)
    {
        _orderBy = orderBy;
    }

    public int Compare(Contact? x, Contact? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        int cmp = 0;

        switch (_orderBy)
        {
            case ContactOrderBy.FirstName:
                cmp = string.Compare(x.GetFirstName(), y.GetFirstName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetLastName(), y.GetLastName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetPhone(), y.GetPhone(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                return string.Compare(x.GetEmail(), y.GetEmail(), StringComparison.OrdinalIgnoreCase);

            case ContactOrderBy.LastName:
                cmp = string.Compare(x.GetLastName(), y.GetLastName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetFirstName(), y.GetFirstName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetPhone(), y.GetPhone(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                return string.Compare(x.GetEmail(), y.GetEmail(), StringComparison.OrdinalIgnoreCase);

            case ContactOrderBy.Phone:
                cmp = string.Compare(x.GetPhone(), y.GetPhone(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetFirstName(), y.GetFirstName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetLastName(), y.GetLastName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                return string.Compare(x.GetEmail(), y.GetEmail(), StringComparison.OrdinalIgnoreCase);

            case ContactOrderBy.Email:
            default:
                cmp = string.Compare(x.GetEmail(), y.GetEmail(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetFirstName(), y.GetFirstName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                cmp = string.Compare(x.GetLastName(), y.GetLastName(), StringComparison.OrdinalIgnoreCase);
                if (cmp != 0) return cmp;
                
                return string.Compare(x.GetPhone(), y.GetPhone(), StringComparison.OrdinalIgnoreCase);
        }
    }
}