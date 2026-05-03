using System;
using System.Linq;

namespace ContactBook;

public class MenuRenderer
{
    // Command keys
    public const string CommandNextPage   = ">";
    public const string CommandPrevPage   = "<";
    public const string CommandGotoPage   = "g";
    public const string CommandChangeSize = "z";
    public const string CommandCreate     = "c";
    public const string CommandReview     = "r";
    public const string CommandUpdate     = "u";
    public const string CommandDelete     = "d";
    public const string CommandFind       = "f";
    public const string CommandSearch     = "s";
    public const string CommandOrder      = "o";
    public const string CommandDedup      = "m";
    public const string CommandExit       = "x";

    public static readonly string[] Commands =
    {
        CommandNextPage,
        CommandPrevPage,
        CommandGotoPage,
        CommandChangeSize,
        CommandCreate,
        CommandReview,
        CommandUpdate,
        CommandDelete,
        CommandFind,
        CommandSearch,
        CommandOrder,
        CommandDedup,
        CommandExit
    };

    private readonly ContactService _contactService;

    private int _currentPage = 1;
    private int _pageSize    = 2; // adjust as you like

    public MenuRenderer(ContactService contactService)
    {
        _contactService = contactService;
    }

    public void Start()
    {
        bool shouldExit = false;

        ShowWelcomeScreen();
        PressEnterToContinue();

        while (!shouldExit)
        {
            Console.Clear();
            ShowContacts();
            Console.WriteLine();
            ShowInputOptions();

            string input = CaptureInput();

            if (!IsValidInput(input))
            {
                Console.WriteLine();
                Console.WriteLine("Invalid input. Please try again.");
                PressEnterToContinue();
                continue;
            }

            shouldExit = ProcessInput(input);
        }

        ShowExitScreen();
        PressEnterToContinue();
    }

    public void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("        Welcome to Contact Book         ");
        Console.WriteLine("========================================");
        Console.WriteLine("Manage your contacts with pagination,");
        Console.WriteLine("sorting, search, and deduplication.");
        Console.WriteLine();
    }

    public void ShowContacts()
    {
        var contacts = _contactService.GetContacts();

        Console.WriteLine($"Current contacts (page {_currentPage}, size {_pageSize}):");
        Console.WriteLine("----------------------------------------");

        if (contacts.Count == 0)
        {
            Console.WriteLine("No contacts found.");
            return;
        }

        int totalPages = Math.Max(1, (int)Math.Ceiling(contacts.Count / (double)_pageSize));
        if (_currentPage > totalPages) _currentPage = totalPages;
        if (_currentPage < 1) _currentPage = 1;

        int startIndex = (_currentPage - 1) * _pageSize;
        var pageItems = contacts.Skip(startIndex).Take(_pageSize).ToList();

       for (int i = 0; i < pageItems.Count; i++)
{
    int globalIndex = startIndex + i;
    var c = pageItems[i];

    Console.WriteLine($"{globalIndex + 1}.");
    Console.WriteLine($"  Name : {c.GetFirstName()} {c.GetLastName()}");
    Console.WriteLine($"  Phone: {c.GetPhone()}");
    Console.WriteLine($"  Email: {c.GetEmail()}");
    Console.WriteLine();
}

        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Page {_currentPage} of {totalPages}");
    }

    public void ShowInputOptions()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Available commands:");
        Console.WriteLine();

        Console.WriteLine($">  ({CommandNextPage}) Next page");
        Console.WriteLine($"<  ({CommandPrevPage}) Previous page");
        Console.WriteLine($"g  ({CommandGotoPage}) Go to page");
        Console.WriteLine($"z  ({CommandChangeSize}) Change page size");
        Console.WriteLine($"c  ({CommandCreate}) Create contact");
        Console.WriteLine($"r  ({CommandReview}) Review contact");
        Console.WriteLine($"u  ({CommandUpdate}) Update contact");
        Console.WriteLine($"d  ({CommandDelete}) Delete contact");
        Console.WriteLine($"f  ({CommandFind}) Find contacts");
        Console.WriteLine($"s  ({CommandSearch}) Search contacts (same as Find)");
        Console.WriteLine($"o  ({CommandOrder}) Order contacts");
        Console.WriteLine($"m  ({CommandDedup}) Deduplicate + merge contacts");
        Console.WriteLine($"x  ({CommandExit}) Exit");
        Console.WriteLine();

        Console.Write("Enter a command: ");
    }

    public string CaptureInput()
    {
        string? input = Console.ReadLine();
        return input?.Trim().ToLowerInvariant() ?? string.Empty;
    }

    public bool IsValidInput(string input)
    {
        return Commands.Contains(input);
    }

    public bool ProcessInput(string input)
    {
        switch (input)
        {
            case CommandNextPage:
                NextPage();
                return false;

            case CommandPrevPage:
                PreviousPage();
                return false;

            case CommandGotoPage:
                GotoPage();
                return false;

            case CommandChangeSize:
                ChangePageSize();
                return false;

            case CommandCreate:
                CreateContact();
                return false;

            case CommandReview:
                ReviewContact();
                return false;

            case CommandUpdate:
                UpdateContact();
                return false;

            case CommandDelete:
                DeleteContact();
                return false;

            case CommandFind:
            case CommandSearch:
                FindContacts();
                return false;

            case CommandOrder:
                OrderContacts();
                return false;

            case CommandDedup:
                DeduplicateAndMergeContacts();
                return false;

            case CommandExit:
                return ConfirmExit();

            default:
                Console.WriteLine();
                Console.WriteLine("Unknown command.");
                PressEnterToContinue();
                return false;
        }
    }

    private void NextPage()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to paginate.");
            PressEnterToContinue();
            return;
        }

        int totalPages = (int)Math.Ceiling(contacts.Count / (double)_pageSize);
        if (_currentPage < totalPages)
        {
            _currentPage++;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("You are already on the last page.");
            PressEnterToContinue();
        }
    }

    private void PreviousPage()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("You are already on the first page.");
            PressEnterToContinue();
        }
    }

    private void GotoPage()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to paginate.");
            PressEnterToContinue();
            return;
        }

        int totalPages = (int)Math.Ceiling(contacts.Count / (double)_pageSize);

        Console.WriteLine();
        Console.Write($"Enter page number (1–{totalPages}): ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int page) || page < 1 || page > totalPages)
        {
            Console.WriteLine("Invalid page number.");
            PressEnterToContinue();
            return;
        }

        _currentPage = page;
    }

    private void ChangePageSize()
    {
        Console.WriteLine();
        Console.Write("Enter page size (positive integer): ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int size) || size <= 0)
        {
            Console.WriteLine("Invalid page size.");
            PressEnterToContinue();
            return;
        }

        _pageSize    = size;
        _currentPage = 1;
    }

    private void CreateContact()
    {
        Console.WriteLine();
        Console.WriteLine("Create new contact (leave first name empty to cancel).");

        Console.Write("First name: ");
        string? firstName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(firstName))
        {
            Console.WriteLine("Creation cancelled.");
            PressEnterToContinue();
            return;
        }

        Console.Write("Last name: ");
        string? lastName = Console.ReadLine() ?? string.Empty;

        Console.Write("Phone: ");
        string? phone = Console.ReadLine() ?? string.Empty;

        Console.Write("Email: ");
        string? email = Console.ReadLine() ?? string.Empty;

        var contact = new Contact(
            firstName: firstName.Trim(),
            lastName:  lastName.Trim(),
            phone:     phone.Trim(),
            email:     email.Trim()
        );

        _contactService.AddContact(contact);

        Console.WriteLine();
        Console.WriteLine("Contact created successfully.");
        PressEnterToContinue();
    }

    private void ReviewContact()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to review.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Review Contact");
        Console.WriteLine("----------------------------------------");

        int index = GetContactIndexFromUser(contacts.Count, "Enter contact number to review");

        var contact = contacts[index];

        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("            Contact Details             ");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine($"Number: {index + 1}");
        Console.WriteLine($"First name: {contact.GetFirstName()}");
        Console.WriteLine($"Last name:  {contact.GetLastName()}");
        Console.WriteLine($"Phone:      {contact.GetPhone()}");
        Console.WriteLine($"Email:      {contact.GetEmail()}");
        Console.WriteLine("----------------------------------------");

        PressEnterToContinue();
    }

    private void UpdateContact()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to update.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Update Contact");
        Console.WriteLine("----------------------------------------");

        int index = GetContactIndexFromUser(contacts.Count, "Enter contact number to update");
        var contact = contacts[index];

        Console.WriteLine();
        Console.WriteLine("Leave input empty to keep existing value.");
        Console.WriteLine();

        Console.WriteLine($"Current first name: {contact.GetFirstName()}");
        Console.Write("New first name (or Enter to keep): ");
        string? newFirst = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newFirst))
        {
            newFirst = contact.GetFirstName();
        }

        Console.WriteLine($"Current last name: {contact.GetLastName()}");
        Console.Write("New last name (or Enter to keep): ");
        string? newLast = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newLast))
        {
            newLast = contact.GetLastName();
        }

        Console.WriteLine($"Current phone: {contact.GetPhone()}");
        Console.Write("New phone (or Enter to keep): ");
        string? newPhone = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newPhone))
        {
            newPhone = contact.GetPhone();
        }

        Console.WriteLine($"Current email: {contact.GetEmail()}");
        Console.Write("New email (or Enter to keep): ");
        string? newEmail = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newEmail))
        {
            newEmail = contact.GetEmail();
        }

        Console.WriteLine();
        Console.WriteLine("Updated contact preview:");
        Console.WriteLine("----------------------------------------");
        var preview = new Contact(newFirst, newLast, newPhone, newEmail);
        Console.WriteLine(preview);
        Console.WriteLine("----------------------------------------");

        string confirm = GetOption(
            prompt: "Apply these changes?",
            validOptions: new[] { "y", "n" },
            defaultOption: "y"
        );

        if (confirm == "y")
        {
            _contactService.UpdateContactAt(index, newFirst, newLast, newPhone, newEmail);
            Console.WriteLine("Contact updated.");
        }
        else
        {
            Console.WriteLine("Update cancelled.");
        }

        PressEnterToContinue();
    }

    private void DeleteContact()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to delete.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Delete Contact");
        Console.WriteLine("----------------------------------------");

        int index = GetContactIndexFromUser(contacts.Count, "Enter contact number to delete");
        var contact = contacts[index];

        Console.WriteLine();
        Console.WriteLine("Contact to delete:");
        Console.WriteLine(contact);
        Console.WriteLine();

        string confirm = GetOption(
            prompt: "Are you sure you want to delete this contact?",
            validOptions: new[] { "y", "n" },
            defaultOption: "n"
        );

        if (confirm == "y")
        {
            _contactService.DeleteContactAt(index);

            var remaining = _contactService.GetContacts().Count;
            int totalPages = Math.Max(1, (int)Math.Ceiling(remaining / (double)_pageSize));
            if (_currentPage > totalPages)
            {
                _currentPage = totalPages;
            }

            Console.WriteLine("Contact deleted.");
        }
        else
        {
            Console.WriteLine("Delete cancelled.");
        }

        PressEnterToContinue();
    }

    private void FindContacts()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to search.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Find/Search Contacts");
        Console.WriteLine("----------------------------------------");
        Console.Write("Enter search text: ");
        string? query = Console.ReadLine();
        query = (query ?? string.Empty).Trim();

        if (string.IsNullOrEmpty(query))
        {
            Console.WriteLine("Empty search. Nothing to do.");
            PressEnterToContinue();
            return;
        }

        string lowerQuery = query.ToLowerInvariant();

        var matches = contacts
            .Select((c, i) => new { Contact = c, Index = i })
            .Where(x =>
            {
                string combined = $"{x.Contact.GetFirstName()} {x.Contact.GetLastName()} {x.Contact.GetPhone()} {x.Contact.GetEmail()}";
                return combined.ToLowerInvariant().Contains(lowerQuery);
            })
            .ToList();

        Console.WriteLine();
        if (matches.Count == 0)
        {
            Console.WriteLine($"No contacts found for \"{query}\".");
        }
        else
        {
            Console.WriteLine($"Found {matches.Count} contact(s) for \"{query}\":");
            Console.WriteLine("----------------------------------------");
            foreach (var match in matches)
            {
                Console.WriteLine($"{match.Index + 1}. {match.Contact}");
            }
            Console.WriteLine("----------------------------------------");
        }

        PressEnterToContinue();
    }

    private void OrderContacts()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to order.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Order Contacts");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("1. First name (ascending)");
        Console.WriteLine("2. Last name (ascending)");
        Console.WriteLine("3. Email (ascending)");
        Console.WriteLine("----------------------------------------");

        string option = GetOption(
            prompt: "Choose how to order",
            validOptions: new[] { "1", "2", "3" },
            defaultOption: "1"
        );

        ContactOrderBy orderBy = option switch
        {
            "1" => ContactOrderBy.FirstName,
            "2" => ContactOrderBy.LastName,
            "3" => ContactOrderBy.Email,
            _   => ContactOrderBy.FirstName
        };

        _contactService.OrderContacts(orderBy);

        _currentPage = 1;

        Console.WriteLine();
        Console.WriteLine("Contacts ordered.");
        PressEnterToContinue();
    }

    private void DeduplicateAndMergeContacts()
    {
        var contacts = _contactService.GetContacts();
        if (contacts.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No contacts to deduplicate.");
            PressEnterToContinue();
            return;
        }

        var duplicateGroups = _contactService.FindDuplicateGroups();

        if (duplicateGroups.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No duplicate contacts found.");
            PressEnterToContinue();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Deduplicate + Merge Contacts");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Found {duplicateGroups.Count} group(s) of duplicates.");
        Console.WriteLine();

        string proceed = GetOption(
            prompt: "Do you want to review and merge these groups?",
            validOptions: new[] { "y", "n" },
            defaultOption: "n"
        );

        if (proceed != "y")
        {
            Console.WriteLine();
            Console.WriteLine("Deduplication cancelled.");
            PressEnterToContinue();
            return;
        }

        var mergedContacts = new List<Contact>();
        var original = _contactService.GetContacts();

        var inAnyGroup = new HashSet<Contact>();
        foreach (var group in duplicateGroups)
        {
            foreach (var c in group)
            {
                inAnyGroup.Add(c);
            }
        }

        foreach (var c in original)
        {
            if (!inAnyGroup.Contains(c))
            {
                mergedContacts.Add(c);
            }
        }

        int groupNumber = 1;
        foreach (var group in duplicateGroups)
        {
            var merged = MergeDuplicateGroup(group, groupNumber, duplicateGroups.Count);
            mergedContacts.Add(merged);
            groupNumber++;
        }

        Console.WriteLine();
        Console.WriteLine("Preview complete. You have built a merged contact list.");
        Console.WriteLine($"Original contacts: {original.Count}, after merge: {mergedContacts.Count}");

        string apply = GetOption(
            prompt: "Do you want to apply these merges to the contacts?",
            validOptions: new[] { "y", "n" },
            defaultOption: "y"
        );

        if (apply == "y")
        {
            _contactService.ApplyMergedContacts(mergedContacts);
            _currentPage = 1;
            Console.WriteLine();
            Console.WriteLine("Merges applied successfully.");
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Merges discarded. No changes applied.");
        }

        PressEnterToContinue();
    }

    private Contact MergeDuplicateGroup(List<Contact> group, int groupNumber, int totalGroups)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine($"Duplicate Group {groupNumber} of {totalGroups}");
            Console.WriteLine("========================================");
            Console.WriteLine();

            for (int i = 0; i < group.Count; i++)
            {
                var c = group[i];
                Console.WriteLine($"{i + 1}. {c}");
            }

            Console.WriteLine();
            Console.WriteLine("Select which contact to use for each field.");
            Console.WriteLine("(Enter the number shown in the list above.)");
            Console.WriteLine();

            int idxFirst = GetFieldChoice("First name", group.Count, group, c => c.GetFirstName());
            int idxLast  = GetFieldChoice("Last name",  group.Count, group, c => c.GetLastName());
            int idxPhone = GetFieldChoice("Phone",      group.Count, group, c => c.GetPhone());
            int idxEmail = GetFieldChoice("Email",      group.Count, group, c => c.GetEmail());

            var merged = new Contact(
                firstName: group[idxFirst].GetFirstName(),
                lastName:  group[idxLast].GetLastName(),
                phone:     group[idxPhone].GetPhone(),
                email:     group[idxEmail].GetEmail()
            );

            Console.WriteLine();
            Console.WriteLine("Merged contact preview:");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(merged);
            Console.WriteLine("----------------------------------------");

            string confirm = GetOption(
                prompt: "Use this merged contact for this group?",
                validOptions: new[] { "y", "n" },
                defaultOption: "y"
            );

            if (confirm == "y")
            {
                return merged;
            }

            Console.WriteLine();
            Console.WriteLine("Okay, let's choose again for this group.");
            PressEnterToContinue();
        }
    }

    private int GetFieldChoice(
        string fieldName,
        int groupCount,
        List<Contact> group,
        Func<Contact, string> selector
    )
    {
        while (true)
        {
            Console.Write($"{fieldName}: choose contact number (1–{groupCount}): ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int index) || index < 1 || index > groupCount)
            {
                Console.WriteLine("Invalid number. Please try again.");
                continue;
            }

            var value = selector(group[index - 1]);
            Console.WriteLine($"Using \"{value}\" for {fieldName}.");

            string confirm = GetOption(
                prompt: $"Confirm choice for {fieldName}?",
                validOptions: new[] { "y", "n" },
                defaultOption: "y"
            );

            if (confirm == "y")
            {
                return index - 1; // zero-based index
            }
        }
    }

    private void ShowStub(string input)
    {
        Console.WriteLine();
        Console.WriteLine($"Command '{input}' is not implemented yet (stub).");
        PressEnterToContinue();
    }

    public void ShowExitScreen()
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("         Thank you for using");
        Console.WriteLine("             Contact Book");
        Console.WriteLine("========================================");
        Console.WriteLine();
    }

    private bool ConfirmExit()
    {
        string option = GetOption(
            prompt: "Do you want to exit?",
            validOptions: new[] { "y", "n" },
            defaultOption: "n"
        );

        return option == "y";
    }

    private string GetOption(string prompt, string[] validOptions, string defaultOption)
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write($"{prompt} [{string.Join("/", validOptions)}] (default: {defaultOption}): ");

            string? option = Console.ReadLine();
            option = string.IsNullOrWhiteSpace(option)
                ? defaultOption
                : option.Trim().ToLowerInvariant();

            if (validOptions.Contains(option))
            {
                return option;
            }

            Console.WriteLine("Invalid option. Please try again.");
        }
    }

    private int GetContactIndexFromUser(int totalContacts, string prompt)
    {
        while (true)
        {
            Console.WriteLine();
            Console.Write($"{prompt} (1–{totalContacts}): ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int number))
            {
                Console.WriteLine("Invalid number. Please enter a valid integer.");
                continue;
            }

            if (number < 1 || number > totalContacts)
            {
                Console.WriteLine($"Invalid number. Please enter a value between 1 and {totalContacts}.");
                continue;
            }

            return number - 1; // zero-based index
        }
    }

    private void PressEnterToContinue()
    {
        Console.WriteLine();
        Console.Write("Press Enter to continue... ");

        while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
        {
        }

        Console.WriteLine();
    }
}