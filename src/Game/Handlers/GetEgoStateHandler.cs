using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Handlers;

public record EgoState
{
    
}

public record GetEgoStatsQuery : IRequest<EgoState> { }

public class GetEgoStatsHandler : IRequestHandler<GetEgoStatsQuery, EgoStats>
{
    private readonly CounterState _state;

    public GetCounterHandler(CounterState state)
    {
        _state = state;
    }

    public Task<int> Handle(GetCounterQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_state.Value);
    }
}

public class IncrementCounterHandler : IRequestHandler<IncrementCounterCommand>
{
    private readonly CounterState _state;

    public IncrementCounterHandler(CounterState state)
    {
        _state = state;
    }

    public Task Handle(IncrementCounterCommand request, CancellationToken cancellationToken)
    {
        _state.Increment();
        return Task.CompletedTask;
    }
}

public class DecrementCounterHandler : IRequestHandler<DecrementCounterCommand>
{
    private readonly CounterState _state;

    public DecrementCounterHandler(CounterState state)
    {
        _state = state;
    }

    public Task Handle(DecrementCounterCommand request, CancellationToken cancellationToken)
    {
        _state.Decrement();
        return Task.CompletedTask;
    }
}
