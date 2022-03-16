using Flunt.Validations;

namespace IWantApp.Domain.Models.Products;

public class Category : Entity
{
    public string Name { get; private set; }
    public bool Active { get; private set; }

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        Active = true;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
    }

    public void EditInfo(string name, bool active)
    {
        Active = active;
        Name = name;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name", "Name is required")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "CreatedBy is required")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "EditedBy is required");

        AddNotifications(contract);
    }
}