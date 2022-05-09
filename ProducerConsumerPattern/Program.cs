using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
namespace ProducerConsumerPattern
{
    internal static class Program
    {
        #region Methods

        private static async void AsynchronousConsumer(ISourceBlock<IList<int>> sourceBlock)
        {
            while (await sourceBlock.OutputAvailableAsync())
            {
                var producedResult = await sourceBlock.ReceiveAsync();
                foreach (var result in producedResult)
                {
                    Console.WriteLine("Consumer consumed :" + result);
                }
                Console.WriteLine();
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Running Test For: Producer Consumer");
            TestProducerConsumerFunction();
            Console.ReadKey();
        }

        private static void TestProducerConsumerFunction()
        {
            var sharedBuffer = new BufferBlock<IList<int>>();
            AsynchronousConsumer(sharedBuffer);
            AsynchronousProducer(sharedBuffer);
 
        }

        private static async void AsynchronousProducer(ITargetBlock<IList<int>> targetBlock)
        {
            var randomInteger = new Random();
            var list = new List<int>();
            int elem = randomInteger.Next(0, 20);
            //Do some work here to produce work for consumer.
            for (var generatorCounter = 0; generatorCounter < elem; generatorCounter++)
            {
                var value = randomInteger.Next(0, 100000);
                Console.WriteLine("Producer Produced: " + value);
                list.Add(value);
            }

            await targetBlock.SendAsync(list);
            targetBlock.Complete();
            Console.WriteLine();
        } 
        #endregion
    }
}