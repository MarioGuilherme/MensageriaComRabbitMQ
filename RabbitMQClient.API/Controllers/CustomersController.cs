using MessagingEvents.Shared;
using MessagingEvents.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQClient.Customers.API.Bus;

namespace RabbitMQClient.Customers.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(IBusService bus) : ControllerBase {
    const string ROUTING_KEY = "customer-created";
    private readonly IBusService _bus = bus;

    [HttpPost]
    public IActionResult Post(CustomerInputModel model) {
        CustomerCreated @event = new(model.Id, model.FullName, model.Email, model.PhoneNumber, model.BirthDate);

        this._bus.Publish(ROUTING_KEY, @event);

        return this.Accepted();
    }
}