using IWantApp.Domain.Models.Orders;

namespace IWantApp.Domain.Models.Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; private set; }
    public ICollection<Order> Orders { get; private set; }

    public Product() { }

    public Product(string name, Category category, string description, bool hasStock, decimal price, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        Price = price;

        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name", "Name is required")
            .IsGreaterOrEqualsThan(Name, 3, "Name should be greater or equals than 3")
            .IsNotNull(Category, "Category not found")
            .IsNotNullOrEmpty(Description, "Description is required")
            .IsGreaterOrEqualsThan(Description, 3, "Description should be greater or equals than 3")
            .IsGreaterOrEqualsThan(Price, 1, "Price should be greater or equals than 1")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "CreatedBy is required")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "EditedBy is required");

        AddNotifications(contract);
    }

}
