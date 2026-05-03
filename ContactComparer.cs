namespace ContactBook;

public enum ContactOrderBy
{
    FirstName,
    LastName,
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

        string xPrimary;
        string yPrimary;
        string xSecondary;
        string ySecondary;
        string xTertiary;
        string yTertiary;

        switch (_orderBy)
        {
            case ContactOrderBy.FirstName:
                xPrimary   = x.GetFirstName();
                yPrimary   = y.GetFirstName();
                xSecondary = x.GetLastName();
                ySecondary = y.GetLastName();
                xTertiary  = x.GetEmail();
                yTertiary  = y.GetEmail();
                break;

            case ContactOrderBy.LastName:
                xPrimary   = x.GetLastName();
                yPrimary   = y.GetLastName();
                xSecondary = x.GetFirstName();
                ySecondary = y.GetFirstName();
                xTertiary  = x.GetEmail();
                yTertiary  = y.GetEmail();
                break;

            case ContactOrderBy.Email:
            default:
                xPrimary   = x.GetEmail();
                yPrimary   = y.GetEmail();
                xSecondary = x.GetFirstName();
                ySecondary = y.GetFirstName();
                xTertiary  = x.GetLastName();
                yTertiary  = y.GetLastName();
                break;
        }

        int cmp = string.Compare(xPrimary, yPrimary, StringComparison.OrdinalIgnoreCase);
        if (cmp != 0) return cmp;

        cmp = string.Compare(xSecondary, ySecondary, StringComparison.OrdinalIgnoreCase);
        if (cmp != 0) return cmp;

        return string.Compare(xTertiary, yTertiary, StringComparison.OrdinalIgnoreCase);
    }
}