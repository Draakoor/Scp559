using System.Collections.Concurrent;
using Exiled.API.Features.Pools;
using Scp559.Utilities.Voice;

namespace Scp559.Utilities.Pooling;

public class PitchShifterPool : IPool<PitchShifter>
{
    public static readonly PitchShifterPool Shared = new();
        
    private readonly ConcurrentQueue<PitchShifter> _pool = new();
        
    public PitchShifter Get() => _pool.TryDequeue(out var shifter) ? shifter : new PitchShifter();

    public void Return(PitchShifter shifter) => _pool.Enqueue(shifter);
}
