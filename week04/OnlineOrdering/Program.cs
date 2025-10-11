using System;
using System.Collections.Generic;
using System.Globalization;

public class Address
{
    private string _street;
    private string _city;
    private string _stateOrProvince;
    private string _country;

    public Address(string street, string city, string stateOrProvince, string country)
    {
        _street = street ?? "";
        _city = city ?? "";
        _stateOrProvince = stateOrProvince ?? "";
        _country = country ?? "";
    }

    public string GetStreet() => _street;
    public string GetCity() => _city;
    public string GetStateOrProvince() => _stateOrProvince;
    public string GetCountry() => _country;

    public void SetStreet(string street) => _street = street ?? "";
    public void SetCity(string city) => _city = city ?? "";
    public void SetStateOrProvince(string stateOrProvince) => _stateOrProvince = stateOrProvince ?? "";
    public void SetCountry(string country) => _country = country ?? "";

    public bool IsInUSA()
    {
        var c = (_country ?? "").Trim().ToLowerInvariant();
        return c == "usa" || c == "united states" || c == "united states of america";
    }

    public string Format()
    {
        return $"{_street}\n{_city}, {_stateOrProvince}\n{_country}";
    }
}

public class Customer
{
    private string _name;
    private Address _address;

    public Customer(string name, Address address)
    {
        _name = name ?? "";
        _address = address ?? throw new ArgumentNullException(nameof(address));
    }

    public string GetName() => _name;
    public Address GetAddress() => _address;

    public void SetName(string name) => _name = name ?? "";
    public void SetAddress(Address address) => _address = address ?? throw new ArgumentNullException(nameof(address));

    public bool LivesInUSA() => _address.IsInUSA();

    public string ShippingBlock() => $"{_name}\n{_address.Format()}";
}

public class Product
{
    private string _name;
    private string _productId;
    private decimal _pricePerUnit;

    public Product(string name, string productId, decimal pricePerUnit)
    {
        _name = name ?? "";
        _productId = productId ?? "";
        _pricePerUnit = pricePerUnit < 0 ? 0 : pricePerUnit;
    }

    public string GetName() => _name;
    public string GetProductId() => _productId;
    public decimal GetPricePerUnit() => _pricePerUnit;

    public void SetName(string name) => _name = name ?? "";
    public void SetProductId(string productId) => _productId = productId ?? "";
    public void SetPricePerUnit(decimal price) => _pricePerUnit = price < 0 ? 0 : price;
}

public class OrderItem
{
    private Product _product;
    private decimal _unitPrice;
    private int _quantity;

    public OrderItem(Product product, int quantity, decimal? unitPriceOverride = null)
    {
        _product = product ?? throw new ArgumentNullException(nameof(product));
        _quantity = quantity < 0 ? 0 : quantity;
        _unitPrice = unitPriceOverride.HasValue ? (unitPriceOverride.Value < 0 ? 0 : unitPriceOverride.Value) : product.GetPricePerUnit();
    }

    public Product GetProduct() => _product;
    public decimal GetUnitPrice() => _unitPrice;
    public int GetQuantity() => _quantity;

    public void SetQuantity(int qty) => _quantity = qty < 0 ? 0 : qty;
    public void SetUnitPrice(decimal price) => _unitPrice = price < 0 ? 0 : price;

    public decimal LineTotal() => _unitPrice * _quantity;

    public string PackingLine() => $"{_product.GetName()} (ID: {_product.GetProductId()}) x{_quantity}";
}

public class ShippingCalculator
{
    private decimal _domestic;
    private decimal _international;

    public ShippingCalculator(decimal domestic = 5m, decimal international = 35m)
    {
        _domestic = domestic < 0 ? 0 : domestic;
        _international = international < 0 ? 0 : international;
    }

    public decimal GetShippingCost(Address address)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));
        return address.IsInUSA() ? _domestic : _international;
    }
}

public class Order
{
    private Customer _customer;
    private List<OrderItem> _items;
    private ShippingCalculator _shippingCalculator;

    public Order(Customer customer, ShippingCalculator shippingCalculator = null)
    {
        _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        _items = new List<OrderItem>();
        _shippingCalculator = shippingCalculator ?? new ShippingCalculator();
    }

    public Customer GetCustomer() => _customer;
    public void SetCustomer(Customer customer) => _customer = customer ?? throw new ArgumentNullException(nameof(customer));

    public ShippingCalculator GetShippingCalculator() => _shippingCalculator;
    public void SetShippingCalculator(ShippingCalculator calculator) => _shippingCalculator = calculator ?? new ShippingCalculator();

    public void AddItem(Product product, int quantity, decimal? unitPriceOverride = null)
    {
        if (product == null) return;
        _items.Add(new OrderItem(product, quantity, unitPriceOverride));
    }

    public IReadOnlyList<OrderItem> GetItems() => _items.AsReadOnly();

    private decimal ShippingCost() => _shippingCalculator.GetShippingCost(_customer.GetAddress());

    public decimal Subtotal()
    {
        decimal sum = 0m;
        foreach (var it in _items) sum += it.LineTotal();
        return sum;
    }

    public decimal TotalPrice() => Subtotal() + ShippingCost();

    public string PackingLabel()
    {
        var lines = new List<string> { "=== PACKING LABEL ===" };
        foreach (var it in _items) lines.Add(it.PackingLine());
        return string.Join("\n", lines);
    }

    public string ShippingLabel() => $"=== SHIPPING LABEL ===\n{_customer.ShippingBlock()}";
}

internal class Program
{
    private static void Main()
    {
        var culture = CultureInfo.CreateSpecificCulture("en-US");

        var productA = new Product("Duff Beer Pack", "DB-001", 3.50m);
        var productB = new Product("Donuts Box", "DN-010", 1.25m);
        var productC = new Product("ECG Sensor", "ECG-8232", 18.99m);
        var productD = new Product("PPG Module", "PPG-30105", 24.50m);
        var productE = new Product("Jumper Wires", "JW-040", 0.10m);

        var address1 = new Address("742 Evergreen Terrace", "Springfield", "IL", "USA");
        var customer1 = new Customer("Homer Simpson", address1);
        var order1 = new Order(customer1);
        order1.AddItem(productA, 6);
        order1.AddItem(productB, 12);

        Console.WriteLine(order1.PackingLabel());
        Console.WriteLine(order1.ShippingLabel());
        Console.WriteLine($"Subtotal: {order1.Subtotal().ToString("C", culture)}");
        Console.WriteLine($"Total:    {order1.TotalPrice().ToString("C", culture)}");
        Console.WriteLine();

        var address2 = new Address("Av. Ayacucho 1234", "Cochabamba", "CBBA", "Bolivia");
        var customer2 = new Customer("Adrian Del Castillo", address2);
        var order2 = new Order(customer2);
        order2.AddItem(productC, 2);
        order2.AddItem(productD, 1);
        order2.AddItem(productE, 50);

        Console.WriteLine(order2.PackingLabel());
        Console.WriteLine(order2.ShippingLabel());
        Console.WriteLine($"Subtotal: {order2.Subtotal().ToString("C", culture)}");
        Console.WriteLine($"Total:    {order2.TotalPrice().ToString("C", culture)}");
    }
}