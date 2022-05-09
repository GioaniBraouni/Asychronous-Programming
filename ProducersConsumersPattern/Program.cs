namespace ProducerConsumerPattern
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Nito.AsyncEx;

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

        private static async void TestProducerConsumerFunction()
        {
            var sharedBuffer = new BufferBlock<IList<int>>();
            var producer1 = AsynchronousProducer(sharedBuffer,1);
            var producer2 = AsynchronousProducer(sharedBuffer,2);
            var producer3 = AsynchronousProducer(sharedBuffer,3);

            await Task.WhenAll(producer1, producer2, producer3);
            sharedBuffer.Complete();
            AsynchronousConsumer(sharedBuffer);

        }

        private static async Task AsynchronousProducer(ITargetBlock<IList<int>> targetBlock,int x)
        {
            var randomInteger = new Random();
            var list = new List<int>();
            int elem = randomInteger.Next(0, 20);
            //Do some work here to produce work for consumer.
            for (var generatorCounter = 0; generatorCounter < elem; generatorCounter++)
            {
                var value = randomInteger.Next(0, 100000);
                Console.WriteLine("Producer:"+x+" Produced: " + value);
                list.Add(value);
            }
            await targetBlock.SendAsync(list);
            Console.WriteLine();
        }
        #endregion
    }
}
