using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern
{
    public interface IDeliveryService
    {
        string GetDescription();
        double GetCost();
    }

    public class BasicDelivery : IDeliveryService
    {
        public string GetDescription()
        {
            return "Базова доставка";
        }

        public double GetCost()
        {
            return 50.0;
        }
    }

    public abstract class DeliveryDecorator : IDeliveryService
    {
        protected IDeliveryService _deliveryService;

        public DeliveryDecorator(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        public virtual string GetDescription()
        {
            return _deliveryService.GetDescription();
        }

        public virtual double GetCost()
        {
            return _deliveryService.GetCost();
        }
    }

    public class GiftWrapDecorator : DeliveryDecorator
    {
        public GiftWrapDecorator(IDeliveryService deliveryService) : base(deliveryService) { }

        public override string GetDescription()
        {
            return _deliveryService.GetDescription() + ", з подарунковою упаковкою";
        }

        public override double GetCost()
        {
            return _deliveryService.GetCost() + 20.0;
        }
    }

    public class ExpressDeliveryDecorator : DeliveryDecorator
    {
        public ExpressDeliveryDecorator(IDeliveryService deliveryService) : base(deliveryService) { }

        public override string GetDescription()
        {
            return _deliveryService.GetDescription() + ", термінова доставка";
        }

        public override double GetCost()
        {
            return _deliveryService.GetCost() + 100.0;
        }
    }
}
