using System.Text;
using Newtonsoft.Json;

var timer1 = new Timer(state =>
  {
    var msg = new StringBuilder();
    msg.Append("EventType `empty` -> should not throw");
    try
    {
      JsonConvert.DeserializeObject<Payload>(@"{ ""eventType"": """" }"); // This does not throw.
      msg.Append(" :)");
    }
    catch (Exception e)
    {
      msg.Append(" BUT IT DID -> ").Append(e.Message);
    }
    Console.WriteLine(msg.ToString());
  },
  null,
  TimeSpan.FromMilliseconds(1),
  TimeSpan.FromMilliseconds(1));
var timer2 = new Timer(state =>
  {
    var msg = new StringBuilder();
    msg.Append("EventType `null` ->");
    try
    {
      JsonConvert.DeserializeObject<Payload>(@"{ ""eventType"": null }"); // This must throw.
      msg.Append(" must throw, BUT IT DID NOT :/");
    }
    catch (Exception e)
    {
      msg.Append(" threw ").Append(e.Message).Append(" :)");
    }
    Console.WriteLine(msg.ToString());
  },
  null,
  TimeSpan.FromMilliseconds(1),
  TimeSpan.FromMilliseconds(1));
// sleep in main thread for a while
Thread.Sleep(500);

public class Payload
{
  [JsonProperty("eventType", Required = Required.Always)]
  public string EventType { get; set; } = null!;
}
