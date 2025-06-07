

using Serilog.Events;
using Serilog.Formatting;

namespace InfraServices;

public class CustomSerilogFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
       output.Write($"[{logEvent.Timestamp:HH:mm:ss} {logEvent.Level}] {logEvent.MessageTemplate}");

       if (logEvent.TraceId.HasValue)
       {
           output.Write($" [TraceId: {logEvent.TraceId}]");
       }

       if (logEvent.SpanId.HasValue)
       {
           output.Write($" [SpanId: {logEvent.SpanId}]");
       }

       if (logEvent.Exception != null)
       {
           output.Write($" [Exception: {logEvent.Exception}]");
       }
    }
}