﻿namespace MassTransit.Customers.API.Models;

public class CustomerInputModel {
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Document { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime BirthDate { get; set; }
}