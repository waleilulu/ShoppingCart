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



		//1. 如果從CustomersController登入畫面轉過來管理者頁面後，需再這裡進行檢查，檢查是否是管理者
		//2. 如果是知道連結進來的人，直接導去CustomersController登入畫面
        public IActionResult Index()
        {
            // 檢查 Session 中是否存在管理員用戶名
           // var adminUsername1 = HttpContext.Session.GetString("adminUsername");
            var adminUsername=HttpContext.Request.Cookies["membercookie"] ;
            if (!string.IsNullOrEmpty(adminUsername))
            { 
                // 存在管理員用戶名，表示是管理員登入後轉過來的，顯示畫面
                var products = context.Products.OrderByDescending(p => p.ProductID).ToList();
                return View(products);
            }
            else
            {
                // 若不存在管理員用戶名，可能是非法訪問導向到登入頁面
                return RedirectToAction("Login", "Customers"); // 導去CustomersController登入畫面
            }
        }


  //      public IActionResult Index()
		//{
			
		//		var products = context.Products.OrderByDescending(p => p.ProductID).ToList();
		//		return View(products);
			
		//}









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

            // 初始化 Image2 和 Image3  和 Image4 的檔案名稱可為空值
            string? newImage2FileName = null;
            string? newImage3FileName = null;
            string? newImage4FileName = null;


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

            // 如果 Image4 有上傳，處理 Image4
            if (productDto.Image4 != null)
            {
                // 儲存圖片4
                newImage4FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image4.FileName);
                string image4FullPath = environment.WebRootPath + "/images/All_product/" + newImage4FileName;
                using (var stream = System.IO.File.Create(image4FullPath))
                {
                    productDto.Image4.CopyTo(stream);
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
                Image4 = newImage4FileName,
                Color1 = productDto.Color1,
                Color2 = productDto.Color2,
                Length = productDto.Length,
                Width = productDto.Width,
                Height = productDto.Height,
                Description = productDto.Description,
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
                Length = product.Length,
                Width = product.Width,
                Height = product.Height,
                Description = product.Description,
                SpecialZoneType = product.SpecialZoneType,
            };
            ViewData["ProductID"] = product.ProductID;
            ViewData["Image1"] = product.Image1;
            ViewData["Image2"] = product.Image2;
            ViewData["Image3"] = product.Image3;
            ViewData["Image4"] = product.Image4;
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
                ViewData["Image4"] = product.Image4;
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
			string newImage2FileName = product.Image2; // 保留原始圖片文件名
			if (productDto.Image2 != null)
			{
				newImage2FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image2.FileName);
				string image2FullPath = environment.WebRootPath + "/images/All_product/" + newImage2FileName;
				using (var stream = System.IO.File.Create(image2FullPath))
				{
					productDto.Image2.CopyTo(stream);
				}

				// 刪除舊圖
				if (!string.IsNullOrEmpty(product.Image2))
				{
					string oldImage2FullPath = environment.WebRootPath + "/images/All_product/" + product.Image2;
					System.IO.File.Delete(oldImage2FullPath);
				}
			}

			// 更新第三張圖
			string newImage3FileName = product.Image3; // 保留原始圖片文件名
			if (productDto.Image3 != null)
			{
				newImage3FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image3.FileName);
				string image3FullPath = environment.WebRootPath + "/images/All_product/" + newImage3FileName;
				using (var stream = System.IO.File.Create(image3FullPath))
				{
					productDto.Image3.CopyTo(stream);
				}

				// 刪除舊圖
				if (!string.IsNullOrEmpty(product.Image3))
				{
					string oldImage3FullPath = environment.WebRootPath + "/images/All_product/" + product.Image3;
					System.IO.File.Delete(oldImage3FullPath);
				}
			}

			// 更新第四張圖
			string newImage4FileName = product.Image4; // 保留原始圖片文件名
			if (productDto.Image4 != null)
			{
				newImage4FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(productDto.Image4.FileName);
				string image4FullPath = environment.WebRootPath + "/images/All_product/" + newImage4FileName;
				using (var stream = System.IO.File.Create(image4FullPath))
				{
					productDto.Image4.CopyTo(stream);
				}

				// 刪除舊圖
				if (!string.IsNullOrEmpty(product.Image4))
				{
					string oldImage4FullPath = environment.WebRootPath + "/images/All_product/" + product.Image4;
					System.IO.File.Delete(oldImage4FullPath);
				}
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
            product.Image4 = newImage4FileName;
            product.Color1 = productDto.Color1;
            product.Color2 = productDto.Color2;
            product.Length = productDto.Length;
            product.Width = productDto.Width;
            product.Height = productDto.Height;
            product.Description = productDto.Description;
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

            // 如果第四張圖片存在則刪除第三張圖片
            if (!string.IsNullOrEmpty(product.Image4))
            {
                string image4FullPath = environment.WebRootPath + "/images/All_product/" + product.Image4;
                System.IO.File.Delete(image4FullPath);
            }

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }

    }
}

