using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern
{
    public abstract class Delivery
    {
        public string Address { get; set; }
        public abstract void Deliver();
    }

    public class CourierDelivery : Delivery
    {
        public override void Deliver()
        {
            Console.WriteLine($"Доставка кур'єром за адресою: {Address}");
        }
    }

    public class DroneDelivery : Delivery
    {
        public override void Deliver()
        {
            Console.WriteLine($"Доставка дроном за адресою: {Address}");
        }
    }

    public abstract class DeliveryFactory
    {
        public abstract Delivery CreateDelivery(string address);

        public void PerformDelivery(string address)
        {
            var delivery = CreateDelivery(address);
            delivery.Deliver();
        }
    }

    public class CourierDeliveryFactory : DeliveryFactory
    {
        public override Delivery CreateDelivery(string address)
        {
            return new CourierDelivery { Address = address };
        }
    }

    public class DroneDeliveryFactory : DeliveryFactory
    {
        public override Delivery CreateDelivery(string address)
        {
            return new DroneDelivery { Address = address };
        }
    }
}
