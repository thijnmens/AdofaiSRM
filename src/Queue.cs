using System;
using System.Collections.Generic;
using System.Linq;

namespace AdofaiSRM.src
{
    internal class Queue
    {
        private static Dictionary<string, QueueItem> queue = new Dictionary<string, QueueItem>();

        public static void AddToQueueAdofai(string id, string name)
        {
            queue.Add(id, new QueueItem(id, name));
            Steam.SubcribeToItem(Convert.ToUInt64(id));
        }

        public static void AddToQueueSteam()
        {
            throw new NotImplementedException();
        }

        public static Dictionary<int, string> GetQueueNames(int amount, int page)
        {
            Dictionary<int, string> queuePage = new Dictionary<int, string>();
            try
            {
                foreach (int i in Enumerable.Range(amount * page, amount * page + page))
                {
                    try
                    {
                        queuePage.Add(i, queue.ElementAt(i).Value.name);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return queuePage;
                    }
                }
                return queuePage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return queuePage;
            }
        }
    }

    internal class QueueItem
    {
        public string id;
        public string name;

        public QueueItem(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}