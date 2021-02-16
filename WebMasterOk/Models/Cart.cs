using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMasterOk.Models.CodeFirst;

namespace WebMasterOk.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }

    public static class ModifyCart
    {
        public static List<Cart> Additem(List<Cart> carts, Product product, int quantity)
        {
            List<Cart> cart = carts;

            if(carts.Where(i=>i.Product.Id == product.Id).Count() == 0)
            {
                cart.Add(new Cart { Product = product, Quantity = quantity });
            }
            else
            {
                foreach(Cart item in cart.Where(i => i.Product.Id == product.Id))
                {
                    item.Quantity += quantity;
                }
            }

            return cart;
        }

        public static List<Cart> RemoveItem(List<Cart> carts, Product product)
        {
            List<Cart> cart = carts;
            cart.RemoveAll(i => i.Product.Id == product.Id);

            return cart;
        }

        public static decimal GetTotalValue(List<Cart> carts)
        {
            return carts.Sum(e => e.Product.Price * e.Quantity);
        }

        public static void ClearList(List<Cart> carts)
        {
            carts.Clear();
        }
    }
}