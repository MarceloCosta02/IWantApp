﻿namespace IWantApp.Domain.Models.Products;

public class Product : Entity
{
    public string Name { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;

    public Product() { }

    public Product(string name, Category category, string description, bool hasStock, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;

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
            .IsNotNullOrEmpty(Description, "Description is required")
            .IsGreaterOrEqualsThan(Description, 3, "Description should be greater or equals than 3")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy", "CreatedBy is required")
            .IsNotNullOrEmpty(EditedBy, "EditedBy", "EditedBy is required");

        AddNotifications(contract);
    }

}
