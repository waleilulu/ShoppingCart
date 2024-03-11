using Microsoft.AspNetCore.Mvc;
using Project0220.Models;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Project0220.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ManualECommerceDBContext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ManualECommerceDBContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.OrderByDescending(p => p.ProductID).ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            // 檢查圖片1是否有上傳
            if (productDto.Image1 == null)
            {
                // 若圖片1未上傳，添加錯誤訊息
                ModelState.AddModelError(nameof(productDto.Image1), "Image1必填");
                // 返回包含錯誤的視圖
                return View(productDto);
            }

            // 如果模型不合法，返回包含錯誤的視圖
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // 儲存圖片1
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.Image1.FileName);

            // 圖片1路徑位置
            string image1FullPath = environment.WebRootPath + "/images/All_product/" + newFileName;
            using (var stream = System.IO.File.Create(image1FullPath))
            {
                productDto.Image1.CopyTo(stream);
            }

            // 初始化 Image2 和 Image3 的檔案名稱可為空值
            string? newImage2FileName = null;
            string? newImage3FileName = null;


            // 如果 Image2 有上傳，處理 Image2
            if (productDto.Image2 != null)
            {
                // 儲存圖片2
                newImage2FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image2.FileName);
                string image2FullPath = environment.WebRootPath + "/images/All_product/" + newImage2FileName;
                using (var stream = System.IO.File.Create(image2FullPath))
                {
                    productDto.Image2.CopyTo(stream);
                }
            }

            // 如果 Image3 有上傳，處理 Image3
            if (productDto.Image3 != null)
            {
                // 儲存圖片3
                newImage3FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image3.FileName);
                string image3FullPath = environment.WebRootPath + "/images/All_product/" + newImage3FileName;
                using (var stream = System.IO.File.Create(image3FullPath))
                {
                    productDto.Image3.CopyTo(stream);
                }
            }

            // 儲存新商品在資料庫裡
            Product product = new Product()
            {
                ProductName = productDto.ProductName,
                SupplierID = productDto.SupplierID,
                CategoryID = productDto.CategoryID,
                UnitPrice = productDto.UnitPrice,
                UnitInStock = productDto.UnitInStock,
                Image1 = newFileName,
                Image2 = newImage2FileName,
                Image3 = newImage3FileName,
                Color1 = productDto.Color1,
                Color2 = productDto.Color2,
                Color3 = productDto.Color3,
                Color4 = productDto.Color4,
                Length = productDto.Length,
                Width = productDto.Width,
                Height = productDto.Height,
                SpecialZoneType = productDto.SpecialZoneType,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            //create ProductDto from product
            var productDto = new ProductDto()
            {
                ProductName = product.ProductName,
                SupplierID = product.SupplierID,
                CategoryID = product.CategoryID,
                UnitPrice = product.UnitPrice,
                UnitInStock = product.UnitInStock,
                Color1 = product.Color1,
                Color2 = product.Color2,
                Color3 = product.Color3,
                Color4 = product.Color4,
                Length = product.Length,
                Width = product.Width,
                Height = product.Height,
                SpecialZoneType = product.SpecialZoneType,
            };
            ViewData["ProductID"] = product.ProductID;
            ViewData["Image1"] = product.Image1;
            ViewData["Image2"] = product.Image2;
            ViewData["Image3"] = product.Image3;
            ViewData["CreatedAt"] = product.CreatedAt.ToString("yyyy/MM/dd");

            return View(productDto);
        }
        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            if (!ModelState.IsValid)
            {
                ViewData["ProductID"] = product.ProductID;
                ViewData["Image1"] = product.Image1;
                ViewData["Image2"] = product.Image2;
                ViewData["Image3"] = product.Image3;
                ViewData["CreatedAt"] = product.CreatedAt.ToString("yyyy/MM/dd");

                return View(productDto);
            }

            //更新第一張圖
            string newFileName = product.Image1;
            if (productDto.Image1 != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.Image1.FileName);

                // 圖片1路徑位置
                string image1FullPath = environment.WebRootPath + "/images/All_product/" + newFileName;
                using (var stream = System.IO.File.Create(image1FullPath))
                {
                    productDto.Image1.CopyTo(stream);
                }
                //刪除舊圖
                string oldImageFullPath = environment.WebRootPath + "/images/All_product/" + product.Image1;
                System.IO.File.Delete(oldImageFullPath);
            }

            // 更新第二張圖
            string newImage2FileName = null;
            if (productDto.Image2 != null)
            {
                newImage2FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image2.FileName);
                string image2FullPath = environment.WebRootPath + "/images/All_product/" + newImage2FileName;
                using (var stream = System.IO.File.Create(image2FullPath))
                {
                    productDto.Image2.CopyTo(stream);
                }
            }

            // 刪除舊圖（如果有）
            if (!string.IsNullOrEmpty(product.Image2))
            {
                string oldImage2FullPath = environment.WebRootPath + "/images/All_product/" + product.Image2;
                System.IO.File.Delete(oldImage2FullPath);
            }

            // 更新第三張圖
            string newImage3FileName = null;
            if (productDto.Image3 != null)
            {
                newImage3FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image3.FileName);
                string image3FullPath = environment.WebRootPath + "/images/All_product/" + newImage3FileName;
                using (var stream = System.IO.File.Create(image3FullPath))
                {
                    productDto.Image3.CopyTo(stream);
                }
            }

            // 刪除舊圖（如果有）
            if (!string.IsNullOrEmpty(product.Image3))
            {
                string oldImage3FullPath = environment.WebRootPath + "/images/All_product/" + product.Image3;
                System.IO.File.Delete(oldImage3FullPath);
            }

            //更新商品資訊至資料庫
            product.ProductName = productDto.ProductName;
            product.SupplierID = productDto.SupplierID;
            product.CategoryID = productDto.CategoryID;
            product.UnitPrice = productDto.UnitPrice;
            product.UnitInStock = productDto.UnitInStock;
            // 更新商品圖片屬性
            product.Image1 = newFileName;
            product.Image2 = newImage2FileName;
            product.Image3 = newImage3FileName;
            product.Color1 = productDto.Color1;
            product.Color2 = productDto.Color2;
            product.Color3 = productDto.Color3;
            product.Color4 = productDto.Color4;
            product.Length = productDto.Length;
            product.Width = productDto.Width;
            product.Height = productDto.Height;
            product.SpecialZoneType = productDto.SpecialZoneType;

            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string image1FullPath = environment.WebRootPath + "/images/All_product/" + product.Image1;
            System.IO.File.Delete(image1FullPath);

            // 如果第二張圖片存在則刪除第二張圖片
            if (!string.IsNullOrEmpty(product.Image2))
            {
                string image2FullPath = environment.WebRootPath + "/images/All_product/" + product.Image2;
                System.IO.File.Delete(image2FullPath);
            }

            // 如果第三張圖片存在則刪除第三張圖片
            if (!string.IsNullOrEmpty(product.Image3))
            {
                string image3FullPath = environment.WebRootPath + "/images/All_product/" + product.Image3;
                System.IO.File.Delete(image3FullPath);
            }

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }

    }
}

