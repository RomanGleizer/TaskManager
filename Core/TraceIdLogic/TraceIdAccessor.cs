using Core.TraceLogic.Interfaces;
using Serilog.Context;

namespace Core.TraceIdLogic;

internal class TraceIdAccessor : ITraceReader, ITraceWriter, ITraceIdAccessor
{
    private string _value;
    public string Name => "TraceId";

    public void WriteValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) value = Guid.NewGuid().ToString();

        _value = value;
        LogContext.PushProperty("TraceId", value);
    }

    public string GetValue()
    {
        return _value;
    }
}