using Game.Controllers;
using Game.Registry;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public partial class GameApp : Node
{
	public override void _Ready()
	{
		var host = Host.CreateDefaultBuilder()
			.ConfigureServices((hostContext, services) =>
			{
				services.RegisterGame();
			})
			.Build();

		InitControllers(host);
	}

	private void InitControllers(IHost host)
	{
		var controllerTypes = ControllerUtil.GetControllerTypes();
		foreach (var controllerType in controllerTypes)
		{
			var controller = (IController)host.Services.GetRequiredService(controllerType);
			controller.Register();
		}
	}
}
