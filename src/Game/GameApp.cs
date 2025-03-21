using Game.Constants;
using Game.Controllers;
using Game.Registry;
using Game.Models;
using Game.Models.State;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class GlobalContainer
{
	public static IHost Host { get; set; }
}

public partial class GameApp : Node
{
	private IHost _host;
	
	public override void _Ready()
	{
		_host = Host.CreateDefaultBuilder()
			.ConfigureServices((hostContext, services) =>
			{
				services.RegisterGame();
			})
			.Build();
	
		InitControllers(_host);
		
		GlobalContainer.Host = _host;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventKey keyEvent) 
			return;
			
		if (!keyEvent.Pressed || keyEvent.Echo)
			return;
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
	
	public override void _ExitTree()
	{
		base._ExitTree();
		
		if (_host == null) return;
		
		_host.StopAsync().Wait();
		_host.Dispose();
		_host = null;
	}
}
