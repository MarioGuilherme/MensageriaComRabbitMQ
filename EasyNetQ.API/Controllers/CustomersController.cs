using EasyNetQ.Customers.API.Bus;
using EasyNetQ.Customers.API.Models;
using MessagingEvents.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EasyNetQ.Customers.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(IBusService bus) : ControllerBase {
    const string ROUTING_KEY = "costumer-created";
    private readonly IBusService _bus = bus;

    [HttpPost]
    public IActionResult Post(CustomerInputModel model) {
        CustomerCreated @event = new(model.Id, model.FullName, model.Email, model.PhoneNumber, model.BirthDate);

        this._bus.Publish(ROUTING_KEY, @event);

        return this.Accepted();
    }
}