using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Handlers;

public record EgoState
{
    public int Con { get; private set; } = 5;
    
    public EgoState ConDelta(int delta)
    {
        Con += delta;
        return this;
    }
}

public record GetEgoStateQuery : IRequest<EgoState> { }

public class GetEgoStateHandler : IRequestHandler<GetEgoStateQuery, EgoState>
{
    private readonly EgoState _state;

    public GetEgoStateHandler(EgoState state)
    {
        _state = state;
    }

    public Task<EgoState> Handle(GetEgoStateQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_state);
    }
}

public record EgoConDeltaCommand : IRequest
{
    public int Delta { get; init; }
}

public class EgoConDeltaHandler : IRequestHandler<EgoConDeltaCommand>
{
    private readonly EgoState _state;
    
    public EgoConDeltaHandler(EgoState state) =>
        _state = state;
    
    public Task Handle(EgoConDeltaCommand request, CancellationToken cancellationToken)
    {
        _state.ConDelta(request.Delta);
        return Task.CompletedTask;
    }
}
