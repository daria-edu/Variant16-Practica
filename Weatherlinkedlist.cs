using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace WeatherList
{
    public class WeatherLinkedList : IEnumerable<WeatherData>
    {
        private WeatherNode? head;
        private WeatherNode? tail;
        private int count;

        public int Length
        {
            get { return count; }
        }

        public WeatherData this[int index]
        {
            get
            {
                CheckIndex(index);
                return GetNode(index).Data;
            }

            set
            {
                CheckIndex(index);
                GetNode(index).Data = value;
            }
        }

        private void CheckIndex(int index)
        {
            if (count == 0)
            {
                throw new InvalidOperationException("Список порожній.");
            }

            if (index < 0 || index >= count)
            {
                throw new ArgumentOutOfRangeException(
                    "Неправильний індекс.");
            }
        }

        private WeatherNode GetNode(int index)
        {
            WeatherNode current = head!;

            for (int i = 0; i < index; i++)
            {
                current = current.Next!;
            }

            return current;
        }

        public void AddFirst(WeatherData data)
        {
            WeatherNode node = new WeatherNode(data);

            if (head == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                node.Next = head;
                head.Prev = node;
                head = node;
            }

            count++;
        }

        public void AddLast(WeatherData data)
        {
            WeatherNode node = new WeatherNode(data);

            if (tail == null)
            {
                head = node;
                tail = node;
            }
            else
            {
                node.Prev = tail;
                tail.Next = node;
                tail = node;
            }

            count++;
        }

        public WeatherData RemoveAt(int index)
        {
            CheckIndex(index);

            WeatherNode node = GetNode(index);

            if (node.Prev != null)
            {
                node.Prev.Next = node.Next;
            }
            else
            {
                head = node.Next;
            }

            if (node.Next != null)
            {
                node.Next.Prev = node.Prev;
            }
            else
            {
                tail = node.Prev;
            }

            count--;

            return node.Data;
        }

        private WeatherNode? iteratorCurrent;

        public WeatherData? IteratorReset()
        {
            iteratorCurrent = head;
            return iteratorCurrent?.Data;
        }

        public WeatherData? IteratorNext()
        {
            if (iteratorCurrent == null)
            {
                return null;
            }

            iteratorCurrent = iteratorCurrent.Next;

            return iteratorCurrent?.Data;
        }

        public (WeatherLinkedList, WeatherLinkedList, WeatherLinkedList)
            SplitByThreshold(double threshold)
        {
            WeatherLinkedList below = new WeatherLinkedList();
            WeatherLinkedList equal = new WeatherLinkedList();
            WeatherLinkedList above = new WeatherLinkedList();

            foreach (WeatherData item in this)
            {
                if (item.Temperature < threshold)
                {
                    below.AddLast(item);
                }
                else if (item.Temperature == threshold)
                {
                    equal.AddLast(item);
                }
                else
                {
                    above.AddLast(item);
                }
            }

            return (below, equal, above);
        }

        public List<WeatherData> SearchRainyBelow15()
        {
            List<WeatherData> result = new List<WeatherData>();

            foreach (WeatherData item in this)
            {
                if (item.WeatherType == WeatherType.Rainy &&
                    item.HasPrecipitation &&
                    item.Temperature < 15)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public void SerializeToJson(string fileName)
        {
            List<WeatherData> data = new List<WeatherData>();

            foreach (WeatherData item in this)
            {
                data.Add(item);
            }

            string json = JsonSerializer.Serialize(data);

            File.WriteAllText(fileName, json);
        }

        public static WeatherLinkedList DeserializeFromJson(string fileName)
        {
            string json = File.ReadAllText(fileName);

            List<WeatherData>? data =
                JsonSerializer.Deserialize<List<WeatherData>>(json);

            WeatherLinkedList list = new WeatherLinkedList();

            if (data != null)
            {
                foreach (WeatherData item in data)
                {
                    list.AddLast(item);
                }
            }

            return list;
        }

        public IEnumerator<WeatherData> GetEnumerator()
        {
            WeatherNode? current = head;

            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}