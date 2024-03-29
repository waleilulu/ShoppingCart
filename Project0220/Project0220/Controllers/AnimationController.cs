using Microsoft.AspNetCore.Mvc;
using Aspose.Imaging;
using Aspose.Imaging.ImageOptions;
using Aspose.Imaging.FileFormats.Apng;
using Aspose.Imaging.FileFormats.Png;
using Aspose.Imaging.Sources;
using System.Diagnostics;

namespace Project0220.Controllers
{
	public class AnimationController : Controller
	{
        private readonly IWebHostEnvironment _hostEnvironment;

        public AnimationController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult GenerateAnimation()
        {
            string imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images", "GG.png");

            using (Image image = Image.Load(imagePath))
            {
                // 检查加载图像的类型
                Debug.Assert(image is ApngImage);

                // 保存为相同格式的 PNG 图像文件
                image.Save(Path.Combine(_hostEnvironment.WebRootPath, "images", "GG_same_format.png"));

                // 导出为 GIF 格式的动画文件
                image.Save(Path.Combine(_hostEnvironment.WebRootPath, "images", "GG.gif"), new GifOptions());
            }

            return View(); // 如果需要返回视图，可以返回相应的视图
        }
        public IActionResult ConvertToApng()
		{
			using (Image image = Image.Load("img4.tif"))
			{
				// 设置默认帧持续时间
				image.Save("wwwroot/images/GG.500ms.png", new ApngOptions() { DefaultFrameTime = 500 }); // 500 ms
				image.Save("wwwroot/images/GGLogo.250ms.png", new ApngOptions() { DefaultFrameTime = 250 }); // 250 ms
			}

			return View(); // 这里可以返回任何需要的视图
		}
	}

}
