using Testcontainers.PostgreSql;

namespace CustomerService.Test;

public sealed class CustomerServiceTest : IAsyncLifetime
{
	private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder().WithImage("postgres:15-alpine").Build();

	public Task InitializeAsync()
	{
		return _postgres.StartAsync();
	}

	public Task DisposeAsync()
	{
		return _postgres.DisposeAsync().AsTask();
	}

	[Fact]
	public void ShouldReturnTwoCustomers()
	{
		var customerService = new CustomerService(new DbConnectionProvider(_postgres.GetConnectionString()));

		customerService.Create(new Customer(1, "Goku"));
		customerService.Create(new Customer(2, "Vegeta"));
		var customers = customerService.GetCustomers();

		Assert.Equal(2, customers.Count());
	}

}

