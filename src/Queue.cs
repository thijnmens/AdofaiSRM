using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityModManagerNet;

namespace AdofaiSRM.src
{
    internal class Queue
    {
        private static List<QueueItem> queue = new List<QueueItem>();

        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.Label("Queue");
            foreach (QueueItem item in queue)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(item.name))
                {
                    ADOBase.controller.LoadCustomWorld(item.path);
                }
                if (GUILayout.Button("Remove", GUILayout.MaxWidth(Screen.width / 10)))
                {
                    queue.Remove(item);
                }
                GUILayout.EndHorizontal();
            }
        }

        public static void AddToQueueAdofai(string id, string name)
        {
            Steam.SubcribeToItem(Convert.ToUInt64(id));
            queue.Add(new QueueItem(id, name));
        }

        public static void AddToQueueFile(string id, string name, string path)
        {
            queue.Add(new QueueItem(id, name, path));
        }

        public static void AddToQueueSteam(string id)
        {
            AdofaiSRM.mod.Logger.Log(id);
            queue.Add(new QueueItem(id, id));
            Steam.SubcribeToItem(Convert.ToUInt64(id));
        }

        public static List<string> GetQueueNames(int amount, int page)
        {
            List<string> items = new List<string>();
            foreach (int i in Enumerable.Range(page * amount, page * amount + amount))
            {
                AdofaiSRM.mod.Logger.Log(i.ToString());
                try
                {
                    items.Add(queue[i].name);
                }
                catch { }
            }
            return items;
        }
    }

    internal class QueueItem
    {
        public string id;
        public string name;
        public string path;

        public QueueItem(string id, string name, string path = null)
        {
            this.id = id;
            this.name = name;
            if (path != null)
            {
                this.path = path;
            }
            else
            {
                this.path = Path.Combine(AppContext.BaseDirectory, $"../../workshop/content/977950/{id}/main.adofai");
            }
        }
    }
}