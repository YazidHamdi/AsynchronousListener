using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousListener
{
  class Program
  {
    //Simulated intervals, tweak to see different behaviors
    const int ReceiveInterval = 50;
    const int ConsumeInterval = 100;
    const int DisplayInterval = 50;
    const int MaxMessageQueueSize = 200;

    /// <summary>
    /// ID of the last received message
    /// </summary>
    static long messageId = 0;
    static void Main(string[] args)
    {
      long queueCount, receivedCount;

      var messageQueue = new ConcurrentQueue<string>();
      var listen = Task.Run(() => { ReceiveMessages(messageQueue, ReceiveInterval); });
      var consume = Task.Run(() => { ConsumeMessages(messageQueue, ConsumeInterval); });

      while (true)
      {
        //Capturing state
        queueCount = messageQueue.Count;
        receivedCount = Interlocked.Read(ref messageId);

        //Displaying
        Console.Clear();
        Console.WriteLine("In queue:       " + queueCount);
        Console.WriteLine("Consumed:       " + (receivedCount - queueCount));
        Console.WriteLine("Total received: " + receivedCount);
        Thread.Sleep(DisplayInterval);
      }
    }

    /// <summary>
    /// Enqueues received messages.
    /// </summary>
    /// <param name="messageQueue">Concurrent message queue</param>
    /// <param name="interval">Reception interval (milliseconds)</param>
    static void ReceiveMessages(ConcurrentQueue<string> messageQueue, int interval)
    {
      while (true)
      {
        //Replace with listening code which enqueues messages
        if (messageQueue.Count < MaxMessageQueueSize)
        {
          messageQueue.Enqueue("Message" + messageId);
          Interlocked.Increment(ref messageId);
          Thread.Sleep(interval);
        }
      }
    }

    /// <summary>
    /// Dequeues messages and performs consumption logic.
    /// </summary>
    /// <param name="messageQueue">Concurrent message queue</param>
    /// <param name="interval">Consumption interval (milliseconds)</param>
    static void ConsumeMessages(ConcurrentQueue<string> messageQueue, int interval)
    {
      string dequeuedMessage;
      while (true)
      {
        if (messageQueue.TryDequeue(out dequeuedMessage))
        {
          //Implement message consumption here

          Thread.Sleep(interval);
        }
      }
    }

  }
}
