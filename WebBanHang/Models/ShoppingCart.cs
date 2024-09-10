using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHang.Models.EF;

namespace WebBanHang.Models
{
    public class ShoppingCart
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        public List<ShoppingCartItem> items { get; set; }
        public ShoppingCart()
        {
            this.items = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item, int quantity)
        {
            var checkExit = items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExit != null)
            {
                checkExit.Quantity += quantity;
                checkExit.Total = checkExit.Price * checkExit.Quantity;
            }
            else
            {
                items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExits = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                items.Remove(checkExits);
            }
        }
        public void UpdateQuantity(int id, int quantity)
        {
            var checkExits = items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.Total = checkExits.Price * checkExits.Quantity;
            }
        }
        public int getQuantityFromdb(int id)
        {
            var model = dbContext.Products.FirstOrDefault(x => x.Id == id);
            int quantity = model.Quantity;
            return quantity;
        }
        public void RemoveQuantity(int id, int quantity)
        {
            //Product product = new Product();
            var model = dbContext.Products.SingleOrDefault(x => x.Id == id);
            int oldQuantity = getQuantityFromdb(id);
            model.Quantity = oldQuantity - quantity;
            dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
            dbContext.SaveChanges();
        }
        public decimal GetTotal()
        {
            return items.Sum(x => x.Total);
        }
        public int GetQuantity()
        {
            return items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
    public class ShoppingCartItem
    {
        public int ProductId { set; get; }
        public string ProductName { set; get; }
        public string Alias { set; get; }
        public string CategoryName { set; get; }
        public string ProductImg { set; get; }
        public int Quantity { set; get; }
        public decimal Total { set; get; }
        public decimal Price { set; get; }
    }
}