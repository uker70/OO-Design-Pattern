using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_Design_Pattern
{
    class BenjaminTest
    {
        class Product
        {
            public interface IProduct
            {
                string GetName();
                string SetPrice(double price);
            }

            public class Phone : IProduct
            {
                private double _price;

                public string GetName()
                {
                    return "Apple TouchPad";
                }

                public string SetPrice(double price)
                {
                    this._price = price;
                    return "success";
                }
            }

            /* Almost same as Factory, just an additional exposure to do something with the created method */
            public abstract class ProductAbstractFactory
            {
                protected abstract IProduct MakeProduct();

                public IProduct GetObject() // Implementation of Factory Method.
                {
                    return this.MakeProduct();
                }
            }

            public class PhoneConcreteFactory : ProductAbstractFactory
            {
                protected override IProduct MakeProduct()
                {
                    IProduct product = new Phone();
                    //Do something with the object after you get the object. 
                    product.SetPrice(20.30);
                    return product;
                }
            }
        }
    }
}
