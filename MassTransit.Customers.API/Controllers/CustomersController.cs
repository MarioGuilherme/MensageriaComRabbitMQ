using MassTransit.Customers.API.Bus;
using MassTransit.Customers.API.Models;
using MessagingEvents.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Customers.API.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(IBusService bus) : ControllerBase {
    private readonly IBusService _bus = bus;

    [HttpPost]
    public async Task<IActionResult> Post(CustomerInputModel model) {
        CustomerCreated @event = new(model.Id, model.FullName, model.Email, model.PhoneNumber, model.BirthDate);

        await this._bus.Publish(@event);

        return this.Accepted();
    }
}