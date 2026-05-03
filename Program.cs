using ContactBook;

namespace ContactBook;

public class Program
{
    public static void Main()
    {
        var contacts = new List<Contact>
{
    new Contact("Michael",  "Padilla",   "1234567890", "padilla@example.com"),
    new Contact("Henry",    "Bruckman",  "5551234567", "bruckman@example.com"),
    new Contact("Ada",      "Lovelace",  "0000000000", "ada@example.com"),
    new Contact("Grace",    "Hopper",    "2125550101", "grace.hopper@example.com"),
    new Contact("Alan",     "Turing",    "2125550102", "alan.turing@example.com"),
    new Contact("Linus",    "Torvalds",  "2125550103", "linus@example.com"),
    new Contact("Dennis",   "Ritchie",   "2125550104", "dennis@example.com"),
    new Contact("Ken",      "Thompson",  "2125550105", "ken.thompson@example.com"),
    new Contact("Margaret", "Hamilton",  "2125550106", "margaret.h@example.com"),
    new Contact("Guido",    "van Rossum","2125550107", "guido@example.com"),
    new Contact("Bjarne",   "Stroustrup","2125550108", "bjarne@example.com"),
    new Contact("James",    "Gosling",   "2125550109", "james.gosling@example.com"),
    new Contact("Tim",      "Berners-Lee","2125550110","timbl@example.com"),
    new Contact("Barbara",  "Liskov",    "2125550111", "barbara.l@example.com"),
    new Contact("Donald",   "Knuth",     "2125550112", "donald.knuth@example.com")
};
        var repository   = new InMemoryContactRepository(contacts);
        var contactSvc   = new ContactService(repository);
        var menuRenderer = new MenuRenderer(contactSvc);

        menuRenderer.Start();
    }
}