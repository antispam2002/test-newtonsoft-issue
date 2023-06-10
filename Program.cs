using System.Reflection;
using System.Text;
using Newtonsoft.Json;

// warm up serializer to initialize the settings for annotated JsonContracts
WarmUpJsonSerializationExtension.WarmUpJsonConverter();
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
Thread.Sleep(300);

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class JsonContract : Attribute
{ }

public static class WarmUpJsonSerializationExtension
{
    public static void WarmUpJsonConverter()
    {
        var typesWithMyAttribute =
          from a in AppDomain.CurrentDomain.GetAssemblies()
          from t in a.GetTypes()
          let attributes = t.GetCustomAttributes(typeof(JsonContract), true)
          where attributes != null && attributes.Length > 0
          select new { Type = t };
        foreach (var jsonContract in typesWithMyAttribute)
        {
          var warmerUp = Activator.CreateInstance(typeof(WarmerUp<>).MakeGenericType(jsonContract.Type));
          MethodInfo warmUp = warmerUp.GetType().GetMethod("WarmUp", BindingFlags.Public | BindingFlags.Instance);
          warmUp.Invoke(warmerUp, null);
        }
    }

}

public class WarmerUp<T>
{
    public void WarmUp()
    {
        try
        {
            JsonConvert.DeserializeObject<T>(@"{}");
        }
        catch
        {
            // ignore
        }
    }
}

[JsonContract]
public class Payload
{
    [JsonProperty("eventType", Required = Required.Always)]
    public string EventType { get; set; } = null!;
}
