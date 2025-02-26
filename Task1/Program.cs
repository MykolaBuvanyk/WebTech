namespace Pattern
{
    class Program
    {
        static void Main()
        {
            DeliveryFactory deliveryFactory = new DroneDeliveryFactory();
            deliveryFactory.PerformDelivery("вул. Шевченка, 10");
            Order order = new Order();
            var customer = new Customer();
            var restaurant = new Restaurant();
            order.Attach(customer);
            order.Attach(restaurant);
            order.Status = "Готується";
            order.Status = "Доставлено";
            IDeliveryService basicDelivery = new BasicDelivery();
            IDeliveryService giftWrappedDelivery = new GiftWrapDecorator(basicDelivery);
            IDeliveryService expressGiftDelivery = new ExpressDeliveryDecorator(giftWrappedDelivery);

            Console.WriteLine($"{expressGiftDelivery.GetDescription()} коштує {expressGiftDelivery.GetCost()} грн");
        }
    }
}
