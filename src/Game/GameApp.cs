using Game.Controllers;
using Game.Registry;
using Godot;
using MassTransit;
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
	
		GlobalContainer.Host = _host;
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
