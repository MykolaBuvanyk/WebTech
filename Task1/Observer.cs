using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern
{
    public class Order
    {
        private List<IOrderObserver> _observers = new List<IOrderObserver>();
        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                NotifyObservers();
            }
        }

        public void Attach(IOrderObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IOrderObserver observer)
        {
            _observers.Remove(observer);
        }

        private void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
    }

    public interface IOrderObserver
    {
        void Update(Order order);
    }

    public class Customer : IOrderObserver
    {
        public void Update(Order order)
        {
            Console.WriteLine($"Клієнт отримав сповіщення: Статус замовлення - {order.Status}");
        }
    }

    public class Restaurant : IOrderObserver
    {
        public void Update(Order order)
        {
            Console.WriteLine($"Ресторан отримав сповіщення: Статус замовлення - {order.Status}");
        }
    }
}
