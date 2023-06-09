using System.Text;
using Newtonsoft.Json;

var timer1 = new Timer(state =>
  {
    var msg = new StringBuilder();
    msg.Append("EventType `empty`: -> should not throw");
    try
    {
      JsonConvert.DeserializeObject<Payload>(@"{ ""eventType"": """" }"); // This does not throw.
    }
    catch (Exception e)
    {
      msg.Append(e.Message);
    }
    Console.WriteLine(msg.ToString());
  },
  null,
  TimeSpan.FromMilliseconds(1),
  TimeSpan.FromMilliseconds(1));
var timer2 = new Timer(state =>
  {
    var msg = new StringBuilder();
    msg.Append("EventType `null` -> must throw: ");
    try
    {
      JsonConvert.DeserializeObject<Payload>(@"{ ""eventType"": null }"); // This does throw.
    }
    catch (Exception e)
    {
      msg.Append(e.Message);
    }
    Console.WriteLine(msg.ToString());
  },
  null,
  TimeSpan.FromMilliseconds(1),
  TimeSpan.FromMilliseconds(1));
// sleep in main thread for a while
Thread.Sleep(200);

public class Payload
{
  [JsonProperty("eventType", Required = Required.Always)]
  public string EventType { get; set; } = null!;
}
